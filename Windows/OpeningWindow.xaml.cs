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
        public OpeningWindow()
        {
            InitializeComponent();
        }

        private void btnNewDB_Click(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["newDB"].Value = "true";
            ConfigurationManager.AppSettings.Set("newDB", "true");
            config.Save(ConfigurationSaveMode.Modified);
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

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
    }
}
