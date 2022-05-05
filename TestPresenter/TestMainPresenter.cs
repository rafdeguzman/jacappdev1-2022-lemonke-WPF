using System;
using Xunit;
using HomeBudgetWPF;
using System.Collections.Generic;
using Budget;

namespace TestPresenter
{
    [Collection("Sequential")]
    public class TestView : ViewInterface
    {
        public bool calledShowFilesCreated;
        public bool calledShowFirstTimeMessage;
        public bool calledShowBudgetItems;
        public bool calledShowBudgetItemsByMonth;
        public bool calledShowBudgetItemsByCategory;
        public bool calledShowBudgetItemsByMonthAndCategory;
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
        public void ShowBudgetItemsByMonth(List<BudgetItemsByMonth> budgetItemsListByMonth)
        {
            calledShowBudgetItemsByMonth = true;
        }

        public void ShowBudgetItemsByCategory(List<BudgetItemsByCategory> budgetItemsListByCategory)
        {
            calledShowBudgetItemsByCategory = true;
        }

        public void ShowBudgetItemsMonthAndCategory(List<Dictionary<string, object>> budgetItemsListByMonthAndCategory)
        {
            calledShowBudgetItemsByMonthAndCategory = true;
        }

        public DateTime? GetStartDate()
        {
            throw new NotImplementedException();
        }

        public DateTime? GetEndDate()
        {
            throw new NotImplementedException();
        }

        public bool GetFilterFlag()
        {
            throw new NotImplementedException();
        }

        public int GetCategoryId()
        {
            throw new NotImplementedException();
        }

        public TestView()
        {
            config = new Config();
        }

        public class TestPresenter
        {
            [Fact]      // sequential tests testing
            public void anotherTest()
            {
                TestView testView = new TestView();
                Presenter p = new Presenter(testView, true);
                TestCategoryView testCatView = new TestCategoryView();
                CategoryPresenter testCatP = new CategoryPresenter(testCatView, p.GetModel());
                Assert.True(testCatView.calledDisplayCategories);
                Assert.True(testCatView.calledDisplayCategoryTypes);
                testCatP.closeDb();
            }
            [Fact]
            public void TestConstructor()
            {

                TestView testView = new TestView();
                Presenter p = new Presenter(testView, true);
                Assert.IsType<Presenter>(p);
                Assert.True(testView.calledShowFilesCreated);
                Assert.True(testView.calledShowFirstTimeMessage);
                p.closeDb();

                testView = new TestView();
                p = new Presenter(testView, true);
                p.Filter("", "BudgetItems", null, null);
                Assert.True(testView.calledShowBudgetItems);
                p.closeDb();

                testView = new TestView();
                p = new Presenter(testView, true);
                p.Filter("", "BudgetItemsByMonth", null, null);
                Assert.True(testView.calledShowBudgetItemsByMonth);
                p.closeDb();

                testView = new TestView();
                p = new Presenter(testView, true);
                p.Filter("", "BudgetItemsByCategory", null, null);
                Assert.True(testView.calledShowBudgetItemsByCategory);
                p.closeDb();

                testView = new TestView();
                p = new Presenter(testView, true);
                p.Filter("", "BudgetItemsByMonthAndCategory", null, null);
                Assert.True(testView.calledShowBudgetItemsByMonthAndCategory);
                p.closeDb();

                testView = new TestView();
                p = new Presenter(testView, true);
                TestCategoryView testCatView = new TestCategoryView();
                CategoryPresenter testCatP = new CategoryPresenter(testCatView, p.GetModel());
                Assert.True(testCatView.calledDisplayCategories);
                Assert.True(testCatView.calledDisplayCategoryTypes);
                testCatP.closeDb();

                testView = new TestView();
                p = new Presenter(testView, true);
                testCatView = new TestCategoryView();
                testCatP = new CategoryPresenter(testCatView, p.GetModel());
                testCatP.AddCategory(2);
                Assert.True(testCatView.calledGetStringInput);
                testCatP.closeDb();

                testView = new TestView();
                p = new Presenter(testView, true);
                testCatView = new TestCategoryView();
                testCatP = new CategoryPresenter(testCatView, p.GetModel());
                Assert.IsType<CategoryPresenter>(testCatP);
                Assert.True(testCatView.calledDisplayCategories);
                Assert.True(testCatView.calledDisplayCategoryTypes);
                testCatP.closeDb();

                testView = new TestView();
                p = new Presenter(testView, true);
                TestExpenseView testExpView = new TestExpenseView();
                ExpensePresenter testExpP = new ExpensePresenter(testExpView, p.GetModel());
                Assert.IsType<ExpensePresenter>(testExpP);
                Assert.True(testExpView.calledDisplayCategories);
                testExpP.closeDb();


                testView = new TestView();
                p = new Presenter(testView, true);
                testExpView = new TestExpenseView();
                testExpP = new ExpensePresenter(testExpView, p.GetModel());
                testExpP.AddExpense(DateTime.Now, 1, 100, "abc", true);
                Assert.True(testExpView.calledLastInput);
                testExpP.closeDb();
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

        public TestCategoryView()
        {
            config = new Config();
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
