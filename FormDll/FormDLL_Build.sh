#!/bin/bash

SOLUTION_DIR="C:/Users/s.ghadiri/Documents/Baya/0/FormDll"
PROJECT_DIR="$SOLUTION_DIR/Forms"
CONFIG_FILE="$PROJECT_DIR/NuGet.config"

cd "$PROJECT_DIR" || { echo "خطا: نمی‌توان به مسیر $PROJECT_DIR وارد شد"; exit 1; }

# بررسی وجود dotnet
if ! command -v dotnet &> /dev/null; then
    echo "❌ خطا: dotnet پیدا نشد. لطفاً .NET SDK را نصب کنید یا PATH را تنظیم کنید."
    exit 1
fi

echo "dotnet version: $(dotnet --version)"

if [[ ! -f "$CONFIG_FILE" ]]; then
    echo "⚠️  فایل $CONFIG_FILE یافت نشد!"
else
    echo "✅ فایل $CONFIG_FILE پیدا شد. ایجاد پشتیبان..."
    cp "$CONFIG_FILE" "$CONFIG_FILE.original"

    sed -i 's|^\(\s*<add key="Private" value="C:\\Nuget" />\s*\)$|<!-- \1 -->|' "$CONFIG_FILE"

    if grep -q '<!-- .*<add key="Private"' "$CONFIG_FILE"; then
        echo "✅ خط Private کامنت شد."
    else
        echo "⚠️  خط Private کامنت نشد. تلاش با روش جایگزین..."
        sed -i '/<add key="Private"/s/^\(\s*\)\(.*\)/\1<!-- \2 -->/' "$CONFIG_FILE"

        if grep -q '<!-- .*<add key="Private"' "$CONFIG_FILE"; then
            echo "✅ خط Private با روش جایگزین کامنت شد."
        else
            echo "❌ خط Private کامنت نشد. ادامه بدون تغییر..."
        fi
    fi
fi

LOG_FILE="$SOLUTION_DIR/build_log.txt"

echo ""
echo "=== شروع build ==="
START_TIME=$(date +%s)

cd "$SOLUTION_DIR" || { echo "خطا: نمی‌توان به مسیر Solution وارد شد"; exit 1; }

dotnet build "FormDllTemplate.sln" --no-incremental --verbosity minimal -consoleloggerparameters:Summary 2>&1 | tee "$LOG_FILE"
BUILD_EXIT_CODE=${PIPESTATUS[0]}

END_TIME=$(date +%s)
ELAPSED=$((END_TIME - START_TIME))
MINUTES=$((ELAPSED / 60))
SECONDS_LEFT=$((ELAPSED % 60))

if [[ -f "$CONFIG_FILE.original" ]]; then
    mv "$CONFIG_FILE.original" "$CONFIG_FILE"
    echo "✅ فایل NuGet.config به حالت اول بازگردانده شد."
fi

echo "-----------------------------------"
echo "✅ زمان اجرای build: ${MINUTES} دقیقه و ${SECONDS_LEFT} ثانیه"
echo "-----------------------------------"

if [[ $BUILD_EXIT_CODE -eq 0 ]]; then
    echo "✅ Build با موفقیت انجام شد."
else
    echo ""
    echo "========== خطاهای Build =========="
    grep -i ": error " "$LOG_FILE" || echo "(خطایی در لاگ پیدا نشد)"
    echo "=================================="
    echo ""
    echo "❌ Build با خطا مواجه شد (کد: $BUILD_EXIT_CODE)"
    echo "📄 لاگ کامل در: $LOG_FILE"
fi

exit $BUILD_EXIT_CODE
