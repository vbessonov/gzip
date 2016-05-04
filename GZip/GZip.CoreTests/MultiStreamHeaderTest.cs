using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VBessonov.GZip.Core;

namespace VBessonov.GZip.CoreTests
{
    [TestClass]
    public class MultiStreamHeaderTest
    {
        [TestMethod]
        public void TestCanDeserializeSerializedHeader()
        {
            MultiStreamHeaderItem[] items = new[]
            {
                new MultiStreamHeaderItem
                {
                    Length = ushort.MaxValue
                },
                new MultiStreamHeaderItem
                {
                    Length = ushort.MaxValue
                },
                new MultiStreamHeaderItem
                {
                    Length = ushort.MaxValue
                },
                new MultiStreamHeaderItem
                {
                    Length = ushort.MaxValue / 2
                }
            };
            GZipMultiStreamHeader header = new GZipMultiStreamHeader();

            foreach (MultiStreamHeaderItem item in items)
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
