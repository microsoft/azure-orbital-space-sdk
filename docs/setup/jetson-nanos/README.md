# Install the nvidia container runtime and CUDA runtime to the Jetson Nano
>Note: this assumes the jetson nano is connected to the internet.  Air-gapped installation of `nvidia-container-runtime` and `cuda-runtime` are beyond the scope of this document
```bash
source /etc/os-release

# Calculate the version and distro name for the keyring download
cuda_distro="${VERSION_ID//./}"
cuda_distro="${ID}${cuda_distro}"

cuda_arch="$(uname -m)"
[[ "${cuda_arch}" == "aarch64" ]] && cuda_arch="arm64"

wget https://developer.download.nvidia.com/compute/cuda/repos/${cuda_distro}/${cuda_arch}/cuda-keyring_1.1-1_all.deb
sudo dpkg -i cuda-keyring_1.1-1_all.deb
sudo apt-get update

sudo apt install -y nvidia-container-runtime cuda-runtime-11-4
```

The `deviceQuery` in this README are references to the [nVidia CUDA deviceQuery] (https://github.com/NVIDIA/cuda-samples/tree/master/Samples/1_Utilities/deviceQuery) in the cuda samples repo.  Container images with pre-built copies of deviceQuery are available via
* [ghcr.io/microsoft/azure-orbital-space-sdk/spacesdk-jetson-devicequery:cuda-11.4-nightly](https://github.com/microsoft/azure-orbital-space-sdk-core/pkgs/container/azure-orbital-space-sdk%2Fspacesdk-jetson-devicequery) ~300MB
* [ghcr.io/microsoft/azure-orbital-space-sdk/spacesdk-jetson-devicequery:cuda-11.4-dev-nightly](https://github.com/microsoft/azure-orbital-space-sdk-core/pkgs/container/azure-orbital-space-sdk%2Fspacesdk-jetson-devicequery) ~2.5 GB
* [ghcr.io/microsoft/azure-orbital-space-sdk/spacesdk-jetson-devicequery:cuda-12.2-nightly](https://github.com/microsoft/azure-orbital-space-sdk-core/pkgs/container/azure-orbital-space-sdk%2Fspacesdk-jetson-devicequery) ~300MB
* [ghcr.io/microsoft/azure-orbital-space-sdk/spacesdk-jetson-devicequery:cuda-12.2-dev-nightly](https://github.com/microsoft/azure-orbital-space-sdk-core/pkgs/container/azure-orbital-space-sdk%2Fspacesdk-jetson-devicequery) ~2.5 GB

Images with `-dev` suffix have the cuda drivers installed in the image and are intended for testing and validation on the ground.  They are not intended for deployment to production hardware.


# Test and Validate the Azure Orbital Space SDK can run GPU accelerated workloads
 >Note: The Azure Orbital Space SDK staging requires Docker to be installed.  This step also assumes you have already logged into the Azure Orbital Space SDK repo at https://github.com/microsoft/azure-orbital-space-sdk-setup

1. Clone the repo
	```bash
	git clone git@github.com:microsoft/azure-orbital-space-sdk-setup.git
	git checkout -b nvidia_gpu_for_jetson origin/nvidia_gpu_for_jetson
	cd azure-orbital-space-sdk-setup
	```

1. Build /var/spacedev to include the nvidia plugin and test container
	```bash
	${PWD}/.vscode/copy_to_spacedev.sh
	/var/spacedev/scripts/stage_spacefx.sh --container spacesdk-jetson-devicequery:cuda-11.4-nightly --nvidia-gpu-plugin
	```

1. Deploy the Azure Orbital Space SDK
	```bash
	/var/spacedev/scripts/deploy_spacefx.sh
	```

1. Validate the nvidia GPU has been recognized by k3s
	```bash
	sudo grep nvidia /var/lib/rancher/k3s/agent/etc/containerd/config.toml
	```

1. Deploy the deviceQuery image tool
	```bash
    cat <<EOF | kubectl apply -f -
    ---
    apiVersion: v1
    kind: Pod
    metadata:
      name: nvidia-device-query
      namespace: default
    spec:
      restartPolicy: OnFailure
      runtimeClassName: nvidia
      containers:
      - name: nvidia-device-query
        image: registry.spacefx.local:5000/spacesdk-jetson-devicequery:cuda-11.4-nightly
        resources:
          limits:
            nvidia.com/gpu: 1
        env:
        - name: NVIDIA_VISIBLE_DEVICES
          value: all
        - name: NVIDIA_DRIVER_CAPABILITIES
          value: all
    EOF
	```

1. Pod may take a few moments to boot due to image pull and configuration update.  Give it a wait and then pull the logs
	```bash
    spacecowboy@jetson-nano:~$ kubectl wait --for=condition=complete pod/nvidia-device-query --timeout=300s && kubectl logs nvidia-device-query

	/usr/local/bin/deviceQuery Starting...

	CUDA Device Query (Runtime API) version (CUDART static linking)

	Detected 1 CUDA Capable device(s)

	Device 0: "Xavier"
	  CUDA Driver Version / Runtime Version          11.4 / 11.4
	  CUDA Capability Major/Minor version number:    7.2
	  Total amount of global memory:                 31002 MBytes (32508235776 bytes)
	  (008) Multiprocessors, (064) CUDA Cores/MP:    512 CUDA Cores
	  GPU Max Clock rate:                            1377 MHz (1.38 GHz)
	  Memory Clock rate:                             675 Mhz
	  Memory Bus Width:                              256-bit
	  L2 Cache Size:                                 524288 bytes
	  Maximum Texture Dimension Size (x,y,z)         1D=(131072), 2D=(131072, 65536), 3D=(16384, 16384, 16384)
	  Maximum Layered 1D Texture Size, (num) layers  1D=(32768), 2048 layers
	  Maximum Layered 2D Texture Size, (num) layers  2D=(32768, 32768), 2048 layers
	  Total amount of constant memory:               65536 bytes
	  Total amount of shared memory per block:       49152 bytes
	  Total shared memory per multiprocessor:        98304 bytes
	  Total number of registers available per block: 65536
	  Warp size:                                     32
	  Maximum number of threads per multiprocessor:  2048
	  Maximum number of threads per block:           1024
	  Max dimension size of a thread block (x,y,z): (1024, 1024, 64)
	  Max dimension size of a grid size    (x,y,z): (2147483647, 65535, 65535)
	  Maximum memory pitch:                          2147483647 bytes
	  Texture alignment:                             512 bytes
	  Concurrent copy and kernel execution:          Yes with 1 copy engine(s)
	  Run time limit on kernels:                     No
	  Integrated GPU sharing Host Memory:            Yes
	  Support host page-locked memory mapping:       Yes
	  Alignment requirement for Surfaces:            Yes
	  Device has ECC support:                        Disabled
	  Device supports Unified Addressing (UVA):      Yes
	  Device supports Managed Memory:                Yes
	  Device supports Compute Preemption:            Yes
	  Supports Cooperative Kernel Launch:            Yes
	  Supports MultiDevice Co-op Kernel Launch:      Yes
	  Device PCI Domain ID / Bus ID / location ID:   0 / 0 / 0
	  Compute Mode:
		 < Default (multiple host threads can use ::cudaSetDevice() with device simultaneously) >

	deviceQuery, CUDA Driver = CUDART, CUDA Driver Version = 11.4, CUDA Runtime Version = 11.4, NumDevs = 1
	Result = PASS
	```

# Test GPU with deviceQuery in Docker (Optional)
The Jetson Nano GPU workload can be tested using Docker to validate the configuration and drivers are installed for the jetson nano.  Testing in Docker allows quicker iteration and retests by not requiring a full Azure Orbital Space SDK cluster to be deployed every time.  This is intended to be for a ground-based installation.

TODO: Add link to docker installation steps

1. Enable the docker daemon to leverage the GPU through the nvidia-ctk cli
	```bash
	# Configure docker daemon to enable nvidia GPU runtime
	sudo nvidia-ctk runtime configure --runtime=docker
	sudo systemctl restart docker
	```

1. Login and run the deviceQuery nvidia application to test and validate it works
	```bash
	docker run --rm \
       --runtime=nvidia \
       -v /usr/local/cuda-11.4:/usr/local/cuda-11.4 \
       ghcr.io/microsoft/azure-orbital-space-sdk/spacesdk-jetson-devicequery:cuda-11.4-nightly
	```

1. Output will list the device and cuda version running.  You should see the final line `RESULT = PASS` if all is functioning correctly
	```bash
	/usr/local/bin/deviceQuery Starting...

	CUDA Device Query (Runtime API) version (CUDART static linking)

	Detected 1 CUDA Capable device(s)

	Device 0: "Xavier"
	  CUDA Driver Version / Runtime Version          11.4 / 11.4
	  CUDA Capability Major/Minor version number:    7.2
	  Total amount of global memory:                 31002 MBytes (32508235776 bytes)
	  (008) Multiprocessors, (064) CUDA Cores/MP:    512 CUDA Cores
	  GPU Max Clock rate:                            1377 MHz (1.38 GHz)
	  Memory Clock rate:                             675 Mhz
	  Memory Bus Width:                              256-bit
	  L2 Cache Size:                                 524288 bytes
	  Maximum Texture Dimension Size (x,y,z)         1D=(131072), 2D=(131072, 65536), 3D=(16384, 16384, 16384)
	  Maximum Layered 1D Texture Size, (num) layers  1D=(32768), 2048 layers
	  Maximum Layered 2D Texture Size, (num) layers  2D=(32768, 32768), 2048 layers
	  Total amount of constant memory:               65536 bytes
	  Total amount of shared memory per block:       49152 bytes
	  Total shared memory per multiprocessor:        98304 bytes
	  Total number of registers available per block: 65536
	  Warp size:                                     32
	  Maximum number of threads per multiprocessor:  2048
	  Maximum number of threads per block:           1024
	  Max dimension size of a thread block (x,y,z): (1024, 1024, 64)
	  Max dimension size of a grid size    (x,y,z): (2147483647, 65535, 65535)
	  Maximum memory pitch:                          2147483647 bytes
	  Texture alignment:                             512 bytes
	  Concurrent copy and kernel execution:          Yes with 1 copy engine(s)
	  Run time limit on kernels:                     No
	  Integrated GPU sharing Host Memory:            Yes
	  Support host page-locked memory mapping:       Yes
	  Alignment requirement for Surfaces:            Yes
	  Device has ECC support:                        Disabled
	  Device supports Unified Addressing (UVA):      Yes
	  Device supports Managed Memory:                Yes
	  Device supports Compute Preemption:            Yes
	  Supports Cooperative Kernel Launch:            Yes
	  Supports MultiDevice Co-op Kernel Launch:      Yes
	  Device PCI Domain ID / Bus ID / location ID:   0 / 0 / 0
	  Compute Mode:
		 < Default (multiple host threads can use ::cudaSetDevice() with device simultaneously) >

	deviceQuery, CUDA Driver = CUDART, CUDA Driver Version = 11.4, CUDA Runtime Version = 11.4, NumDevs = 1
	Result = PASS

	```

1. Clean up and free used space
	# Clean up to free up space
	docker system prune --all --force --volumes
	```
