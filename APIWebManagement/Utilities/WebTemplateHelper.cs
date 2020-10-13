using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebManagement.Utilities
{
    public class WebTemplateHelper
    {
        public static string GetTemplateContent(string rootPath, string pathFile, string defaultPathFile = null)
        {
            if (string.IsNullOrWhiteSpace(pathFile))
            {
                return null;
            }

            string physicalPathFile = Path.Combine(rootPath, "Template", Path.Combine(pathFile.Replace('\\', '/').Split('/')));
            if (File.Exists(physicalPathFile))
            {
                return File.ReadAllText(physicalPathFile);
            }
            else if (!string.IsNullOrEmpty(defaultPathFile))
            {
                physicalPathFile = Path.Combine(rootPath, "Template", Path.Combine(defaultPathFile.Replace('\\', '/').Split('/')));
                if (File.Exists(physicalPathFile))
                {
                    return File.ReadAllText(physicalPathFile);
                }
            }

            return null;
        }
    }
}
