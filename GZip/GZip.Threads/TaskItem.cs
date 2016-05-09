using System;

namespace VBessonov.GZip.Threads
{
    internal class TaskItem
    {
        public TaskHandle Handle { get; set; }

        public DistributedThread Handler { get; private set; }

        public TaskState State { get; set; }

        public TaskItem(TaskHandle handle, DistributedThread handler)
        {
            Handle = handle;
            Handler = handler;
            State = TaskState.NotStarted;
        }
    }
}
