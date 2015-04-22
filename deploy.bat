@echo off

path=%path%;%windir%\Microsoft.net\Framework\v4.0.30319

cd %~dp0

set /p VersionNumber=<latest

msbuild build.proj /p:CreateZip=1

pause