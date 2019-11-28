using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Super.Core.Infrastruct.File
{
    public class FileUtility
    {
        public static void CreateCategoryIfNotExist(string category)
        {
            if (!Directory.Exists(category))
            {
                Directory.CreateDirectory(category);
            }
        }
    }
}
