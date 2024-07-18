#include <iostream>
#include "ImageProcessor.h"

int main(int argc, char **argv)
{
    if (argc != 3)
    {
        std::cout << "Usage: " << argv[0] << " <InputImagePath> <OutputImagePath>" << std::endl;
        return -1;
    }

    // Call ProcessImage with both input and output file paths
    std::string outputImagePath = ImageProcessor::ProcessImage(argv[1], argv[2]);
    if (outputImagePath.empty())
    {
        std::cout << "Image processing failed." << std::endl;
        return -1;
    }
    else
    {
        std::cout << "Processed image saved to: " << outputImagePath << std::endl;
        return 0;
    }
}