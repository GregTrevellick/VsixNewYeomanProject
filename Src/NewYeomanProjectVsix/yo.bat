@echo off

REM INITIALISE VARIABLES
set arg1RegularProjDir=%1

REM INSTALL YEOMAN TEMPLATE
cd %arg1RegularProjDir%
setlocal enableDelayedExpansion
call yo
