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
            model = new HomeBudget("newDB");
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

        public Expense AddExpense()
        {


            return 
        }
    }
}
