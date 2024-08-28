set RASPBERRY_PI_USER=pi
set RASPBERRY_PI_HOST=192.168.1.99
set RASPBERRY_PI_DIR=Lionk

rem Check if the remote directory exists, create if it doesn't
ssh %RASPBERRY_PI_USER%@%RASPBERRY_PI_HOST% "mkdir -p %RASPBERRY_PI_DIR%"

if %ERRORLEVEL% NEQ 0 (
    echo Failed to create or access the directory on Raspberry Pi!
    exit /b %ERRORLEVEL%
)

rem Use SCP to copy files (make sure you have SSH enabled on your Raspberry Pi)
scp -r %PROJECT_OUTPUT_DIR%\* %RASPBERRY_PI_USER%@%RASPBERRY_PI_HOST%:%RASPBERRY_PI_DIR%

if %ERRORLEVEL% NEQ 0 (
    echo File transfer failed!
    exit /b %ERRORLEVEL%
)

echo Application successfully published and transferred to Raspberry Pi!
pause