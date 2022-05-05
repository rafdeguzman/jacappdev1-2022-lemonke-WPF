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
            //[Fact]
            //public void anotherTEst()
            //{
            //    TestView testView = new TestView();
            //    Presenter p = new Presenter(testView, true);
            //    TestCategoryView testCatView = new TestCategoryView();
            //    CategoryPresenter testCatP = new CategoryPresenter(testCatView, p.GetModel());
            //    Assert.True(testCatView.calledDisplayCategories);
            //    Assert.True(testCatView.calledDisplayCategoryTypes);
            //    testCatP.closeDb();
            //}
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

                //p = new Presenter(testView, true);
                //TestCategoryView testCatView = new TestCategoryView();
                //CategoryPresenter testCatP = new CategoryPresenter(testCatView, p.GetModel());
                //Assert.True(testCatView.calledDisplayCategories);
                //Assert.True(testCatView.calledDisplayCategoryTypes);
                //p.closeDb();

                //p = new Presenter(testView, true);
                //testCatView = new TestCategoryView();
                //testCatP = new CategoryPresenter(testCatView, p.GetModel());
                //testCatP.AddCategory(2);
                //Assert.True(testCatView.calledGetStringInput);
                //p.closeDb();

                //testcategoryview view1 = new testcategoryview();
                //categorypresenter p1 = new categorypresenter(view1, p.getmodel());
                //assert.istype<categorypresenter>(p1);
                //assert.true(view1.calleddisplaycategories);
                //assert.true(view1.calleddisplaycategorytypes);

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
