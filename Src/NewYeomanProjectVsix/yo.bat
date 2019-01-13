@echo off

REM INITIALISE VARIABLES
set arg1RegularProjDir=%1
::set arg2AssemblyDirectory=%2

REM INSTALL YEOMAN TEMPLATE
cd %arg1RegularProjDir%
setlocal enableDelayedExpansion
call yo

REM OPEN VISUAL STUDIO 
::cd %arg2AssemblyDirectory%
::CommonIdeLauncher.exe %arg1RegularProjDir%
