using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace HomeBudgetWPF
{
    public interface ExpenseInterface
    {
        bool CheckUserInput();
        void GetUserInput();
        void LastInput (string categories, string date, string amount, string description, string creditFlag);
        void DisplayCategories(List <Category> categories);

        void DisplaySameAsLastInput();

    }
}
