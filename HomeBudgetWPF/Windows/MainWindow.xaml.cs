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
            aew.ShowDialog();
            Refresh();
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

            dataBudgetLists.ItemsSource = budgetItems;
            dataBudgetLists.Columns.Clear(); // Clear all existing columns on the
            var column1 = new DataGridTextColumn(); // Create a text column object
            column1.Header = "Id";
            column1.Binding = new Binding("ExpenseID"); // Bind to an object propery
            dataBudgetLists.Columns.Add(column1); // Add the defined column to the

            var column2 = new DataGridTextColumn(); // Create a text column object
            column2.Header = "Amount";
            column2.Binding = new Binding("Amount"); // Bind to an object propery
            dataBudgetLists.Columns.Add(column2);

            var column3 = new DataGridTextColumn(); // Create a text column object
            column3.Header = "Category";
            column3.Binding = new Binding("Category"); // Bind to an object propery
            dataBudgetLists.Columns.Add(column3);

            var column4 = new DataGridTextColumn(); // Create a text column object
            column4.Header = "Date";
            column4.Binding = new Binding("Date");
            column4.Binding.StringFormat = "d";
            dataBudgetLists.Columns.Add(column4);

            var column5 = new DataGridTextColumn(); // Create a text column object
            column5.Header = "Description";
            column5.Binding = new Binding("ShortDescription");
            dataBudgetLists.Columns.Add(column5);
        }

        /// <summary
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
        private void Refresh()
        {
            DateTime defaultDate = new DateTime(1, 1, 1);
            DateTime sdt = Convert.ToDateTime(StartDate.SelectedDate);
            DateTime edt = Convert.ToDateTime(EndDate.SelectedDate);
            int categoryIndex = cmbCategory.SelectedIndex;
            bool filterByCategory = FilterByCategory.IsChecked.Value;
            bool filterByDate = FilterByDate.IsChecked.Value;
            dataBudgetLists.ContextMenu.IsOpen = false;
            dataBudgetLists.ContextMenu.StaysOpen = false;

            if (filterByDate || filterByCategory)
            {
                dataBudgetLists.ContextMenu.IsEnabled = false;
                Summary(filterByCategory, filterByDate, categoryIndex);
                
            }
            else
            {
                dataBudgetLists.ContextMenu.IsEnabled = true;
                if (sdt != defaultDate && edt != defaultDate && categoryIndex != -1)
                {
                    presenter.BudgetItemsList(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt == defaultDate && edt == defaultDate && categoryIndex != -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetItemsList(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt == defaultDate && edt != defaultDate && categoryIndex == -1)
                {
                    presenter.BudgetItemsList(sdt, edt);
                }
                else if (sdt == defaultDate && edt != defaultDate && categoryIndex != -1)
                {
                    presenter.BudgetItemsList(sdt, edt, categoryIndex, true);
                }
                else if (sdt != defaultDate && edt == defaultDate && categoryIndex == -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetItemsList(sdt, edt);
                }
                else if (sdt != defaultDate && edt == defaultDate && categoryIndex != -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetItemsList(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt != defaultDate && edt != defaultDate && categoryIndex == -1)
                {
                    presenter.BudgetItemsList(sdt, edt);
                }
                else
                {
                    edt = DateTime.Now;
                    presenter.BudgetItemsList(sdt, edt);
                }
                
            }

            DisplayCategories(presenter.GetCategories());
            cmbCategory.SelectedIndex = categoryIndex;
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

        private void Summary(bool  filterByCategory, bool filterByDate,int  categoryIndex)
        {
            DateTime defaultDate = new DateTime(1, 1, 1);
            DateTime sdt = Convert.ToDateTime(StartDate.SelectedDate);
            DateTime edt = Convert.ToDateTime(EndDate.SelectedDate);
            if(filterByCategory && filterByDate)
            {
                if (sdt != defaultDate && edt != defaultDate && categoryIndex != -1)
                {
                    presenter.BudgetIByDateandCategory(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt == defaultDate && edt == defaultDate && categoryIndex != -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetIByDateandCategory(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt == defaultDate && edt != defaultDate && categoryIndex == -1)
                {
                    presenter.BudgetIByDateandCategory(sdt, edt);
                }
                else if (sdt == defaultDate && edt != defaultDate && categoryIndex != -1)
                {
                    presenter.BudgetIByDateandCategory(sdt, edt, categoryIndex, true);
                }
                else if (sdt != defaultDate && edt == defaultDate && categoryIndex == -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetIByDateandCategory(sdt, edt);
                }
                else if (sdt != defaultDate && edt == defaultDate && categoryIndex != -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetIByDateandCategory(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt != defaultDate && edt != defaultDate && categoryIndex == -1)
                {
                    presenter.BudgetIByDateandCategory(sdt, edt);
                }
                else
                {
                    edt = DateTime.Now;
                    presenter.BudgetIByDateandCategory(sdt, edt);
                }
            }
            else if (filterByCategory)
            {
                if (sdt != defaultDate && edt != defaultDate && categoryIndex != -1)
                {
                    presenter.BudgetByCategory(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt == defaultDate && edt == defaultDate && categoryIndex != -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetByCategory(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt == defaultDate && edt != defaultDate && categoryIndex == -1)
                {
                    presenter.BudgetByCategory(sdt, edt);
                }
                else if (sdt == defaultDate && edt != defaultDate && categoryIndex != -1)
                {
                    presenter.BudgetByCategory(sdt, edt, categoryIndex, true);
                }
                else if (sdt != defaultDate && edt == defaultDate && categoryIndex == -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetByCategory(sdt, edt);
                }
                else if (sdt != defaultDate && edt == defaultDate && categoryIndex != -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetByCategory(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt != defaultDate && edt != defaultDate && categoryIndex == -1)
                {
                    presenter.BudgetByCategory(sdt, edt);
                }
                else
                {
                    edt = DateTime.Now;
                    presenter.BudgetByCategory(sdt, edt);
                }
            }
            else
            {
                if (sdt != defaultDate && edt != defaultDate && categoryIndex != -1)
                {
                    presenter.BudgetByDate(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt == defaultDate && edt == defaultDate && categoryIndex != -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetByDate(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt == defaultDate && edt != defaultDate && categoryIndex == -1)
                {
                    presenter.BudgetByDate(sdt, edt);
                }
                else if (sdt == defaultDate && edt != defaultDate && categoryIndex != -1)
                {
                    presenter.BudgetByDate(sdt, edt, categoryIndex, true);
                }
                else if (sdt != defaultDate && edt == defaultDate && categoryIndex == -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetByDate(sdt, edt);
                }
                else if (sdt != defaultDate && edt == defaultDate && categoryIndex != -1)
                {
                    edt = DateTime.Now;
                    presenter.BudgetByDate(sdt, edt, categoryIndex + 1, true);
                }
                else if (sdt != defaultDate && edt != defaultDate && categoryIndex == -1)
                {
                    presenter.BudgetByDate(sdt, edt);
                }
                else
                {
                    edt = DateTime.Now;
                    presenter.BudgetByDate(sdt, edt);
                }
            }
            DisplayCategories(presenter.GetCategories());
            cmbCategory.SelectedIndex = categoryIndex;

        }

        public void ShowBudgetItemsByDate(List<BudgetItemsByMonth> budgetItemsListByMonth)
        {
            dataBudgetLists.ItemsSource = budgetItemsListByMonth;
            dataBudgetLists.Columns.Clear(); // Clear all existing columns on the
            var column1 = new DataGridTextColumn(); // Create a text column object
            column1.Header = "Month";
            column1.Binding = new Binding("Month"); // Bind to an object propery
            dataBudgetLists.Columns.Add(column1); // Add the defined column to the

            var column2 = new DataGridTextColumn(); // Create a text column object
            column2.Header = "Total";
            column2.Binding = new Binding("Total"); // Bind to an object propery
            column2.Binding.StringFormat = "c";
            dataBudgetLists.Columns.Add(column2);
        }

        public void ShowBudgetItemsByCategory(List<BudgetItemsByCategory> budgetItemsListByCategory)
        {
            dataBudgetLists.ItemsSource = budgetItemsListByCategory;
            dataBudgetLists.Columns.Clear(); // Clear all existing columns on the
            var column1 = new DataGridTextColumn(); // Create a text column object
            column1.Header = "Category";
            column1.Binding = new Binding("Category"); // Bind to an object propery
            dataBudgetLists.Columns.Add(column1); // Add the defined column to the

            var column2 = new DataGridTextColumn(); // Create a text column object
            column2.Header = "Total";
            column2.Binding = new Binding("Total"); // Bind to an object propery
            column2.Binding.StringFormat = "c";
            dataBudgetLists.Columns.Add(column2);
        }

        public void ShowBudgetItemsDateAndCategory(List<Dictionary<string, object>> budgetItemsListByMonthAndCategory)
        {

            dataBudgetLists.ItemsSource = budgetItemsListByMonthAndCategory;
            dataBudgetLists.Columns.Clear();
            foreach (string key in budgetItemsListByMonthAndCategory[0].Keys)
            {
                if (!key.Contains("details:"))
                {
                    var column = new DataGridTextColumn();
                    column.Header = key;
                    column.Binding = new Binding($"[{key}]"); // Notice the square brackets!.
                    column.Binding.StringFormat = "c";
                    dataBudgetLists.Columns.Add(column);
                }
            }
        }

        private void Refresh_Event(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void cmbCategory_DropDownClosed(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
