@echo off

echo ######## Packing %1 directory to %2 ########
cd %~dp0
packfolder.exe %1 %2 -binary
rem fart.exe -- ..\ArchiveResource.* SciterAppResource SciterSharp
echo OK