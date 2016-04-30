using System;
using System.Runtime.InteropServices;

namespace VBessonov.GZip.Core.WinApi
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SystemInfo
    {
        public ProcessorArchitecture ProcessorArchitecture;
        public uint PageSize;
        public IntPtr MinimumApplicationAddress;
        public IntPtr MaximumApplicationAddress;
        public IntPtr ActiveProcessorMask;
        public uint NumberOfProcessors;
        public uint ProcessorType;
        public uint AllocationGranularity;
        public ushort ProcessorLevel;
        public ushort ProcessorRevision;

        [DllImport("kernel32.dll", SetLastError = false)]
        private static extern void GetSystemInfo(out SystemInfo info);

        public static SystemInfo Current
        {
            get
            {
                SystemInfo systemInfo;

                SystemInfo.GetSystemInfo(out systemInfo);

                return systemInfo;
            }
        }
    }
}
