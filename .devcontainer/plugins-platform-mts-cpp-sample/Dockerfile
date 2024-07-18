# Use a temporary stage for compiling CMake from source if needed
FROM mcr.microsoft.com/devcontainers/dotnet:0-6.0 as cmake-builder
ARG REINSTALL_CMAKE_VERSION_FROM_SOURCE="none"
COPY ./reinstall-cmake.sh /tmp/reinstall-cmake.sh
RUN if [ "${REINSTALL_CMAKE_VERSION_FROM_SOURCE}" != "none" ]; then \
        apt-get update && export DEBIAN_FRONTEND=noninteractive \
        && apt-get -y install --no-install-recommends build-essential wget \
        && chmod +x /tmp/reinstall-cmake.sh \
        && /tmp/reinstall-cmake.sh ${REINSTALL_CMAKE_VERSION_FROM_SOURCE} \
        && rm -rf /var/lib/apt/lists/*; \
    else \
        echo "Using packaged CMake version"; \
    fi

# Start the final image
FROM mcr.microsoft.com/devcontainers/dotnet:0-6.0

# Copy CMake from the builder stage if it was compiled from source
COPY --from=cmake-builder /usr/local /usr/local

# Install OpenCV, g++, make, git, and potentially the packaged version of CMake
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get -y install --no-install-recommends libopencv-dev g++ gdb build-essential make git \
    && if [ -z "$(command -v cmake)" ]; then apt-get install -y cmake; fi \
    && rm -rf /var/lib/apt/lists/*
