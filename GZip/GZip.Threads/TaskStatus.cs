using System;

namespace VBessonov.GZip.Threads
{
    public class TaskStatus
    {
        public bool Success { get; internal set; }

        public Exception Error { get; internal set; }

        internal TaskStatus()
        {
            Success = true;
        }
    }
}
