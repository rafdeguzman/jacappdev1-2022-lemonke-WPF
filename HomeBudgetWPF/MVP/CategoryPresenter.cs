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
    public class CategoryPresenter
    {
        HomeBudget model;
        CategoryInterface view;
        Config config;
        public CategoryPresenter(CategoryInterface v, HomeBudget model)
        {
            config = new Config();
            string filePath = config.lastUsedFilePath;
            this.model = model;
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

        public void closeDb()
        {
            model.CloseDB();
        }

    }
}