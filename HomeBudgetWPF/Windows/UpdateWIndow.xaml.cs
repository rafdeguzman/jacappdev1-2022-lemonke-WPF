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
    public partial class UpdateWindow : Window, ExpenseInterface
    {
        private int id;
        private readonly ExpensePresenter presenter;
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

        public static void CallUpdateWindow(int id, int catId, string desc, double amount, DateTime dt)
        {
            UpdateWindow uw = new UpdateWindow(id, catId, desc, amount, dt);
            uw.ShowDialog();
        }

        public int ExpenseId
        {
            get { return id; }
            private set { id = value; }
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

        public void GetUserInput()
        {
            presenter.UpdateExpense(ExpenseId ,Convert.ToDateTime(datePicker.SelectedDate), cmbCategory.SelectedIndex + 1, double.Parse(txtAmount.Text), txtDescription.Text);
        }

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

        // Both of these wont be used here
        public void LastInput(string categories, string date, string amount, string description, string creditFlag)
        {
            throw new NotImplementedException();
        }
        public void DisplaySameAsLastInput()
        {
            throw new NotImplementedException();
        }
    }
}