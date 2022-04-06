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
                    //set firstTimeUser to false
                    ConfigurationManager.AppSettings.Set("firstTimeUser", "false");
                    //set lastUsedFilePath to \\Documents\\BudgetFiles\\budget.db
                    ConfigurationManager.AppSettings.Set("lastUsedFilePath", defaultDirectory + defaultFileName);
                    //create directory
                    string newDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + defaultDirectory;
                    Directory.CreateDirectory(newDirectory);
                    string newFilePath = newDirectory + defaultFileName;
                    model = new HomeBudget(newFilePath, !File.Exists(newFilePath));
                    view = v;
                }

            }
        }

    }
}
