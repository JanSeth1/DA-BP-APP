using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using LiveCharts.Wpf;
using LiveCharts;

namespace DA_BP_APP
{
    public partial class Home : MaterialForm
    {
        public Home()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Green200, TextShade.WHITE);
            txt_farmerCount.Font = new Font(txt_farmerCount.Font.FontFamily, 50); // Change font size to 14
            foreach (Control control in this.Controls)
            {
                if (control is Label)
                {
                    Label label = (Label)control;
                    label.Font = new Font(label.Font.FontFamily, 20); // Or your desired font settings
                }
            }
            comboBox1.SelectedIndex = 2; // Sets the third item as the default
            LoadCount();
        }

        private void LoadFarmerHarvestChart()
        {
            try
            {
                string query = @"
                    SELECT f.FirstName, f.LastName, SUM(h.Quantity) AS TotalQuantity
                    FROM Harvest h
                    JOIN Farmers f ON h.FarmerID = f.FarmerID
                    GROUP BY f.FarmerID, f.FirstName, f.LastName
                    ORDER BY TotalQuantity DESC"; // You can add LIMIT if needed

                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        MySqlDataReader reader = command.ExecuteReader();

                        // Initialize series collection and x-axis labels
                        var seriesCollection = new SeriesCollection();
                        var xAxisLabels = new List<string>();

                        while (reader.Read())
                        {
                            string farmerName = reader.GetString("FirstName") + " " + reader.GetString("LastName");
                            double totalQuantity = reader.GetDouble("TotalQuantity");

                            // Add a series for each farmer
                            seriesCollection.Add(new ColumnSeries
                            {
                                Title = farmerName,
                                Values = new ChartValues<double> { totalQuantity }
                            });

                            // Add farmer name to x-axis labels
                            xAxisLabels.Add(farmerName);
                        }

                        cartesianChart1.Series = seriesCollection;
                        cartesianChart1.AxisX.Add(new Axis
                        {
                            Title = "Farmers",
                            Labels = xAxisLabels.ToArray()
                        });

                        cartesianChart1.AxisY.Add(new Axis
                        {
                            Title = "Total Harvest",
                            LabelFormatter = value => value.ToString("N")
                        });

                        // Customize chart appearance as needed (e.g., colors, labels)
                        // ...
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadCount()
        {
            string query = "SELECT COUNT(*) FROM Farmers";
            try
            {

                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        txt_farmerCount.Text = $"Total Count: {count}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void Home_Load(object sender, EventArgs e)
        {

        }


    }

}

