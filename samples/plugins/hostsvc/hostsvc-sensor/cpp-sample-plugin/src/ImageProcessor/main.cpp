#include <iostream>
#include "ImageProcessor.h"

int main(int argc, char** argv) {
    if(argc != 2) {
        std::cout << "Usage: " << argv[0] << " <ImagePath>" << std::endl;
        return -1;
    }

    return ImageProcessor::ProcessImage(argv[1]);
}