﻿# Image Analysis API

## Overview

> [NOTE]
> **This project is experimental and is not intended for production use.**
> It is provided as-is, without any warranties or guarantees. Use at your own risk.

Welcome to the **Image Analysis API**, a robust and efficient service built with **C# .NET Core**. This API is designed to process image files and extract valuable metadata, providing a programmatic way to analyze various aspects of your images. Whether you're building a content management system, a photo organizer, or simply need to programmatically access image properties, this API offers a solid foundation.

---

## Features

* **Basic Metadata Extraction:** Retrieve essential metadata (like dimensions, file format, creation date, camera model, etc.) from uploaded image files.
* **Subject identification:** Identify subjects (people, place, animals, ect.) using AI

---

## Technologies Used

* **C#**: The core language for the API.
* **.NET Core**: The framework providing cross-platform capabilities and high performance.
* **ASP.NET Core**: For building the web API endpoints.
* **MetadataExtractor**: A powerful library for reading metadata from various image file formats.

---

## Getting Started

Follow these steps to get your local copy of the Image Analysis API up and running.

### Prerequisites

Before you begin, ensure you have the following installed:

* [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or newer recommended)
* [Git](https://git-scm.com/downloads)

### Installation

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/your-username/ImageAnalysisApi.git](https://github.com/your-username/ImageAnalysisApi.git)
    cd ImageProcessor
    ```
    *(Replace `your-username` with your actual GitHub username and `ImageAnalysisApi` with your repository name)*

2.  **Restore NuGet packages:**
    ```bash
    dotnet restore
    ```

3.  **Build the project:**
    ```bash
    dotnet build
    ```

### Running the API

You can run the API directly from the command line:

```bash
dotnet run --project ImageProcessor.Api
```

```bash
dotnet test
```

---

## Current Limitations

*   **No Persistent Storage**: The API currently operates without a database or any form of persistent storage. All data from processed images is held in-memory and will be lost when the application restarts.

---

## Roadmap

Here are some of the features and improvements planned for future releases:

- [ ] **Expanded Image Format Support**: Add support for more image formats like HEIC, WebP, and SVG.
- [ ] **Advanced Analysis**: Implement more advanced analysis features, such as:
    - Color palette extraction.
    - Object detection and recognition.
    - Facial recognition and analysis.
- [ ] **Cloud Storage Integration**: Allow processing images directly from cloud storage providers like AWS S3, Azure Blob Storage, or Google Cloud Storage.
- [ ] **Authentication and Authorization**: Add user authentication and authorization to secure the API endpoints.
- [ ] **Batch Processing**: Allow users to upload and process multiple images in a single request.

---