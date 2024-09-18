using System.Collections.Generic;

namespace BmpFileProcessor
{
    public class Sample
    {
        public string Name { get; set; }
        public string Compound { get; set; }
        public string GUID { get; set; }
        public List<SampleImage> SampleImages { get; set; }
        public string StorageLocation { get; set; }

        // Additional properties and methods as needed
    }
}
