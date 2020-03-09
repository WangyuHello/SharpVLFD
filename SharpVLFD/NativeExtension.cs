using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace VLFD
{
    /// <summary>
    /// Takes care of loading C# native extension and provides access to PInvoke calls the library exports.
    /// </summary>
    internal sealed class NativeExtension
    {
        static readonly object staticLock = new object();
        static volatile NativeExtension instance;

        readonly NativeMethods nativeMethods;

        private NativeExtension()
        {
            this.nativeMethods = LoadNativeMethods();
        }

        /// <summary>
        /// Gets singleton instance of this class.
        /// The native extension is loaded when called for the first time.
        /// </summary>
        public static NativeExtension Get()
        {
            if (instance == null)
            {
                lock (staticLock)
                {
                    if (instance == null)
                    {
                        instance = new NativeExtension();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// Provides access to the exported native methods.
        /// </summary>
        public NativeMethods NativeMethods
        {
            get { return this.nativeMethods; }
        }

        /// <summary>
        /// Detects which configuration of native extension to load and load it.
        /// </summary>
        private static UnmanagedLibrary LoadUnmanagedLibrary()
        {
            // TODO: allow customizing path to native extension (possibly through exposing a GrpcEnvironment property).
            // See https://github.com/grpc/grpc/pull/7303 for one option.
            var assemblyDirectory = Path.GetDirectoryName(GetAssemblyPath());

            // With old-style VS projects, the native libraries get copied using a .targets rule to the build output folder
            // alongside the compiled assembly.
            // With dotnet cli projects targeting net45 framework, the native libraries (just the required ones)
            // are similarly copied to the built output folder, through the magic of Microsoft.NETCore.Platforms.
            var classicPath = Path.Combine(assemblyDirectory, GetNativeLibraryFilename());

            // With dotnet cli project targeting netcoreapp1.0, projects will use Grpc.Core assembly directly in the location where it got restored
            // by nuget. We locate the native libraries based on known structure of Grpc.Core nuget package.
            // When "dotnet publish" is used, the runtimes directory is copied next to the published assemblies.
            string runtimesDirectory = string.Format("runtimes/{0}/native", GetPlatformString());
            var netCorePublishedAppStylePath = Path.Combine(assemblyDirectory, runtimesDirectory, GetNativeLibraryFilename());
            var netCoreAppStylePath = Path.Combine(assemblyDirectory, "../..", runtimesDirectory, GetNativeLibraryFilename());

            // Look for the native library in all possible locations in given order.
            string[] paths = new[] { classicPath, netCorePublishedAppStylePath, netCoreAppStylePath };
            return new UnmanagedLibrary(paths);
        }

        /// <summary>
        /// Loads native extension and return native methods delegates.
        /// </summary>
        private static NativeMethods LoadNativeMethods()
        {
            if (PlatformApis.IsUnity)
            {
                return LoadNativeMethodsUnity();
            }
            if (PlatformApis.IsXamarin)
            {
                return LoadNativeMethodsXamarin();
            }
            return new NativeMethods(LoadUnmanagedLibrary());
        }

        /// <summary>
        /// Return native method delegates when running on Unity platform.
        /// Unity does not use standard NuGet packages and the native library is treated
        /// there as a "native plugin" which is (provided it has the right metadata)
        /// automatically made available to <c>[DllImport]</c> loading logic.
        /// WARNING: Unity support is experimental and work-in-progress. Don't expect it to work.
        /// </summary>
        private static NativeMethods LoadNativeMethodsUnity()
        {
            switch (PlatformApis.GetUnityRuntimePlatform())
            {
                case "IPhonePlayer":
                    return new NativeMethods(new NativeMethods.DllImportsFromStaticLib());
                default:
                    // most other platforms load unity plugins as a shared library
                    return new NativeMethods(new NativeMethods.DllImportsFromSharedLib());
            }
        }

        /// <summary>
        /// Return native method delegates when running on the Xamarin platform.
        /// WARNING: Xamarin support is experimental and work-in-progress. Don't expect it to work.
        /// </summary>
        private static NativeMethods LoadNativeMethodsXamarin()
        {
            if (PlatformApis.IsXamarinAndroid)
            {
                return new NativeMethods(new NativeMethods.DllImportsFromSharedLib());
            }
            // not tested yet
            return new NativeMethods(new NativeMethods.DllImportsFromStaticLib());
        }

        private static string GetAssemblyPath()
        {
            var assembly = typeof(NativeExtension).GetTypeInfo().Assembly;
#if NETSTANDARD1_5
            // Assembly.EscapedCodeBase does not exist under CoreCLR, but assemblies imported from a nuget package
            // don't seem to be shadowed by DNX-based projects at all.
            return assembly.Location;
#else
            // If assembly is shadowed (e.g. in a webapp), EscapedCodeBase is pointing
            // to the original location of the assembly, and Location is pointing
            // to the shadow copy. We care about the original location because
            // the native dlls don't get shadowed.

            var escapedCodeBase = assembly.EscapedCodeBase;
            if (IsFileUri(escapedCodeBase))
            {
                return new Uri(escapedCodeBase).LocalPath;
            }
            return assembly.Location;
#endif
        }

#if !NETSTANDARD1_5
        private static bool IsFileUri(string uri)
        {
            return uri.ToLowerInvariant().StartsWith(Uri.UriSchemeFile);
        }
#endif

        private static string GetPlatformString()
        {
            if (PlatformApis.IsWindows)
            {
                return "win";
            }
            if (PlatformApis.IsLinux)
            {
                return "linux";
            }
            if (PlatformApis.IsMacOSX)
            {
               return "osx";
            }
            throw new InvalidOperationException("Unsupported platform.");
        }

        // Currently, only Intel platform is supported.
        private static string GetArchitectureString()
        {
            if (PlatformApis.IsArm64)
            {
                return "arm64";
            }
            else if(PlatformApis.IsArm)
            {
                return "armhf";
            }
            else //x86_64
            {
                if (PlatformApis.Is64Bit)
                {
                    return "x64";
                }
                else
                {
                    return "x86";
                }
            }

            throw new InvalidOperationException("Unsupported platform.");
        }

        // platform specific file name of the extension library
        private static string GetNativeLibraryFilename()
        {
            string architecture = GetArchitectureString();
            if (PlatformApis.IsWindows)
            {
                return string.Format("VLFD.{0}.dll", architecture);
            }
            if (PlatformApis.IsLinux)
            {
                return string.Format("libVLFD.{0}.so", architecture);
            }
            if (PlatformApis.IsMacOSX)
            {
                return GetMacOSNativeLibraryFilename(architecture);
            }
            throw new InvalidOperationException("Unsupported platform.");
        }

        private static Dictionary<string, string> MacOSVersionDict = new Dictionary<string, string>()
        {
            { "19.3","10.15.3" },
            { "19.2","10.15.3" },
            { "19.0","10.15.3" },
            { "18.2","" },
            { "18.0","" },
            { "17.7","" },
            { "17.6","" },
            { "17.5","" },
            { "17.0","" },
        };

        private static string GetMacOSVersion()
        {
            var v = Environment.OSVersion.Version;
            var major = v.Major;
            var minor = v.Minor;
            var ver = $"{major}.{minor}";
            var osName = "";
            if (MacOSVersionDict.ContainsKey(ver))
            {
                osName = MacOSVersionDict[ver];
            }
            return osName;
        }

        private static string GetMacOSNativeLibraryFilename(string architecture)
        {
            var osName = GetMacOSVersion();
            if (!string.IsNullOrEmpty(osName))
            {
                return string.Format($"libVLFD.{architecture}.{osName}.dylib");
            }
            return string.Format($"libVLFD.{architecture}.dylib");
        }
    }
}