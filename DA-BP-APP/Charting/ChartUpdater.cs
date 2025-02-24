using LiveCharts;
using LiveCharts.WinForms;
using System.Collections.Generic;
using System.Linq;
using DA_BP_APP.Models;

namespace DA_BP_APP.Charting
{
    public class ChartUpdater
    {
        public void UpdateChart(CartesianChart chart, List<CommodityData> data)
        {
            chart.Series.Clear();
            var series = new LiveCharts.Wpf.ColumnSeries
            {
                Title = "Commodities",
                Values = new ChartValues<double>(data.Select(d => d.TotalQuantity))
            };
            chart.Series.Add(series);
            chart.AxisX.Clear();
            chart.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Commodity Name",
                Labels = data.Select(d => d.CommodityName).ToArray()
            });
            chart.AxisY.Clear();
            chart.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Total Quantity"
            });
        }
    }
}

