#include "ImageProcessor.h"

#include <filesystem>
#include <iostream>

#include <opencv2/opencv.hpp>

namespace fs = std::filesystem;

// Expose ProcessImage with C linkage
extern "C"
{
    const char *ProcessImageC(const char *imagePath)
    {
        static std::string outputPath; // Static to persist the string
        outputPath = ImageProcessor::ProcessImage(imagePath);
        return outputPath.empty() ? nullptr : outputPath.c_str();
    }
}

namespace ImageProcessor
{
    std::string ProcessImage(const std::string &imagePath)
    {
        // Load an image from file
        cv::Mat image = cv::imread(imagePath);
        if (image.empty())
        {
            std::cout << "Could not open or find " << imagePath << std::endl;
            return "";
        }
        std::cout << "Loaded " << imagePath << std::endl;

        // Change the image to grayscale
        cv::Mat gray_image;
        cv::cvtColor(image, gray_image, cv::COLOR_BGR2GRAY);
        std::cout << "Converted image to grayscale" << std::endl;

        // Use histogram equalization to improve the contrast
        cv::Mat equalized_greyscale_image;
        cv::equalizeHist(gray_image, equalized_greyscale_image);
        std::cout << "Equalized the greyscale image" << std::endl;

        // Construct output filename from input filename
        fs::path inputPath(imagePath);
        fs::path outputPath = inputPath.parent_path() / ("equalized_greyscale_" + inputPath.filename().string());

        // Save the updated image to a file
        try
        {
            cv::imwrite(outputPath.string(), equalized_greyscale_image);
            std::cout << "Saved the equalized greyscale image to " << outputPath << std::endl;
            return outputPath.string();
        }
        catch (std::exception &e)
        {
            std::cout << "Could not save the image: " << e.what() << std::endl;
            return "";
        }
    }
}