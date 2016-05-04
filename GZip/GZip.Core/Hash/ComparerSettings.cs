using System;

namespace VBessonov.GZip.Core.Hash
{
    public class ComparerSettings
    {
        public HashAlgorithmType HashType { get; set; }

        public ComparerSettings()
        {
            HashType = HashAlgorithmType.MD5;
        }
    }
}
