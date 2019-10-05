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
        public readonly Delegates.VLFD_AppFIFOReadData_delegate VLFD_AppFIFOReadData_init;
        public readonly Delegates.VLFD_AppFIFOWriteData_delegate VLFD_AppFIFOWriteData_init;
        public readonly Delegates.VLFD_AppChannelSelector_delegate VLFD_AppChannelSelector_init;
        public readonly Delegates.VLFD_AppClose_delegate VLFD_AppClose_init;
        public readonly Delegates.VLFD_GetLastErrorMsg_delegate VLFD_GetLastErrorMsg_init;

        #endregion

        public NativeMethods(UnmanagedLibrary library)
        {
            this.VLFD_ProgramFPGA_init = GetMethodDelegate<Delegates.VLFD_ProgramFPGA_delegate>(library);
            this.VLFD_AppOpen_init = GetMethodDelegate<Delegates.VLFD_AppOpen_delegate>(library);
            this.VLFD_AppFIFOReadData_init = GetMethodDelegate<Delegates.VLFD_AppFIFOReadData_delegate>(library);
            this.VLFD_AppFIFOWriteData_init = GetMethodDelegate<Delegates.VLFD_AppFIFOWriteData_delegate>(library);
            this.VLFD_AppChannelSelector_init = GetMethodDelegate<Delegates.VLFD_AppChannelSelector_delegate>(library);
            this.VLFD_AppClose_init = GetMethodDelegate<Delegates.VLFD_AppClose_delegate>(library);
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
            public delegate byte VLFD_ProgramFPGA_delegate(int iBoard, string BitFile);
            public delegate byte VLFD_AppOpen_delegate(int iBoard, string SerialNo);
            public delegate byte VLFD_AppFIFOReadData_delegate(int iBoard, IntPtr Buffer, uint size);
            public delegate byte VLFD_AppFIFOWriteData_delegate(int iBoard, IntPtr Buffer, uint size);
            public delegate byte VLFD_AppChannelSelector_delegate(int iBoard, byte channel);
            public delegate byte VLFD_AppClose_delegate(int iBoard);
            public delegate IntPtr VLFD_GetLastErrorMsg_delegate(int iBoard);
        }

        /// <summary>
        /// sharp_native used as a static library (e.g Unity iOS).
        /// </summary>
        internal class DllImportsFromStaticLib
        {
            private const string ImportName = "__Internal";

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_AppOpen(int iBoard, [MarshalAs(UnmanagedType.LPStr)] string SerialNo);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_ProgramFPGA(int iBoard, [MarshalAs(UnmanagedType.LPStr)] string BitFile);

            //BOOL VLFD_AppFIFOReadData(int iBoard, WORD *Buffer, unsigned size);
            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_AppFIFOReadData(int iBoard, IntPtr Buffer, uint size);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_AppFIFOWriteData(int iBoard, IntPtr Buffer, uint size);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_AppChannelSelector(int iBoard, byte channel);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_AppClose(int iBoard);

            //const char *  VLFD_GetLastErrorMsg(int iBoard);
            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr VLFD_GetLastErrorMsg(int iBoard);
        }

        /// <summary>
        /// sharp_native used a shared library (e.g on Unity Standalone and Android).
        /// </summary>
        internal class DllImportsFromSharedLib
        {
            private const string ImportName = "VLFD";

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_AppOpen(int iBoard,[MarshalAs(UnmanagedType.LPStr)] string SerialNo);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_ProgramFPGA(int iBoard, [MarshalAs(UnmanagedType.LPStr)] string BitFile);

            //BOOL VLFD_AppFIFOReadData(int iBoard, WORD *Buffer, unsigned size);
            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_AppFIFOReadData(int iBoard, IntPtr Buffer, uint size);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_AppFIFOWriteData(int iBoard, IntPtr Buffer, uint size);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_AppChannelSelector(int iBoard, byte channel);

            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern byte VLFD_AppClose(int iBoard);

            //const char *  VLFD_GetLastErrorMsg(int iBoard);
            [DllImport(ImportName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr VLFD_GetLastErrorMsg(int iBoard);
        }
    }
}