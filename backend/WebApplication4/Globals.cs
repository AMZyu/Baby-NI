using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4
{
    public static class Globals
    {
        public static string sourceFolder = "C:\\Users\\User\\Desktop\\InFolder\\SOEM1_TN_RADIO_LINK_POWER_20200312_001500.txt";
        public static string filename = Path.GetFileNameWithoutExtension(sourceFolder);
    }
}
