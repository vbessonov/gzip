using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VBessonov.GZip.Core;

namespace VBessonov.GZip.CoreTests
{
    [TestClass]
    public class MultiStreamHeaderItemTest
    {
        [TestMethod]
        public void TestSerialize()
        {
            long length = long.MaxValue;
            MultiStreamHeaderItem item = new MultiStreamHeaderItem
            {
                Length = length
            };
            byte[] buffer = item.Serialize();

            Assert.AreEqual(length, BitConverter.ToInt64(buffer, 0));
        }

        [TestMethod]
        public void TestDeserialize()
        {
            long length = long.MaxValue;
            MultiStreamHeaderItem item = new MultiStreamHeaderItem();
            byte[] buffer = BitConverter.GetBytes(length);

            item.Deserialize(buffer);

            Assert.AreEqual(length, item.Length);
        }

        [TestMethod]
        public void TestCanDeserializeSerializedItem()
        {
            long length = long.MaxValue;
            MultiStreamHeaderItem item = new MultiStreamHeaderItem
            {
                Length = length
            };
            byte[] buffer = item.Serialize();
            MultiStreamHeaderItem newItem = new MultiStreamHeaderItem();

            newItem.Deserialize(buffer);

            Assert.AreEqual(length, newItem.Length);
        }
    }
}
