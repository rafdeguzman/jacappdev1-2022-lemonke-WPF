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
    public partial class MainWindow : Window
    {
        //encapsulate each in presenter view etc after.

        //IF BUDGET.DB DOESNT EXIST IT MEANS IT'S A FIRST TIME USER
        //show messagebox window and ask if they want to create using DEFAULT PATH
        //or select the directory
        const string DEFAULT_DIRECTORY = "\\Documents\\BudgetFiles\\";
        //const string DEFAULT_FILENAME = "budget.db";
        //const string DEFAULT_FILEPATH = DEFAULT_DIRECTORY + DEFAULT_FILENAME
        const string DEFAULT_FILENAME = "./budget.db";
        public MainWindow()
        {
            InitializeComponent();
            
            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseWindow aew = new AddExpenseWindow();
            aew.Show();
        }
    }
}
