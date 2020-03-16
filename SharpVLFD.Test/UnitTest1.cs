using System;
using System.Collections.Generic;
using System.Linq;
using VLFD;
using Xunit;

namespace SharpVLFD.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var major = 19;
            var minor = 2;
            var architecture = "x64";
            var oss = new List<string>();
            oss.Add($"libVLFD.{architecture}.dylib");
            if (Dependency.MacOSDeps.ContainsKey(major))
            {
                var m = Dependency.MacOSDeps[major];
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
        }
    }
}
