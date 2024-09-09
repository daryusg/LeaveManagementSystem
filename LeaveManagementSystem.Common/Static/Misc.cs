using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Common.Static
{
    public static class Misc
    {
        public static bool IsAzureEnv()
        {
            return !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
        }


        public static DateTime getAssemblyBuildDateTime()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
        }
    }
}
