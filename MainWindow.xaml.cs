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
        private readonly Presenter presenter;
        public MainWindow()
        {
            InitializeComponent();
            presenter = new Presenter(this);
        }

        private void btnAdd1_Click(object sender, RoutedEventArgs e)
        {
            cmbCategories.Items.Add(txtBox1.Text);
            presenter.AddCategory();
        }

        public string GetStringInput()
        {
            return txtBox1.Text;
        }

    }
}
