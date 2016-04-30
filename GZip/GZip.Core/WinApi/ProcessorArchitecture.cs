using System;

namespace VBessonov.GZip.Core.WinApi
{
    internal enum ProcessorArchitecture
    {
        X86 = 0,
        X64 = 9,
        @Arm = -1,
        Itanium = 6,
        Unknown = 0xFFFF,
    }
}
