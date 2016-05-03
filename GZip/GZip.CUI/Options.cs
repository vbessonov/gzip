using CommandLine;
using CommandLine.Text;

namespace Vbessonov.GZip.CUI
{
    internal class Options
    {
        [VerbOption(CompressSubOptions.VerbName, HelpText = "Compress the file.")]
        public CompressSubOptions CompressVerb { get; set; }

        [VerbOption(DecompressSubOptions.VerbName, HelpText = "Decompress the file.")]
        public DecompressSubOptions DecompressVerb { get; set; }

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
