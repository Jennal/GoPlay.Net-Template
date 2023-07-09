#!/bin/sh
DIR=$(dirname "$0")/../Server/GoPlay.Net/Frameworks/Client
SRC_DIR=$(dirname "$0")/../Server/GoPlay.Net/Frameworks/Client/bin/Release/netstandard2.1
DST_DIR=$(dirname "$0")/../Client/Assets/GoPlay/Plugins

cd $DIR
dotnet build -c release -f netstandard2.1
cd -

#    "System.Runtime.CompilerServices.Unsafe.dll"
#    "Google.Protobuf.dll"
#    "System.Buffers.dll"
#    "System.Memory.dll"
outFiles=(
    "GoPlay.Service.Client.dll"
    "GoPlay.Service.Client.pdb"
    "GoPlay.Service.Core.dll"
    "GoPlay.Service.Core.pdb"
    "GoPlay.Common.Data.dll"
    "GoPlay.Common.Data.pdb"
    "Transport.NetCoreServer.dll"
    "Transport.NetCoreServer.pdb"
    "NetCoreServer.dll"
    "NetCoreServer.pdb"
)

for str in ${outFiles[@]}; do
  echo "copying: "$str
  cp -f $SRC_DIR/$str $DST_DIR/
done