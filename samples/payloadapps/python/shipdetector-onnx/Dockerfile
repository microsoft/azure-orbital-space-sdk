# Stage 0: Stage the application files
FROM shipdetector-app:testing as app


# Stage 1: Build the application
FROM ghcr.io/microsoft/azure-orbital-space-sdk/python-base:testing as builder
ENV DEBIAN_FRONTEND=noninteractive

# Copy the SSL certificate to the container
COPY ca.spacefx.local.crt /etc/pki/ca-trust/source/anchors/ca.spacefx.local.crt

# Update the CA certificates
RUN mkdir -p /tmp && chmod 1777 /tmp && update-ca-certificates

# Stage 1.5: Install the application
WORKDIR /workspace/app-python-shipdetector-onnx

# Copy the application files to the container
COPY --from=app /workspace/app-python-shipdetector-onnx /workspace/app-python-shipdetector-onnx

# Install poetry using pip with local repository configuration
RUN pip install --index-url https://registry.spacefx.local:8080 --trusted-host registry.spacefx.local poetry

# Add the local repository to the poetry configuration
RUN poetry config repositories.local https://registry.spacefx.local:8080

# Disable the default PyPI repository by setting it to an invalid URL
RUN poetry config repositories.pypi http://invalid-url

# Configure Poetry to create the virtual environment in the .venv directory
RUN poetry config virtualenvs.in-project true

# Configure poetry to use the local repository and install the package
RUN poetry install --no-interaction --no-ansi --compile


# Stage 2: Create the final image
FROM ghcr.io/microsoft/azure-orbital-space-sdk/spacesdk-base-python:testing as final

WORKDIR /workspace/app-python-shipdetector-onnx

# Copy the installed application from the builder stage
COPY --from=builder /workspace/app-python-shipdetector-onnx /workspace/app-python-shipdetector-onnx

# Run the application
ENTRYPOINT ["/workspace/app-python-shipdetector-onnx/.venv/bin/python", "/workspace/app-python-shipdetector-onnx/src/app/main.py"]