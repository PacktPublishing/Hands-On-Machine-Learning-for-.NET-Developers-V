@echo off

IF EXIST .\images\ exit 0

echo Copying image data set into this directory
robocopy ..\..\images .\images /s /e
if %errorlevel% leq 1 exit 0 else exit %errorlevel%
