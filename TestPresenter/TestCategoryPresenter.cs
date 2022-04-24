using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using HomeBudgetWPF;

namespace TestPresenter
{
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

    public class TestCategoryPresenter
    {
        [Fact]
        public void TestCategoryConstructor()
        {
            TestCategoryView view = new TestCategoryView();
            CategoryPresenter p = new CategoryPresenter(view);
            Assert.IsType<CategoryPresenter>(p);
            Assert.True(view.calledDisplayCategories);
            Assert.True(view.calledDisplayCategoryTypes);
        }
    }
}
