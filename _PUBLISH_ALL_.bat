@echo off
dotnet publish -c Release -p:PublishProfile=selfsigned_ClickOnce
rem dotnet publish -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o ReleaseFolderName -c Release /p:AssemblyVersion=1.2.3.4 /p:Version=1.2.3.4-product-version
dotnet publish -c Release -p:PublishProfile=LocalFolder
dotnet publish -c Release -p:PublishProfile=FolderProfile
dotnet publish -c Release -p:PublishProfile=x64
dotnet publish -c Release -p:PublishProfile=x86


cd ../WinGPT_Setup
"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.com" WinGPT_Setup.vdproj /Build Release
cd ../WinGPT

echo Done!

rem pause