# Instructions for Building Shipdetector-ONNX from a Multi-Stage Image from the Core-Registry PyPI Server

## Generate the wheels from ship-detector-onnx
1. Rebuild the `shipdetector-onnx` devcontainer
1. Run `generate-wheels-from-packages.sh`

## Transfer Wheels to azure-orbital-space-sdk-setup
1. Move the wheels from `shipdetector-onnx/dist` into `spacedev_cache/tmp`
1. Open the `azure-orbital-space-sdk-setup` repo
1. Move the wheels from `spacedev_cache/tmp` into the `wheel` folder

## Stage poetry and its dependent wheels into azure-orbital-space-sdk-setup
1. Open azure-orbital-space-sdk-setup
1. Run `pip download --dest ./wheel poetry` download the wheels into the `wheel` folder

## Copy to spacedev
1. Open azure-orbital-space-sdk-setup
1. Run `./.vscode/copy-to-spacedev.sh`

## Stage SpaceFX
1. Run `/var/spacedev/scripts/stage_spacefx.sh`

## Deploy Core-Registry 
1. Run `/var/spacedev/scripts/deploy_spacefx.sh` or `/var/spacedev/scripts/core-registry.sh --start`

## Download SpaceFX CA Cert
1. Open `shipdetector-onnx` in SSH
1. Run `cp /usr/local/share/ca-certificates/ca.spacefx.local/ca.spacefx.local.crt .` to get the CA cert for the PyPI registry locally so that it can be built into the docker image.

## Build the python-base image
1. Open azure-orbital-space-sdk-setup
1. Run `docker build --build-arg PYTHON_VERSION=3.10 -t ghcr.io/microsoft/azure-orbital-space-sdk/python-base:testing -f build/python/Dockerfile.python-base .`

## Build the spacesdk-base-python image
1. Open azure-orbital-space-sdk-setup
1. Run `docker build -t ghcr.io/microsoft/azure-orbital-space-sdk/spacesdk-base-python:testing -f build/python/Dockerfile.python-spacesdk-base .`

## Build the Application Files Image
1. Run `docker build -t shipdetector-app:testing -f Dockerfile.app .`

## Build the Image
1. Run `docker build --add-host registry.spacefx.local:127.0.0.1 --network=host -t shipdetector:testing .`

## Push the image to coresvc-registry
1. Run `docker tag shipdetector:testing registry.spacefx.local:5000/shipdetector:testing`
1. RUn `docker push registry.spacefx.local:5000/shipdetector:testing`

## Run the app
1. `sudo cp schedules/debug_image/sample-app-shipdetector-onnx.yaml /var/spacedev/xfer/platform-deployment/inbox/schedule/`
1. `sudo cp schedules/debug_image/app-config.json /var/spacedev/xfer/platform-deployment/inbox/schedule/`
1. `sudo cp model/* /var/spacedev/xfer/platform-deployment/inbox/schedule/`
1. `sudo cp schedules/debug_image/sample-app-shipdetector-onnx.json /var/spacedev/xfer/platform-deployment/inbox/schedule/`