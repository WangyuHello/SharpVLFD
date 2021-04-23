# SharpVLFD
复旦FPGA开发板C#编程接口

VeriLink interface for C#

# 如何安装 | How to install

## 从Nuget包管理安装 | from the Nuget Package Manager
```powershell
Install-Package SharpVLFD -Version 0.4.2
```

## 从.NET命令行安装 | from .NET CLI
```powershell
dotnet add package SharpVLFD --version 0.4.2
```

# 如何使用 | How to use

时钟示例

Alarm Clock example

```C#
using System;
using static VLFD.VLFDInterop;

namespace AlarmClock
{
    class Programm
    {
        const int NOW_USE_BOARD = 0;

        static void Main(string[] args)
        {
            AlarmClock();
        }

        static void AlarmClock()
        {
            const int CLOCK_COUNT = 1024;

            var WriteBuffer = new Memory<ushort>(new ushort[CLOCK_COUNT]);
            var ReadBuffer = new Memory<ushort>(new ushort[CLOCK_COUNT]);

            Console.WriteLine(Environment.CurrentDirectory);

            if (!VLFD_ProgramFPGA(NOW_USE_BOARD, @"../../../AlarmClock_fde_dc.bit"))
            {
                Console.WriteLine(VLFD_GetLastErrorMsg(NOW_USE_BOARD));
                Environment.Exit(1);
            }

            if (!VLFD_IO_Open(NOW_USE_BOARD, "F4YF-K2II-Y0Z0-AT05-F805-A478"))
            {
                Console.WriteLine("VLFD_AppOpen error.");
                Console.WriteLine(VLFD_GetLastErrorMsg(NOW_USE_BOARD));
                Environment.Exit(1);
            }

            for (int i = 0; i < CLOCK_COUNT; i++)
            {
                if (i < 2)
                {
                    WriteBuffer.Span[0] = 0x00;
                }
                else
                {
                    WriteBuffer.Span[0] = 0x01;
                }

                WriteBuffer.Span[1] = 0x00;
                WriteBuffer.Span[2] = 0x00;
                WriteBuffer.Span[3] = 0x00;

                ReadBuffer.Span.Clear();

                if (!VLFD_IO_WriteReadData(NOW_USE_BOARD, WriteBuffer.Span, ReadBuffer.Span))
                {
                    Console.WriteLine("error: VLFD_WriteReadData");
                    Console.WriteLine(VLFD_GetLastErrorMsg(NOW_USE_BOARD));
                }

                var hr_out = ReadBuffer.Span[0] & 0x000F;
                var min_out = (ReadBuffer.Span[0] & 0x03F0) >> 4;
                var sec_out = (ReadBuffer.Span[0] & 0xFC00) >> 10;
                var hr_alarm = ReadBuffer.Span[1] & 0x000F;
                var min_alarm = (ReadBuffer.Span[1] & 0x03F0) >> 4;
                var alarm = (ReadBuffer.Span[1] & 0x0400) >> 10;

                Console.WriteLine($"[{i}] hr_out[{hr_out}] min_out[{min_out}] sec_out[{sec_out}] hr_alarm[{hr_alarm}] min_alarm[{min_alarm}] alarm[{alarm}]");
            }

            if (!VLFD_IO_Close(NOW_USE_BOARD))
            {
                Console.WriteLine("error: VLFD_AppClose");
                Console.WriteLine(VLFD_GetLastErrorMsg(NOW_USE_BOARD));
            }
            else
            {
                Console.WriteLine("VLFD_AppClose OK!");
            }
        }
    }
}
```
