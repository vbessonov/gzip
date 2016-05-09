using System;

namespace VBessonov.GZip.Threads
{
    public class ClientHandle
    {
        public Guid ID { get; private set; }

        internal ClientHandle(Guid id)
        {
            ID = id;
        }
    }
}
