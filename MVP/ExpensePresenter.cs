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
    class ExpensePresenter
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private readonly ExpenseInterface view;
        private readonly HomeBudget model;

        private static DateTime previousDate ;
        private static int previousCategoryID;
        private static double previousAmount;
        private static string previousDescription;
        private static bool previousIsChecked;
        private static bool userInputFromDuplicateExpense;

        string filePath = ConfigurationManager.AppSettings.Get("lastUsedFilePath");

        public ExpensePresenter(ExpenseInterface v)
        {
            //get config settings and read database filepath
            var settings = config.AppSettings.Settings;
            string filePath = settings["lastUsedFilePath"].Value;
            model = new HomeBudget(filePath, !File.Exists(filePath));

            view = v;

            view.DisplayCategories(ExpensePopulateCategories());
        }

        /// <summary>
        /// Populate the combox box with list of categories
        /// </summary>
        /// <returns>List of the Categories</returns>
        public List<Category> ExpensePopulateCategories()
        {
            List<Category> categoriesList = new List<Category>();
            foreach (Category categories in model.categories.List())
            {
                categoriesList.Add(categories);
            }
            return categoriesList;
        }

        /// <summary>
        /// Add the expense to the database
        /// </summary>
        /// <param name="dt">Date when the was bought </param>
        /// <param name="catID"> Category Id</param>
        /// <param name="amount">the amount of the expense </param>
        /// <param name="desc">Description of the expense </param>
        /// <param name="isChecked">If credit was user for the expense</param>
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
        /// <summary>
        /// Check if the user want expense in the database
        /// </summary>
        /// <param name="dt">Date when the was bought </param>
        /// <param name="catID"> Category Id</param>
        /// <param name="amount">the amount of the expense </param>
        /// <param name="desc">Description of the expense </param>
        /// <param name="isChecked">If credit was user for the expense</param>
        /// <returns></returns>
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

        /// <summary>
        /// Set the userInputFromDuplicateExpense 
        /// </summary>
        /// <param name="response"> This is the user input </param>
        public void SetUserInputFromDuplicateExpense(bool response)
        {
            userInputFromDuplicateExpense = response;
        }
        private bool GetUserInputFromDuplicateExpense()
        {
            return userInputFromDuplicateExpense;
        }
    }

}
