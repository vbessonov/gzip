using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VBessonov.GZip.Core;

namespace VBessonov.GZip.CoreTests
{
    [TestClass]
    public class GZipMultiStreamHeaderTest
    {
        [TestMethod]
        public void TestCanDeserializeSerializedHeader()
        {
            GZipMultiStreamHeaderItem[] items = new[]
            {
                new GZipMultiStreamHeaderItem
                {
                    Offset = 0,
                    Length = ushort.MaxValue
                },
                new GZipMultiStreamHeaderItem
                {
                    Offset = ushort.MaxValue,
                    Length = ushort.MaxValue
                },
                new GZipMultiStreamHeaderItem
                {
                    Offset = ushort.MaxValue * 2,
                    Length = ushort.MaxValue
                },
                new GZipMultiStreamHeaderItem
                {
                    Offset = ushort.MaxValue * 3,
                    Length = ushort.MaxValue / 2
                }
            };
            GZipMultiStreamHeader header = new GZipMultiStreamHeader();

            foreach (GZipMultiStreamHeaderItem item in items)
            {
                header.Items.Add(item);
            }

            byte[] buffer = header.Serialize();

            GZipMultiStreamHeader newHeader = new GZipMultiStreamHeader();

            newHeader.Deserialize(buffer);

            Assert.AreEqual(items.Length, newHeader.Items.Count, "Counts of items are not equal");

            for (int i = 0; i < items.Length; i++)
            {
                Assert.AreEqual(
                    items[i],
                    newHeader.Items[i],
                    string.Format("Items # {0} are not equal", i + 1)
                );
            }
        }
    }
}
