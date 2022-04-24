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

            string defaultDirectory;
            Config config = new Config();
            //config file setup
            defaultDirectory = config.getDirectory;
            string defaultFileName = config.getFileName;
            view = v;
            //if newDB
            if (newDB)
            {
                if (v.ShowFirstTimeMessage())
                {
                    //set firstTimeUser to false
                    config.firstTimeUser = "false";
                    //create directory
                    string newDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + defaultDirectory;
                    Directory.CreateDirectory(newDirectory);
                    string newFilePath = newDirectory + defaultFileName;
                    model = new HomeBudget(newFilePath, true);
                    //set lastUsedFilePath to \\Documents\\BudgetFiles\\budget.db
                    config.lastUsedFilePath = newFilePath;
                    config.currentFile = config.getFileName;
                    config.saveConfig();
                    view.ShowFilesCreated(config.lastUsedFilePath);
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
                        config.firstTimeUser = "false";
                        //initialize new db with chosen filename
                        string filePath = saveFileDialog.FileName;
                        int index = filePath.LastIndexOf('\\') + 1;
                        model = new HomeBudget(filePath, true);
                        config.lastUsedFilePath = filePath;
                        config.currentFile = filePath.Substring(index);
                        config.saveConfig();
                        view.ShowFilesCreated(config.lastUsedFilePath);
                    }
                }
            }
            else
            {
                //newDB is false (opening file)

                //ask for budget folder location
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string initialPath = config.lastUsedFilePath;
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
                    config.firstTimeUser = "False";
                    //initialize db using filename
                    string filePath = openFileDialog.FileName;
                    index = filePath.LastIndexOf('\\') + 1;
                    model = new HomeBudget(filePath);
                    config.lastUsedFilePath = filePath;
                    config.currentFile = filePath.Substring(index);
                    config.saveConfig();
                }
            }
        }
        public void BudgetItemsList(DateTime? start, DateTime? end, bool filterFlag = false, int categoryID = -1)
        {
            List<BudgetItem> budgetItemsList = model.GetBudgetItems(start, end, filterFlag, categoryID);
            view.ShowBudgetItems(budgetItemsList);
        }
        public void DeleteExpense(int id)
        {
            model.expenses.Delete(id);

        }

    }
}