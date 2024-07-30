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

## Build the Image
1. Run `docker build --add-host registry.spacefx.local:127.0.0.1 --network=host -t shipdetector .`

## Run the app
1. Run `docker run --rm -it shipdetector:latest`