# Data Generators

The Azure Orbital Space SDK provides several data generators that you can use to develop and test your applications. They're also a great starting point for creating your own data generators for more specialized use cases.

A data generator is any component that produces data to be used for the development and test of payload applications. A data generator could be a web service, an FTP server, an Azure Blob container, or any other source of data that exposes an interface. VTH plugins encapsulate the logic necessary to interact with the data generator, keeping interactions generic elsewhere in the data pipeline. 

## Planetary Computer

The Azure Orbital Space SDK provides a data generator that integrates with [Planetary Computer](https://planetarycomputer.microsoft.com), Microsoft's multi-petabyte catalog of open environmental monitoring data. Planetary Computer houses a variety of current and historic Earth observational data. This data generator leverages [Planetary Computer's STAC API](https://planetarycomputer.microsoft.com/docs/quickstarts/reading-stac/) to easily query for geotiffs given latitude, longitude, and a broad array of additional query options to further refine results based on collections, time ranges, and assets.

<!-- TODO: Add link to data generator repo -->

## Image Provider

The Azure Orbital Space SDK Image Provider offers developers an out-of-the-box data generator for vending local image assets. Image Provider supports many data formats including geotiffs, PNGs, and JPEGs. Exposed as a simple web server exposing contents of a local directory, Image Provider offers a lightweight, flexible, and user-friendly starting point for creating custom data generators for sensor imagery.

<!-- TODO: Add link to data generator repo -->

## Star Viewer

The Azure Orbital Space SDK Star Viewer generates noised grey-scale images of star fields for a given line-of-sight vector. Built on the [Hipparcos-Yale-Gliese Star Catalogue](https://github.com/astronexus/HYG-Database), Star Viewer offers accurate synthetic stellar imagery for space-observation applications.

<!-- TODO: Add link to data generator repo -->

## Hello World

The Azure Orbital Space SDK Hello World data generator...

<!-- TODO: Finish this and add a link -->

## Temperature Sensor

The Azure Orbital Space SDK Temperature Sensor provides a lean example of a data generator for non-imagery data. A sensor is anything that captures a measurement. This could be a magnetometer, an altimeter, or accelerometer. In this data generator, a synthetic temperature sensor that returns a random is exposed.

<!-- TODO: Finish this and add a link -->