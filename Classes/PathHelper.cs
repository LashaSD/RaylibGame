using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


public static class PathHelper
{
    public static string GetProjectDirectory([CallerFilePath] string callerFilePath = "")
    {
        return Path.GetDirectoryName(Path.GetDirectoryName(callerFilePath));
    }
}

