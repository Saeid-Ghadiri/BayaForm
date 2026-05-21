#!/bin/bash

PROJECT_DIR="C:/Users/s.ghadiri/Documents/Baya/0/FormDll/Forms"
cd "$PROJECT_DIR" || { echo "خطا: نمی‌توان به مسیر $PROJECT_DIR وارد شد"; exit 1; }

CONFIG_FILE="NuGet.config"

if [[ ! -f "$CONFIG_FILE" ]]; then
    echo "⚠️  فایل $CONFIG_FILE یافت نشد!"
else
    echo "✅ فایل $CONFIG_FILE پیدا شد. ایجاد پشتیبان..."
    cp "$CONFIG_FILE" "$CONFIG_FILE.bak"
    
    # حذف خط حاوی Private (کل خط حذف می‌شود)
    # اما چون نمی‌خواهیم حذف کنیم، کامنت گذاری مطمئن:
    # با استفاده از perl که دقیق‌تر است
    perl -pi.bak -e 's/^(\s*<add key="Private" value="C:\\Nuget" \/>\s*)$/<!-- $1 -->/g' "$CONFIG_FILE"
    
    # اگر خط پیدا نشد، شاید به خاطر فاصله یا backslash متفاوت است
    if grep -q '<add key="Private" value="C:\\Nuget" />' "$CONFIG_FILE" && ! grep -q '<!-- <add key="Private"' "$CONFIG_FILE"; then
        echo "⚠️  خط پیدا شد اما کامنت نشد. شاید فرمت متفاوت است."
        echo "لطفاً فایل را دستی اصلاح کنید."
    else
        echo "✅ خط کامنت شد."
    fi
fi

echo "=== شروع build ==="
START_TIME=$(date +%s)
dotnet build --no-incremental -v:normal
BUILD_EXIT_CODE=$?
END_TIME=$(date +%s)

ELAPSED=$((END_TIME - START_TIME))
MINUTES=$((ELAPSED / 60))
SECONDS=$((ELAPSED % 60))

# بازگردانی فایل (اگر پشتیبان وجود داشته باشد)
if [[ -f "$CONFIG_FILE.bak" ]]; then
    # توجه: قبلاً با perl پشتیبان .bak گرفته شده، اما ما می‌خواهیم پس از build برگردانیم
    # برای جلوگیری از تداخل، پشتیبان اصلی را جدا نگه می‌داریم
    mv "$CONFIG_FILE.bak" "$CONFIG_FILE"
    echo "فایل $CONFIG_FILE به حالت اول بازگردانده شد (تغییرات build موقتی بودند)."
fi

echo "-----------------------------------"
echo "✅ زمان اجرای build: ${MINUTES} دقیقه و ${SECONDS} ثانیه"
echo "-----------------------------------"

if [[ $BUILD_EXIT_CODE -eq 0 ]]; then
    echo "✅ Build با موفقیت انجام شد."
else
    echo "❌ Build با خطا مواجه شد (کد: $BUILD_EXIT_CODE)"
fi

exit $BUILD_EXIT_CODE