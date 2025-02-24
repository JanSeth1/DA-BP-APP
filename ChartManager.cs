using System.Collections.Generic;
using System.Linq;
using LiveCharts.Wpf;
using LiveCharts;

namespace DA_BP_APP.Services
{
    public class ChartManager
    {
        public void UpdateChart(CartesianChart chart, List<KeyValuePair<string, double>> dataPoints)
        {
            chart.Series.Clear();
            var series = new ColumnSeries
            {
                Title = "Revenue",
                Values = new ChartValues<double>(dataPoints.Select(dp => dp.Value))
            };

            chart.Series.Add(series);

            chart.AxisX.Clear();
            chart.AxisX.Add(new Axis
            {
                Title = "Barangays",
                Labels = dataPoints.Select(dp => dp.Key).ToArray()
            });

            chart.AxisY.Clear();
            chart.AxisY.Add(new Axis
            {
                Title = "Revenue"
            });
        }
    }
}
