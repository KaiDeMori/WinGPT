@echo off
rem dotnet publish -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o ReleaseFolderName -c Release /p:AssemblyVersion=1.2.3.4 /p:Version=1.2.3.4-product-version

rem "C:\Program Files (x86)\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"

rem msbuild WinGPT.csproj -t:Publish -p:Configuration=Release -p:PublishProfile=selfsigned_ClickOnce
rem "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" WinGPT.csproj -t:Publish -p:Configuration=Release -p:PublishProfile=selfsigned_ClickOnce
rem dotnet publish WinGPT.csproj -c Release -p:PublishProfile=selfsigned_ClickOnce

dotnet publish WinGPT.csproj -c Release -p:PublishProfile=LocalFolder
dotnet publish WinGPT.csproj -c Release -p:PublishProfile=FolderProfile
dotnet publish WinGPT.csproj -c Release -p:PublishProfile=x64
dotnet publish WinGPT.csproj -c Release -p:PublishProfile=x86


cd ../WinGPT_Setup
"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.com" WinGPT_Setup.vdproj /Build Release
cd ../WinGPT

echo Done!

rem pause