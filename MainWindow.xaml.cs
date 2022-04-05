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

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window, IView
    {
        private string category;
        private readonly Presenter presenter;
        //for use with combo box items
        private List<string> categories = new List<string>();
        private List<string> categoryTypes = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            presenter = new Presenter(this);
        }

        public string GetStringInput()
        {
            return category;
        }

        public void DisplayCategories(List<string> categories)
        {
            foreach (string category in categories)
            {
                this.categories.Add(category);
                cmbCategories.Items.Add(category);
            }
        }

        public void DisplayCategoryTypes(List<string> categoryTypes)
        {
            foreach (string categoryType in categoryTypes)
            {
                this.categoryTypes.Add(categoryType);
                cmbCategoryTypes.Items.Add(categoryType);
            }
        }

        private void AddCategoriesToList()
        {
            foreach (var item in cmbCategories.Items)
                categories.Add(item.ToString());
        }
        private void AddCategoryToDB(string categoryName)
        {
            //verify that user wants to add
            string messageBoxText = "Are you sure you want to add " + categoryName + " as a category?";
            string caption = "Confirm";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            if (result == MessageBoxResult.Yes)
            {
                cmbCategories.Items.Add(categoryName);
                category = categoryName;
                presenter.AddCategory(cmbCategoryTypes.SelectedIndex);
            }
            else
            {
                //do nothing
            }
        }
        private void ShowCategoryError(string categoryName)
        {
            //taken from https://docs.microsoft.com/en-us/dotnet/desktop/wpf/windows/how-to-open-message-box?view=netdesktop-6.0
            string messageBoxText = categoryName + " is already on the list.";
            string caption = "Error";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        //Stackoverflow references: https://stackoverflow.com/questions/4609847/wpf-combobox-with-iseditable-true-how-can-i-indicate-that-no-match-was-found
        private void cmbCategories_KeyDown(object sender, KeyEventArgs e)
        {
            //textbox part of editable combo box
            TextBox tb = cmbCategories.Template.FindName("PART_EditableTextBox", cmbCategories) as TextBox;

            //dont run if tb.Text is empty
            if (!(tb.Text == String.Empty || tb.Text == " "))
            {
                AddCategoriesToList();

                //if items doesnt have tb.text and return is pressed
                if (e.Key == Key.Return && !categories.Contains(tb.Text))
                    AddCategoryToDB(tb.Text);   
                
                else if (e.Key == Key.Return && categories.Contains(tb.Text))
                    ShowCategoryError(tb.Text);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //textbox part of editable combo box
            TextBox tb = cmbCategories.Template.FindName("PART_EditableTextBox", cmbCategories) as TextBox;

            //dont run if tb.Text is empty
            if (!(tb.Text == String.Empty || tb.Text == " "))
            {
                AddCategoriesToList();

                //if items doesnt have tb.text and return is pressed
                if (!categories.Contains(tb.Text))
                    AddCategoryToDB(tb.Text);

                else if (categories.Contains(tb.Text))
                    ShowCategoryError(tb.Text);
            }
        }
    }
}
