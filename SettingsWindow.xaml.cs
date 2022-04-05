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

        private static string currentTheme;

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

        private static string Theme
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
                    Theme = "LightRed";
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
        private void cbNavyOption_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeThemeColor("Blue");
        }

        private void cbRedOption_Onclick(object sender, RoutedEventArgs e)
        {
            ChangeThemeColor("Red");
        }

        private void cbGreenOption_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeThemeColor("Green");
        }

        // Sets the old theme color, then calls CurrentTheme to get the color to switch to, then switch to the new theme
        private void ChangeThemeColor(string color)
        {
            // Sets the base theme
            Theme _newTheme = new Theme(new Uri(String.Format(emptyUri.ToString() + Theme + ".xaml")), lightRedUri, baseRDUri);

            // Sets the current theme
            switch (Theme)
            {
                case "LightBlue":
                    _newTheme.old_theme_uri = lightBlueUri;
                    break;
                case "LightRed":
                    _newTheme.old_theme_uri = lightRedUri;
                    break;
                case "LightGreen":
                    _newTheme.old_theme_uri = lightGreenUri;
                    break;
                case "Navy":
                    _newTheme.old_theme_uri = NavyUri;
                    break;
                case "Crimson":
                    _newTheme.old_theme_uri = CrimsonUri;
                    break;
                case "Green":
                    _newTheme.old_theme_uri = GreenUri;
                    break;
            }

            // Gets the new theme
            CurrentTheme(color);

            // Sets the new theme
            _newTheme.new_theme_uri = new Uri(String.Format(emptyUri.ToString() + Theme + ".xaml"));

            // Changes themes
            ThemeAP.SetNewTheme(this, _newTheme);
        }

        // Gets Blue / Red / Green for the CurrentTheme method
        private string GetCurrentColor()
        {
            switch (Theme)
            {
                case "LightBlue":
                    return "Blue";
                case "LightRed":
                    return "Red";
                case "LightGreen":
                    return "Green";
                case "Navy":
                    return "Blue";
                case "Crimson":
                    return "Red";
                case "Green":
                    return "Green";
            }
            return string.Empty;
        }

        // Goes over the new color and sets the theme according to the Dark Mode button clicked or not
        // Unchecks all checkboxes other than the one it is setting the color to
        private void CurrentTheme(string color)
        {
            switch (color)
            {
                case "Blue":
                    if (DarkMode)
                        Theme = "LightBlue";
                    else
                        Theme = "Navy";
                    cbNavyOption.IsChecked = true;
                    cbGreenOption.IsChecked = false;
                    cbRedOption.IsChecked = false;
                    break;
                case "Red":
                    if (DarkMode)
                        Theme = "LightRed";
                    else
                        Theme = "Crimson";
                    cbNavyOption.IsChecked = false;
                    cbGreenOption.IsChecked = false;
                    cbRedOption.IsChecked = true;
                    break;
                case "Green":
                    if (DarkMode)
                        Theme = "LightGreen";
                    else
                        Theme = "Green";
                    cbNavyOption.IsChecked = false;
                    cbGreenOption.IsChecked = true;
                    cbRedOption.IsChecked = false;
                    break;
            }
        }

        #endregion
    }
}
