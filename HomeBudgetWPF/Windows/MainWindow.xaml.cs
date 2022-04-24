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
using Budget;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        const int EXTENSION_LENGTH = 3;
        private readonly Presenter presenter;
        Config config;
        public MainWindow()
        {
            InitializeComponent();
            config = new Config();
            presenter = new Presenter(this, config.newDB);
            SetCurrentFile();

            //get expenses

            presenter.BudgetItemsList(null, null);

        }
        private void SetCurrentFile()
        {
            config = new Config();  //call new config to "refresh" the data
            string configFileName = config.currentFile;
            int index = configFileName.LastIndexOf('\\') + 1;
            configFileName.Substring(index);
            fileName.Text = "Current file: " + configFileName.Remove(configFileName.Length -    EXTENSION_LENGTH);
        }
        private void Expense_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseWindow aew = new AddExpenseWindow();
            aew.Show();
            presenter.BudgetItemsList(null, null);
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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedItem = dataBudgetLists.SelectedItem;
            UpdateWindow.CallUpdateWindow(selectedItem.ExpenseID, selectedItem.CategoryID - 1, selectedItem.ShortDescription, selectedItem.Amount, selectedItem.Date);
            dataBudgetLists.Items.Refresh();
        }

        /// <summary>
        /// Shows a Message Box asking to create default files or to choose where and what to name budget file.
        /// </summary>
        /// <returns>True if result is yes, false otherwise.</returns>
        public bool ShowFirstTimeMessage()
        {
            string messageBoxText = "Would you like to create default budget files? If no, specify file location.";
            string caption = "New Database File";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            if (result == MessageBoxResult.Yes)
                return true;

            else if (result == MessageBoxResult.No)
                return false;

            else if (result == MessageBoxResult.Cancel)
                Environment.Exit(0);

            return false;
        }
        /// <summary>
        /// Shows a message box when called.
        /// </summary>
        /// <param name="path">File path showing where file is created</param>
        public void ShowFilesCreated(string path)
        {
            string messageBoxText = "Files created! \nFiles located in " + path;
            string caption = "Create Files Success";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result;
            result = MessageBox.Show(messageBoxText, caption, button, icon);
        }
        private void btnCloseAllWindows_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to force-close the app? Changes are automatically saved.",
                    "Close App",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }

        private void btnOpenFile(object sender, RoutedEventArgs e)
        {
            config.newDB = false;
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void btnSaveFile(object sender, RoutedEventArgs e)
        {
            config.newDB = true;
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        public void ShowBudgetItems(List<BudgetItem> budgetItems)
        {
            dataBudgetLists.ItemsSource = budgetItems;
        }


    }
}
