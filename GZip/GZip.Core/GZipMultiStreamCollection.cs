using System.Collections.Generic;
using System.IO;

namespace VBessonov.GZip.Core
{
    public class GZipMultiStreamCollection
    {
        public IList<Stream> Streams { get; private set; }

        public GZipMultiStreamCollection()
        {
            Streams = new List<Stream>();
        }
    }
}
