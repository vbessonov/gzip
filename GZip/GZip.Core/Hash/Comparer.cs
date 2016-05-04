using System;
using System.IO;
using System.Security.Cryptography;

namespace VBessonov.GZip.Core.Hash
{
    public class Comparer : IComparer
    {
        private readonly ComparerSettings _settings;

        public ComparerSettings Settings
        {
            get { return _settings; }
        }

        public Comparer()
            : this(CreateDefaultSettings())
        {

        }

        public Comparer(ComparerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("Settings must be non-empty");
            }

            _settings = settings;
        }

        private static ComparerSettings CreateDefaultSettings()
        {
            return new ComparerSettings();
        }

        private bool CompareHashes(byte[] firstHash, byte[] secondHash)
        {
            bool result = false;

            do
            {
                if (firstHash.Length != secondHash.Length)
                {
                    break;
                }

                for (int i = 0; i < firstHash.Length; i++)
                {
                    if (firstHash[i] != secondHash[i])
                    {
                        break;
                    }
                }

                result = true;
            } while (false);

            return result;
        }

        public bool Compare(string inputFilePath, string outputFilePath)
        {
            if (string.IsNullOrEmpty(inputFilePath))
            {
                throw new ArgumentException("Input file path must be non-empty");
            }
            if (!File.Exists(inputFilePath))
            {
                throw new ArgumentException("Input file must exist");
            }
            if (string.IsNullOrEmpty(outputFilePath))
            {
                throw new ArgumentException("Output file path must be non-empty");
            }
            if (!File.Exists(outputFilePath))
            {
                throw new ArgumentException("Output file must exist");
            }

            HashAlgorithmFactory factory = new HashAlgorithmFactory();
            HashAlgorithm hashAlgorithm = factory.Create(_settings.HashType);
            byte[] inputFileHash = hashAlgorithm.ComputeHash(File.Open(inputFilePath, FileMode.Open));
            byte[] outputFileHash = hashAlgorithm.ComputeHash(File.Open(outputFilePath, FileMode.Open));

            return CompareHashes(inputFileHash, outputFileHash);
        }
    }
}
