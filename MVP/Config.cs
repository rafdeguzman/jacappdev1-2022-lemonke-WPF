using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Budget;
using System.Configuration;
using System.Collections.Specialized;
using System.Windows;
using Microsoft.Win32;

namespace HomeBudgetWPF
{
    public class Config : ConfigInterface
    {
        public string getDirectory => ConfigurationManager.AppSettings.Get("defaultFileDirectory");
        public string getFileName => ConfigurationManager.AppSettings.Get("defaultFileName");
        Configuration config;
        public Config()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string lastUsedFilePath {
            get
            {
                return config.AppSettings.Settings["lastUsedFilePath"].Value;
            }
            set
            {
                config.AppSettings.Settings["lastUsedFilePath"].Value = value;
                saveConfig();
            }
        }

        public string currentFile
        {
            get { return config.AppSettings.Settings["currentFile"].Value; }
            set { config.AppSettings.Settings["currentFile"].Value = value;
                saveConfig();
            }
        }
        public string firstTimeUser 
        {
            get { return config.AppSettings.Settings["firstTimeUser"].Value; }
            set { config.AppSettings.Settings["firstTimeUser"].Value = value;
                saveConfig();
            }
        }
        public bool newDB
        {
            get { return bool.Parse(config.AppSettings.Settings["newDB"].Value); }
            set { config.AppSettings.Settings["newDB"].Value = value.ToString(); saveConfig(); }
        }

        public void saveConfig()
        {
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
