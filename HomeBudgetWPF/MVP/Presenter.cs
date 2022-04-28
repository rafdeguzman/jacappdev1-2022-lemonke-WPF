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
        public List<Category> GetCategories()
        {
            return model.categories.List();
        }
        public void BudgetItemsList(DateTime? start, DateTime? end, int categoryID = -1 , bool FilterFlag = false)
        {
                List<BudgetItem> budgetItemsList = model.GetBudgetItems(start, end, FilterFlag, categoryID);
                view.ShowBudgetItems(budgetItemsList);
        }
        public void BudgetByCategory(DateTime? start, DateTime? end, int categoryID = -1, bool FilterFlag = false )
        {
            List<BudgetItemsByCategory> budgetItemsList = model.GetBudgetItemsByCategory(start, end, FilterFlag, categoryID);
            view.ShowBudgetItemsByCategory(budgetItemsList);
        }
        public void BudgetByDate(DateTime? start, DateTime? end, int categoryID = -1, bool FilterFlag = false)
        {
            List<BudgetItemsByMonth> budgetItemsList = model.GetBudgetItemsByMonth(start, end, FilterFlag, categoryID);
            view.ShowBudgetItemsByDate(budgetItemsList);
        }
        public void BudgetIByDateandCategory(DateTime? start, DateTime? end, int categoryID = -1, bool FilterFlag = false)
        {
            List <Dictionary<string, object>> budgetItemsList = model.GetBudgetDictionaryByCategoryAndMonth(start, end, FilterFlag, categoryID);
            view.ShowBudgetItemsDateAndCategory(budgetItemsList);
        }

        public void DeleteExpense(int id)
        {
            model.expenses.Delete(id);
        }

        public void Search(string search, string returnType, DateTime Start, DateTime End, bool FilterFlag, int CategoryId)
        {
            switch (returnType)
            {
                case "BudgetItem":
                    List<BudgetItem> budgetItems = model.GetBudgetItems(Start, End, FilterFlag, CategoryId);
                    List<BudgetItem> searchedItems = new List<BudgetItem>();
                    foreach (BudgetItem bi in budgetItems)
                    {
                        if (bi.Amount.ToString().Contains(search) ||
                            bi.Balance.ToString().Contains(search) ||
                            bi.Category.ToString().Contains(search) ||
                            bi.CategoryID.ToString().Contains(search) ||
                            bi.Date.ToString().Contains(search) ||
                            bi.ExpenseID.ToString().Contains(search) ||
                            bi.ShortDescription.ToString().Contains(search))
                        {
                            searchedItems.Add(bi);
                        }
                    }
                    view.ShowBudgetItems(searchedItems);
                    break;
                case "BudgetItemsByCategory":
                    List<BudgetItemsByCategory> budgetItemsByCategory = model.GetBudgetItemsByCategory(Start, End, FilterFlag, CategoryId);
                    List<BudgetItemsByCategory> searchedBudgetItemsByCategory = new List<BudgetItemsByCategory>();
                    foreach(BudgetItemsByCategory bibc in budgetItemsByCategory)
                    {
                        if(bibc.Category.ToString().Contains(search) ||
                            bibc.CategoryID.ToString().Contains(search) ||
                            bibc.Details.ToString().Contains(search) ||
                            bibc.Total.ToString().Contains(search))
                        {
                            searchedBudgetItemsByCategory.Add(bibc);
                        }
                    }
                    view.ShowBudgetItemsByCategory(searchedBudgetItemsByCategory);
                    break;
                case "BudgetItemsByMonth":
                    List<BudgetItemsByMonth> budgetItemsByMonth = model.GetBudgetItemsByMonth(Start, End, FilterFlag, CategoryId);
                    List<BudgetItemsByMonth> searchedBudgetItemsByMonth = new List<BudgetItemsByMonth>();
                    foreach(BudgetItemsByMonth bibm in budgetItemsByMonth)
                    {
                        if(bibm.Details.ToString().Contains(search) ||
                            bibm.Month.ToString().Contains(search) ||
                            bibm.Total.ToString().Contains(search))
                        {
                            searchedBudgetItemsByMonth.Add(bibm);
                        }
                    }
                    view.ShowBudgetItemsByDate(searchedBudgetItemsByMonth);
                    break;
                case "budgetItemsByCategoryAndMonth":
                    List<Dictionary<string, object>> budgetItemsByCategoryAndMonth = model.GetBudgetDictionaryByCategoryAndMonth(Start, End, FilterFlag, CategoryId);
                    List<Dictionary<string, object>> searchedBudgetItemsByCategoryAndMonth = new List<Dictionary<string, object>>();
                    foreach (Dictionary<string, object> o in budgetItemsByCategoryAndMonth)
                    {
                        foreach(string s in o.Values)
                        {
                            if (!s.Contains(search))
                            {
                                break;
                            }
                        }
                    }
                    break;

            }
            
        }

    }
}