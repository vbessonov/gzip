using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VBessonov.GZip.Core.Hash
{
    public interface IComparer
    {
        ComparerSettings Settings { get; }

        bool Compare(string inputFilePath, string outputFilePath);
    }
}
