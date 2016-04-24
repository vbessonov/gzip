using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using VBessonov.GZip.Core;

namespace VBessonov.GZip.CoreTests
{
    [TestClass]
    public class GZipBlockReaderWriterTest
    {
        [TestMethod]
        public void TestWrite()
        {
            IGZipBlockWriter blockWriter = new GZipBlockWriter();
            Stream compressedStream = TestUtils.Compress(10);
            GZipBlock block = new GZipBlock
            {
                ExtraField = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                OriginalFileName = "test.txt",
                Comment = "test",
                Flags = GZipFlags.FEXTRA | GZipFlags.FNAME | GZipFlags.FCOMMENT
            };

            blockWriter.Write(
                compressedStream,
                block,
                GZipBlockFlags.ExtraField | GZipBlockFlags.OriginalFileName | GZipBlockFlags.Comment | GZipBlockFlags.Flags
            );

            IGZipBlockReader blockReader = new GZipBlockReader();
            GZipBlock newBlock = blockReader.Read(compressedStream, GZipBlockFlags.All);

            Assert.AreEqual(block.Flags, newBlock.Flags);
            CollectionAssert.AreEqual(block.ExtraField, newBlock.ExtraField);
            Assert.AreEqual(block.Comment, newBlock.Comment);
            Assert.AreEqual(block.OriginalFileName, newBlock.OriginalFileName);
        }
    }
}
