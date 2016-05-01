using System;
using System.Runtime.InteropServices;

namespace VBessonov.GZip.Core.WinApi
{
    internal sealed class MemoryStatus
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MEMORYSTATUSEX
        {
            public uint dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        private MEMORYSTATUSEX _memoryStatus;

        public static MemoryStatus Current
        {
            get { return new MemoryStatus(); }
        }

        public uint MemoryLoad
        {
            get { return _memoryStatus.dwMemoryLoad; }
        }

        public ulong TotalPhysicalMemorySize
        {
            get { return _memoryStatus.ullTotalPhys; }
        }

        public ulong AvailablePhysicalMemorySize
        {
            get { return _memoryStatus.ullAvailPhys; }
        }

        public ulong TotalPageFileMemorySize
        {
            get { return _memoryStatus.ullTotalPageFile; }
        }

        public ulong AvailablePageFileMemorySize
        {
            get { return _memoryStatus.ullAvailPageFile; }
        }

        public ulong TotalVirtualMemorySize
        {
            get { return _memoryStatus.ullTotalVirtual; }
        }

        public ulong AvailableVirtualMemorySize
        {
            get { return _memoryStatus.ullAvailVirtual; }
        }

        private MemoryStatus()
        {
            _memoryStatus = new MEMORYSTATUSEX();

            if (!GlobalMemoryStatusEx(_memoryStatus))
            {
                throw new InvalidOperationException("Unable to initalize the GlobalMemoryStatusEx API");
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);
    }
}
