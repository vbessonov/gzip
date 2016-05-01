using System;
using Vbessonov.GZip.CUI;
using VBessonov.GZip.Core.Compression;

namespace VBessonov.GZip.CUI
{
    public static class Program
    {
        private static int ParseCommandLine(
            Options options,
            string[] args,
            out string invokedVerb,
            out CommonSubOptions invokeSubOptions)
        {
            invokedVerb = null;
            invokeSubOptions = null;

            int resultCode = 1;
            string currentInvokedVerb = null;
            CommonSubOptions currentInvokedSubOptions = null;

            do
            {
                bool parsingResult = CommandLine.Parser.Default.ParseArguments(
                    args,
                    options,
                    (verb, subOptions) =>
                    {
                        currentInvokedVerb = verb;
                        currentInvokedSubOptions = subOptions as CommonSubOptions;
                    }
                );

                if (!parsingResult)
                {
                    break;
                }

                if (currentInvokedVerb == null ||
                    string.IsNullOrEmpty(currentInvokedSubOptions.InputFile) ||
                    string.IsNullOrEmpty(currentInvokedSubOptions.OutputFile))
                {
                    Console.WriteLine(options.GetUsage());
                    break;
                }

                invokedVerb = currentInvokedVerb;
                invokeSubOptions = currentInvokedSubOptions;
                resultCode = 0;
            } while (false);

            return resultCode;
        }

        private static int Compress(CompressSubOptions options)
        {
            int resultCode = 1;
            ICompressor compressor = new Compressor();

            if (options.WorkersCount.HasValue)
            {
                compressor.Settings.WorkersCount = options.WorkersCount.Value;
            }
            if (options.StreamsCount.HasValue)
            {
                compressor.Settings.Reader.Settings.StreamsCount = options.StreamsCount.Value;
            }
            if (options.ChunkSize.HasValue)
            {
                compressor.Settings.Reader.Settings.ChunkSize = options.ChunkSize.Value;
            }

            try
            {
                compressor.Compress(options.InputFile, options.OutputFile);
                resultCode = 0;

                Console.WriteLine("The file has been successfully compressed.");
            }
            catch (Exception exception)
            {
                Console.WriteLine("An unexpected error has occured: {0}", exception.Message);
            }

            return resultCode;
        }

        private static int Decompress(DecompressSubOptions options)
        {
            throw new NotImplementedException();
        }

        public static int Main(string[] args)
        {
            int resultCode = 0;
            Options options = new Options();
            string invokedVerb;
            CommonSubOptions invokedSubOptions;

            resultCode = ParseCommandLine(options, args, out invokedVerb, out invokedSubOptions);

            if (resultCode == 0)
            {
                switch (invokedVerb)
                {
                    case CompressSubOptions.VerbName:
                        resultCode = Compress((CompressSubOptions)invokedSubOptions);
                        break;

                    case DecompressSubOptions.VerbName:
                        resultCode = Decompress((DecompressSubOptions)invokedSubOptions);
                        break;

                    default:
                        Console.WriteLine(options.GetUsage());
                        resultCode = 1;
                        break;
                }
            }

            return resultCode;
        }
    }
}
