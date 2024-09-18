#!/bin/bash

# If we're running in the devcontainer with the k3s-on-host feature, source the .env file
[[ -f "/devfeature/k3s-on-host/.env" ]] && source /devfeature/k3s-on-host/.env

# Pull in the app.env file built by the feature
[[ -n "${SPACEFX_DEV_ENV}" ]] && [[ -f "${SPACEFX_DEV_ENV}" ]] && source "${SPACEFX_DEV_ENV:?}"

source "${SPACEFX_DIR:?}/modules/load_modules.sh" $@ --log_dir "${SPACEFX_DIR:?}/logs/${APP_NAME:?}"


function move_protos() {
    info_log "START: ${FUNCNAME[0]}"

    run_a_script "mkdir -p ${CONTAINER_WORKING_DIR}/.protos/datagenerator/planetary_computer"
    # Copy the .proto file to the target directory
    run_a_script "${SPACEFX_DIR}/protos/datagenerator/planetary-computer/PlanetaryComputer.proto ${CONTAINER_WORKING_DIR}/.protos/datagenerator/planetary_computer/PlanetaryComputer.proto"

    info_log "END: ${FUNCNAME[0]}"
}


function load_model(){
    info_log "START: ${FUNCNAME[0]}"

    # Load the model
    run_a_script "mkdir -p ${CONTAINER_WORKING_DIR}/phi-3-vision-128k-instruct-onnx-cpu"

    MODEL_FOR_DOWNLOAD="microsoft/Phi-3-vision-128k-instruct-onnx-cpu"

    # Download the model
    run_a_script "huggingface-cli download $MODEL_FOR_DOWNLOAD --local-dir ${CONTAINER_WORKING_DIR}/phi-3-vision-128k-instruct-onnx-cpu"

    info_log "END: ${FUNCNAME[0]}"
}


function main(){
    info_log "START: ${FUNCNAME[0]}"

    move_protos
    load_model

    info_log "END: ${FUNCNAME[0]}"
}

main


