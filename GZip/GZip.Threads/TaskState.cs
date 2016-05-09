using System;

namespace VBessonov.GZip.Threads
{
    public enum TaskState
    {
        NotStarted,
        Processing,
        Completed,
        Aborted
    }
}
