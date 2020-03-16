using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VLFD
{
    public class Dependency
    {
        public static Dictionary<int, Dictionary<int, string>> MacOSDeps;

        static Dependency()
        {
            MacOSDeps = new Dictionary<int, Dictionary<int, string>>();
            var macOS13 = new Dictionary<int, string>() { { 0, "10.13" } };
            var macOS14 = new Dictionary<int, string>() { { 0, "10.14" }, { 2, "10.14.1" } };
            var macOS15 = new Dictionary<int, string>() { { 0, "10.15" }, { 2, "10.15.2" }, { 3, "10.15.3" } };
            MacOSDeps.Add(17, macOS13);
            MacOSDeps.Add(18, macOS14);
            MacOSDeps.Add(19, macOS15);
        }

        public static List<string> GetMacOSNativeLibraryFilename(string architecture)
        {
            var (major, minor) = GetMacOSKernelVersion();
            var oss = new List<string>();
            oss.Add($"libVLFD.{architecture}.dylib");
            if (MacOSDeps.ContainsKey(major))
            {
                var m = MacOSDeps[major];
                oss.Add($"libVLFD.{architecture}.{m[0]}.dylib");
                if (minor > 0)
                {
                    if (m.ContainsKey(minor))
                    {
                        var osName = m[minor];
                        oss.Add($"libVLFD.{architecture}.{osName}.dylib");
                    }
                    else if (m.Count > 1)
                    {
                        var osName = m.Last().Value;
                        oss.Add($"libVLFD.{architecture}.{osName}.dylib");
                    }
                }
            }
            oss.Reverse();
            return oss;
        }

        private static (int major, int minor) GetMacOSKernelVersion()
        {
            var v = Environment.OSVersion.Version;
            var major = v.Major;
            var minor = v.Minor;
            return (major, minor);
        }
    }
}
