using Finalaplication.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

namespace Finalaplication.ControllerHelpers.UniversalHelpers
{
    public class UniversalFunctions
    {
        public static void DeleteFile(string path)
        {
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
        }
        public static bool File_is_not_empty(IFormFile file)
        {
            if (file.Length > 0)
                return true;
            else
                return false;
        }

        public static bool File_is_not_empty(IList<IFormFile> files)
        {
            if (files!=null)
                return true;
            else
                return false;
        }

        internal static byte[] Image(IList<IFormFile> image)
        {
            byte[] return_image = new Byte[64];

            
            foreach (var item in image)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        item.CopyTo(stream);
                        return_image = stream.ToArray();
                    }
                }
                }
            

            return return_image;
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

        public static int GetNumberOfItemPerPageFromSettings(ITempDataDictionary tempDataDic)
        {
            try
            {
                string numberOfDocumentsAsString = tempDataDic.Peek(VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE).ToString();
                return Convert.ToInt16(numberOfDocumentsAsString);
            }
            catch
            {
                return VolMongoConstants.DEFAULT_NUMBER_OF_ITEMS_PER_PAGE;
            }
        }

        internal static int GetCurrentPage(int page)
        {
            if (page > 0)
                return page;
            else
            {
                page = 1;
                return page;
            }
        }

        internal static bool ContainsSpecialChar(string incomingstring)
        {
            bool containsspecialchar = false;
            if (incomingstring.Contains(";"))
            {
                containsspecialchar = true;
            }
            return containsspecialchar;
        }

        internal static byte[] Addimage(IFormFile image)
        {
            byte[] returnedimage = null;
            if (image != null)
            {
                var stream = new MemoryStream();
                image.CopyTo(stream);
                returnedimage = stream.ToArray();
            }
            return returnedimage;
        }

        public static DateTime Dateformatter(string datestring)
        {
            DateTime date;
            if (datestring.Contains("currently"))
            {
                date = DateTime.Today;
                return date;
            }
            else if (datestring.Length == 8)
            {
                datestring = datestring.Insert(0, "0");
                datestring = datestring.Insert(3, "0");
            }
            else if (datestring.Length == 9 && datestring[2] != '/')
            {
                datestring = datestring.Insert(0, "0");
            }
            else if (datestring.Length == 9)
            {
                datestring = datestring.Insert(2, "0");
            }
            date = DateTime.ParseExact(datestring, "dd/MM/yyyy", CultureInfo.DefaultThreadCurrentCulture);
            return date;
        }
    }
}