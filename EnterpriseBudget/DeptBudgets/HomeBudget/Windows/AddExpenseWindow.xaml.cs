using Budget;
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
using Microsoft.Win32;
using Path = System.IO.Path;
using LiveCharts;
using LiveCharts.Wpf;

namespace EnterpriseBudget.DeptBudgets.HomeBudget
{
    /// <summary>
    /// Interaction logic for AddExpense.xaml
    /// </summary>
    public partial class AddExpenseWindow : Window, ExpenseInterface
    {
        #region Backing Fields
        private readonly ExpensePresenter presenter;
        private Budget.HomeBudget model;
        private List<KeyValuePair<string, decimal>> limits;
        private List<KeyValuePair<string, decimal>> alreadySpent; 
        #endregion

        #region Constructors
        /// <summary>
        /// Adds an expense to the database from input fields
        /// </summary>
        public AddExpenseWindow(Budget.HomeBudget model, int budgId)
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Today;
            presenter = new ExpensePresenter(this, model);
            this.model = model;
            checkCredit.IsChecked = false;
            limits = presenter.getExpenses(budgId);
            doStuff();
        }
        #endregion

        #region Methods

        private void doStuff()
        {
            List<Budget.BudgetItem> expenses = presenter.getAllExpenses();
            List<String> labels = new List<string>();
            ChartValues<decimal> spent = new ChartValues<decimal>();
            ChartValues<decimal> remainder = new ChartValues<decimal>();
            alreadySpent = new List<KeyValuePair<string, decimal>>();

            // For each category
            for (int i = 0; i < limits.Count; i++)
            {
                decimal totalSpent = 0;
                decimal limit = limits[i].Value;
                string currentCat = limits[i].Key;
                labels.Add(currentCat);

                // For each category, add all the fields (Available / Spent)
                for (int x = 0; x < expenses.Count; x++)
                {
                    Budget.BudgetItem currentExpense = expenses[x];
                    if (currentExpense != null && currentExpense.Category == currentCat)
                    {
                        totalSpent += (decimal)currentExpense.Amount;
                    }
                }
                // i dont know about this tbh but the values have to be initialized
                remainder.Add(limit - totalSpent);
                spent.Add(totalSpent);
                alreadySpent.Add(new KeyValuePair<string, decimal>(currentCat, totalSpent));
            }
            barChartControl.DisplayChart(spent, remainder, labels);
        }

        /// <summary>
        /// Resets the input fields
        /// </summary>
        public void ResetText()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills out the categories combobox from a provided list of categories
        /// </summary>
        /// <param name="categories">The list of all the categories</param>
        public void DisplayCategories(List<Category> categories)
        {
            cmbCategory.DisplayMemberPath = "Description";
            cmbCategory.Items.Clear();
            foreach (Category Displaycategories in categories)
            {
                cmbCategory.Items.Add(Displaycategories);
            }
        }

        /// <summary>
        /// Checks that all the inputs are valid
        /// </summary>
        /// <returns>True if all the inputs are valid and false if they are not</returns>
        public bool CheckUserInput()
        {
            StringBuilder msg = new StringBuilder();
            //Description
            if (string.IsNullOrEmpty(txtDescription.Text))
                msg.AppendLine("Name is a required field.");
            //Amount
            if (string.IsNullOrEmpty(txtAmount.Text))
                msg.AppendLine("Amount is a required field.");            
            //Quantity
            try
            {
                if (double.Parse(txtAmount.Text) <= 0)
                    msg.AppendLine("Invalid Amount");
            }
            catch
            {
                msg.AppendLine("Invalid Amount");
            }

            //Brand
            if (cmbCategory.SelectedIndex == -1)
                msg.AppendLine("Category is a required field.");
            else
            {
                for (int i = 0; i < limits.Count; i++)
                {
                    if (limits[i].Key == cmbCategory.SelectedItem.ToString())
                    {
                        if ((limits[i].Value - alreadySpent[i].Value) < decimal.Parse(txtAmount.Text))
                        {
                            msg.AppendLine("Expense Refused, limit would be exceded for category: " + limits[i].Key);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(msg.ToString()))
                return true;

            MessageBox.Show(msg.ToString(), "Missing Fields", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        /// <summary>
        /// Gets the inputs from the input fields and calls the presenter method that adds the expense
        /// </summary>
        public void GetUserInput()
        {
            presenter.AddExpense(Convert.ToDateTime(datePicker.SelectedDate), cmbCategory.SelectedIndex, double.Parse(txtAmount.Text), txtDescription.Text, checkCredit.IsChecked.Value);

        }

        /// <summary>
        /// The main logic after the add button is clicked, checks if the inputs are valid and if they are calls GetUserInput()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUserInput())
            {
                doStuff();
                GetUserInput();
                clear();
                blastInput.Visibility = Visibility.Visible;
                ((MainWindow)this.Owner).showOnGrid();
            }
        }

        /// <summary>
        /// Calls Clear(), which clears all the fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }

        /// <summary>
        /// Clears all the input fields except for the Category and the DateTime
        /// </summary>
        private void clear()
        {
            txtAmount.Text = string.Empty;
            txtDescription.Text = string.Empty;
            checkCredit.IsChecked = false;
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
        /// Closes all the windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseAllWindows_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want close app!!!",
                    "Close App",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Warning message saying the input is the same as the last input
        /// </summary>
        public void DisplaySameAsLastInput()
        {
            if (MessageBox.Show("Current input is the same as the last input, would you like sill add? ",
                    "Same Input",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                presenter.setUserInputFromDuplicateExpense(true);
            }
            else
            {
                presenter.setUserInputFromDuplicateExpense(false);
            }
        }
        #endregion
    }
}