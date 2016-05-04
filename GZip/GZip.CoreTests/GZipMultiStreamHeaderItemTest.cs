using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VBessonov.GZip.Core;

namespace VBessonov.GZip.CoreTests
{
    [TestClass]
    public class GZipMultiStreamHeaderItemTest
    {
        [TestMethod]
        public void TestSerialize()
        {
            ulong offset = ushort.MaxValue;
            ushort length = ushort.MaxValue;
            GZipMultiStreamHeaderItem item = new GZipMultiStreamHeaderItem
            {
                Length = length
            };
            byte[] buffer = item.Serialize();

            Assert.AreEqual(offset, BitConverter.ToUInt64(buffer, 0));
            Assert.AreEqual(length, BitConverter.ToUInt16(buffer, sizeof(ulong)));
        }

        [TestMethod]
        public void TestDeserialize()
        {
            ulong offset = ushort.MaxValue;
            ushort length = ushort.MaxValue;
            GZipMultiStreamHeaderItem item = new GZipMultiStreamHeaderItem();
            byte[] offsetBytes = BitConverter.GetBytes(offset);
            byte[] lengthBytes = BitConverter.GetBytes(length);
            byte[] buffer = new byte[offsetBytes.Length + lengthBytes.Length];

            offsetBytes.CopyTo(buffer, 0);
            lengthBytes.CopyTo(buffer, offsetBytes.Length);

            item.Deserialize(buffer);

            Assert.AreEqual(length, item.Length);
        }

        [TestMethod]
        public void TestCanDeserializeSerializedItem()
        {
            ushort offset = ushort.MaxValue;
            ushort length = ushort.MaxValue;
            GZipMultiStreamHeaderItem item = new GZipMultiStreamHeaderItem
            {
                Length = length
            };
            byte[] buffer = item.Serialize();
            GZipMultiStreamHeaderItem newItem = new GZipMultiStreamHeaderItem();

            newItem.Deserialize(buffer);

            Assert.AreEqual(length, newItem.Length);
        }
    }
}
