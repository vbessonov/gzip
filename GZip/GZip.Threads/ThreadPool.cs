using System;
using System.Collections.Generic;
using System.Threading;

namespace VBessonov.GZip.Threads
{
    public static class ThreadPool
    {
        private const int SchedulingInterval = 10;

        private static readonly object _queueLock = new object();

        private static readonly object _poolLock = new object();

        private static readonly Queue<TaskHandle> _readyQueue = new Queue<TaskHandle>();

        private static readonly List<TaskItem> _pool = new List<TaskItem>();

        private static int _minCount = 1;

        private static int _maxCount = Environment.ProcessorCount;

        private static bool _initialized;

        private static Thread _taskScheduler;

        public static int MinCount
        {
            get { return _minCount; }
            set
            {
                if (value < 1 || value > _maxCount)
                {
                    throw new ArgumentOutOfRangeException("Min count must be between 1 and max count");
                }

                _minCount = value;
            }
        }

        public static int MaxCount
        {
            get { return _maxCount; }
            set
            {
                if (value < _minCount)
                {
                    throw new ArgumentOutOfRangeException("Max count must be equal or greater than min count");
                }

                _maxCount = value;
            }
        }

        private static void AddTaskToPool(TaskHandle taskHandle)
        {
            DistributedThread handler = new DistributedThread(TaskBody);

            if (_pool.Count < Environment.ProcessorCount)
            {
                handler.ProcessorAffinity = 1 << _pool.Count;
            }

            handler.ManagedThread.Name = "Thread Pool Thread";
            handler.ManagedThread.IsBackground = true;

            TaskItem taskItem = new TaskItem(taskHandle, handler);

            taskItem.Handler.Start(taskItem);

            lock (_poolLock)
            {
                _pool.Add(taskItem);
            }
        }

        private static void InitializeThreadPool()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            _taskScheduler = new Thread(TaskSchedulerBody);
            _taskScheduler.Name = "Thead Pool Scheduler";
            _taskScheduler.Priority = ThreadPriority.AboveNormal;
            _taskScheduler.IsBackground = true;

            _taskScheduler.Start();
        }

        private static void TaskSchedulerBody()
        {
            do
            {
                lock (_queueLock)
                {
                    // 1. Remove aborted tasks
                    while (_readyQueue.Count > 0 && _readyQueue.Peek().Task == null)
                    {
                        _readyQueue.Dequeue();
                    }

                    int readyTaskCount = _readyQueue.Count;

                    for (int i = 0; i < readyTaskCount; i++)
                    {
                        TaskHandle readyTask = _readyQueue.Peek();
                        bool added = false;

                        // 2. Try to assign a new task to a free handler
                        lock (_poolLock)
                        {
                            foreach (TaskItem taskItem in _pool)
                            {
                                if (taskItem.State == TaskState.Completed)
                                {
                                    taskItem.Handle = readyTask;
                                    taskItem.State = TaskState.NotStarted;

                                    added = true;
                                    _readyQueue.Dequeue();
                                    break;
                                }
                            }
                        }

                        // 3. Try to create a new handler
                        if (!added && _pool.Count < _maxCount)
                        {
                            AddTaskToPool(readyTask);

                            added = true;
                            _readyQueue.Dequeue();
                        }

                        if (!added)
                        {
                            break;
                        }
                    }
                }

                // 4. Sleep for a while to not waste CPU
                Thread.Sleep(SchedulingInterval);
            } while (true);
        }

        private static void TaskBody(object parameter)
        {
            TaskItem taskItem = (TaskItem)parameter;

            do
            {
                bool canProcess = false;

                lock (taskItem)
                {
                    if (taskItem.State == TaskState.Aborted)
                    {
                        break;
                    }

                    if (taskItem.State == TaskState.NotStarted)
                    {
                        taskItem.State = TaskState.Processing;
                        canProcess = true;
                    }
                }

                if (canProcess)
                {
                    TaskStatus taskStatus = new TaskStatus();

                    try
                    {
                        taskItem.Handle.Task.Invoke(taskItem.Handle.UserState);
                        taskStatus.Success = true;
                    }
                    catch (Exception exception)
                    {
                        taskStatus.Success = false;
                        taskStatus.Error = exception;
                    }

                    if (taskItem.State != TaskState.Aborted)
                    {
                        lock (taskItem)
                        {
                            taskItem.State = TaskState.Completed;

                            if (taskItem.Handle.Callback != null)
                            {
                                try
                                {
                                    taskItem.Handle.Callback(taskStatus);
                                }
                                catch { }
                            }
                        }
                    }
                }

                // TODO: Need to use kernel synchonization mechanism to sleep handler thread and not waste CPU
                Thread.Yield();
                Thread.Sleep(SchedulingInterval);
            } while (true);
        }

        public static ClientHandle QueueUserTask(UserTask task, object userState, Action<TaskStatus> callback)
        {
            InitializeThreadPool();

            ClientHandle clientHandle = new ClientHandle(Guid.NewGuid());
            TaskHandle taskHandle = new TaskHandle(clientHandle, task, userState, callback);

            lock (_queueLock)
            {
                _readyQueue.Enqueue(taskHandle);
            }

            return clientHandle;
        }
    }
}
