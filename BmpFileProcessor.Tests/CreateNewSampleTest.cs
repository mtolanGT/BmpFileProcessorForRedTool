using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BmpFileProcessor; // Reference to your main project's namespace

namespace BmpFileProcessor.Tests
{
    [TestClass]
    public class SampleProcessingTests
    {
        private SampleProcessing _processor;
        private string _testDirectory;

        [TestInitialize]
        public void Setup()
        {
            _processor = new SampleProcessing();

            // Set up a test directory in the system's temporary folder
            _testDirectory = Path.Combine(Path.GetTempPath(), "BmpFileProcessorTest");
            Directory.CreateDirectory(_testDirectory);
        }

        [TestCleanup]
        public void TearDown()
        {
            // Clean up the test directory after each test
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }

        [TestMethod]
        public void ExtractCompoundFromFilename_WithValidFilename_ReturnsCompound()
        {
            string fileName = "CompoundA_Sample1_Image";
            string expected = "CompoundA";

            string result = _processor.ExtractCompoundFromFilename(fileName);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ExtractCompoundFromFilename_WithNoUnderscores_ReturnsEmptyString()
        {
            string fileName = "Image";
            string expected = string.Empty;

            string result = _processor.ExtractCompoundFromFilename(fileName);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ExtractCompoundFromFilename_WithEmptyFilename_ReturnsEmptyString()
        {
            string fileName = "";
            string expected = string.Empty;

            string result = _processor.ExtractCompoundFromFilename(fileName);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CreateNewSample_CreatesSampleCorrectly()
        {
            Sample sample = new Sample
            {
                GUID = Guid.NewGuid().ToString()
            };

            // Modify the method to accept a base directory for testing
            _processor.CreateNewSample(sample, _testDirectory);

            string expectedDirectory = Path.Combine(_testDirectory, sample.GUID);

            Assert.IsTrue(Directory.Exists(expectedDirectory));
            Assert.AreEqual(expectedDirectory, sample.StorageLocation);
        }

        [TestMethod]
        public void ProcessBmpFiles_RenamesFilesCorrectly()
            // TODO: I keep getting access to the path ... is denied error despite being connected to VPN. 
            // Not sure what is going on here. Will visit later
        {
            // Create test BMP files
            string[] testFiles = new string[]
            {
        "CompoundA_Sample1_Image.bmp",
        "CompoundB_Sample2_Image.bmp",
        "InvalidFile.bmp"
            };

            foreach (var file in testFiles)
            {
                string filePath = Path.Combine(_testDirectory, file);
                File.WriteAllText(filePath, "Test Content"); // Create a dummy file
            }

            // Run the method
            _processor.ProcessBmpFiles(_testDirectory);

            // Verify that files are renamed
            var filesAfterProcessing = Directory.GetFiles(_testDirectory, "*.bmp");

            // Update the expected file count to 2
            Assert.AreEqual(2, filesAfterProcessing.Length);

            foreach (var filePath in filesAfterProcessing)
            {
                string fileName = Path.GetFileName(filePath);

                // Check that the file name follows the naming convention
                StringAssert.Matches(fileName, new Regex(@"^[\w\-]{36}_\w+\.bmp$"));

                // Extract GUID and compound
                string[] parts = fileName.Split('_');
                Assert.AreEqual(2, parts.Length);

                string guidPart = parts[0];
                string compoundPart = Path.GetFileNameWithoutExtension(parts[1]);

                Assert.IsTrue(Guid.TryParse(guidPart, out _));
                Assert.IsFalse(string.IsNullOrEmpty(compoundPart));
            }
        }
        [TestMethod]
        public void ProcessBmpFiles_SkipsFilesWithEmptyCompound()
        {
            // Create a BMP file with an invalid name format
            string invalidFileName = "InvalidFile.bmp";
            string filePath = Path.Combine(_testDirectory, invalidFileName);
            File.WriteAllText(filePath, "Test Content");

            // Run the method
            _processor.ProcessBmpFiles(_testDirectory);

            // Verify that the invalid file remains unchanged
            var filesAfterProcessing = Directory.GetFiles(_testDirectory, "*.bmp");
            Assert.AreEqual(1, filesAfterProcessing.Length);

            string processedFileName = Path.GetFileName(filesAfterProcessing[0]);
            Assert.AreEqual("InvalidFile.bmp", processedFileName);
        }


        [TestMethod]
        public void ProcessBmpFiles_WithNoBmpFiles_DoesNothing()
        {
            // Directory is empty
            _processor.ProcessBmpFiles(_testDirectory);

            var filesAfterProcessing = Directory.GetFiles(_testDirectory);
            Assert.AreEqual(0, filesAfterProcessing.Length);
        }

    }
}
