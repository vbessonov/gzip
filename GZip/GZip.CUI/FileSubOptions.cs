using CommandLine;

namespace VBessonov.GZip.CUI
{
    internal class FileSubOptions
    {
        [ValueOption(0)]
        public string InputFile { get; set; }

        [ValueOption(1)]
        public string OutputFile { get; set; }
    }
}
