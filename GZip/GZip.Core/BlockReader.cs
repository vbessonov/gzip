using System;
using System.IO;
using VBessonov.GZip.Core.Extensions;

namespace VBessonov.GZip.Core
{
    public class BlockReader : IBlockReader
    {
        public Block Read(Stream stream, BlockFlags flags)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("Stream must be non-empty");
            }

            Block block = new Block();

            stream.Position = 0;

            BinaryReader reader = new BinaryReader(stream);

            byte[] buffer = reader.ReadBytes(10);

            if (flags.HasFlag(BlockFlags.ID1))
            {
                block.ID1 = buffer[0];
            }

            if (flags.HasFlag(BlockFlags.ID2))
            {
                block.ID2 = buffer[1];
            }

            if (flags.HasFlag(BlockFlags.CompressionMethod))
            {
                block.CompressionMethod = buffer[2];
            }

            if (flags.HasFlag(BlockFlags.Flags) ||
                flags.HasFlag(BlockFlags.ExtraField) ||
                flags.HasFlag(BlockFlags.OriginalFileName) ||
                flags.HasFlag(BlockFlags.Comment) ||
                flags.HasFlag(BlockFlags.CRC16))
            {
                block.Flags = (GZipFlags)buffer[3];
            }

            if (flags.HasFlag(BlockFlags.ModificationTime))
            {
                block.ModificationTime = DateTimeExtensions.FromUnixTimeSeconds(BitConverter.ToUInt32(buffer, 4));
            }

            if (flags.HasFlag(BlockFlags.ExtraFlags))
            {
                block.ExtraFlags = buffer[8];
            }

            if (flags.HasFlag(BlockFlags.OS))
            {
                block.OS = buffer[9];
            }

            if (flags.HasFlag(BlockFlags.ExtraField) && block.Flags.HasFlag(GZipFlags.FEXTRA))
            {
                ushort length = reader.ReadUInt16();

                if (length == 0)
                {
                    throw new BlockException("Invalid extra field's length");
                }

                block.ExtraField = reader.ReadBytes(length);
            }

            if (flags.HasFlag(BlockFlags.OriginalFileName) && block.Flags.HasFlag(GZipFlags.FNAME))
            {
                block.OriginalFileName = reader.ReadNullTerminatedString();
            }

            if (flags.HasFlag(BlockFlags.Comment) && block.Flags.HasFlag(GZipFlags.FCOMMENT))
            {
                block.Comment = reader.ReadNullTerminatedString();
            }

            if (flags.HasFlag(BlockFlags.CRC16) && block.Flags.HasFlag(GZipFlags.FHCRC))
            {
                block.CRC16 = reader.ReadUInt16();
            }

            if (flags.HasFlag(BlockFlags.CompressedBlocks))
            {
                int length = (int)(stream.Length - stream.Position - 8);
                block.CompressedBlocks = new byte[length];

                reader.Read(block.CompressedBlocks, 0, length);
            }

            if (flags.HasFlag(BlockFlags.CRC32))
            {
                block.CRC32 = reader.ReadUInt32();
            }

            if (flags.HasFlag(BlockFlags.OriginalFileSize))
            {
                block.OriginalFileSize = reader.ReadUInt32();
            }

            return block;
        }
    }
}
