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

        public TestView()
        {
            config = new Config();
        }

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
        }
    }
    
}
