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
        private Config config;

        // URI's to the different themes
        private Uri NavyUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/Navy.xaml");
        private Uri CrimsonUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/Crimson.xaml");
        private Uri GreenUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/Green.xaml");
        private Uri YellowUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/Yellow.xaml");
        private Uri lightBlueUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/LightBlue.xaml");
        private Uri lightRedUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/LightRed.xaml");
        private Uri lightGreenUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/LightGreen.xaml");
        private Uri lightYellowUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/LightYellow.xaml");


        private Uri tbUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/TextBlockDictionary.xaml");
        private Uri buttonUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/ButtonDictionary.xaml");
        private Uri ListOfColorsUri =
            new Uri("pack://application:,,,/HomeBudgetWPF;component/Resource Dictionaries/Colors/ListOfColors.xaml");
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
        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Settings Window constructor 
        /// Makes sure all the correct checkboxes are checked
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
            config = new Config();

            CurrentTheme();
            ChangeDictionaries();
        }
        #endregion

        #region METHODS


        // When Dark Mode checkbox is checked
        private void CbDarkMode_OnClick(object sender, RoutedEventArgs e)
        {
            if (cbDarkMode.IsChecked == true)
                config.darkMode = "true";
            else
                config.darkMode = "false";
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
            SetCurrentColor(color);

            //Get the selected option
            CurrentTheme();

            ChangeDictionaries();
        }
        private void ChangeDictionaries()
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = tbUri });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = buttonUri });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = ListOfColorsUri });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = currentTheme });
        }

        // Gets Blue / Red / Green for the CurrentTheme method
        private string GetCurrentColor()
        {
            return config.themeColor;
        }
        private void SetCurrentColor(string color)
        {
            config.themeColor = color;
        }
        private bool GetDarkMode()
        {
            return bool.Parse(config.darkMode);
        }

        // Goes over the new color and sets the theme according to the Dark Mode button clicked or not
        // Unchecks all checkboxes other than the one it is setting the color to
        public void CurrentTheme()
        {
            switch (GetCurrentColor())
            {
                case "Blue":
                    if (GetDarkMode())
                        Theme = lightBlueUri;
                    else
                        Theme = NavyUri;
                    cbNavyOption.IsChecked = true;
                    cbGreenOption.IsChecked = false;
                    cbRedOption.IsChecked = false;
                    cbYellowOption.IsChecked = false;
                    break;
                case "Red":
                    if (GetDarkMode())
                        Theme = lightRedUri;
                    else
                        Theme = CrimsonUri;
                    cbNavyOption.IsChecked = false;
                    cbGreenOption.IsChecked = false;
                    cbRedOption.IsChecked = true;
                    cbYellowOption.IsChecked = false;
                    break;
                case "Green":
                    if (GetDarkMode())
                        Theme = lightGreenUri;
                    else
                        Theme = GreenUri;
                    cbNavyOption.IsChecked = false;
                    cbGreenOption.IsChecked = true;
                    cbRedOption.IsChecked = false;
                    cbYellowOption.IsChecked = false;
                    break;
                case "Yellow":
                    if (GetDarkMode())
                        Theme = lightYellowUri;
                    else
                        Theme = YellowUri;
                    cbNavyOption.IsChecked = false;
                    cbGreenOption.IsChecked = false;
                    cbRedOption.IsChecked = false;
                    cbYellowOption.IsChecked = true;
                    break;
            }
            if (GetDarkMode())
                cbDarkMode.IsChecked = true;
        }

        #endregion
    }
}
