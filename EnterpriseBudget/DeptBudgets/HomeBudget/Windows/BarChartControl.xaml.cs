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

        public void DisplayChart(List<KeyValuePair<string, decimal>> kvp, List<Budget.BudgetItem> expenses, int numberOfItemsInExpenses)
        {
            SeriesCollection = new SeriesCollection();
            List<String> labels = new List<string>();

            // For each category
            for (int i = 0; i < kvp.Count; i++)
            {
                SeriesCollection.Add(new StackedColumnSeries
                {
                    StackMode = StackMode.Values,
                    DataLabels = true,
                    LabelPoint = p => p.X.ToString("c")
                });
                string currentCat = kvp[i].Key;
                labels.Add(currentCat);
                decimal limit = kvp[i].Value;
                decimal totalSpent = 0;
                // For each category, add all the fields (Available / Spent)
                for(int x = 0; x < numberOfItemsInExpenses; x++)
                {
                    Budget.BudgetItem currentExpense = expenses[x];
                    if (currentExpense != null && currentExpense.Category == currentCat)
                    {
                        totalSpent += (decimal)currentExpense.Amount;
                    }
                }
                // i dont know about this tbh but the values have to be initialized
                SeriesCollection[i].Values = new ChartValues<decimal> { totalSpent };
                SeriesCollection[i].Values = new ChartValues<decimal> { limit - totalSpent };
                
                SeriesCollection[i].Values.Add(totalSpent);
                SeriesCollection[i].Values.Add(limit - totalSpent);

            }

            Formatter = val => val.ToString("P");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
