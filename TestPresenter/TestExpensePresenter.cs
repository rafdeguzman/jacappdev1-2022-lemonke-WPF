using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeBudgetWPF;
using Budget;
namespace TestPresenter
{
    public class TestExpenseView : ExpenseInterface
    {
        public bool CheckUserInput()
        {
            throw new NotImplementedException();
        }

        public void DisplayCategories(List<Category> categories)
        {
            throw new NotImplementedException();
        }

        public void DisplaySameAsLastInput()
        {
            throw new NotImplementedException();
        }

        public void GetUserInput()
        {
            throw new NotImplementedException();
        }

        public void LastInput(string categories, string date, string amount, string description, string creditFlag)
        {
            throw new NotImplementedException();
        }

        public void ResetText()
        {
            throw new NotImplementedException();
        }
    }
    class TestExpensePresenter
    {

    }
}
