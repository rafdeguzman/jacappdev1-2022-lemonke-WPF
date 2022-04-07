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
        /// <summary>
        /// Adds categories from model to local categories list.
        /// </summary>
        /// <param name="categoryType">Integer taken from CategoryType enum</param>
        public void AddCategory(int categoryType)
        {
            model.categories.Add(view.GetStringInput(), (Category.CategoryType)categoryType + 1);
        }
        /// <summary>
        /// Takes all categories from model.categories.List() output, and stores it in local categories list.
        /// This list is used to convert into string.
        /// </summary>
        /// <returns></returns>
        public List<string> CategoryPopulateCategories()
        {
            List<string> categoriesListString = new List<string>();
            foreach (var category in model.categories.List())
                categoriesListString.Add(category.ToString());

            return categoriesListString;
        }
        /// <summary>
        /// Converts from CategoryTypes enum to string and stores in categoryTypes list.
        /// </summary>
        /// <returns></returns>
        public List<string> PopulateCategoryTypes()
        {
            List<string> categoryTypes = new List<string>();
            foreach (var categoryType in Enum.GetValues(typeof(Category.CategoryType)))
                categoryTypes.Add(categoryType.ToString());
            return categoryTypes;
        }

    }
}