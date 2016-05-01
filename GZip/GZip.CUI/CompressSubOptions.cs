using CommandLine;

namespace Vbessonov.GZip.CUI
{
    internal class CompressSubOptions : CommonSubOptions
    {
        public const string VerbName = "compress";

        [Option("streams-count", Required = false, HelpText = "Number of gzip streams to create.")]
        public int? StreamsCount { get; set; }

        [Option("workers-count", Required = false, HelpText = "Number of threads used for compression.")]
        public int? WorkersCount { get; set; }

        [Option("multistream", Required = false, DefaultValue = false, HelpText = "Create multi-stream gzip compression.")]
        public bool CreateMultiStreamHeader { get; set; }

        [Option("available-memory", Required = false, HelpText = "Size of memory available for compression.")]
        public long? AvailableMemorySize { get; set; }
    }
}
