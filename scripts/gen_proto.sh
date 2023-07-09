#!/bin/sh
DIR=$(dirname "$0")/../Proto
PROTOC=$DIR/../scripts/proto/bin/protoc
$PROTOC -I=$DIR --csharp_opt=file_extension=.g.cs --csharp_out=$DIR/../Server/Common/Protocols/Generated $DIR/*.proto