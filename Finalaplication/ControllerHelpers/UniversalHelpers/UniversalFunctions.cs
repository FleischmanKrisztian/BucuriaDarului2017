using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Finalaplication.ControllerHelpers.UniversalHelpers
{
    public class UniversalFunctions
    {
        public static bool File_is_not_empty(IFormFile file)
        {
            if (file.Length > 0)
                return true;
            else
                return false;
        }

        internal static void CreateFileStream(IFormFile Files, string path)
        {
            using var stream = new FileStream(path, FileMode.Create);
            Files.CopyTo(stream);
        }

        internal static void RemoveTempFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }
    }
}
