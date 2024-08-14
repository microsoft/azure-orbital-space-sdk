FROM scratch

WORKDIR /workspace/app-python-shipdetector-onnx

COPY README.md /workspace/app-python-shipdetector-onnx/README.md
COPY .protos /workspace/app-python-shipdetector-onnx/.protos
COPY .wheel /workspace/app-python-shipdetector-onnx/.wheel
COPY src /workspace/app-python-shipdetector-onnx/src
COPY pyproject.toml /workspace/app-python-shipdetector-onnx/pyproject.toml