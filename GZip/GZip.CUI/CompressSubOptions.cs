﻿using CommandLine;

namespace Vbessonov.GZip.CUI
{
    internal class CompressSubOptions : CommonSubOptions
    {
        public const string VerbName = "compress";

        [Option("streams-count", Required = false, HelpText = "Number of gzip streams to create.")]
        public int? StreamsCount { get; set; }

        [Option("workers-count", Required = false, HelpText = "Number of threads using for compression.")]
        public int? WorkersCount { get; set; }
    }
}