using System;

namespace VBessonov.GZip.Threads
{
    internal class TaskHandle
    {
        public ClientHandle ClientHandle { get; private set; }

        public UserTask Task { get; private set; }

        public object UserState { get; private set; }

        public Action<TaskStatus> Callback { get; private set; }

        public TaskHandle(ClientHandle clientHandle, UserTask task, object userState, Action<TaskStatus> callback)
        {
            ClientHandle = clientHandle;
            Task = task;
            UserState = userState;
            Callback = callback;
        }
    }
}
