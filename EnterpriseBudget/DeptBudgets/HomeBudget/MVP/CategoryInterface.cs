using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseBudget.DeptBudgets.HomeBudget
{
    public interface CategoryInterface
    {
        string GetStringInput();
        void DisplayCategories(List<string> categories);
        void DisplayCategoryTypes(List<string> categoryTypes);
    }
}
