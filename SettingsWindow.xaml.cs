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
    /// </summary>
    public partial class SettingsWindow : Window
    {
        #region BACKING FIELDS

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
        private Uri baseRDUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/BaseRD.xaml");
        private Uri emptyUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/");
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
        public SettingsWindow()
        {
            InitializeComponent();
            if (darkMode)
                cbDarkMode.IsChecked = true;
            if (Theme is null)
                Theme = lightRedUri;
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

            var app = (App)Application.Current;
            app.ChangeTheme(currentTheme);

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
