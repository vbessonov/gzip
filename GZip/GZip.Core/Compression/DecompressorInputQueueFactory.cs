using System;
using System.Collections.Generic;
using System.IO;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression
{
    internal class DecompressorInputQueueFactory : InputQueueFactory
    {
        public DecompressorInputQueueFactory(CompressionSettings compressionSettings)
            : base(compressionSettings)
        {

        }

        protected override long CalculateRequiredForCompressionStreamMemorySize(IEnumerable<InputStream> inputStreams, FileInfo inputFileInfo)
        {
            DecompressorSettings decompressorSettings = CompressionSettings as DecompressorSettings;

            if (decompressorSettings == null)
            {
                throw new ArgumentException("Settings must be an instance of DecompressorSettings");
            }

            return
                base.CalculateRequiredForCompressionStreamMemorySize(inputStreams, inputFileInfo) *
                decompressorSettings.DecompressionFactor;
        }
    }
}
