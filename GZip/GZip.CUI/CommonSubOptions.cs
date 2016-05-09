using CommandLine;

namespace VBessonov.GZip.CUI
{
    internal class CommonSubOptions : FileSubOptions
    {
        [Option("available-memory", Required = false, HelpText = "Size of memory available for compression.")]
        public long? AvailableMemorySize { get; set; }

        [Option("chunk-size", Required = false, HelpText = "Size of a file chunk.")]
        public int? ChunkSize { get; set; }

        [Option("min-threads-count", Required = false, HelpText = "Minimum number of threads in thread pool.")]
        public int? MinThreadsCount { get; set; }

        [Option("max-threads-count", Required = false, HelpText = "Maximum number of threads in thread pool.")]
        public int? MaxThreadsCount { get; set; }
    }
}
