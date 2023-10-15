using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CheronChatroom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application{
        static string DataBaseName = "messages.db";
        static string FolderPath = "C:/Users/chase ueltschey/source/repos/CheronChatroom/CheronChatroom";
        public static string dbpath = System.IO.Path.Combine(FolderPath, DataBaseName);
    }
}
