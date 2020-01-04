using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace VLFD
{
    internal partial class NativeMethods
    {
        #region Native methods

        public readonly Delegates.VLFD_ProgramFPGA_delegate VLFD_ProgramFPGA_init;
        public readonly Delegates.VLFD_AppOpen_delegate VLFD_AppOpen_init;
        public readonly Delegates.VLFD_IO_Open_delegate VLFD_IO_Open_init;
        public readonly Delegates.VLFD_AppFIFOReadData_delegate VLFD_AppFIFOReadData_init;
        public readonly Delegates.VLFD_AppFIFOWriteData_delegate VLFD_AppFIFOWriteData_init;
        public readonly Delegates.VLFD_IO_WriteReadData_delegate VLFD_IO_WriteReadData_init;
        public readonly Delegates.VLFD_AppChannelSelector_delegate VLFD_AppChannelSelector_init;
        public readonly Delegates.VLFD_AppClose_delegate VLFD_AppClose_init;
        public readonly Delegates.VLFD_IO_Close_delegate VLFD_IO_Close_init;
        public readonly Delegates.VLFD_GetLastErrorMsg_delegate VLFD_GetLastErrorMsg_init;

        #endregion

        public NativeMethods(UnmanagedLibrary library)
        {
            this.VLFD_ProgramFPGA_init = GetMethodDelegate<Delegates.VLFD_ProgramFPGA_delegate>(library);
            this.VLFD_AppOpen_init = GetMethodDelegate<Delegates.VLFD_AppOpen_delegate>(library);
            this.VLFD_IO_Open_init = GetMethodDelegate<Delegates.VLFD_IO_Open_delegate>(library);
            this.VLFD_AppFIFOReadData_init = GetMethodDelegate<Delegates.VLFD_AppFIFOReadData_delegate>(library);
            this.VLFD_AppFIFOWriteData_init = GetMethodDelegate<Delegates.VLFD_AppFIFOWriteData_delegate>(library);
            this.VLFD_IO_WriteReadData_init = GetMethodDelegate<Delegates.VLFD_IO_WriteReadData_delegate>(library);
            this.VLFD_AppChannelSelector_init = GetMethodDelegate<Delegates.VLFD_AppChannelSelector_delegate>(library);
            this.VLFD_AppClose_init = GetMethodDelegate<Delegates.VLFD_AppClose_delegate>(library);
            this.VLFD_IO_Close_init = GetMethodDelegate<Delegates.VLFD_IO_Close_delegate>(library);
            this.VLFD_GetLastErrorMsg_init = GetMethodDelegate<Delegates.VLFD_GetLastErrorMsg_delegate>(library);
        }

        public NativeMethods(DllImportsFromStaticLib unusedInstance)
        {
            this.VLFD_ProgramFPGA_init = DllImportsFromStaticLib.VLFD_ProgramFPGA;
            this.VLFD_AppOpen_init = DllImportsFromStaticLib.VLFD_AppOpen;
            this.VLFD_AppFIFOReadData_init = DllImportsFromStaticLib.VLFD_AppFIFOReadData;
            this.VLFD_AppFIFOWriteData_init = DllImportsFromStaticLib.VLFD_AppFIFOWriteData;
            this.VLFD_AppChannelSelector_init = DllImportsFromStaticLib.VLFD_AppChannelSelector;
            this.VLFD_AppClose_init = DllImportsFromStaticLib.VLFD_AppClose;
            this.VLFD_GetLastErrorMsg_init = DllImportsFromStaticLib.VLFD_GetLastErrorMsg;
        }

        public NativeMethods(DllImportsFromSharedLib unusedInstance)
        {
            this.VLFD_ProgramFPGA_init = DllImportsFromSharedLib.VLFD_ProgramFPGA;
            this.VLFD_AppOpen_init = DllImportsFromSharedLib.VLFD_AppOpen;
            this.VLFD_AppFIFOReadData_init = DllImportsFromSharedLib.VLFD_AppFIFOReadData;
            this.VLFD_AppFIFOWriteData_init = DllImportsFromSharedLib.VLFD_AppFIFOWriteData;
            this.VLFD_AppChannelSelector_init = DllImportsFromSharedLib.VLFD_AppChannelSelector;
            this.VLFD_AppClose_init = DllImportsFromSharedLib.VLFD_AppClose;
            this.VLFD_GetLastErrorMsg_init = DllImportsFromSharedLib.VLFD_GetLastErrorMsg;
        }

        /// <summary>
        /// Delegate types for all published native methods. Declared under inner class to prevent scope pollution.
        /// </summary>
        public class Delegates
        {
            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public delegate bool VLFD_ProgramFPGA_delegate(int iBoard, [MarshalAs(UnmanagedType.LPStr)] string BitFile);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public delegate bool VLFD_AppOpen_delegate(int iBoard, [MarshalAs(UnmanagedType.LPStr)] string SerialNo);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public delegate bool VLFD_IO_Open_delegate(int iBoard, [MarshalAs(UnmanagedType.LPStr)] string SerialNo);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public delegate bool VLFD_AppFIFOReadData_delegate(int iBoard, IntPtr Buffer, uint size);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public delegate bool VLFD_AppFIFOWriteData_delegate(int iBoard, IntPtr Buffer, uint size);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public delegate bool VLFD_IO_WriteReadData_delegate(int iBoard, IntPtr WriteBuffer, IntPtr ReadBuffer, uint size);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public delegate bool VLFD_AppChannelSelector_delegate(int iBoard, byte channel);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public delegate bool VLFD_AppClose_delegate(int iBoard);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public delegate bool VLFD_IO_Close_delegate(int iBoard);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            //[return: MarshalAs(UnmanagedType.LPStr)]
            public delegate IntPtr VLFD_GetLastErrorMsg_delegate(int iBoard);
        }

        /// <summary>
        /// sharp_native used as a static library (e.g Unity iOS).
        /// </summary>
        internal class DllImportsFromStaticLib
        {
            private const string ImportName = "__Internal";

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_AppOpen(int iBoard, [MarshalAs(UnmanagedType.LPStr)] string SerialNo);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_ProgramFPGA(int iBoard, [MarshalAs(UnmanagedType.LPStr)] string BitFile);

            //BOOL VLFD_AppFIFOReadData(int iBoard, WORD *Buffer, unsigned size);
            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_AppFIFOReadData(int iBoard, IntPtr Buffer, uint size);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_AppFIFOWriteData(int iBoard, IntPtr Buffer, uint size);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_AppChannelSelector(int iBoard, byte channel);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_AppClose(int iBoard);

            //const char *  VLFD_GetLastErrorMsg(int iBoard);
            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            //[return: MarshalAs(UnmanagedType.LPStr)]
            public static extern IntPtr VLFD_GetLastErrorMsg(int iBoard);
        }

        /// <summary>
        /// sharp_native used a shared library (e.g on Unity Standalone and Android).
        /// </summary>
        internal class DllImportsFromSharedLib
        {
            private const string ImportName = "VLFD";

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_AppOpen(int iBoard,[MarshalAs(UnmanagedType.LPStr)] string SerialNo);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_ProgramFPGA(int iBoard, [MarshalAs(UnmanagedType.LPStr)] string BitFile);

            //BOOL VLFD_AppFIFOReadData(int iBoard, WORD *Buffer, unsigned size);
            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_AppFIFOReadData(int iBoard, IntPtr Buffer, uint size);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_AppFIFOWriteData(int iBoard, IntPtr Buffer, uint size);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_AppChannelSelector(int iBoard, byte channel);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool VLFD_AppClose(int iBoard);

            //const char *  VLFD_GetLastErrorMsg(int iBoard);
            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            //[return: MarshalAs(UnmanagedType.LPStr)]
            public static extern IntPtr VLFD_GetLastErrorMsg(int iBoard);
        }
    }
}