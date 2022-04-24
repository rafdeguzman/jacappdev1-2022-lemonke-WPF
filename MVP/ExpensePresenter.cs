﻿using System;
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
    public class ExpensePresenter
    {
        Config config;

        private readonly ExpenseInterface view;
        private readonly HomeBudget model;

        private static DateTime previousDate ;
        private static int previousCategoryID;
        private static double previousAmount;
        private static string previousDescription;
        private static bool previousIsChecked;
        private static bool userInputFromDuplicateExpense;

        string filePath = ConfigurationManager.AppSettings.Get("lastUsedFilePath");

        public ExpensePresenter()
        {
            config = new Config();
            string filePath = config.lastUsedFilePath;
            model = new HomeBudget(filePath, !File.Exists(filePath));

        }
        public ExpensePresenter(ExpenseInterface v)
        {
            config = new Config();
            string filePath = config.lastUsedFilePath;
            model = new HomeBudget(filePath, !File.Exists(filePath));

            view = v;

            view.DisplayCategories(ExpensePopulateCategories());
        }

        public List<Category> ExpensePopulateCategories()
        {
            List<Category> categoriesList = new();
            foreach (Category categories in model.categories.List())
            {
                categoriesList.Add(categories);
            }
            return categoriesList;
        }

        public void AddExpense(DateTime dt, int catID, double amount, string desc, bool isChecked)
        {
            string isCredit = "";
            if (SameInputAsLastInput(dt, catID, amount, desc, isChecked))
            {
                Category categoryType = model.categories.GetCategoryFromId(catID + 1);
                if (isChecked == false)
                {
                    if (categoryType.Type == Category.CategoryType.Credit || categoryType.Type == Category.CategoryType.Savings)
                    {
                        model.expenses.Add(dt, catID, amount * -1, desc);
                    }
                    else
                    {
                        model.expenses.Add(dt, catID, amount, desc);
                    }
                    isCredit = "Credit Unchecked";
                }
                else
                {
                    model.expenses.Add(dt, catID, amount * -1, desc);
                    model.expenses.Add(dt, catID, amount, desc);
                    isCredit = "Credit Checked";
                }
                view.LastInput(categoryType.Type.ToString(), dt.ToString("yyyy-MM-dd"),amount.ToString(), desc,isCredit);
            }
        }

        private bool SameInputAsLastInput(DateTime dt, int catID, double amount, string desc, bool isChecked)
        {
            if (previousAmount == amount && previousDate == dt && previousCategoryID == catID && previousDescription == desc && previousIsChecked == isChecked)
            {
                view.DisplaySameAsLastInput();
                if (GetUserInputFromDuplicateExpense())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                previousDate = dt;
                previousCategoryID = catID;
                previousAmount = amount;
                previousDescription = desc;
                previousIsChecked = isChecked;
                return true;
            }
        }
        public void setUserInputFromDuplicateExpense(bool response)
        {
            userInputFromDuplicateExpense = response;
        }
        private bool GetUserInputFromDuplicateExpense()
        {
            return userInputFromDuplicateExpense;
        }

        public List<BudgetItem> BudgetItemsList()
        {
             List<BudgetItem> budgetItemsList =model.GetBudgetItems(null,null,false , -1 );

            return budgetItemsList;
            
        }
    }

}
