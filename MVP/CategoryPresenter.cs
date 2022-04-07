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
    class CategoryPresenter
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        HomeBudget model;
        CategoryInterface view;

        public CategoryPresenter(CategoryInterface v)
        {
            var settings = config.AppSettings.Settings;
            string filePath = settings["lastUsedFilePath"].Value;
            model = new HomeBudget(filePath, !File.Exists(filePath));
            view = v;

            view.DisplayCategories(CategoryPopulateCategories());
            view.DisplayCategoryTypes(PopulateCategoryTypes());
            
        }
        public void AddCategory(int categoryType)
        {
            model.categories.Add(view.GetStringInput(), (Category.CategoryType)categoryType + 1);
        }

        public List<string> CategoryPopulateCategories()
        {
            List<Category> categoriesList = new List<Category>();
            foreach (Category categories in model.categories.List())
                categoriesList.Add(categories);

            List<string> categoriesListString = new List<string>();
            foreach (var category in categoriesList)
                categoriesListString.Add(category.ToString());

            return categoriesListString;
        }
        
        public List<string> PopulateCategoryTypes()
        {
            List<string> categoryTypes = new List<string>();
            foreach (var categoryType in Enum.GetValues(typeof(Category.CategoryType)))
                categoryTypes.Add(categoryType.ToString());
            return categoryTypes;
        }

    }
}