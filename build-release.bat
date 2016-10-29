@echo off

echo ###### INITIALIZE ######
if [%APPVEYOR%] == [] (
    rd /s /q release
) else (
    choco install 7zip.commandline -version 15.12
)

for /f "tokens=*" %%i in ('git rev-parse --short HEAD') do set COMMITID=%%i 

dotnet restore


echo ###### UNIT TESTS ######
dotnet test "%~dp0\src\ScmBackup.Tests" -c Release
if errorlevel 1 goto end


echo .
echo ###### INTEGRATION TESTS ######
dotnet test "%~dp0\src\ScmBackup.Tests.Integration" -c Release
if errorlevel 1 goto end


echo .
echo ###### PUBLISH ######
dotnet publish "%~dp0\src\ScmBackup" -c Release -o "%~dp0\release\bin"
if errorlevel 1 goto end


echo .
echo ###### ZIP ######
call 7za a -r -tzip release\scm-backup-%COMMITID%.zip .\release\bin\*



:end
if [%APPVEYOR%] == [] (
    pause
)