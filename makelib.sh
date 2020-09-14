#!/bin/sh

# If the files don't exist, then we need to extract them first
if [ ! -d "sgsdk_source" ]; then
    unzip sgsdk_source.zip
fi

echo Moving into sgsdk_source
cd sgsdk_source

# If we haven't compiled the source yet, we should do that now,
# as well as cleaning up any old libraries if they exist.
if [ ! -f "bin/linux/libSGSDK.so.4.0" ]; then
    echo Building sgsdk
    ./build.sh
    echo Finished building

    if [ -f "bin/Debug/libsgsdk.so" ]; then
        echo Removing old library
        rm -f bin/Debug/libsgsdk.so
    fi
fi

# If our project has been cleaned previously, we need to relink the file
if [ ! -f "../bin/Debug/libsgsdk.so" ]; then
# FIXME: Liv suggested copying the library instead of symlinking it. I've
#        made the change for now, but as I'm not sure what is more
#        appropriate, I've left the old line commented out for now.

    #ln -s $(readlink -f bin/linux/libSGSDK.so.4.0) ../bin/Debug/libsgsdk.so
    echo Copying new file
    cp bin/linux/libSGSDK.so.4.0 ../bin/Debug/libsgsdk.so
fi
