using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace CheronChatroom
{
    public class PythonForm
    {
        public PythonForm() {}
        
        public static string RunCheron()
        {
            string fileName = @"C:\Users\chase ueltschey\source\repos\CheronChatroom\CheronChatroom\Cheron.py";
            string pythonPath = @"C:\Users\chase ueltschey\AppData\Local\Programs\Python\Python311\python.exe";
            string importFolder = @"C:\Users\chase ueltschey\AppData\Local\Programs\Python\Python311\Lib\sqlite3";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath;
            start.Arguments = string.Format("\"{0}\" \"{1}\"", fileName, importFolder);
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true;
            start.RedirectStandardError = true;
            start.RedirectStandardOutput= true;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                    return result;
                }
            }
        }
    }
}