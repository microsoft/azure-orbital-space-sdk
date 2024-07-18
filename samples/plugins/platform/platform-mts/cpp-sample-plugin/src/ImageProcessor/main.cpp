#include <iostream>
#include "ImageProcessor.h"

int main(int argc, char **argv)
{
    if (argc != 2)
    {
        std::cout << "Usage: " << argv[0] << " <ImagePath>" << std::endl;
        return -1;
    }

    std::string newImagePath = ImageProcessor::ProcessImage(argv[1]);
    if (newImagePath.empty())
    {
        std::cout << "Image processing failed." << std::endl;
        return -1;
    }
    else
    {
        std::cout << "Processed image saved to: " << newImagePath << std::endl;
        return 0;
    }
}