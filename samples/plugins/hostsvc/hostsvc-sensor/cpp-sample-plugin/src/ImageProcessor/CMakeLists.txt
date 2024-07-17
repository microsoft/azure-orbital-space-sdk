cmake_minimum_required(VERSION 3.10)
project(ImageProcessor)

# Set C++ standard
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF) # Optional: Do not use compiler-specific extensions

# Find packages
find_package(OpenCV REQUIRED)

# Include directories
include_directories(${OpenCV_INCLUDE_DIRS})

# Specify the directory where the source files are located
set(SOURCE_DIR "${CMAKE_CURRENT_SOURCE_DIR}/src")

# Add the shared library
add_library(${PROJECT_NAME}_lib SHARED
    # List your library source files here
    ImageProcessor.cpp
)

# Add the executable
add_executable(${PROJECT_NAME} main.cpp)

# Link the shared library with the OpenCV libraries
target_link_libraries(${PROJECT_NAME}_lib ${OpenCV_LIBS})

# Link the executable to the shared library and the OpenCV libraries
target_link_libraries(${PROJECT_NAME} ${PROJECT_NAME}_lib ${OpenCV_LIBS})