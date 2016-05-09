using CommandLine;

namespace VBessonov.GZip.CUI
{
    internal class CompressSubOptions : CommonSubOptions
    {
        public const string VerbName = "compress";

        [Option("streams-count", Required = false, HelpText = "Number of gzip streams to create.")]
        public int? StreamsCount { get; set; }

        [Option("multistream", Required = false, DefaultValue = false, HelpText = "Create multi-stream gzip compression.")]
        public bool CreateMultiStreamHeader { get; set; }
    }
}
