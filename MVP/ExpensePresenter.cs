using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace HomeBudgetWPF
{
    class ExpensePresenter
    {

        private static Dictionary<string, int> coordinate = new Dictionary<string, int>();
        private readonly ExpenseInterface view;
        private readonly HomeBudget model;

        //private static int previousCategory;
        //private static DateTime previousDate;
        //private static double previousAmount;
        //private static string previousDescristion;
        //private static bool previousIsCredit;
        public ExpensePresenter(ExpenseInterface v)
        {
            model = new HomeBudget("testDB");
            view = v;

            view.DisplayCategories(PopulateCategories());
            
        }

        public List<Category> PopulateCategories()
        {
            List<Category> categoriesList = new();
            foreach (Category categories in model.categories.List())
            {
                categoriesList.Add(categories);
            }
            return categoriesList;
        }

        public void AddExpense(DateTime dt, int catID, double amount, string desc, bool? isChecked)
        {
            
            string isCredit = "";
            Category categoryType = model.categories.GetCategoryFromId(catID+1);
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

            view.LastInput(categoryType.Type.ToString(), dt.ToString(),amount.ToString(), desc,isCredit);
        }
    }

}
