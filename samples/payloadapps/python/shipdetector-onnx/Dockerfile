# Stage 1: Build the application
FROM mcr.microsoft.com/cbl-mariner/base/python:3.9 as builder

WORKDIR /workspace/app-python-shipdetector-onnx

# Install necessary dependencies for building Python with SSL support
RUN tdnf install -y \
    binutils \
    bzip2-devel \
    ca-certificates \
    gawk \
    gcc \
    gcc-c++ \
    gdbm-devel \
    glibc-devel \
    kernel-headers \
    libffi-devel \
    make \
    ncurses-devel \
    nss-devel \
    openssl-devel \
    readline-devel \
    sqlite-devel \
    tar \
    wget \
    xz-devel \
    zlib-devel \
    && tdnf clean all

# Install OpenSSL
RUN tdnf install -y openssl \
    && tdnf clean all

# Download and install Python 3.10
RUN wget https://www.python.org/ftp/python/3.10.0/Python-3.10.0.tgz \
    && tar -xzf Python-3.10.0.tgz \
    && cd Python-3.10.0 \
    && ./configure --enable-shared --with-ensurepip=install --with-threads --enable-optimizations \
    && make -j$(nproc) \
    && make altinstall

# Remove the Python 3.10 source files and archives
RUN rm -rf Python-3.10.0 Python-3.10.0.tgz

# Set the library path
ENV LD_LIBRARY_PATH=/usr/local/lib:$LD_LIBRARY_PATH

# Create symbolic links for shared libraries
RUN ln -sf /usr/local/lib/libffi.so.8 /usr/local/lib/libffi.so

# Update the dynamic linker run-time bindings
RUN ldconfig

# Update alternatives to make Python 3.10 the default
RUN update-alternatives --install /usr/bin/python3 python3 /usr/local/bin/python3.9 1 \
    && update-alternatives --install /usr/bin/python3 python3 /usr/local/bin/python3.10 2 \
    && update-alternatives --set python3 /usr/local/bin/python3.10

# Remove the Python 3.10 test directory
RUN [ -d /usr/local/lib/python3.10/test ] && rm -rf /usr/local/lib/python3.10/test

# Install pip for Python 3.10
RUN python3 -m ensurepip --default-pip

# Ensure that the python3.10 version of pip is the default
RUN rm -f /usr/bin/pip \
    && update-alternatives --install /usr/bin/pip pip /usr/local/bin/pip3.9 1 \
    && update-alternatives --install /usr/bin/pip pip /usr/local/bin/pip3.10 2 \
    && update-alternatives --set pip /usr/local/bin/pip3.10

# Copy the application files to the container
COPY README.md /workspace/app-python-shipdetector-onnx/README.md
COPY .protos /workspace/app-python-shipdetector-onnx/.protos
COPY .wheel /workspace/app-python-shipdetector-onnx/.wheel
COPY src /workspace/app-python-shipdetector-onnx/src
COPY model /workspace/app-python-shipdetector-onnx/model
COPY pyproject.toml /workspace/app-python-shipdetector-onnx/pyproject.toml

# Copy the SSL certificate to the container
COPY ca.spacefx.local.crt /etc/pki/ca-trust/source/anchors/ca.spacefx.local.crt

# Update the CA certificates
RUN update-ca-trust

# Install poetry using pip with local repository configuration
RUN pip install --index-url https://registry.spacefx.local:8080 --trusted-host registry.spacefx.local poetry

# Add the local repository to the poetry configuration
RUN poetry config repositories.local https://registry.spacefx.local:8080

# Disable the default PyPI repository by setting it to an invalid URL
RUN poetry config repositories.pypi http://invalid-url

# Configure Poetry to create the virtual environment in the .venv directory
RUN poetry config virtualenvs.in-project true

# Configure poetry to use the local repository and install the package
RUN poetry install --no-interaction --no-ansi

# Stage 2: Create the final image
FROM ghcr.io/microsoft/azure-orbital-space-sdk/spacesdk-base-python:0.11.0_3.10 as final

WORKDIR /workspace/app-python-shipdetector-onnx

# Copy the installed application from the builder stage
COPY --from=builder /workspace/app-python-shipdetector-onnx /workspace/app-python-shipdetector-onnx

# Add python3 to the PATH
ENV PATH=/usr/local/bin:$PATH

# Run the application
ENTRYPOINT ["/usr/local/bin/python3.10", "src/app/main.py"]