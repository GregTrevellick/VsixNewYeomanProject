@echo off

REM INITIALISE VARIABLES
set arg1RegularProjDir=%1

REM INSTALL YEOMAN TEMPLATE
cd %arg1RegularProjDir%
setlocal enableDelayedExpansion
call yo2

REM Check for error (e.g. yo command not installed, npm not exists)
IF %ERRORLEVEL% NEQ 0 GOTO ProcessError

REM Success
exit /b 0

:ProcessError
REM Failure
exit /b 1