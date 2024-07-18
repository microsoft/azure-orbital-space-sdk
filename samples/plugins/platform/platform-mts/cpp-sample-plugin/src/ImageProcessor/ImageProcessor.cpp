#include "ImageProcessor.h"

#include <filesystem>
#include <iostream>

#include <opencv2/opencv.hpp>

namespace fs = std::filesystem;

// Expose ProcessImage with C linkage
extern "C"
{
    const char *ProcessImageC(const char *inputImagePath, const char *outputImagePath)
    {
        static std::string outputPath; // Static to persist the string
        outputPath = ImageProcessor::ProcessImage(inputImagePath, outputImagePath);
        return outputPath.empty() ? nullptr : outputPath.c_str();
    }
}

namespace ImageProcessor
{
    std::string ProcessImage(const std::string &inputImagePath, const std::string &outputImagePath)
    {
        // Load an image from file
        cv::Mat image = cv::imread(inputImagePath);
        if (image.empty())
        {
            std::cout << "Could not open or find " << inputImagePath << std::endl;
            return "";
        }
        std::cout << "Loaded " << inputImagePath << std::endl;

        // Change the image to grayscale
        cv::Mat gray_image;
        cv::cvtColor(image, gray_image, cv::COLOR_BGR2GRAY);
        std::cout << "Converted image to grayscale" << std::endl;

        // Use histogram equalization to improve the contrast
        cv::Mat equalized_greyscale_image;
        cv::equalizeHist(gray_image, equalized_greyscale_image);
        std::cout << "Equalized the greyscale image" << std::endl;

        // Save the updated image to the specified output file
        try
        {
            cv::imwrite(outputImagePath, equalized_greyscale_image);
            std::cout << "Saved the equalized greyscale image to " << outputImagePath << std::endl;
            return outputImagePath;
        }
        catch (std::exception &e)
        {
            std::cout << "Could not save the equalized greyscale image: " << e.what() << std::endl;
            return "";
        }
    }
}