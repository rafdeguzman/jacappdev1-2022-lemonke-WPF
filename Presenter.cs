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
    class Presenter
    {
        ViewInterface view;
        HomeBudget model;
        public Presenter(ViewInterface v, bool newDB = false)
        {
            string defaultDirectory = ConfigurationManager.AppSettings.Get("defaultFileDirectory");
            string defaultFileName = ConfigurationManager.AppSettings.Get("defaultFileName");
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //if firstTimeUser
            if (bool.Parse(ConfigurationManager.AppSettings.Get("newDB")))
            {
                if (v.ShowFirstTimeMessage())
                {
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
                    //lastindexof reference: https://stackoverflow.com/questions/21733756/best-way-to-split-string-by-last-occurrence-of-character

                    //create budget file using SaveFileDialog
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.DefaultExt = "db";
                    saveFileDialog.Filter = "Database files (*.db)|*.db|All files (*.*)|*.*";
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        //set firstTimeUser to false
                        config.AppSettings.Settings["firstTimeUser"].Value = "false";
                        //initialize new db with chosen filename
                        string filePath = saveFileDialog.FileName;
                        int index = filePath.LastIndexOf('\\') + 1;
                        model = new HomeBudget(filePath, true);
                        config.AppSettings.Settings["lastUsedFilePath"].Value = filePath;
                        config.AppSettings.Settings["currentFile"].Value = filePath.Substring(index);
                        config.Save(ConfigurationSaveMode.Modified);
                    }
                }
            }
            else
            {
                //newDB is false (opening file)

                //ask for budget folder location
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string initialPath = config.AppSettings.Settings["lastUsedFilePath"].Value;
                int index = initialPath.LastIndexOf('\\') + 1;
                openFileDialog.Filter = "Database Files (*.db)|*.db|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = initialPath.Remove(index);
                //open file window
                if (openFileDialog.ShowDialog() == true)
                {
                    //work here
                    //if true, set first time user to false
                    config.AppSettings.Settings["firstTimeUser"].Value = "false";
                    //initialize db using filename
                    string filePath = openFileDialog.FileName;
                    index = filePath.LastIndexOf('\\') + 1;
                    model = new HomeBudget(filePath);
                    config.AppSettings.Settings["lastUsedFilePath"].Value = filePath;
                    config.AppSettings.Settings["currentFile"].Value = filePath.Substring(index);
                    config.Save(ConfigurationSaveMode.Modified);
                }
            }
        }
        public HomeBudget GetPresenterModel()
        {
            return model;
        }

    }
}
