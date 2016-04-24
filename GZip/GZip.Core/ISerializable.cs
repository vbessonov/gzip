using System;

namespace VBessonov.GZip.Core
{
    public interface ISerializable
    {
        byte[] Serialize();

        void Deserialize(byte[] data);
    }
}
