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
    /// Interaction logic for UpdateExpense.xaml
    /// </summary>
    public partial class UpdateWindow : Window, ExpenseInterface
    {
        #region Backing Fields
        private int id;
        private readonly ExpensePresenter presenter;
        #endregion

        #region Constructor
        /// <summary>
        /// Used to update an expense
        /// </summary>
        /// <param name="id">The id of the expense that is to be updated</param>
        /// <param name="catId">The new category Id of the expense</param>
        /// <param name="desc">The new description of the expense</param>
        /// <param name="amount">The new amount of the expense</param>
        /// <param name="dt">The new DateTime of the expense</param>
        public UpdateWindow(int id, int catId, string desc, double amount, DateTime dt)
        {
            InitializeComponent();
            // Set the properties
            ExpenseId = id;

            // Set the fields
            cmbCategory.SelectedIndex = catId;
            txtDescription.Text = desc;
            txtAmount.Text = amount.ToString();
            datePicker.SelectedDate = dt;
            presenter = new ExpensePresenter(this);
            checkCredit.IsChecked = false;
        }

        /// <summary>
        /// Static method that calls the UpdateWindow, used to pass data between windows
        /// </summary>
        /// <param name="id">The id of the expense that is to be updated</param>
        /// <param name="catId">The new category Id of the expense</param>
        /// <param name="desc">The new description of the expense</param>
        /// <param name="amount">The new amount of the expense</param>
        /// <param name="dt">The new DateTime of the expense</param>
        public static void CallUpdateWindow(int id, int catId, string desc, double amount, DateTime dt)
        {
            UpdateWindow uw = new UpdateWindow(id, catId, desc, amount, dt);
            uw.ShowDialog();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Property for the expense id
        /// </summary>
        public int ExpenseId
        {
            get { return id; }
            private set { id = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Resets the text fields
        /// </summary>
        public void ResetText()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updated the categories in the categories combobox
        /// </summary>
        /// <param name="categories">The list of categories that will be placed in the combobox</param>
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
        /// Validated that the inputed values are of the good types and within the given ranges
        /// </summary>
        /// <returns>True if the inputs are all valid and false if any of them are not</returns>
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
                double i = double.Parse(txtAmount.Text);
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
        /// Calls the presenter method to update the expense
        /// </summary>
        public void GetUserInput()
        {
            presenter.UpdateExpense(ExpenseId ,Convert.ToDateTime(datePicker.SelectedDate), cmbCategory.SelectedIndex + 1, double.Parse(txtAmount.Text), txtDescription.Text);
        }

        /// <summary>
        /// Updated the Expense if the inputs are valid
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
                this.Close();
            }
        }

        /// <summary>
        /// Calls the clear method, which clears the input fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }
        /// <summary>
        /// Clears the input fields
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
        /// Opens the add Category Window when pressing enter in any category combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                CategoryWindow cw = new CategoryWindow();
                cw.Show();
                TextBox tb = cmbCategory.Template.FindName("PART_EditableTextBox", cmbCategory) as TextBox;
                cw.categoryCBText = tb.Text;
                DisplayCategories(presenter.ExpensePopulateCategories());
            }            
        }

        // Both of these wont be used in this window, just dont want to make another interface
        /// <summary>
        /// Displays the last input
        /// </summary>
        /// <param name="categories">Category of the last input</param>
        /// <param name="date">DateTime of the last input</param>
        /// <param name="amount">Amount of the last input</param>
        /// <param name="description">Description of the last input</param>
        /// <param name="creditFlag">Credit Flag of the last input</param>
        public void LastInput(string categories, string date, string amount, string description, string creditFlag)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Displays the last input in the fields
        /// </summary>
        public void DisplaySameAsLastInput()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}