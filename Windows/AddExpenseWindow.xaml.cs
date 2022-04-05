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


        public void ResetText()
        {
            throw new NotImplementedException();
        }
        public void DisplayCategories(List<Category> categories)
        {
            cmbCategory.DisplayMemberPath = "Description";
            foreach (Category Displaycategories in categories)
            {
                cmbCategory.Items.Add(Displaycategories);
            }
        }

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
                    msg.AppendLine("Invalid Quantity");
            }
            catch
            {
                msg.AppendLine("Invalid Quantity");
            }

            if (string.IsNullOrEmpty(msg.ToString()))
                return true;

            MessageBox.Show(msg.ToString(), "Missing Fields", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        public void GetUserInput()
        {
            presenter.AddExpense(Convert.ToDateTime(datePicker.SelectedDate), cmbCategory.SelectedIndex, double.Parse(txtAmount.Text), txtDescription.Text, checkCredit.IsChecked);
        }

        public void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUserInput())
            {
                GetUserInput();
                clear();
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

        public void LastInput(string categories, string date, string amount, string description, string creditFlag)
        {
            previousCategory.Text = categories;
            previousDate.Text = date;
            previousAmount.Text = amount;
            previousDescription.Text = description;
            isCredit.Text = creditFlag;
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Show();
        }
    }
}