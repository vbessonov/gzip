using CommandLine;
using VBessonov.GZip.Core.Hash;

namespace Vbessonov.GZip.CUI
{
    internal class CompareSubOptions : FileSubOptions
    {
        public const string VerbName = "compare";

        [Option("hash-type", Required = false, HelpText = "Type of hash algorithm.")]
        public HashAlgorithmType? HashType { get; set; }
    }
}
