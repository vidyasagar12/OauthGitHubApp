using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using GitHubOauth.Models;

namespace VstsDemoBuilder.Extensions
{
    public static class Extension
    {
        public static string ReadJsonFile(this ProjectDetails file, string filePath)
        {
            string fileContents = string.Empty;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    fileContents = sr.ReadToEnd();
                }
            }

            return fileContents;
        }

        public static string ErrorId(this string str)
        {
            str = str + "_Errors";
            return str;
        } 
    }
}