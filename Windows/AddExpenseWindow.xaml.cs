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
        private readonly ExpensePresenter presenter;
        public AddExpenseWindow()
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Today;
            presenter = new ExpensePresenter(this);
            checkCredit.IsChecked = false;
        }
        

        /// <summary>
        /// Display the categories to the user 
        /// </summary>
        /// <param name="categories"> list of categories</param>
        public void DisplayCategories(List<Category> categories)
        {
            cmbCategory.DisplayMemberPath = "Description";
            foreach (Category Displaycategories in categories)
            {
                cmbCategory.Items.Add(Displaycategories);
            }
        }

        /// <summary>
        /// Validate the user input
        /// </summary>
        /// <returns>true if everything ok or else false</returns>
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
        /// Get user input
        /// </summary>
        public void GetUserInput()
        {
            presenter.AddExpense(Convert.ToDateTime(datePicker.SelectedDate), cmbCategory.SelectedIndex, double.Parse(txtAmount.Text), txtDescription.Text, checkCredit.IsChecked.Value);
        }
        /// <summary>
        /// Add the expense information to the presente
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
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }
        private void clear()
        {
            txtAmount.Text = string.Empty;
            txtDescription.Text = string.Empty;
            checkCredit.IsChecked = false;
        }

        /// <summary>
        /// Display the last expense information
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="description"></param>
        /// <param name="creditFlag"></param>
        public void LastInput(string categories, string date, string amount, string description, string creditFlag)
        {
            previousCategory.Text = categories;
            previousDate.Text = date;
            previousAmount.Text = amount;
            previousDescription.Text = description;
            isCredit.Text = creditFlag;
        }
        /// <summary>
        /// Type of theme available for the window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Show();
        }


        /// <summary>
        /// Close all the windows
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
                Environment.Exit(0);
            }
        }

        
        private void cmbCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                CategoryWindow cw = new CategoryWindow();
                cw.Show();
                TextBox tb = cmbCategory.Template.FindName("PART_EditableTextBox", cmbCategory) as TextBox;
                cw.categoryCBText = tb.Text;
            }            
        }

        /// <summary>
        /// Display the last warning message for duplicate expense 
        /// </summary>
        public void DisplaySameAsLastInput()
        {
            if (MessageBox.Show("Current input is the same as the last input, would you like sill add? ",
                    "Same Input",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                presenter.SetUserInputFromDuplicateExpense(true);
            }
            else
            {
                presenter.SetUserInputFromDuplicateExpense(false);
            }
        }
    }
}