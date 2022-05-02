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
using Budget;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>

    public partial class CategoryWindow : Window, CategoryInterface
    {
        #region Backing Fields
        private string category;
        private readonly CategoryPresenter presenter;
        //for use with combo box items
        private List<string> categories = new List<string>();
        private List<string> categoryTypes = new List<string>();
        #endregion

        #region Properties
        /// <summary>
        /// Sets Category combo box text to a specific value
        /// </summary>
        public String categoryCBText
        {
            set { cmbCategories.Text = value; }
        }
        #endregion

        #region Contructors
        /// <summary>
        /// Used to add a category
        /// </summary>
        public CategoryWindow(HomeBudget model)
        {
            InitializeComponent();
            presenter = new CategoryPresenter(this, model);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the current value of category string.
        /// </summary>
        /// <returns>String category.</returns>
        public string GetStringInput()
        {
            return category;
        }
        
        /// <summary>
        /// Adds current category string to categories list.
        /// Adds category string to Combo box.
        /// </summary>
        /// <param name="categories">List of string that holds all categories.</param>
        public void DisplayCategories(List<string> categories)
        {
            foreach (string category in categories)
            {
                this.categories.Add(category);
                cmbCategories.Items.Add(category);
            }
        }
        
        /// <summary>
        /// Adds category types to categoryTypes list.
        /// Also adds categoryTypes to comboBox.
        /// </summary>
        /// <param name="categoryTypes">List of Strings</param>
        public void DisplayCategoryTypes(List<string> categoryTypes)
        {
            foreach (string categoryType in categoryTypes)
            {
                this.categoryTypes.Add(categoryType);
                cmbCategoryTypes.Items.Add(categoryType);
            }
        }

        /// <summary>
        /// Fills out the combobox with the preexisting expenses
        /// </summary>
        private void AddCategoriesToList()
        {
            foreach (var item in cmbCategories.Items)
                categories.Add(item.ToString());
        }

        /// <summary>
        /// Adds the category to the db
        /// </summary>
        /// <param name="categoryName">The name of the category being added to the db</param>
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
                if (this.Owner is MainWindow)
                    ((MainWindow)this.Owner).redrawCategories();
                else if (this.Owner is AddExpenseWindow)
                    ((MainWindow)((AddExpenseWindow)this.Owner).Owner).redrawCategories();    // get aew.Owner and call that one's function
            }
            else
            {
                //do nothing
            }
        }

        /// <summary>
        /// Displays an error message if the category is already on the list
        /// </summary>
        /// <param name="categoryName"></param>
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

        /// <summary>
        /// Allows the user to press enter on any of the comboxes, and if both have content, then add it to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //Stackoverflow references: https://stackoverflow.com/questions/4609847/wpf-combobox-with-iseditable-true-how-can-i-indicate-that-no-match-was-found
        private void cmbCategories_KeyDown(object sender, KeyEventArgs e)
        {
            //textbox part of editable combo box
            TextBox tb = cmbCategories.Template.FindName("PART_EditableTextBox", cmbCategories) as TextBox;
            TextBox tbTypes = cmbCategoryTypes.Template.FindName("PART_EditableTextBox", cmbCategoryTypes) as TextBox;
            
            //dont run if tb.Text is empty and if tb.Text has default values
            if (!(tb.Text == String.Empty || tb.Text == " " || tb.Text == "Categories" || tbTypes.Text == "Category Type" || !categoryTypes.Contains(tbTypes.Text)))
            {
                AddCategoriesToList();

                //if items doesnt have tb.text and return is pressed
                if (e.Key == Key.Return && !categories.Contains(tb.Text))
                    AddCategoryToDB(tb.Text);

                else if (e.Key == Key.Return && categories.Contains(tb.Text))
                    ShowCategoryError(tb.Text);
            }
        }

        /// <summary>
        /// Main logic after the Add button is clicked
        /// Checks if the inputs are valid and adds to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //textbox part of editable combo box
            TextBox tb = cmbCategories.Template.FindName("PART_EditableTextBox", cmbCategories) as TextBox;
            TextBox tbTypes = cmbCategoryTypes.Template.FindName("PART_EditableTextBox", cmbCategoryTypes) as TextBox;
            //dont run if tb.Text is empty
            if (!(tb.Text == "Categories" || tbTypes.Text == "Category Type" || !categoryTypes.Contains(tbTypes.Text)))
            {
                AddCategoriesToList();

                //if items doesnt have tb.text and return is pressed
                if (!categories.Contains(tb.Text))
                    AddCategoryToDB(tb.Text);

                else if (categories.Contains(tb.Text))
                    ShowCategoryError(tb.Text);
            }
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
            if (MessageBox.Show("Are you sure you want to close app!!!",
                    "Close App",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
        
        /// <summary>
        /// Closes the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}