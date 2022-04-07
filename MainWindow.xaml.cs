using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        //encapsulate each in presenter view etc after.

        //IF BUDGET.DB DOESNT EXIST IT MEANS IT'S A FIRST TIME USER
        //show messagebox window and ask if they want to create using DEFAULT PATH
        //or select the directory
        //const string DEFAULT_DIRECTORY = "\\Documents\\BudgetFiles\\";
        //const string DEFAULT_FILENAME = "budget.db";
        //const string DEFAULT_FILEPATH = DEFAULT_DIRECTORY + DEFAULT_FILENAME

        private readonly Presenter presenter;
        public MainWindow()
        {
            InitializeComponent();
            presenter = new Presenter(this);
            SetCurrentFile();
        }
        private void SetCurrentFile()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string configFileName = config.AppSettings.Settings["currentFile"].Value;
            fileName.Text = "Current file: " + configFileName.Remove(configFileName.Length - 3);
        }
        private void Expense_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseWindow aew = new AddExpenseWindow();
            aew.ShowDialog();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            CategoryWindow cw = new CategoryWindow();
            cw.ShowDialog();
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Show();
        }
        public bool ShowFirstTimeMessage()
        {
            string messageBoxText = "Would you like to create default budget files? If no, specify file location.";
            string caption = "First Time User";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            if (result == MessageBoxResult.Yes)
                return true;
            
            else if(result == MessageBoxResult.No)
                return false;
            
            return false;
        }

        private void btnCloseAllWindows_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close app!!!",
                    "Close App",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }
    }
}
