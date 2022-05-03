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
using System.Windows.Controls.DataVisualization.Charting;
using System.Linq;


namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for PieChart.xaml
    /// </summary>
    public partial class DataChartControl : UserControl
    {
        // ---------------------------------------------------------------------
        // private globals
        // ---------------------------------------------------------------------
        private List<object> _dataSource;
        private enum ChartType
        {
            Standard,
            ByCategory,
            ByMonth,
            ByMonthAndCategory
        }
        private ChartType chartType = ChartType.Standard;
        private List<string> Categories;
        // ---------------------------------------------------------------------
        // public properites
        // ---------------------------------------------------------------------
        public Presenter presenter { get; set; }
        public List<object> DataSource
        {
            get { return _dataSource; }
            set
            {
                // if changing data source, then redraw chart
                _dataSource = value;
            }
        }
        #region public methods

        // ---------------------------------------------------------------------
        // constructor
        // ---------------------------------------------------------------------
        public DataChartControl()
        {
            InitializeComponent();
        }
        // ---------------------------------------------------------------------
        // clear the current data
        // ---------------------------------------------------------------------
        public void DataClear()
        {
            ((PieSeries)chPie.Series[0]).ItemsSource = null;
        }

        // ---------------------------------------------------------------------
        // Get prepared for displaying Month and Category
        // Inputs: usedCategoryList... a list of categories
        // ---------------------------------------------------------------------
        #endregion
    }
}