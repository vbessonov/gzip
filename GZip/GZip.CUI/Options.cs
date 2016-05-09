using CommandLine;
using CommandLine.Text;

namespace VBessonov.GZip.CUI
{
    internal class Options
    {
        [VerbOption(CompressSubOptions.VerbName, HelpText = "CompressAsync the file.")]
        public CompressSubOptions CompressVerb { get; set; }

        [VerbOption(DecompressSubOptions.VerbName, HelpText = "DecompressAsync the file.")]
        public DecompressSubOptions DecompressVerb { get; set; }

        [VerbOption(CompareSubOptions.VerbName, HelpText = "Compare two files.")]
        public CompareSubOptions CompareVerb { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            HelpText helpText = HelpText.AutoBuild(this, verb);

            helpText.AddDashesToOption = false;

            return helpText;
        }
    }
}
