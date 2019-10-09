using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VLFD
{
    public static class VLFDInterop
    {
        public static bool VLFD_ProgramFPGA(int iBoard, string BitFile) => NativeExtension.Get().NativeMethods.VLFD_ProgramFPGA_init(iBoard, BitFile) == 1;
        public static bool VLFD_AppOpen(int iBoard, string SerialNO) => NativeExtension.Get().NativeMethods.VLFD_AppOpen_init(iBoard, SerialNO) == 1;
        public static bool VLFD_AppFIFOReadData(int iBoard, Span<ushort> Buffer)
        {
            var p = Marshal.AllocHGlobal(Marshal.SizeOf<ushort>() * Buffer.Length);
            var r = NativeExtension.Get().NativeMethods.VLFD_AppFIFOReadData_init(iBoard, p, (uint)Buffer.Length);
            unsafe
            {
                var buf = new Span<ushort>(p.ToPointer(), Buffer.Length);
                buf.CopyTo(Buffer);
            }
            Marshal.FreeHGlobal(p);
            return r == 1;
        }

        public static bool VLFD_AppFIFOWriteData(int iBoard, Span<ushort> Buffer)
        {
            var p = Marshal.AllocHGlobal(Marshal.SizeOf<ushort>() * Buffer.Length);
            unsafe
            {
                var buf = new Span<ushort>(p.ToPointer(), Buffer.Length);
                Buffer.CopyTo(buf);

                var r = NativeExtension.Get().NativeMethods.VLFD_AppFIFOWriteData_init(iBoard, p, (uint)Buffer.Length);
                return r == 1;
            }
        }

        public static bool VLFD_AppChannelSelector(int iBoard, byte channel) => NativeExtension.Get().NativeMethods.VLFD_AppChannelSelector_init(iBoard, channel) == 1;
        public static bool VLFD_AppClose(int iBoard) => NativeExtension.Get().NativeMethods.VLFD_AppClose_init(iBoard) == 1;
        public static string VLFD_GetLastErrorMsg(int iBoard) => NativeExtension.Get().NativeMethods.VLFD_GetLastErrorMsg_init(iBoard);

    }
}
