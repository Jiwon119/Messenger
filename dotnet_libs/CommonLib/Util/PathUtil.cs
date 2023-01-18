using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace CommonLib.Util
{
    public static class PathUtil
    {
        public static string GetProjectPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))  
            {
                return Directory.GetCurrentDirectory();
            }

            return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        }

        public static string GetSolutionPath()
        {
            return Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}
