#!/bin/bash

# Variables
PACKAGES_DIR="/usr/local/lib/python3.10/site-packages"
WHEEL_OUTPUT_DIR="/workspace/app-python-shipdetector-onnx/dist"

# Reconstitutes a python wheel from a package in site-packages/dist-packages
generate_wheels() {
    PACKAGE_NAME=$1
    PACKAGE_VERSION=$2

    # Find the package's RECORD file
    RECORD_FILE="${PACKAGES_DIR}/${PACKAGE_NAME}-${PACKAGE_VERSION}.dist-info/RECORD"
    if [ ! -f "$RECORD_FILE" ]; then
        echo "WARNING: RECORD file not found for $PACKAGE_NAME. Skipping package."
        return
    fi

    # Find the package's WHEEL file
    WHEEL_FILE="${PACKAGES_DIR}/${PACKAGE_NAME}-${PACKAGE_VERSION}.dist-info/WHEEL"
    if [ ! -f "$WHEEL_FILE" ]; then
        echo "WARNING: WHEEL file not found for $PACKAGE_NAME. Skipping package."
        return
    fi

    TEMP_DIR="${PACKAGE_NAME}_package"
    echo "Creating temporary directory: $TEMP_DIR"
    mkdir -p $TEMP_DIR

    # Copy the dist-info directory to the temporary directory
    cp -r $PACKAGES_DIR/${PACKAGE_NAME}-${PACKAGE_VERSION}.dist-info $TEMP_DIR

    # Create symlinks to package contents stated in RECORD_FILE to the temporary directory
    # skip paths starting with ../ or containing dist-info or __pycache__ from the RECORD file
    echo "Creating symlinks for package contents in $TEMP_DIR"
    while IFS= read -r line; do
        FILEPATH=$(echo $line | cut -d ',' -f 1)
        DIRNAME=$(dirname $FILEPATH)
        FILE=$(basename $FILEPATH)
        # Skip paths starting with ../ or containing dist-info or __pycache__
        if [[ $DIRNAME == ../* ]] || [[ $DIRNAME == *'dist-info'* ]] || [[ $DIRNAME == *'__pycache__'* ]]; then
            continue
        fi
        # Remove the PACKAGES_DIR prefix from the FILEPATH if it exists
        TEMP_DIRNAME=$(echo $DIRNAME | sed "s|$PACKAGES_DIR/||")
        mkdir -p $TEMP_DIR/$TEMP_DIRNAME 2>/dev/null
        # Create a symlink to the file in the temporary directory
        ln -s $PACKAGES_DIR/$DIRNAME/$FILE $TEMP_DIR/$TEMP_DIRNAME/$FILE
    done < $RECORD_FILE

    # Read the RECORD file in the temporary directory
    # remove paths starting with ../ or containing __pycache__ from the RECORD file
    RECORD_FILE="$TEMP_DIR/${PACKAGE_NAME}-${PACKAGE_VERSION}.dist-info/RECORD"
    while IFS= read -r line; do
        FILEPATH=$(echo $line | cut -d ',' -f 1)
        DIRNAME=$(dirname $FILEPATH)
        FILE=$(basename $FILEPATH)
        # remove paths starting with ../ or containing __pycache__
        if [[ $DIRNAME == ../* ]] || [[ $DIRNAME == *'__pycache__'* ]]; then
            sed -i "/$FILE/d" $RECORD_FILE
        fi
        # Update the path in the RECORD file to the temporary directory
        sed -i "s|$PACKAGES_DIR|$TEMP_DIR|g" $RECORD_FILE
    done < $RECORD_FILE

    # Read tags from the WHEEL file
    WHEEL_FILE_PATH="$TEMP_DIR/${PACKAGE_NAME}-${PACKAGE_VERSION}.dist-info/WHEEL"

    if [ ! -f "$WHEEL_FILE_PATH" ]; then
        echo "ERROR: WHEEL file not found at $WHEEL_FILE_PATH"
        exit 1
    fi

    TAGS=$(grep '^Tag:' $WHEEL_FILE_PATH | cut -d ' ' -f 2)

    # For each tag, determine if it's a multi-platform tag and generate a separate tag for each platform
    for TAG in $TAGS; do
        PYTHON_TAG=$(echo $TAG | cut -d '-' -f 1)
        ABI_TAG=$(echo $TAG | cut -d '-' -f 2)
        PLATFORM_TAG=$(echo $TAG | cut -d '-' -f 3)

        # Split the platform tag into multiple tags if it contains multiple platforms
        IFS='.' read -r -a PLATFORM_TAGS <<< "$PLATFORM_TAG"

        # If the platform tag contains multiple platforms, generate a separate tag for each platform
        if [ ${#PLATFORM_TAGS[@]} -gt 1 ]; then
            # Generate a separate tag for each platform
            for PLATFORM in ${PLATFORM_TAGS[@]}; do
                TAGS="$TAGS $PYTHON_TAG-$ABI_TAG-$PLATFORM"
            done

            # Remove the original tag
            TAGS=$(echo $TAGS | sed "s/$TAG//")
        fi
    done

    # Generate a wheel file for each tag
    for TAG in $TAGS; do
        echo "Generating wheel file for $PACKAGE_NAME with tag: $TAG"

        # Remove all tags from the WHEEL file
        sed -i '/^Tag:/d' $WHEEL_FILE_PATH

        # Add the current tag to the WHEEL file
        echo "Tag: $TAG" >> $WHEEL_FILE_PATH

        # Remove all blank lines from the WHEEL file
        sed -i '/^$/d' $WHEEL_FILE_PATH

        # Generate the wheel file
        python3 -m wheel pack $TEMP_DIR
        WHEEL_FILE=$(ls *.whl)
        mv $WHEEL_FILE $WHEEL_OUTPUT_DIR
    done

    # Remove the symlinked package contents
    echo "Removing symlinks for package contents in $TEMP_DIR"
    find $TEMP_DIR -type l -exec rm {} \;

    # # Remove the temporary directory
    echo "Cleaning up temporary directory: $TEMP_DIR"
    rm -rf $TEMP_DIR
}

main() {
    # Get the list of packages in site-packages via ${PACKAGES_DIR}/*.dist-info
    echo "Iterating over packages in $PACKAGES_DIR"
    for dist_info in ${PACKAGES_DIR}/*.dist-info; do
        PACKAGE_NAME=$(basename $dist_info | sed 's/-[0-9].*//')
        PACKAGE_VERSION=$(basename $dist_info | sed 's/.*-\([0-9].*\)\.dist-info/\1/')
        echo "Found package: $PACKAGE_NAME, version: $PACKAGE_VERSION"
    done

    # Ensure the output directory exists
    echo "Creating output directory: $WHEEL_OUTPUT_DIR"
    mkdir -p $WHEEL_OUTPUT_DIR

    # Iterate over each package in site-packages
    for dist_info in ${PACKAGES_DIR}/*.dist-info; do
        PACKAGE_NAME=$(basename $dist_info | sed 's/-[0-9].*//')
        PACKAGE_VERSION=$(basename $dist_info | sed 's/.*-\([0-9].*\)\.dist-info/\1/')

        echo "Processing package: $PACKAGE_NAME, version: $PACKAGE_VERSION"
        generate_wheels $PACKAGE_NAME $PACKAGE_VERSION
    done
}

main