using System;
using Xunit;
using HomeBudgetWPF;
using System.Collections.Generic;
using Budget;

namespace TestPresenter
{
    public class TestView : ViewInterface
    {
        public bool calledShowFilesCreated;
        public bool calledShowFirstTimeMessage;
        public bool calledShowBudgetItems;
        Config config;
        public void ShowFilesCreated(string path)
        {
            calledShowFilesCreated = true;
        }

        public bool ShowFirstTimeMessage()
        {
            calledShowFirstTimeMessage = true;
            return true;
        }

        public void ShowBudgetItems(List<BudgetItem> budgetItemsList)
        {
            calledShowBudgetItems = true;
        }

        public void ShowBudgetItemsByDate(List<BudgetItemsByMonth> budgetItemsListByMonth)
        {
            throw new NotImplementedException();
        }

        public void ShowBudgetItemsByCategory(List<BudgetItemsByCategory> budgetItemsListByCategory)
        {
            throw new NotImplementedException();
        }

        public void ShowBudgetItemsDateAndCategory(List<Dictionary<string, object>> budgetItemsListByMonthAndCategory)
        {
            throw new NotImplementedException();
        }

        public TestView()
        {
            config = new Config();
        }

        public class TestPresenter
        {
            [Fact]
            public void TestConstructor()
            {
                TestView view = new TestView();
                Presenter p = new Presenter(view, true);
                Assert.IsType<Presenter>(p);
                Assert.True(view.calledShowFilesCreated);
                Assert.True(view.calledShowFirstTimeMessage);

                TestCategoryView view1 = new TestCategoryView();
                CategoryPresenter p1 = new CategoryPresenter(view1, p.GetModel());
                Assert.IsType<CategoryPresenter>(p1);
                Assert.True(view1.calledDisplayCategories);
                Assert.True(view1.calledDisplayCategoryTypes);

                TestExpenseView view2 = new TestExpenseView();
                ExpensePresenter p2 = new ExpensePresenter(view2, p.GetModel());
                Assert.IsType<ExpensePresenter>(p2);
                Assert.True(view2.calledDisplayCategories);

                TestExpenseView view3 = new TestExpenseView();
                ExpensePresenter p3 = new ExpensePresenter(view3, p.GetModel());
                p3.AddExpense(DateTime.Now, 1, 100, "abc", true);
                Assert.True(view3.calledLastInput);
            }
        }
    }
    
    public class TestCategoryView : CategoryInterface
    {
        public bool calledDisplayCategories;
        public bool calledDisplayCategoryTypes;
        public bool calledGetStringInput;
        Config config;
        public void DisplayCategories(List<string> categories)
        {
            calledDisplayCategories = true;
        }

        public void DisplayCategoryTypes(List<string> categoryTypes)
        {
            calledDisplayCategoryTypes = true;
        }

        public string GetStringInput()
        {
            calledGetStringInput = true;
            return String.Empty;
        }
    }
    
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
    
}
