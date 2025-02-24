using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using DA_BP_APP.Models;

public class ChartUpdater
{
    public void UpdateChart(CartesianChart chart, List<CommodityData> data)
    {
        chart.Series.Clear();
        var series = new ColumnSeries
        {
            Title = "Commodities",
            Values = new ChartValues<double>(data.Select(d => d.TotalQuantity))
        };
        chart.Series.Add(series);
        chart.AxisX.Clear();
        chart.AxisX.Add(new Axis
        {
            Title = "Commodity Name",
            Labels = data.Select(d => d.CommodityName).ToArray()
        });
        chart.AxisY.Clear();
        chart.AxisY.Add(new Axis
        {
            Title = "Total Quantity"
        });
    }
}
