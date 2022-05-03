﻿using System;
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
    public partial class UserControlWindow : Window
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
                cbMonths.ItemsSource = _dataSource;
                if (chartType == ChartType.ByMonthAndCategory)

                    drawByMonthPieChart();

                if (chartType == ChartType.ByMonth) drawByMonthLineChart();
            }
        }
        #region public methods

        // ---------------------------------------------------------------------
        // constructor
        // ---------------------------------------------------------------------
        public UserControlWindow()
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
        public void InitializeByCategoryAndMonthDisplay(List<string> CategoryList)
        {
            txtTitle.Text = "By Month";
            chartType = ChartType.ByMonthAndCategory; // set chart type appropriately

            chPie.Visibility = Visibility.Visible; // show the pie chart

            this.Categories = CategoryList; // save the categorieslist
        }
        // ---------------------------------------------------------------------
        // prepare for 'byCategory',
        // NOTE: just show invalid text... this chart is not implemented
        // ---------------------------------------------------------------------
        public void InitializeByCategoryDisplay()
        {
            chPie.Visibility = Visibility.Hidden;
        }
        // ----------------------------------------------------------------------
        // prepare for 'byMonth',
        // NOTE: just show invalid text... this chart is not implemented
        // ---------------------------------------------------------------------
        public void InitializeByMonthDisplay()
        {
            chPie.Visibility = Visibility.Hidden;
        }
        // ---------------------------------------------------------------------
        // prepare for standard display,
        // NOTE: just show invalid text... this chart is not implemented
        // ---------------------------------------------------------------------
        public void InitializeStandardDisplay()
        {
            chPie.Visibility = Visibility.Hidden;
        }
        #endregion

        // ---------------------------------------------------------------------
        // draw by Month is NOT implemented :(
        // ---------------------------------------------------------------------
        private void drawByMonthLineChart()
        {
        }

        #region byMonthAndCategory
        // --------------------------------------------------------------------
        // Draw the 'ByMonth' chart
        // --------------------------------------------------------------------
        private void drawByMonthPieChart()
        {
            // create a list of months from the source data
            List<String> months = new List<String>();
            foreach (object obj in _dataSource)
            {
                var item = obj as Dictionary<String, object>;
                if (item != null)
                {
                    months.Add(item["Month"].ToString());
                }
            }
            // add the months to the combobox dropdown
            cbMonths.ItemsSource = months;
            // reset selected index to last 'month' in list
            cbMonths.SelectedIndex = -1;
            // set the data for the pie-chart
            set_MonthCategory_Data();
        }

        // --------------------------------------------------------------------
        // define the data for the given month from the datasoure,
        // ... which in this case is a list of Dictionary<String,object>
        // defining totals for each category for a given month
        // --------------------------------------------------------------------
        private void set_MonthCategory_Data()
        {
            DataClear();
            // bail out if there are no 'month' items in the drop down
            if (cbMonths.Items.Count == 0) return;
            // set the default selection to the last in the list
            if (cbMonths.SelectedIndex < 0 || cbMonths.SelectedIndex >

            cbMonths.Items.Count - 1)

            {
                cbMonths.SelectedIndex = cbMonths.Items.Count - 1;
            }
            // what is the selected month?
            String selectedMonth = cbMonths.SelectedItem.ToString();
            // ---------------------------------------------------------------
            // define which data is to be displayed
            // ---------------------------------------------------------------
            var DisplayData = new List<KeyValuePair<String, double>>();
            foreach (object obj in _dataSource)
            {
                var item = obj as Dictionary<String, object>;
                // is the item listed in the _dataSource part of the selected month ?

                if (item != null && (string)item["Month"] == selectedMonth)
                {
                    // go through each key/value pair in this item (item is adictionary)

                    foreach (var pair in item)
                    {
                        String category = pair.Key;
                        String value = pair.Value.ToString();
                        // if the key is not a category, skip processing
                        if (!Categories.Contains(category)) continue;
                        // what is the amount of money for this category (item[category])

                        var amount = 0.0;
                        double.TryParse(value, out amount);
                        // only display expenses (i.e., amount < 0)
                        if (amount < 0)
                        {
                            DisplayData.Add(new KeyValuePair<String, double>

                            (category, -amount));
                        }
                    }
                    // we found the month we wanted, no need to loop through other months, so

                    // stop looking
                    break;
                }
            }
        // set the data for the pie-chart
        ((PieSeries)chPie.Series[0]).ItemsSource = DisplayData;
        }
        #endregion
        private void cbMonths_SelectionChanged(object sender,
        SelectionChangedEventArgs e)
        {
            set_MonthCategory_Data();
        }
    }
}