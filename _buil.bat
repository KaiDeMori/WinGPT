set PATH=%PATH%;C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin


rem msbuild WinGPT.csproj /t:Publish /p:Configuration=Release /p:PublishProfile="Properties\PublishProfiles\selfsigned_ClickOnce_msbuild.pubxml"

msbuild.EXE WinGPT.csproj /t:rebuild /t:publish /p:PlatformTarget=x86 /p:Configuration=Release   /p:PublishType=web