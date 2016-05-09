using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace VBessonov.GZip.Threads
{
    internal class DistributedThread
    {
        private readonly Thread _thread;

        private readonly ThreadStart _threadStart;

        private readonly ParameterizedThreadStart _parameterizedThreadStart;

        private ProcessThread CurrentThread
        {
            get
            {
                int id = GetCurrentThreadId();

                return Process.GetCurrentProcess().Threads.Cast<ProcessThread>().Single(thread => thread.Id == id);
            }
        }

        public Thread ManagedThread
        {
            get { return _thread; }
        }

        public int ProcessorAffinity { get; set; }

        public DistributedThread()
        {
            _thread = new Thread(ThreadBody);
            _threadStart = null;
            _parameterizedThreadStart = null;
        }

        public DistributedThread(ThreadStart threadStart)
            : this()
        {
            if (threadStart == null)
            {
                throw new ArgumentNullException("Thread start must be non-empty");
            }

            _threadStart = threadStart;
        }

        public DistributedThread(ParameterizedThreadStart parameterizedThreadStart)
            : this()
        {
            if (parameterizedThreadStart == null)
            {
                throw new ArgumentNullException("Parameterized thread start must be non-empty");
            }

            _parameterizedThreadStart = parameterizedThreadStart;
        }

        [DllImport("kernel32.dll")]
        private static extern int GetCurrentThreadId();

        private void ThreadBody(object parameter)
        {
            try
            {
                Thread.BeginThreadAffinity();

                if (ProcessorAffinity != 0)
                {
                    CurrentThread.ProcessorAffinity = new IntPtr(ProcessorAffinity);
                }

                if (_threadStart != null)
                {
                    _threadStart();
                }
                else if (_parameterizedThreadStart != null)
                {
                    _parameterizedThreadStart(parameter);
                }
                else
                {
                    throw new InvalidOperationException("Thread callback must be set first");
                }
            }
            finally
            {
                CurrentThread.ProcessorAffinity = new IntPtr(0xFFFF);
                Thread.EndThreadAffinity();
            }
        }

        public void Start()
        {
            if (_threadStart == null)
            {
                throw new InvalidOperationException("Non-parameterized thread callback must be set first");
            }

            _thread.Start();
        }

        public void Start(object parameter)
        {
            if (_parameterizedThreadStart == null)
            {
                throw new InvalidOperationException("Parameterized thread callback must be set first");
            }

            _thread.Start(parameter);
        }
    }
}
