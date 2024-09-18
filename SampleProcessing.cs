using System;
using System.Collections.Generic;
using System.IO;

namespace BmpFileProcessor
{
    public class SampleProcessing
    {
        // Function to process BMP files from a specified directory.
        public void ProcessBmpFiles(string directory)
        {
            // Loop through each BMP file in the specified directory.
            foreach (string filePath in Directory.EnumerateFiles(directory, "*.bmp"))
            {
                // Extract the name of the file without its extension
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                // Extract the compound information from the file name
                string compound = ExtractCompoundFromFilename(fileName);

                // Generate a new GUID for the sample
                string sampleGUID = Guid.NewGuid().ToString();

                // Create a new sample and set its properties
                Sample sample = new Sample
                {
                    Name = fileName,
                    Compound = compound,
                    GUID = sampleGUID
                };

                // Perform any sample initialization or processing as needed
                CreateNewSample(sample);

                // Generate the new file name using the naming convention
                string newFileName = $"{sampleGUID}_{compound}.bmp";

                // Build the new file path
                string newFilePath = Path.Combine(directory, newFileName);

                // Rename the file
                File.Move(filePath, newFilePath);

                // Additional processing can be added here
            }
        }

        // Extract the compound from the filename
        public string ExtractCompoundFromFilename(string fileName)
        {
            // Split the fileName string into an array of substrings, using the underscore '_' as the delimiter.
            string[] elements = fileName.Split('_');

            // Check if the resulting array contains fewer than 1 element.
            if (elements.Length < 1)
            {
                return string.Empty;
            }

            // Return the first element, which is assumed to represent the compound information.
            return elements[0];
        }

        // Create a new sample
        public void CreateNewSample(Sample sample)
        {
            // Initialize the sample's image collection
            sample.SampleImages = new List<SampleImage>();

            // Create the directory path for storing images related to this sample.
            // TODO: Swap out string concatenation to string interpolation
            // Original:
            // string directory = @"\\gtweed.corp\datashare\Manufacturing\Cognex\Samples\" + sample.GUID;

            // Using string interpolation
            string directory = $@"\\gtweed.corp\datashare\Manufacturing\Cognex\Samples\{sample.GUID}";

            // Create the directory on the file system.
            var directoryInfo = Directory.CreateDirectory(directory);

            // Check if the directory was successfully created.
            if (directoryInfo.Exists)
            {
                // Assign the path as the storage location for the images in this sample.
                sample.StorageLocation = directory;
            }
        }
    }
}
