using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Budget;
using System.Configuration;
using System.Collections.Specialized;


namespace HomeBudgetWPF
{
    class Presenter
    {
        ViewInterface view;
        HomeBudget model;
        public Presenter(ViewInterface v)
        {
            string defaultDirectory = ConfigurationManager.AppSettings.Get("defaultFileDirectory");
            string defaultFileName = ConfigurationManager.AppSettings.Get("defaultFileName");
            //if firstTimeUser
            if (bool.Parse(ConfigurationManager.AppSettings.Get("firstTimeUser")))
            {
                if (v.ShowFirstTimeMessage())
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    //set firstTimeUser to false
                    config.AppSettings.Settings["firstTimeUser"].Value = "false";
                    //create directory
                    string newDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + defaultDirectory;
                    Directory.CreateDirectory(newDirectory);
                    string newFilePath = newDirectory + defaultFileName;
                    model = new HomeBudget(newFilePath, true);
                    view = v;
                    //set lastUsedFilePath to \\Documents\\BudgetFiles\\budget.db
                    config.AppSettings.Settings["lastUsedFilePath"].Value = newFilePath;
                    config.AppSettings.Settings["currentFile"].Value = config.AppSettings.Settings["defaultFileName"].Value;
                    config.Save(ConfigurationSaveMode.Modified);
                }
                else
                {
                    //do stuff
                }
            }
        }
        public HomeBudget GetPresenterModel()
        {
            return model;
        }

    }
}
