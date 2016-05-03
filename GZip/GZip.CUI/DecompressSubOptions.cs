using CommandLine;

namespace Vbessonov.GZip.CUI
{
    internal class DecompressSubOptions : CommonSubOptions
    {
        public const string VerbName = "decompress";

        [Option("decompression-factor", Required = false, HelpText = "Decompression factor.")]
        public int? DecompressionFactor { get; set; }

        [Option("memory-threshold", Required = false, HelpText = "Memory load threshold.")]
        public int? MemoryLoadThreshold { get; set; }
    }
}
