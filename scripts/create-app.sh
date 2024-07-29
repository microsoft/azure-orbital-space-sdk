#!/bin/bash
#
# Creates a new app or plugin from the starter templates with renames
#

set -e
############################################################
# Script variables
############################################################
OUTPUT_DIR=""
DEV_LANGUAGE="dotnet"
APP_NAME=""
OVERWRITE=false
REPO_ROOT=$(git rev-parse --show-toplevel)

############################################################
# Help                                                     #
############################################################
function show_help() {
   # Display Help
   echo "Create a new Microsoft Azure Orbital Space SDK payload app in a target directory."
   echo
   echo "Syntax: bash ./scripts/create-app.sh --output-dir <output-dir> --app-name <name of app> [--language python | dotnet] [--overwrite]"
   echo "options:"
   echo "--output-dir | -d                  [REQUIRED] Target directory to create the new app.  If the directory does not exist, it will be created."
   echo "--app-name | -n                    [REQUIRED] Name of your new app.  This will be used to name the app directory and the app itself."
   echo "--language | -l                    [OPTIONAL] Language of the new app.  Default is 'dotnet'.  Options are 'python' or 'dotnet'."
   echo "--overwrite                        [OPTIONAL] If the destination exists, overwrite it."
   echo "--help | -h                        [OPTIONAL] Help script (this screen)"
   echo
   exit 1
}


############################################################
# Process the input options.
############################################################
while [[ "$#" -gt 0 ]]; do
    case $1 in
        -n | --app-name)
            shift
            APP_NAME=$1
        ;;
        --overwrite)
            OVERWRITE=true
        ;;
        -d | --output-dir)
            shift
            OUTPUT_DIR=$1
            # Removing the trailing slash if there is one
            OUTPUT_DIR=${OUTPUT_DIR%/}
            ;;
        -l | --language)
            DEV_LANGUAGE=$1
            if [[ ! "${DEV_LANGUAGE}" == "dotnet" ]] && [[ ! "${DEV_LANGUAGE}" == "python" ]]; then
                echo "--language must be 'dotnet' or 'python'.  '${DEV_LANGUAGE}' is not valid."
                show_help
            fi
            ;;
        *) echo "Unknown parameter '$1'"; show_help ;;
    esac
    shift
done

if [[ -z "${APP_NAME}" ]]; then
    echo "--app-name is a required parameter and was not provided"
    show_help
fi

############################################################
# Helper function to duplicate a directory from source to destination
############################################################
function copy_directory_to_dest(){
  local dest_directory=""
  local src_directory=""

  while [[ "$#" -gt 0 ]]; do
        case $1 in
            --source-dir)
                shift
                src_directory=$1
                ;;
            --dest-dir)
                shift
                dest_directory=$1
                ;;
            *)
                echo "Unknown parameter '$1'"
                exit 1
                ;;
      esac
      shift
  done

  echo "Copying dotnet app from '${src_directory:?}' to '${dest_directory}'..."

  rsync -a --update \
            --exclude='/*.git' \
            --exclude='/*.log' \
            "${src_directory:?}/" "${dest_directory}/"

}

function scaffold_dotnet() {
    copy_directory_to_dest --source-dir "${REPO_ROOT}/samples/payloadapps/dotnet/starter-app" --dest-dir "${OUTPUT_DIR:?}/${APP_NAME:?}"
    copy_directory_to_dest --source-dir "${REPO_ROOT}/.devcontainer/app-dotnet-starter" --dest-dir "${OUTPUT_DIR:?}/${APP_NAME:?}/.devcontainer"

    echo "INFO: Updating .devcontainer/devcontainer.json with new app name..."
    sed -i "s/app-dotnet-starter/$APP_NAME/g" "${OUTPUT_DIR:?}/${APP_NAME:?}/.devcontainer/devcontainer.json"
    sed -i "s/dotnet-starter-app/$APP_NAME/g" "${OUTPUT_DIR:?}/${APP_NAME:?}/.devcontainer/devcontainer.json"

    echo "INFO: Updating .devcontainer/devcontainer.json workspace with new app name..."
    sed -i "s:samples/payloadapps/dotnet/starter-app:${APP_NAME}:g" "${OUTPUT_DIR:?}/${APP_NAME:?}/.devcontainer/devcontainer.json"

    sed -i "s/starter-app/$APP_NAME/g" "${OUTPUT_DIR:?}/${APP_NAME:?}/.devcontainer/devcontainer.json"


    echo "INFO: Renaming project files to new app name ${APP_NAME:?}..."
    mv "${OUTPUT_DIR:?}/${APP_NAME:?}/src/starter-app.csproj" "${OUTPUT_DIR:?}/${APP_NAME:?}/src/${APP_NAME:?}.csproj"
    mv "${OUTPUT_DIR:?}/${APP_NAME:?}/starter-app.sln" "${OUTPUT_DIR:?}/${APP_NAME:?}/${APP_NAME:?}.sln"

    echo "INFO: Updating project references with new app name ${APP_NAME:?}..."
    sed -i "s/starter-app/$APP_NAME/g" "${OUTPUT_DIR:?}/${APP_NAME:?}/${APP_NAME:?}.sln"

    sed -i "s/starter-app/$APP_NAME/g" "${OUTPUT_DIR:?}/${APP_NAME:?}/.vscode/launch.json"
    sed -i "s/starter-app/$APP_NAME/g" "${OUTPUT_DIR:?}/${APP_NAME:?}/.vscode/tasks.json"
    sed -i "s/starter-app/$APP_NAME/g" "${OUTPUT_DIR:?}/${APP_NAME:?}/.vscode/settings.json"

    sed -i "s/StarterApp/$APP_NAME/g" "${OUTPUT_DIR:?}/${APP_NAME:?}/src/Program.cs"

}

function scaffold_python() {
    echo "Copying dotnet app..."


    cp -r ./templates/dotnet/* "${OUTPUT_DIR:?}/${APP_NAME:?}"
}

function main() {
    echo "Creating new app in ${OUTPUT_DIR} using ${DEV_LANGUAGE} language..."

    # Create the directory if we need to and remove any existing files
    if [[ -d "${OUTPUT_DIR:?}/${APP_NAME:?}" ]]; then
        if [[ "${OVERWRITE}" == true ]]; then
            echo "WARN: the directory ${OUTPUT_DIR:?}/${APP_NAME:?} already exists.  Removing it."
            rm -rf "${OUTPUT_DIR:?}/${APP_NAME:?}"
        else
            echo "ERROR: the directory ${OUTPUT_DIR:?}/${APP_NAME:?} already exists.  Please remove it or choose a different name."
            show_help
        fi

    fi

    echo "INFO: creating directory '${OUTPUT_DIR:?}/${APP_NAME:?}'"
    mkdir -p "${OUTPUT_DIR:?}/${APP_NAME:?}"

    if [[ "${DEV_LANGUAGE:?}" == "dotnet" ]]; then
        scaffold_dotnet
    fi

    if [[ "${DEV_LANGUAGE:?}" == "python" ]]; then
        echo "This is not ready yet"
        exit 1
    fi


}


main

set +e
