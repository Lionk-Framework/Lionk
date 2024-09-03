@echo off
rem Define variables
set CONFIGURATION=Debug
set RUNTIME_IDENTIFIER=linux-arm64
set TARGET_FRAMEWORK=net8.0
set PUBLISH_DIR=output
set PROJECT_NAME=LionkApp

rem Create project-specific output directory
set PROJECT_OUTPUT_DIR=%PUBLISH_DIR%\%PROJECT_NAME%

rem Publish the application
dotnet publish -c %CONFIGURATION% -r %RUNTIME_IDENTIFIER% -o %PROJECT_OUTPUT_DIR% --self-contained

rem Check if the publish was successful
if %ERRORLEVEL% NEQ 0 (
    echo Publication failed!
    exit /b %ERRORLEVEL%
)

set RASPBERRY_PI_USER=pi
set RASPBERRY_PI_HOST=192.168.1.111
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
