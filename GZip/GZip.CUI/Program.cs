using System;
using VBessonov.GZip.CUI;
using VBessonov.GZip.Core.Compression;
using VBessonov.GZip.Core.Hash;
using System.Threading;
using System.Diagnostics;

namespace VBessonov.GZip.CUI
{
    public static class Program
    {
        private static int ParseCommandLine(
            Options options,
            string[] args,
            out string invokedVerb,
            out FileSubOptions invokeSubOptions)
        {
            invokedVerb = null;
            invokeSubOptions = null;

            int resultCode = 1;
            string currentInvokedVerb = null;
            FileSubOptions currentInvokedSubOptions = null;

            do
            {
                bool parsingResult = CommandLine.Parser.Default.ParseArguments(
                    args,
                    options,
                    (verb, subOptions) =>
                    {
                        currentInvokedVerb = verb;
                        currentInvokedSubOptions = subOptions as FileSubOptions;
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
            int resultCode = 0;
            ICompressor compressor = new Compressor();

            compressor.Settings.CreateMultiStream = options.CreateMultiStreamHeader;

            if (options.MinThreadsCount.HasValue)
            {
                compressor.Settings.MinWorkersCount = options.MinThreadsCount.Value;
            }
            if (options.MaxThreadsCount.HasValue)
            {
                compressor.Settings.MaxWorkersCount = options.MaxThreadsCount.Value;
            }
            if (options.AvailableMemorySize.HasValue)
            {
                compressor.Settings.AvailableMemorySize = options.AvailableMemorySize.Value;
            }
            if (options.StreamsCount.HasValue)
            {
                ((CompressorReaderSettings)compressor.Settings.Reader.Settings).StreamsCount = options.StreamsCount.Value;
            }
            if (options.ChunkSize.HasValue)
            {
                compressor.Settings.Reader.Settings.ChunkSize = options.ChunkSize.Value;
            }

            Exception error = null;
            bool completed = false;
            bool canceled = false;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress +=
                (sender, args) =>
                {
                    cancellationTokenSource.Cancel();

                    completed = true;
                    canceled = true;
                    args.Cancel = true;
                };

            Console.WriteLine("Compression has started. Press CTRL+C to interupt...");

            try
            {
                compressor.CompressAsync(
                    options.InputFile,
                    options.OutputFile,
                    (args) =>
                    {
                        completed = true;
                        canceled = args.Cancelled;
                        error = args.Error;
                    },
                    cancellationTokenSource.Token
                );

                while (!completed)
                {
                    Thread.Sleep(500);
                }

                if (error != null)
                {
                    PrintError(error);
                    resultCode = 1;
                }
                else if (canceled)
                {
                    Console.WriteLine("Compression has been interrupted");
                }
                else
                {
                    Console.WriteLine("The file has been successfully compressed");
                }
            }
            catch (Exception exception)
            {
                PrintError(exception);
            }

            return resultCode;
        }

        private static int Decompress(DecompressSubOptions options)
        {
            int resultCode = 0;
            IDecompressor decompressor = new Decompressor();

            if (options.MinThreadsCount.HasValue)
            {
                decompressor.Settings.MinWorkersCount = options.MinThreadsCount.Value;
            }
            if (options.MaxThreadsCount.HasValue)
            {
                decompressor.Settings.MaxWorkersCount = options.MaxThreadsCount.Value;
            }
            if (options.AvailableMemorySize.HasValue)
            {
                decompressor.Settings.AvailableMemorySize = options.AvailableMemorySize.Value;
            }
            if (options.ChunkSize.HasValue)
            {
                decompressor.Settings.Reader.Settings.ChunkSize = options.ChunkSize.Value;
            }
            if (options.DecompressionFactor.HasValue)
            {
                decompressor.Settings.DecompressionFactor = options.DecompressionFactor.Value;
            }
            if (options.MemoryLoadThreshold.HasValue)
            {
                decompressor.Settings.MemoryLoadThreshold = options.MemoryLoadThreshold.Value;
            }

            Exception error = null;
            bool completed = false;
            bool canceled = false;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress +=
                (sender, args) =>
                {
                    cancellationTokenSource.Cancel();

                    completed = true;
                    canceled = true;
                    args.Cancel = true;
                };

            Console.WriteLine("Decompression has started. Press CTRL+C to interupt...");

            try
            {
                decompressor.DecompressAsync(
                    options.InputFile,
                    options.OutputFile,
                    (args) =>
                    {
                        completed = true;
                        canceled = args.Cancelled;
                        error = args.Error;
                    },
                    cancellationTokenSource.Token
                );

                while (!completed)
                {
                    Thread.Sleep(500);
                }

                if (error != null)
                {
                    PrintError(error);
                    resultCode = 1;
                }
                else if (canceled)
                {
                    Console.WriteLine("Decompression has been interrupted");
                }
                else
                {
                    Console.WriteLine("The file has been successfully decompressed");
                }
            }
            catch (Exception exception)
            {
                PrintError(exception);
            }

            return resultCode;
        }

        private static int Compare(CompareSubOptions options)
        {
            int resultCode = 0;
            IComparer comparer = new Comparer();

            if (options.HashType.HasValue)
            {
                comparer.Settings.HashType = options.HashType.Value;
            }

            try
            {
                bool result = comparer.Compare(options.InputFile, options.OutputFile);

                if (result)
                {
                    Console.WriteLine("Files are the same");
                }
                else
                {
                    Console.WriteLine("Files are not the same");
                }
            }
            catch (Exception exception)
            {
                PrintError(exception);
            }

            return resultCode;
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            PrintError((Exception)args.ExceptionObject);
        }

        private static void PrintError(Exception error)
        {
            AggregateException aggregateException = error as AggregateException;

            if (aggregateException != null)
            {
                aggregateException = aggregateException.Flatten();

                if (aggregateException.InnerExceptions.Count == 1)
                {
                    PrintError(aggregateException.InnerException);
                }
                else
                {
                    Console.WriteLine("Unexpected errors have occured:");

                    foreach (Exception innerException in aggregateException.InnerExceptions)
                    {
                        Console.WriteLine(innerException.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("An unexpected error has occured: {0}", error.Message);
            }
        }

        public static int Main(string[] args)
        {
            Options options = new Options();
            string invokedVerb;
            FileSubOptions invokedSubOptions;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            int resultCode = ParseCommandLine(options, args, out invokedVerb, out invokedSubOptions);

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

                    case CompareSubOptions.VerbName:
                        resultCode = Compare((CompareSubOptions)invokedSubOptions);
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
