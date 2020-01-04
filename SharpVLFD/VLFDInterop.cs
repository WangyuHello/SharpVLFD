using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VLFD
{
    public static class VLFDInterop
    {
        public static bool VLFD_ProgramFPGA(int iBoard, string BitFile) => NativeExtension.Get().NativeMethods.VLFD_ProgramFPGA_init(iBoard, BitFile);
        public static bool VLFD_AppOpen(int iBoard, string SerialNO) => NativeExtension.Get().NativeMethods.VLFD_AppOpen_init(iBoard, SerialNO);
        public static bool VLFD_IO_Open(int iBoard, string SerialNO) => NativeExtension.Get().NativeMethods.VLFD_IO_Open_init(iBoard, SerialNO);
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
            return r;
        }

        public static bool VLFD_AppFIFOWriteData(int iBoard, Span<ushort> Buffer)
        {
            var p = Marshal.AllocHGlobal(Marshal.SizeOf<ushort>() * Buffer.Length);
            unsafe
            {
                var buf = new Span<ushort>(p.ToPointer(), Buffer.Length);
                Buffer.CopyTo(buf);

                var r = NativeExtension.Get().NativeMethods.VLFD_AppFIFOWriteData_init(iBoard, p, (uint)Buffer.Length);
                Marshal.FreeHGlobal(p);
                return r;
            }
        }

        public static bool VLFD_IO_WriteReadData(int iBoard, Span<ushort> WriteBuffer, Span<ushort> ReadBuffer)
        {
            var writebufpointer = Marshal.AllocHGlobal(Marshal.SizeOf<ushort>() * WriteBuffer.Length);
            var readbufpointer = Marshal.AllocHGlobal(Marshal.SizeOf<ushort>() * ReadBuffer.Length);
            unsafe
            {
                var writebuf = new Span<ushort>(writebufpointer.ToPointer(), WriteBuffer.Length);
                WriteBuffer.CopyTo(writebuf);

                var readbuf = new Span<ushort>(readbufpointer.ToPointer(), ReadBuffer.Length);
                var r = NativeExtension.Get().NativeMethods.VLFD_IO_WriteReadData_init(iBoard, writebufpointer, readbufpointer, (uint)ReadBuffer.Length);
                
                readbuf.CopyTo(ReadBuffer);

                Marshal.FreeHGlobal(writebufpointer);
                Marshal.FreeHGlobal(readbufpointer);

                return r;
            }
        }

        public static bool VLFD_AppChannelSelector(int iBoard, byte channel) => NativeExtension.Get().NativeMethods.VLFD_AppChannelSelector_init(iBoard, channel);
        public static bool VLFD_AppClose(int iBoard) => NativeExtension.Get().NativeMethods.VLFD_AppClose_init(iBoard);
        public static bool VLFD_IO_Close(int iBoard) => NativeExtension.Get().NativeMethods.VLFD_IO_Close_init(iBoard);
        public static string VLFD_GetLastErrorMsg(int iBoard) {
            var p = NativeExtension.Get().NativeMethods.VLFD_GetLastErrorMsg_init(iBoard);
            return Marshal.PtrToStringAnsi(p);
        }

    }
}
