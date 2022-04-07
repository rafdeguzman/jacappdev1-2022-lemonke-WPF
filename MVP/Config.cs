using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace HomeBudgetWPF
{
    class Config : ConfigInterface
    {
        public string getDirectory => ConfigurationManager.AppSettings.Get("defaultFileDirectory");
        public string getFileName => ConfigurationManager.AppSettings.Get("defaultFileName");

        public string lastUsedFilePath => ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["lastUsedFilePath"].Value;

        public string currentFile => ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["currentFile"].Value;

        public string firstTimeUser => ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["firstTimeUser"].Value;
        public void saveConfig()
        {
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).Save(ConfigurationSaveMode.Modified);
        }
    }
}
