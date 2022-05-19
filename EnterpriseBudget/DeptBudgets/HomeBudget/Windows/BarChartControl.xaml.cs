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
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;

namespace EnterpriseBudget.DeptBudgets.HomeBudget
{
    /// <summary>
    /// Interaction logic for BarChartControl.xaml
    /// </summary>
    public partial class BarChartControl : UserControl
    {
        public BarChartControl()
        {
            InitializeComponent();
        }

        public void DisplayChart(ChartValues<decimal> spent, ChartValues<decimal> remainder, List<string> labels)
        {
            SeriesCollection = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Title = "Amount Spent",
                    Values = spent,
                    StackMode = StackMode.Percentage, // this is not necessary, values is the default stack mode
                    DataLabels = true,
                    UseLayoutRounding = false
                },
                new StackedColumnSeries
                {
                    Title = "Amount Remaining",
                    Values = remainder,
                    StackMode = StackMode.Percentage,
                    DataLabels = true,
                    UseLayoutRounding = false
                }
            };

            Formatter = val => val.ToString("c");
            Labels = labels.ToArray();
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
