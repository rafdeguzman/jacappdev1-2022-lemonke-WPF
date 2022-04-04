using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace HomeBudgetWPF
{
    class Presenter
    {

        private static Dictionary<string, int> coordinate = new Dictionary<string, int>();
        private readonly ViewInterface view;
        private readonly HomeBudget model;
        public Presenter(ViewInterface v)
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

            model.categories.
            if (isChecked == false)
            {
                if ( == credit || catID == saving)
                {
                    model.expenses.Add(dt, catID, amount*-1, desc);
                }
                else
                {
                    model.expenses.Add(dt, catID, amount, desc);
                }
            
                
            }
            else
            {
                model.expenses.Add(dt, catID, amount * -1, desc);
                model.expenses.Add(dt, catID, amount, desc);
            }
        }
    }

}
