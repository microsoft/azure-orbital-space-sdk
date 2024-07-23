#ifndef IMAGEPROCESSOR_H
#define IMAGEPROCESSOR_H

#include <string>

namespace ImageProcessor
{
    std::string ProcessImage(const std::string &inputImagePath, const std::string &outputImagePath);
}

#endif // IMAGEPROCESSOR_H