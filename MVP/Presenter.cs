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
    public class Presenter
    {
        ViewInterface view;
        HomeBudget model;
        public Presenter(ViewInterface v, bool newDB = false)
        {
            //config file setup
            string defaultDirectory = GetConfig.getDirectory;
            string defaultFileName = GetConfig.getFileName;
            view = v;
            //if newDB
            if (newDB)
            {
                if (v.ShowFirstTimeMessage())
                {
                    //set firstTimeUser to false
                    GetConfig.firstTimeUser = "false";
                    //create directory
                    string newDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + defaultDirectory;
                    Directory.CreateDirectory(newDirectory);
                    string newFilePath = newDirectory + defaultFileName;
                    model = new HomeBudget(newFilePath, true);
                    //set lastUsedFilePath to \\Documents\\BudgetFiles\\budget.db
                    GetConfig.lastUsedFilePath = newFilePath;
                    GetConfig.currentFile = GetConfig.getFileName;
                    GetConfig.saveConfig();
                    view.ShowFilesCreated(GetConfig.lastUsedFilePath);
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
                        GetConfig.firstTimeUser = "false";
                        //initialize new db with chosen filename
                        string filePath = saveFileDialog.FileName;
                        int index = filePath.LastIndexOf('\\') + 1;
                        model = new HomeBudget(filePath, true);
                        GetConfig.lastUsedFilePath = filePath;
                        GetConfig.currentFile = filePath.Substring(index);
                        GetConfig.saveConfig();
                        view.ShowFilesCreated(GetConfig.lastUsedFilePath);
                    }
                }
            }
            else
            {
                //newDB is false (opening file)

                //ask for budget folder location
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string initialPath = GetConfig.lastUsedFilePath;
                int index = initialPath.LastIndexOf('\\') + 1;
                if (initialPath.Length != 0)
                {
                    index = 0;
                    openFileDialog.Filter = "Database Files (*.db)|*.db|All files (*.*)|*.*";
                    openFileDialog.InitialDirectory = initialPath.Remove(index);
                }
                //open file window
                if (openFileDialog.ShowDialog() == true)
                {
                    //work here
                    //if true, set first time user to false
                    GetConfig.firstTimeUser = "false";
                    //initialize db using filename
                    string filePath = openFileDialog.FileName;
                    index = filePath.LastIndexOf('\\') + 1;
                    model = new HomeBudget(filePath);
                    GetConfig.lastUsedFilePath = filePath;
                    GetConfig.currentFile = filePath.Substring(index);
                    GetConfig.saveConfig();
                }
            }
        }

    }
}