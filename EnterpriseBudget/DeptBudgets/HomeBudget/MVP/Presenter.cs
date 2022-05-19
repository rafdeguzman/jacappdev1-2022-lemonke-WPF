using Budget;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EnterpriseBudget.DeptBudgets.HomeBudget
{
    public class Presenter
    {
        Model.DepartmentBudgets budget;
        int deptId;
        ViewInterface view;
        Budget.HomeBudget model;
        Config config;
        // change this code because we're downloading the sql file from the jac servers
        public Presenter(ViewInterface v, string filePath, int deptId)
        {
            this.deptId = deptId;
            config = new Config();
            //config file setup
            view = v;
            // use filePath given by MainWindow
            LoadData();
            model = new Budget.HomeBudget(filePath, false);


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
                    if (search != string.Empty)
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
                    if (search != string.Empty)
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
                    if (search != string.Empty)
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
                case "BudgetItemsByMonthAndCategory":
                    List<Dictionary<string, object>> budgetItemsByCategoryAndMonth = model.GetBudgetDictionaryByCategoryAndMonth(Start, End, FilterFlag, CategoryId);
                    if (search != string.Empty)
                    {
                        List<Dictionary<string, object>> searchedBudgetItemsByCategoryAndMonth = new List<Dictionary<string, object>>();
                        foreach (Dictionary<string, object> d in budgetItemsByCategoryAndMonth)
                        {
                            Dictionary<string, object> dictionary = new Dictionary<string, object>();
                            foreach (KeyValuePair<string, object> s in d)
                            {
                                if (s.Key.ToLower().Contains(search) || s.Value.ToString().ToLower().Contains(search))
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
        public Budget.HomeBudget GetModel()
        {
            return model;
        }

        public void GeneratePieChart()
        {           
            UserControlWindow ucw = new UserControlWindow(this);
            if (ucw.GetCountItems() > 0)
                ucw.Show();
            else
                ucw.ShowError();
        }

        public List<object> GetDataSource()
        {
            List<Dictionary<string, object>> myItems = model.GetBudgetDictionaryByCategoryAndMonth(view.GetStartDate(), view.GetEndDate(), false, view.GetCategoryId());
            return myItems.Cast<object>().ToList();            
        }

        /// <summary>
        /// Get the data from the database, etc
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        public bool LoadData()
        {
            budget = new Model.DepartmentBudgets();
            return budget.DownLoadAndOpenDepartmentBudgetFile(deptId);
        }

        public string getFilePath()
        {
            return budget.getFilePath();
        }


        /// <summary>
        /// The view is closing, and needs to tidy-up by calling
        /// this routine.
        /// </summary>
        public void onClose()
        {
            if (budget != null)
            {
                budget.Close();
            }
        }

        public void setDeptId(int depId)
        {
            this.deptId = depId;
        }
        public void Save()
        {
            budget.SaveToSQLServer(deptId);
        }
        public int GetDeptId()
        {
            return this.deptId;
        }
    }
}