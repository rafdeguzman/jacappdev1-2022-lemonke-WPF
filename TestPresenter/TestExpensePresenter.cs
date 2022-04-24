using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeBudgetWPF;
using Budget;
using Xunit;
namespace TestPresenter
{
    public class TestExpenseView : ExpenseInterface
    {
        public bool calledCheckUserInput;
        public bool calledDisplayCategories;
        public bool calledDisplaySameAsLastInput;
        public bool calledGetUserInput;
        public bool calledLastInput;
        public bool calledResetText;
        Config config;
        public bool CheckUserInput()
        {
            calledCheckUserInput = true;
            return calledCheckUserInput;
        }

        public void DisplayCategories(List<Category> categories)
        {
            calledDisplayCategories = true;
        }

        public void DisplaySameAsLastInput()
        {
            calledDisplaySameAsLastInput = true;
        }

        public void GetUserInput()
        {
            calledGetUserInput = true; 
        }

        public void LastInput(string categories, string date, string amount, string description, string creditFlag)
        {
            calledLastInput = true;
        }

        public void ResetText()
        {
            calledResetText = true; 
        }
        public TestExpenseView()
        {
            config = new Config();
        }
    }

    public class TestExpensePresenter
    {
        [Fact]
        public void TestExpenseConstructor()
        {
            TestExpenseView view = new TestExpenseView();
            ExpensePresenter p = new ExpensePresenter(view);
            Assert.IsType<ExpensePresenter>(p);
            Assert.True(view.calledDisplayCategories);
        }
        [Fact]
        public void TestAddExpense()
        {
            TestExpenseView view = new TestExpenseView();
            ExpensePresenter p = new ExpensePresenter(view);
            p.AddExpense(DateTime.Now, 1, 100, "abc", true);
            Assert.True(view.calledLastInput);
        }
    }
}
