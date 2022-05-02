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
        Config config;
        public Presenter(ViewInterface v, bool newDB = false)
        {
            string defaultDirectory;
            config = new Config();
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
                    bool? saveFile= saveFileDialog.ShowDialog();
                    switch (saveFile)
                    {
                        case true:
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
                            break;
                        case false:
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                //newDB is false (opening file)

                // open recent file
                if (config.recentDB)
                {
                    //uses last file
                    model = new HomeBudget(config.lastUsedFilePath, false);
                }
                else
                {
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
                    bool? openFile = openFileDialog.ShowDialog();
                    switch (openFile)
                    {
                        case true:
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
                            break;
                        case false:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public List<Category> GetCategories()
        {
            return model.categories.List();
        }

        public void DeleteExpense(int id)
        {
            model.expenses.Delete(id);
        }

        public void Filter(string search, string returnType, DateTime? Start, DateTime? End, bool FilterFlag = false, int CategoryId = -1)
        {
            search = search.ToLower();
            switch (returnType)
            {
                case "BudgetItems":
                    List<BudgetItem> budgetItems = model.GetBudgetItems(Start, End, FilterFlag, CategoryId);
                    if(search != string.Empty)
                    {
                        List<BudgetItem> searchedItems = new List<BudgetItem>();
                        foreach (BudgetItem bi in budgetItems)
                        {
                            if (bi.Amount.ToString().ToLower().Contains(search) ||
                                bi.Balance.ToString().ToLower().Contains(search) ||
                                bi.Category.ToString().ToLower().Contains(search) ||
                                bi.CategoryID.ToString().ToLower().Contains(search) ||
                                bi.Date.ToString().ToLower().Contains(search) ||
                                bi.ExpenseID.ToString().ToLower().Contains(search) ||
                                bi.ShortDescription.ToString().ToLower().Contains(search))
                            {
                                searchedItems.Add(bi);
                            }
                        }
                        budgetItems = searchedItems;
                    }                    
                    view.ShowBudgetItems(budgetItems);
                    break;
                case "BudgetItemsByCategory":
                    List<BudgetItemsByCategory> budgetItemsByCategory = model.GetBudgetItemsByCategory(Start, End, FilterFlag, CategoryId);
                    if(search != string.Empty)
                    {
                        List<BudgetItemsByCategory> searchedBudgetItemsByCategory = new List<BudgetItemsByCategory>();
                        foreach (BudgetItemsByCategory bibc in budgetItemsByCategory)
                        {
                            if (bibc.Category.ToString().ToLower().Contains(search) ||
                                bibc.CategoryID.ToString().ToLower().Contains(search) ||
                                bibc.Details.ToString().ToLower().Contains(search) ||
                                bibc.Total.ToString().ToLower().Contains(search))
                            {
                                searchedBudgetItemsByCategory.Add(bibc);
                            }
                        }
                        budgetItemsByCategory = searchedBudgetItemsByCategory;
                    }                    
                    view.ShowBudgetItemsByCategory(budgetItemsByCategory);
                    break;
                case "BudgetItemsByMonth":
                    List<BudgetItemsByMonth> budgetItemsByMonth = model.GetBudgetItemsByMonth(Start, End, FilterFlag, CategoryId);
                    if(search != string.Empty)
                    {
                        List<BudgetItemsByMonth> searchedBudgetItemsByMonth = new List<BudgetItemsByMonth>();
                        foreach (BudgetItemsByMonth bibm in budgetItemsByMonth)
                        {
                            if (bibm.Details.ToString().ToLower().Contains(search) ||
                                bibm.Month.ToString().ToLower().Contains(search) ||
                                bibm.Total.ToString().ToLower().Contains(search))
                            {
                                searchedBudgetItemsByMonth.Add(bibm);
                            }
                        }
                        budgetItemsByMonth = searchedBudgetItemsByMonth;
                    }                    
                    view.ShowBudgetItemsByMonth(budgetItemsByMonth);
                    break;
                case "budgetItemsByMonthAndCategory":
                    List<Dictionary<string, object>> budgetItemsByCategoryAndMonth = model.GetBudgetDictionaryByCategoryAndMonth(Start, End, FilterFlag, CategoryId);
                    if(search != string.Empty)
                    {
                        List<Dictionary<string, object>> searchedBudgetItemsByCategoryAndMonth = new List<Dictionary<string, object>>();
                        foreach (Dictionary<string, object> d in budgetItemsByCategoryAndMonth)
                        {
                            Dictionary<string, object> dictionary = new Dictionary<string, object>();
                            foreach (KeyValuePair<string, object> s in d)
                            {
                                if (s.Key.ToLower().Contains(search) ||
                                    s.Value.ToString().ToLower().Contains(search))
                                {
                                    dictionary.Add(s.Key, s.Value);
                                }
                            }
                            searchedBudgetItemsByCategoryAndMonth.Add(dictionary);
                        }
                        budgetItemsByCategoryAndMonth = searchedBudgetItemsByCategoryAndMonth;
                    }
                    view.ShowBudgetItemsMonthAndCategory(budgetItemsByCategoryAndMonth);
                    break;

            }
            
        }

        public void closeDb()
        {
            model.CloseDB();
        }

    }
}