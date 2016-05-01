using CommandLine;

namespace Vbessonov.GZip.CUI
{
    internal class CommonSubOptions : FileSubOptions
    {
        [Option("chunk-size", Required = false, HelpText = "Size of a file chunk.")]
        public int? ChunkSize { get; set; }
    }
}
