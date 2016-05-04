using System;
using System.ComponentModel;

namespace VBessonov.GZip.Core.Compression.Workers
{
    public class WorkCompletedEventArgs : AsyncCompletedEventArgs
    {
        public WorkCompletedEventArgs(Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {

        }
    }
}
