#!/bin/sh

#echo ######## Packing '../LibConsole' directory to 'ArchiveResource.cs' ########
cd "$(dirname "$0")"
chmod +x packfolder
./packfolder $1 $2 -binary
#sed -i '' 's/SciterAppResource/SciterSharp/' ./../ArchiveResource.cs
