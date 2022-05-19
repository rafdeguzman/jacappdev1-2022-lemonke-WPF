using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace EnterpriseBudget.DeptBudgets.HomeBudget
{
    public interface ExpenseInterface
    {
        bool CheckUserInput();
        void GetUserInput();
        void ResetText();
        void DisplayCategories(List <Category> categories);
        void DisplaySameAsLastInput();

    }
}
