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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Configuration;

namespace HomeBudgetWPF.Windows
{
    /// <summary>
    /// Interaction logic for OpeningWindow.xaml
    /// </summary>
    public partial class OpeningWindow : Window
    {
        #region Backing Fields
        Config config;
        #endregion

        #region Constructor
        /// <summary>
        /// The first window the user sees, there are two options.
        /// One opens up a new db and the other opens up an existing db file
        /// </summary>
        public OpeningWindow()
        {
            InitializeComponent();
            config = new Config();

            // This sets the correct color
            SettingsWindow sw = new SettingsWindow();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Logic for opening new db file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewDB_Click(object sender, RoutedEventArgs e)
        {
            config.newDB = true;
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        /// <summary>
        /// Logic for opening an existing db file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenDB_Click(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["newDB"].Value = "false";
            ConfigurationManager.AppSettings.Set("newDB", "false");
            config.Save(ConfigurationSaveMode.Modified);
            //work here
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
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
        /// Closes all the open windows
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
        #endregion
    }
}
