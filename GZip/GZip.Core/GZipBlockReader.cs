using System;
using System.IO;
using VBessonov.GZip.Core.Extensions;

namespace VBessonov.GZip.Core
{
    public class GZipBlockReader : IGZipBlockReader
    {
        public GZipBlock Read(Stream stream, GZipBlockFlags flags)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("Stream must be non-empty");
            }

            GZipBlock block = new GZipBlock();

            stream.Position = 0;

            BinaryReader reader = new BinaryReader(stream);

            byte[] buffer = reader.ReadBytes(10);

            if (flags.HasFlag(GZipBlockFlags.ID1))
            {
                block.ID1 = buffer[0];
            }

            if (flags.HasFlag(GZipBlockFlags.ID2))
            {
                block.ID2 = buffer[1];
            }

            if (flags.HasFlag(GZipBlockFlags.CompressionMethod))
            {
                block.CompressionMethod = buffer[2];
            }

            if (flags.HasFlag(GZipBlockFlags.Flags | GZipBlockFlags.ExtraField | GZipBlockFlags.OriginalFileName | GZipBlockFlags.Comment | GZipBlockFlags.CRC16))
            {
                block.Flags = (GZipFlags)buffer[3];
            }

            if (flags.HasFlag(GZipBlockFlags.ModificationTime))
            {
                block.ModificationTime = DateTimeExtensions.FromUnixTimeSeconds(BitConverter.ToUInt32(buffer, 4));
            }

            if (flags.HasFlag(GZipBlockFlags.ExtraFlags))
            {
                block.ExtraFlags = buffer[8];
            }

            if (flags.HasFlag(GZipBlockFlags.OS))
            {
                block.OS = buffer[9];
            }

            if (flags.HasFlag(GZipBlockFlags.ExtraField) && block.Flags.HasFlag(GZipFlags.FEXTRA))
            {
                ushort length = reader.ReadUInt16();

                if (length == 0)
                {
                    throw new GZipBlockException("Invalid extra field's length");
                }

                block.ExtraField = reader.ReadBytes(length);
            }

            if (flags.HasFlag(GZipBlockFlags.OriginalFileName) && block.Flags.HasFlag(GZipFlags.FNAME))
            {
                block.OriginalFileName = reader.ReadNullTerminatedString();
            }

            if (flags.HasFlag(GZipBlockFlags.Comment) && block.Flags.HasFlag(GZipFlags.FCOMMENT))
            {
                block.Comment = reader.ReadNullTerminatedString();
            }

            if (flags.HasFlag(GZipBlockFlags.CRC16) && block.Flags.HasFlag(GZipFlags.FHCRC))
            {
                block.CRC16 = reader.ReadUInt16();
            }

            if (flags.HasFlag(GZipBlockFlags.CompressedBlocks))
            {
                int length = (int)(stream.Length - stream.Position - 8);
                block.CompressedBlocks = new byte[length];

                reader.Read(block.CompressedBlocks, 0, length);
            }

            if (flags.HasFlag(GZipBlockFlags.CRC32))
            {
                block.CRC32 = reader.ReadUInt32();
            }

            if (flags.HasFlag(GZipBlockFlags.OriginalFileSize))
            {
                block.OriginalFileSize = reader.ReadUInt32();
            }

            return block;
        }
    }
}
