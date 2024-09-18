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
            foreach (string filePath in Directory.EnumerateFiles(directory, "*.bmp"))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string compound = ExtractCompoundFromFilename(fileName);

                // Skip files with empty compound
                if (string.IsNullOrEmpty(compound))
                {
                    continue; // Skip processing this file
                }

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
            }
        }

        // Extract the compound from the filename
        public string ExtractCompoundFromFilename(string fileName)
        {
            // Split the fileName string into an array of substrings, using the underscore '_' as the delimiter.
            string[] elements = fileName.Split('_');

            // Check if the resulting array contains fewer than 3 elements.
            // If it does, the filename does not meet the expected format, so return an empty string.
            if (elements.Length < 3)
            {
                return string.Empty;
            }

            // Return the first element, which is assumed to represent the compound information.
            return elements[0];
        }


        // Create a new sample
        public void CreateNewSample(Sample sample, string baseDirectory = @"\\gtweed.corp\datashare\Manufacturing\Cognex\Samples\")
        {
            sample.SampleImages = new List<SampleImage>();

            // Use Path.Combine and string interpolation
            string directory = Path.Combine(baseDirectory, sample.GUID);

            var directoryInfo = Directory.CreateDirectory(directory);

            if (directoryInfo.Exists)
            {
                sample.StorageLocation = directory;
            }
        }

    }
}
