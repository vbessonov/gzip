using System;
using System.ComponentModel;

namespace VBessonov.GZip.Core.Compression
{
    public class CompressionCompletedEventArgs : AsyncCompletedEventArgs
    {
        internal CompressionCompletedEventArgs(Exception error, bool cancelled)
            : base(error, cancelled, null)
        {

        }
    }
}
