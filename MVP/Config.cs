using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace HomeBudgetWPF
{
    public class GetConfig : ConfigInterface
    {
        public static string getDirectory => ConfigurationManager.AppSettings.Get("defaultFileDirectory");
        public static string getFileName => ConfigurationManager.AppSettings.Get("defaultFileName");

        public static string lastUsedFilePath {
            get
            {
                return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["lastUsedFilePath"].Value;
            }
            set
            {
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["lastUsedFilePath"].Value = value;
            }
        }

        public static string currentFile
        {
            get { return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["currentFile"].Value; }
            set { ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["currentFile"].Value = value; }
        }
        public static string firstTimeUser 
        {
            get { return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["firstTimeUser"].Value; }
            set { ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["firstTimeUser"].Value = value; }    
        }

        public static void saveConfig()
        {
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).Save(ConfigurationSaveMode.Modified);
        }
    }
}
