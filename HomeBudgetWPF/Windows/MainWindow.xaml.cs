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
using System.Diagnostics;
using Budget;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        #region Backing Fields
        const int EXTENSION_LENGTH = 3;
        private readonly Presenter presenter;
        Config config;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for the main Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            config = new Config();
            presenter = new Presenter(this, config.newDB);
            SetCurrentFile();
            FilterByCategory.IsChecked = false;
            FilterByDate.IsChecked = false;
            //get expenses
            Refresh();
        }
        #endregion

        /// <summary>
        /// Fills the category combobox with all the categories
        /// </summary>
        /// <param name="categories">The list of categories to put in the combobox</param>
        private void DisplayCategories(List<Category> categories)
        {
            cmbCategory.DisplayMemberPath = "Description";
            cmbCategory.Items.Clear();
            foreach (var category in categories)
            {
                cmbCategory.Items.Add(category);
            }
        }

        /// <summary>
        /// Displays the current db file at the top
        /// </summary>
        private void SetCurrentFile()
        {
            config = new Config();  //call new config to "refresh" the data
            string configFileName = config.currentFile;
            int index = configFileName.LastIndexOf('\\') + 1;
            configFileName.Substring(index);
            fileName.Text = "Current file: " + configFileName.Remove(configFileName.Length - EXTENSION_LENGTH);
        }

        /// <summary>
        /// Displays the Add expense window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Expense_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseWindow aew = new AddExpenseWindow();
            aew.Owner = this;
            aew.Show();
        }
        public void showOnGrid()
        {
            Refresh();
            dataBudgetLists.SelectedIndex = dataBudgetLists.Items.Count - 1;
            dataBudgetLists.ScrollIntoView(dataBudgetLists.SelectedItem);
        }

        /// <summary>
        /// Displays the add category window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Category_Click(object sender, RoutedEventArgs e)
        {
            CategoryWindow cw = new CategoryWindow();
            cw.ShowDialog();
        }

        /// <summary>
        /// Displays the settings window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Show();
        }

        /// <summary>
        /// Passes the expense to update to the method in Update Expense that calls itself
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedItem = dataBudgetLists.SelectedItem;
            UpdateWindow.CallUpdateWindow(selectedItem.ExpenseID, selectedItem.CategoryID - 1, selectedItem.ShortDescription, selectedItem.Amount, selectedItem.Date);
            Refresh();
            dataBudgetLists.ScrollIntoView(selectedItem);
            dataBudgetLists.SelectedIndex = selectedItem.ExpenseID - 1;
        }

        /// <summary>
        /// Deletes the selected expense
        /// Displays a warning before doing so
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int ExpenseId = (dataBudgetLists.SelectedItem as dynamic).ExpenseID;
            string deleteWarning = $"Are you sure you want to delete expense #{ExpenseId}?";
            string caption = $"Delete Expense #{ExpenseId}";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(deleteWarning, caption, button, icon);
            if(result == MessageBoxResult.Yes)
            {
                presenter.DeleteExpense(ExpenseId);
                Refresh();
                //if(ExpenseId >= dataBudgetLists.Items.Count)
            }
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
        /// <summary>
        /// Closes all windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseAllWindows_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to force-close the app? Changes are automatically saved.",
                    "Close App",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Opens a different database file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Fills out the datagrid with expenses, is called by the presenter
        /// </summary>
        /// <param name="budgetItems">The list of expenses to bind to the datagrid</param>
        public void ShowBudgetItems(List<BudgetItem> budgetItems)
        {
            dataBudgetLists.ItemsSource = null;
            dataBudgetLists.ItemsSource = budgetItems;
        }

        /// <summary>
        /// Calls the refresh method that refreshes the data grid with the filter options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        /// <summary>
        /// Refreshes the data grid with the filter options
        /// </summary>
        public void Refresh()
        {
            bool fbc = FilterByCategory.IsChecked.Value;
            bool fbd = FilterByDate.IsChecked.Value;
            if (fbc && fbd)
            {
                try
                {
                    DateTime sdt = Convert.ToDateTime(StartDate.SelectedDate);
                    DateTime edt = Convert.ToDateTime(EndDate.SelectedDate);
                    if(DateTime.Compare(sdt, edt) < 0)
                    {
                        if(cmbCategory.SelectedIndex == -1)
                        {
                            string caption = "Invalid Category";
                            string warning = $"Please select a category";
                            MessageBoxButton button = MessageBoxButton.OK;
                            MessageBoxImage icon = MessageBoxImage.Warning;
                            MessageBox.Show(warning, caption, button, icon);
                        }
                        else
                        {
                            presenter.BudgetItemsList(sdt, edt, cmbCategory.SelectedIndex + 1, true);
                        }                        
                    }
                    else
                    {
                        string caption = "Error with filter dates";
                        string warning = $"End date cannot be after the first date";
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Warning;
                        MessageBox.Show(warning, caption, button, icon);
                    }
                }
                catch
                {
                    string caption = "Invalid dates";
                    string warning = $"Please select start and end dates";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBox.Show(warning, caption, button, icon);                    
                }
            }
            else if (fbc)
            {
                if (cmbCategory.SelectedIndex == -1)
                {
                    string caption = "Invalid Category";
                    string warning = $"Please select a category";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBox.Show(warning, caption, button, icon);
                }
                else
                {
                    presenter.BudgetItemsList(null, null, cmbCategory.SelectedIndex + 1, true);
                }
            }
            else if (fbd)
            {
                try
                {
                    DateTime sdt = Convert.ToDateTime(StartDate.SelectedDate);
                    DateTime edt = Convert.ToDateTime(EndDate.SelectedDate);
                    if (DateTime.Compare(sdt, edt) < 0)
                    {
                        presenter.BudgetItemsList(sdt, edt);
                    }
                    else
                    {
                        string caption = "Error with filter dates";
                        string warning = $"End date cannot be after the first date";
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Warning;
                        MessageBox.Show(warning, caption, button, icon);
                    }
                }
                catch
                {
                    string caption = "Invalid dates";
                    string warning = $"Please select start and end dates";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBox.Show(warning, caption, button, icon);
                }
            }
            else
            {
                presenter.BudgetItemsList(null, null);
            }
            DisplayCategories(presenter.GetCategories());
            cmbCategory.Text = "Category";
        }
        /// <summary>
        /// If you manually close, since the closing settings were changed due to the CloseAllWindows method, calls shutdown to make sure the process ends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
