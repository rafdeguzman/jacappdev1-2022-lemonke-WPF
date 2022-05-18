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
        HomeBudget model;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for the main Window
        /// </summary>
        /// 

        // create a presenter where i can pass in a filepath to initialize the db
        public MainWindow(string filePath)
        {
            InitializeComponent();
            config = new Config();
            // takes in view and if new db or not
            // expenses is empty by default
            presenter = new Presenter(this, filePath);
            model = presenter.GetModel();
            FilterByCategory.IsChecked = false;
            FilterByDate.IsChecked = false;
            ResetFilter();
            Filter();
            SettingsWindow sw = new SettingsWindow();
            sw.CurrentTheme();
            sw.Close();
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
            AddExpenseWindow aew = new AddExpenseWindow(model);
            aew.Owner = this;
            aew.Show();
            aew.Closed += AddExpenseWindowClosed;
        }
        private void AddExpenseWindowClosed(object sender, EventArgs e)
        {
            ((AddExpenseWindow)sender).Closed -= AddExpenseWindowClosed;
            //bring main window to front when closed
            this.Activate();
        }
        public void showOnGrid()
        {
            Filter();
            if (dataBudgetLists.Items.Count > 0)
            {          
                dataBudgetLists.SelectedIndex = dataBudgetLists.Items.Count - 1;
                dataBudgetLists.ScrollIntoView(dataBudgetLists.SelectedItem);
            }
            else
            {
                dataBudgetLists.SelectedIndex = 0;
                dataBudgetLists.ScrollIntoView(dataBudgetLists.SelectedItem);
            }
        }

        /// <summary>
        /// Displays the add category window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Category_Click(object sender, RoutedEventArgs e)
        {
            CategoryWindow cw = new CategoryWindow(model);
            cw.Owner = this;
            //cw.ShowDialog();
            cw.Show();
            cw.Closed += CategoryWindowClosed;
        }
        private void CategoryWindowClosed(object sender, EventArgs e)
        {
            ((CategoryWindow)sender).Closed -= CategoryWindowClosed;
            DisplayCategories(presenter.GetCategories());
            this.Activate();
        }
        public void redrawCategories()
        {
            DisplayCategories(presenter.GetCategories());
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
            UpdateWindow.CallUpdateWindow(selectedItem.ExpenseID, selectedItem.CategoryID - 1, selectedItem.ShortDescription, selectedItem.Amount, selectedItem.Date, presenter.GetModel());
            Filter();
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
            if (result == MessageBoxResult.Yes)
            {
                int lastIndex = dataBudgetLists.SelectedIndex - 1;
                presenter.DeleteExpense(ExpenseId);
                Filter();
                if (dataBudgetLists.Items.Count > 0)
                {
                    // if last item is deleted, change selected item to item above
                    if (lastIndex == dataBudgetLists.Items.Count - 1)
                    {
                        dataBudgetLists.SelectedIndex = dataBudgetLists.Items.Count - 1;
                        dataBudgetLists.ScrollIntoView(dataBudgetLists.SelectedItem);
                    }
                    else
                    {
                        dataBudgetLists.SelectedIndex = (lastIndex += 1);
                        dataBudgetLists.ScrollIntoView(dataBudgetLists.SelectedItem);
                    }
                }
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
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
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
            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
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
                    MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
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
            column4.Binding.StringFormat = "dd-MM-yyyy";
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
            ResetFilter();
        }
        /// If you manually close, since the closing settings were changed due to the CloseAllWindows method, calls shutdown to make sure the process ends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        public void ShowBudgetItemsByMonth(List<BudgetItemsByMonth> budgetItemsListByMonth)
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
            dataBudgetLists.Columns.Add(column2); // Add the defined column to the
        }

        public void ShowBudgetItemsMonthAndCategory(List<Dictionary<string, object>> budgetItemsListByMonthAndCategory)
        {
            dataBudgetLists.ItemsSource = budgetItemsListByMonthAndCategory;
            dataBudgetLists.Columns.Clear();
            var columnTotal = new DataGridTextColumn();
            List<string> keys = new List<string>();

            for (int i = 0; i < budgetItemsListByMonthAndCategory.Count; i++)
            {
                foreach (string key in budgetItemsListByMonthAndCategory[i].Keys)
                {
                    if (!keys.Contains(key))
                    {
                        keys.Add(key);
                        if (key.Contains("Total"))
                        {
                            columnTotal = new DataGridTextColumn();
                            columnTotal.Header = key;
                            columnTotal.Binding = new Binding($"[{key}]"); // Notice the square brackets!.
                            columnTotal.Binding.StringFormat = "c";

                        }
                        if (!key.Contains("details:") && !key.Contains("Total"))
                        {
                            var column = new DataGridTextColumn();
                            column.Header = key;
                            column.Binding = new Binding($"[{key}]"); // Notice the square brackets!.
                            column.Binding.StringFormat = "c";
                            dataBudgetLists.Columns.Add(column);
                        }
                    }
                }
            }
            dataBudgetLists.Columns.Add(columnTotal);
        }

        private void Refresh_Event(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void cmbCategory_DropDownClosed(object sender, EventArgs e)
        {
            Filter();
        }
        private void Filter()
        {
            displaySearchHint();
            string filterType = "BudgetItems";
            if (FilterByCategory.IsChecked.Value && !FilterByDate.IsChecked.Value)
            {
                filterType = "BudgetItemsByCategory";
            }
            else if (!FilterByCategory.IsChecked.Value && FilterByDate.IsChecked.Value)
            {
                filterType = "BudgetItemsByMonth";
            }
            else if (FilterByCategory.IsChecked.Value && FilterByDate.IsChecked.Value)
            {
                filterType = "BudgetItemsByMonthAndCategory";
            }
            bool filterFlag = cmbCategory.SelectedIndex == -1 ? false : true;
            int x = dataBudgetLists.SelectedIndex;
            presenter.Filter(search.Text, filterType, StartDate.SelectedDate, EndDate.SelectedDate, filterFlag, cmbCategory.SelectedIndex + 1);
            dataBudgetLists.SelectedIndex = x;
        }
        private void ResetFilter()
        {
            presenter.Filter("", "BudgetItems", null, null);
            search.Text = string.Empty;
            displaySearchHint();
            StartDate.SelectedDate = null;
            EndDate.SelectedDate = null;
            FilterByCategory.IsChecked = false;
            FilterByDate.IsChecked = false;
            DisplayCategories(presenter.GetCategories());
            cmbCategory.Text = "Category";
        }
        private void displaySearchHint()
        {
            if (search.Text.Length == 0)
            {
                searchHint.Visibility = Visibility.Visible;
            }
            else
            {
                searchHint.Visibility = Visibility.Hidden;
            }
        }

        private void search_KeyDown(object sender, KeyEventArgs e)
        {
            displaySearchHint();
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                if (dataBudgetLists.SelectedIndex == -1)
                {
                    dataBudgetLists.SelectedIndex = 0;
                }
                else
                {
                    int count = dataBudgetLists.Items.Count;
                    dataBudgetLists.SelectedIndex = (dataBudgetLists.SelectedIndex + 1) % count;
                }
                ScrollIntoView();
            }
            else
            {
                Filter();
            }
        }

        private void search_KeyUp(object sender, KeyEventArgs e)
        {
            displaySearchHint();
            Filter();
        }
        private void Refresh(bool start)
        {
            if (EndDate.SelectedDate != null && StartDate.SelectedDate != null)
            {
                string messageBoxText = "";
                string caption = "Date Conflict Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                if (start)
                {
                    if (StartDate.SelectedDate > EndDate.SelectedDate)
                    {
                        messageBoxText = "Selected start date is after end date. End Date was changed to new start date.";
                        EndDate.SelectedDate = StartDate.SelectedDate;
                        MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
                    }                    
                }
                else
                {
                    if(StartDate.SelectedDate > EndDate.SelectedDate)
                    {
                        messageBoxText = "Selected end date is before start date. Start Date was changed to new end date.";
                        StartDate.SelectedDate = EndDate.SelectedDate;
                        MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
                    }
                }
            }
            Filter();
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh(true);
        }

        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh(false);
        }
        private void ScrollIntoView()
        {
            if (dataBudgetLists.SelectedItem != null)
            {
                dataBudgetLists.ScrollIntoView(dataBudgetLists.SelectedItem);
            }
        }

        private void GenerateChart_Click(object sender, RoutedEventArgs e)
        {
            presenter.GeneratePieChart();
        }

        public DateTime? GetStartDate()
        {
            return StartDate.SelectedDate;
        }

        public DateTime? GetEndDate()
        {
            return EndDate.SelectedDate;
        }

        public bool GetFilterFlag()
        {
            return FilterByCategory.IsChecked.Value;
        }

        public int GetCategoryId()
        {
            return cmbCategory.SelectedIndex + 1;
        }
    }
}
