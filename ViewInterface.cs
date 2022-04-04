using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace HomeBudgetWPF
{
    public interface ViewInterface
    {
        void GetUserInput();
        void ResetText();
        void LastInput (Categories categories, DateTime date, double amount, string description, bool creditFlag);
        void DisplayCategories(List <Category> categories);

    }
}
