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
        //const string DEFAULT_FILENAME = "budget.db";
        //const string DEFAULT_FILEPATH = DEFAULT_PATH + DEFAULT_FILENAME;
        const string DEFAULT_FILENAME = "./budget.db";
        HomeBudget model;
        IView view;

        public Presenter(IView v)
        {
            if (!Directory.Exists(DEFAULT_PATH))
                Directory.CreateDirectory(DEFAULT_PATH);

            model = new HomeBudget(DEFAULT_FILENAME, !File.Exists(DEFAULT_FILENAME));
            view = v;

            view.DisplayCategories(PopulateCategories());
            
        }
        public void AddCategory()
        {
            model.categories.Add(view.GetStringInput());
        }

        public List<string> PopulateCategories()
        {
            List<Category> categoriesList = new();
            foreach (Category categories in model.categories.List())
            {
                categoriesList.Add(categories);
            }

            List<string> categoriesListString = new List<string>();
            foreach (var category in categoriesList)
                categoriesListString.Add(category.ToString());

            return categoriesListString;
        }

    }
}