using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Budget;

namespace HomeBudgetWPF
{
    class Presenter
    {
        const string DEFAULT_PATH = "\\Documents\\BudgetFiles\\";
        const string DEFAULT_FILENAME = "budget.db";
        const string DEFAULT_FILEPATH = DEFAULT_PATH + DEFAULT_FILENAME;
        //const string DEFAULT_FILENAME = "./budget.db";
        HomeBudget model;
        IView view;
        
        public Presenter(IView v)
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
