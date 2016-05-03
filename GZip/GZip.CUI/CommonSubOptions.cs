using CommandLine;

namespace Vbessonov.GZip.CUI
{
    internal class CommonSubOptions : FileSubOptions
    {
        [Option("available-memory", Required = false, HelpText = "Size of memory available for compression.")]
        public long? AvailableMemorySize { get; set; }

        [Option("chunk-size", Required = false, HelpText = "Size of a file chunk.")]
        public int? ChunkSize { get; set; }

        [Option("workers-count", Required = false, HelpText = "Number of threads used for compression.")]
        public int? WorkersCount { get; set; }
    }
}
