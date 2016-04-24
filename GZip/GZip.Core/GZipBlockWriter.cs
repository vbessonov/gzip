using System;
using System.IO;
using System.Text;
using VBessonov.GZip.Core.Extensions;

namespace VBessonov.GZip.Core
{
    public class GZipBlockWriter : IGZipBlockWriter
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

        public void Write(Stream stream, GZipBlock block, GZipBlockFlags flags)
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

            if (flags.HasFlag(GZipBlockFlags.ID1))
            {
                stream.WriteByte(block.ID1);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(GZipBlockFlags.ID2))
            {
                stream.WriteByte(block.ID2);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(GZipBlockFlags.CompressionMethod))
            {
                stream.WriteByte(block.CompressionMethod);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(GZipBlockFlags.Flags))
            {
                stream.WriteByte((byte)block.Flags);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(GZipBlockFlags.ModificationTime))
            {
                stream.Write(BitConverter.GetBytes((int)block.ModificationTime.ToUnixTimeSeconds()), 0, 4);
            }
            else
            {
                stream.Position += 4;
            }

            if (flags.HasFlag(GZipBlockFlags.ExtraFlags))
            {
                stream.WriteByte(block.ExtraFlags);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(GZipBlockFlags.OS))
            {
                stream.WriteByte(block.OS);
            }
            else
            {
                stream.Position++;
            }

            if (flags.HasFlag(GZipBlockFlags.ExtraField) && (block.Flags.HasFlag(GZipFlags.FEXTRA)))
            {
                InsertIntoStream(stream, BitConverter.GetBytes((ushort)block.ExtraField.Length));
                InsertIntoStream(stream, block.ExtraField);
            }

            if (flags.HasFlag(GZipBlockFlags.OriginalFileName) && (block.Flags.HasFlag(GZipFlags.FNAME)))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(block.OriginalFileName);
                byte[] data = new byte[buffer.Length + 1];

                buffer.CopyTo(data, 0);
                data[buffer.Length] = 0;

                InsertIntoStream(stream, data);
            }

            if (flags.HasFlag(GZipBlockFlags.Comment) && (block.Flags.HasFlag(GZipFlags.FCOMMENT)))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(block.Comment);
                byte[] data = new byte[buffer.Length + 1];

                buffer.CopyTo(data, 0);
                data[buffer.Length] = 0;

                InsertIntoStream(stream, data);
            }

            if (flags.HasFlag(GZipBlockFlags.CRC16) && (block.Flags.HasFlag(GZipFlags.FHCRC)))
            {
                InsertIntoStream(stream, BitConverter.GetBytes(block.CRC16));
            }

            if (flags.HasFlag(GZipBlockFlags.CompressedBlocks))
            {
                stream.Write(block.CompressedBlocks, 0, block.CompressedBlocks.Length);
            }

            if (flags.HasFlag(GZipBlockFlags.CRC32))
            {
                if (!flags.HasFlag(GZipBlockFlags.CompressedBlocks))
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

            if (flags.HasFlag(GZipBlockFlags.OriginalFileSize))
            {
                if (!flags.HasFlag(GZipBlockFlags.CompressedBlocks))
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
