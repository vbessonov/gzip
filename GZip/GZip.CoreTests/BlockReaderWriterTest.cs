using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using VBessonov.GZip.Core;

namespace VBessonov.GZip.CoreTests
{
    [TestClass]
    public class BlockReaderWriterTest
    {
        [TestMethod]
        public void TestWrite()
        {
            IGZipBlockWriter blockWriter = new BlockWriter();
            Stream compressedStream = TestUtils.Compress(10);
            Block block = new Block
            {
                ExtraField = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                OriginalFileName = "test.txt",
                Comment = "test",
                Flags = GZipFlags.FEXTRA | GZipFlags.FNAME | GZipFlags.FCOMMENT
            };

            blockWriter.Write(
                compressedStream,
                block,
                BlockFlags.ExtraField | BlockFlags.OriginalFileName | BlockFlags.Comment | BlockFlags.Flags
            );

            IBlockReader blockReader = new BlockReader();
            Block newBlock = blockReader.Read(compressedStream, BlockFlags.All);

            Assert.AreEqual(block.Flags, newBlock.Flags);
            CollectionAssert.AreEqual(block.ExtraField, newBlock.ExtraField);
            Assert.AreEqual(block.Comment, newBlock.Comment);
            Assert.AreEqual(block.OriginalFileName, newBlock.OriginalFileName);
        }
    }
}
