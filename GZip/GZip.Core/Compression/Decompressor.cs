using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression
{
    public class Decompressor : CompressionAlgorithm, IDecompressor
    {
        private readonly DecompressorSettings _settings;

        public event CompressionCompletedEventHandler DecompressionCompleted;

        public DecompressorSettings Settings
        {
            get { return _settings; }
        }

        public Decompressor()
            : this(CreateDefaultSettings())
        {

        }

        public Decompressor(DecompressorSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("Settings must be non-empty");
            }

            _settings = settings;
        }

        private static DecompressorSettings CreateDefaultSettings()
        {
            return new DecompressorSettings();
        }

        protected override CompressionSettings GetSettings()
        {
            return _settings;
        }

        public void DecompressAsync(string inputFilePath, string outputFilePath)
        {
            DecompressAsync(inputFilePath, outputFilePath, null);
        }

        public void DecompressAsync(string inputFilePath, string outputFilePath, Action<CompressionCompletedEventArgs> callback)
        {
            DecompressAsync(inputFilePath, outputFilePath, callback, CancellationToken.None);
        }

        public void DecompressAsync(string inputFilePath, string outputFilePath, Action<CompressionCompletedEventArgs> callback, CancellationToken cancellationToken)
        {
            Process(
                inputFilePath,
                outputFilePath,
                (args) =>
                {
                    callback(args);
                },
                cancellationToken
            );
        }
    }
}
