@echo off

REM INITIALISE VARIABLES
set arg1RegularProjDir=%1

REM INSTALL YEOMAN TEMPLATE
cd %arg1RegularProjDir%
setlocal enableDelayedExpansion
call yo




@rem some code
IF %ERRORLEVEL% NEQ 0 GOTO ProcessError

@rem ... other code
exit /b 0

:ProcessError
@rem process error
exit /b 1