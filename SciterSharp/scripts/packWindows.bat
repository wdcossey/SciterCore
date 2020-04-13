@echo off

rem echo ######## Packing '../LibConsole' directory to 'ArchiveResource.cs' ########
cd %~dp0
rem packfolder.exe ../LibConsole ../ArchiveResource.cs -csharp
packfolder.exe %~dp1 %~dp2 -binary
rem fart.exe -- ..\ArchiveResource.* SciterAppResource SciterSharp
echo OK