#!/bin/sh
DIR=$(dirname "$0")/../Proto
PROTOC=$DIR/../scripts/proto/bin/protoc

# Server
$PROTOC -I=$DIR --csharp_opt=file_extension=.g.cs --csharp_out=$DIR/../Server/Common/Protocols/Generated $DIR/*.proto

# Client
$PROTOC -I=$DIR --csharp_opt=file_extension=.g.cs --csharp_out=$DIR/../Client/Assets/GoPlay/Scripts/Protocols/Generated $DIR/*.proto