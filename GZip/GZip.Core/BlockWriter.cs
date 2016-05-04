using System;
using System.IO;
using System.Text;
using VBessonov.GZip.Core.Extensions;

namespace VBessonov.GZip.Core
{
    public class BlockWriter : IGZipBlockWriter
    {
        private void InsertIntoStream(Stream stream, byte[] data)
        {
            int length = (int)(stream.Length - stream.Position);
            byte[] buffer = new byte[length];
            long savedPosition = stream.Position;

            stream.Read(buffer, 0, length);

            stream.Position = savedPosition;

            stream.Write(data, 0, data.Length);
            stream.Write(buffer, 0, length);

            stream.Position = savedPosition + data.Length;
        }

        public void Write(Stream stream, Block block, BlockFlags flags)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("Stream must be non-empty");
            }
            if (block == null)
            {
                throw new ArgumentNullException("Block must be non-empty");
            }

            stream.Position = 0;

            if (flags.HasFlag(BlockFlags.ID1))
            {
                stream.WriteByte(block.ID1);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(BlockFlags.ID2))
            {
                stream.WriteByte(block.ID2);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(BlockFlags.CompressionMethod))
            {
                stream.WriteByte(block.CompressionMethod);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(BlockFlags.Flags))
            {
                stream.WriteByte((byte)block.Flags);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(BlockFlags.ModificationTime))
            {
                stream.Write(BitConverter.GetBytes((int)block.ModificationTime.ToUnixTimeSeconds()), 0, 4);
            }
            else
            {
                stream.Position += 4;
            }

            if (flags.HasFlag(BlockFlags.ExtraFlags))
            {
                stream.WriteByte(block.ExtraFlags);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(BlockFlags.OS))
            {
                stream.WriteByte(block.OS);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(BlockFlags.ExtraField) && (block.Flags.HasFlag(GZipFlags.FEXTRA)))
            {
                InsertIntoStream(stream, BitConverter.GetBytes((ushort)block.ExtraField.Length));
                InsertIntoStream(stream, block.ExtraField);
            }

            if (flags.HasFlag(BlockFlags.OriginalFileName) && (block.Flags.HasFlag(GZipFlags.FNAME)))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(block.OriginalFileName);
                byte[] data = new byte[buffer.Length + 1];

                buffer.CopyTo(data, 0);
                data[buffer.Length] = 0;

                InsertIntoStream(stream, data);
            }

            if (flags.HasFlag(BlockFlags.Comment) && (block.Flags.HasFlag(GZipFlags.FCOMMENT)))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(block.Comment);
                byte[] data = new byte[buffer.Length + 1];

                buffer.CopyTo(data, 0);
                data[buffer.Length] = 0;

                InsertIntoStream(stream, data);
            }

            if (flags.HasFlag(BlockFlags.CRC16) && (block.Flags.HasFlag(GZipFlags.FHCRC)))
            {
                InsertIntoStream(stream, BitConverter.GetBytes(block.CRC16));
            }

            if (flags.HasFlag(BlockFlags.CompressedBlocks))
            {
                stream.Write(block.CompressedBlocks, 0, block.CompressedBlocks.Length);
            }

            if (flags.HasFlag(BlockFlags.CRC32))
            {
                if (!flags.HasFlag(BlockFlags.CompressedBlocks))
                {
                    throw new ArgumentException("Unknown compressed block size");
                }
                else if (block.CompressedBlocks == null || block.CompressedBlocks.Length == 0)
                {
                    throw new ArgumentException("Unknown compressed block size");
                }

                stream.Write(BitConverter.GetBytes(block.CRC32), 0, 4);
            }
            else
            {
                stream.Position += 4;
            }

            if (flags.HasFlag(BlockFlags.OriginalFileSize))
            {
                if (!flags.HasFlag(BlockFlags.CompressedBlocks))
                {
                    throw new ArgumentException("Unknown compressed block size");
                }
                else if (block.CompressedBlocks == null || block.CompressedBlocks.Length == 0)
                {
                    throw new ArgumentException("Unknown compressed block size");
                }

                stream.Write(BitConverter.GetBytes(block.OriginalFileSize), 0, 4);
            }
        }
    }
}
