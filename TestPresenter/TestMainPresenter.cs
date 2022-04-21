using System;
using Xunit;
using HomeBudgetWPF;
namespace TestPresenter
{
    public class TestView : ViewInterface
    {
        public bool calledShowFilesCreated;
        public bool calledShowFirstTimeMessage;
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
        public TestView()
        {
            config = new Config();
        }

    }
    public class UnitTest
    {
        [Fact]
        public void TestConstructor()
        {
            TestView view = new TestView();
            Presenter p = new Presenter(view, true);
            Assert.IsType<Presenter>(p);
        }
    }
    
}
