using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
    /// Interaction logic for SettingsWindow.xaml
    /// Used to change the apps Theme
    /// </summary>
    public partial class SettingsWindow : Window
    {
        #region BACKING FIELDS

        // Holds the current Theme
        private static Uri currentTheme;

        private static bool darkMode = true;
        // URI's to the different themes
        private Uri NavyUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/Navy.xaml");
        private Uri CrimsonUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/Crimson.xaml");
        private Uri GreenUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/Green.xaml");
        private Uri lightBlueUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/LightBlue.xaml");
        private Uri lightRedUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/LightRed.xaml");
        private Uri lightGreenUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/LightGreen.xaml");


        private Uri tbUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/TextBlockDictionary.xaml");
        private Uri buttonUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/ButtonDictionary.xaml");
        private Uri ListOfColorsUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/ListOfColors.xaml");
        #endregion

        #region PROPERTIES

        private static Uri Theme
        {
            get { return currentTheme; }
            set
            {
                currentTheme = value;
            }
        }
        private static bool DarkMode
        {
            get { return darkMode; }
            set
            {
                darkMode = value;
            }
        }
        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Settings Window constructor 
        /// Makes sure all the correct checkboxes are checked
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
            // Makes sure Dark Mode cb is correctly checked
            if (darkMode)
                cbDarkMode.IsChecked = true;

            // Sets a base theme if there is none
            if (Theme is null)
                Theme = lightRedUri;

            // Sets the correct Theme
            switch (GetCurrentColor())
            {
                case "Red":
                    cbRedOption.IsChecked = true;
                    break;
                case "Blue":
                    cbNavyOption.IsChecked = true;
                    break;
                case "Green":
                    cbGreenOption.IsChecked = true;
                    break;
                default:
                    Theme = lightRedUri;
                    cbRedOption.IsChecked = true;
                    break;
            }

        }
        #endregion

        #region METHODS
        // When Dark Mode checkbox is checked
        private void CbDarkMode_OnClick(object sender, RoutedEventArgs e)
        {
            if (cbDarkMode.IsChecked == true)
                darkMode = true;
            else
                darkMode = false;
            // Calls ChangeDarkTheme
            ChangeThemeColor(GetCurrentColor());
        }

        // On Color Switch Checkbox click, calls ChangeThemeColor(Color to switch to)
        private void cb_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeThemeColor((sender as CheckBox).Content.ToString());
        }

        // Sets the old theme color, then calls CurrentTheme to get the color to switch to, then switch to the new theme
        private void ChangeThemeColor(string color)
        {
            //Get the selected option
            CurrentTheme(color);

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add( new ResourceDictionary() { Source = tbUri});
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = buttonUri});
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = ListOfColorsUri});
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = currentTheme});
        }

        // Gets Blue / Red / Green for the CurrentTheme method
        private string GetCurrentColor()
        {
            if (currentTheme.Equals(lightBlueUri) || currentTheme.Equals(NavyUri))
                return "Navy";
            if (currentTheme.Equals(lightGreenUri) || currentTheme.Equals(GreenUri))
                return "Green";
            return "Crimson";
        }

        // Goes over the new color and sets the theme according to the Dark Mode button clicked or not
        // Unchecks all checkboxes other than the one it is setting the color to
        private void CurrentTheme(string color)
        {
            switch (color)
            {
                case "Navy":
                    if (DarkMode)
                        currentTheme = lightBlueUri;
                    else
                        currentTheme = NavyUri;
                    cbNavyOption.IsChecked = true;
                    cbGreenOption.IsChecked = false;
                    cbRedOption.IsChecked = false;
                    break;
                case "Crimson":
                    if (DarkMode)
                        currentTheme = lightRedUri;
                    else
                        currentTheme = CrimsonUri;
                    cbNavyOption.IsChecked = false;
                    cbGreenOption.IsChecked = false;
                    cbRedOption.IsChecked = true;
                    break;
                case "Green":
                    if (DarkMode)
                        currentTheme = lightGreenUri;
                    else
                        currentTheme = GreenUri;
                    cbNavyOption.IsChecked = false;
                    cbGreenOption.IsChecked = true;
                    cbRedOption.IsChecked = false;
                    break;
            }
        }

        #endregion
    }
}
