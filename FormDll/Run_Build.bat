@echo off
echo ===== Starting build script =====
echo.

REM مسیر کامل به bash.exe (مسیر صحیح Git Bash)
set BASH_PATH="C:\Program Files\Git\bin\bash.exe"

REM مسیر کامل به اسکریپت شل شما
set SCRIPT_PATH="C:\Users\s.ghadiri\Documents\Baya\0\FormDll\FormDLL_Build.sh"

REM بررسی وجود bash.exe
if not exist %BASH_PATH% (
    echo ERROR: bash.exe not found at %BASH_PATH%
    echo Please install Git Bash or correct the path.
    pause
    exit /b 1
)

REM بررسی وجود فایل اسکریپت
if not exist %SCRIPT_PATH% (
    echo ERROR: Script file not found at %SCRIPT_PATH%
    pause
    exit /b 1
)

REM اجرای اسکریپت با bash
echo Running bash %SCRIPT_PATH% ...
%BASH_PATH% %SCRIPT_PATH%

REM نمایش کد خروجی
echo.
echo Build script finished with exit code: %ERRORLEVEL%

REM نگه داشتن پنجره تا فشردن یک کلید
echo.
echo Press any key to close this window...
pause > nul