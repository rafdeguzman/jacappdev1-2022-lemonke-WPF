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

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for AddExpense.xaml
    /// </summary>
    public partial class AddExpenseWindow : Window, ExpenseInterface
    {
        #region Backing Fields
        private readonly ExpensePresenter presenter;
        private HomeBudget model;
        #endregion

        #region Constructors
        /// <summary>
        /// Adds an expense to the database from input fields
        /// </summary>
        public AddExpenseWindow(HomeBudget model)
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Today;
            presenter = new ExpensePresenter(this, model);
            this.model = model;
            checkCredit.IsChecked = false;
        }
        #endregion

        #region Methods

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
            //Brand
            if (cmbCategory.SelectedIndex == -1)
                msg.AppendLine("Category is a required field.");
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
        /// Displays the last inputed expense
        /// </summary>
        /// <param name="categories">Category of the last expense</param>
        /// <param name="date">Date of the last expense</param>
        /// <param name="amount">Amount of the last expense</param>
        /// <param name="description">Description of the last expense</param>
        /// <param name="creditFlag">CreditFlag of the last expense</param>
        public void LastInput(string categories, string date, string amount, string description, string creditFlag)
        {
            previousCategory.Text = categories;
            previousDate.Text = date;
            previousAmount.Text = amount;
            previousDescription.Text = description;
            isCredit.Text = creditFlag;
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

        public int getBudgetWithDepartment(int department)
        {
            
            return department;
        }
        #endregion
    }
}