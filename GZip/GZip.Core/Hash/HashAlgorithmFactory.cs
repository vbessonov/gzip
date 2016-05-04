using System;
using System.Security.Cryptography;

namespace VBessonov.GZip.Core.Hash
{
    public class HashAlgorithmFactory
    {
        public HashAlgorithm Create(HashAlgorithmType type)
        {
            switch (type)
            {
                case HashAlgorithmType.MD5:
                    return MD5.Create();

                case HashAlgorithmType.SHA1:
                    return SHA1.Create();

                default:
                    throw new ArgumentException("Unknown algorithm type");
            }
        }
    }
}
