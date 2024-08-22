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
