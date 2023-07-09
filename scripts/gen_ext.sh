#!/bin/sh
DIR=$(dirname "$0")/..
goplay extension -i $DIR/Server -of $DIR/Client/Assets/GoPlay/Scripts/Clients/ClientExtensions.cs -b ProcessorBase,Processor
