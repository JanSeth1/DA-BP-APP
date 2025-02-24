using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using LiveCharts.Wpf;
using LiveCharts;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using ADGV;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using DA_BP_APP.DataAccess;
using DA_BP_APP.Models;
using DA_BP_APP.ReportGeneration;
using DA_BP_APP.Charting;
using ChartUpdater = DA_BP_APP.Charting.ChartUpdater;
using Font = System.Drawing.Font;
using ITextFont = iTextSharp.text.Font;
using iTextSharp.text.pdf.draw;
using PdfImage = iTextSharp.text.Image;
using OfficeOpenXml;  // Add this line for EPPlus

namespace DA_BP_APP
{
    public partial class Form1 : MaterialForm

    {

        public DataGridView FarmersGrid
        {
            get { return farmersGrid; }
        }

        private DatabaseManager _dbManager;
        private BarangayReportGenerator _barangayReportGenerator;
        private ChartUpdater _chartUpdater;
        private string tableName = "farmer";
        private Login loginForm;
        private Form1 form1;
        private IChartValues farmerCounts;
        private object monthLabels;
        private string reportType; // To store "Barangay" or "Farmer"
        private List<ReportData> reportData; // Declare the reportData list
        private int year;
        private string commodityName;
        private int currentFarmerIndex = 0;
        private float currentY = 0;
        private List<int> farmersToPrint;
        private System.Drawing.Printing.PrintDocument currentPrintDocument;
        private bool isDarkMode = true;
        private List<Details> openDetailsForms = new List<Details>();
        private TabPage comparisonTab = null;
        private string userRole; // Add this field to store user role
        private List<(string FirstName, string LastName)> farmersToPrintNames;

        // Add this helper method at class level
        private string GetCellValue(DataGridViewRow row, string columnName)
        {
            var cell = row.Cells[columnName];
            if (cell != null && cell.Value != null && cell.Value != DBNull.Value)
            {
                return cell.Value.ToString();
            }
            return "";
        }

        public Form1(string role = "User") // Add role parameter with default value
        {
            // Set EPPlus license context with full namespace
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            InitializeComponent();
            userRole = role;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Green200, TextShade.WHITE);
            
            // Create FarmerNotes table if it doesn't exist
            CreateFarmerNotesTable();
            
            Load += Form1_Load;
            LoadCount();
            LoadFarmerRegistrationChart();
            farmersGrid.CellDoubleClick += new DataGridViewCellEventHandler(advancedDataGridView1_CellDoubleClick);

            InitializePredictionTab(Prediction);
            InitializeSettingsTab();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCommodities();
            comboBoxCommodities.SelectedIndexChanged += ComboBoxCommodities_SelectedIndexChanged;
            
            LoadCount();
            LoadFarmerRegistrationChart();
            timer1.Start();
            LoadFarmersData();
            ConfigureDataGridView();
            LoadYearComboBoxData();
            LoadMonthComboBoxData();
        }

        private void InitializeReportSection()
        {
            // Remove this entire method as it's no longer needed
        }


        private ChartData FetchChartData(string barangay, int year, int month)
        {
            var values = new List<double>();
            string query = @"
SELECT h.TotalRevenue 
FROM Harvest h
JOIN Farmers f ON h.FarmerID = f.FarmerID 
WHERE f.Barangay = @barangay 
AND YEAR(h.HarvestDate) = @year 
AND MONTH(h.HarvestDate) = @month";

            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@barangay", barangay);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);

                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    values.Add(Convert.ToDouble(reader.GetDecimal("TotalRevenue")));
                }
            }

            Console.WriteLine($"Fetched {values.Count} values for {barangay}, {year}, {month}.");
            return new ChartData { Values = values };
        }




        // Define a class to hold chart data
        public class ChartData
        {
            public List<double> Values { get; set; }
        }





        private void PopulateCommodities()
        {
            // Remove this method as it's for the old report section
        }

        private void GenerateBarangayReport(int year, string commodityName)
        {
            // Remove this method as it's for the old report section
        }

        private void GenerateFarmerReport(int year, string commodityName)
        {
            // Remove this method as it's for the old report section
        }

        private void UpdateChart(List<string> names, ChartValues<double> quantities, ChartValues<double> revenues)
        {
            // Remove this method as it's for the old report section
        }

        private void InitializeChartWithStaticData()
        {
            // Remove this method as it's for the old report section
        }

        private void ButtonSaveReport_Click(object sender, EventArgs e)
        {
            // Remove this method as it's for the old report section
        }

        private void GeneratePdfReport(List<ReportData> reportData, int year, string commodityName, string reportType)
        {
            // Remove this method as it's for the old report section
        }

       
        public class ReportData
        {
            // Remove this class as it's for the old report section
        }

        private void buttonGenerateReport_Click(object sender, EventArgs e)
        {
            // Remove this method as it's for the old report section
        }

private async Task LoadFarmersData()
{
    try
    {
        string query = @"
            SELECT 
                COALESCE(f.FirstName, '') AS 'First Name',
                COALESCE(f.LastName, '') AS 'Last Name',
                COALESCE(f.MiddleName, '') AS 'Middle Name',
                COALESCE(DATE_FORMAT(f.Birthdate, '%Y-%m-%d'), '') AS 'Birthdate',
                COALESCE(f.Address, '') AS Address,
                COALESCE(f.Barangay, '') AS Barangay,
                COALESCE(f.ContactInfo, '') AS 'Contact Info',
                COALESCE(total_area.TotalFarmSize, f.FarmSize, 0) AS 'Total Farm Size',
                COALESCE(DATE_FORMAT(f.RegistrationDate, '%Y-%m-%d'), '') AS 'Registration Date',
                COALESCE(GROUP_CONCAT(
                    DISTINCT 
                    CASE 
                        WHEN c.CommodityName IS NOT NULL THEN 
                            CONCAT(
                                c.CommodityName,
                                COALESCE(
                                    CONCAT(' (', CAST(h.TotalArea AS CHAR), ' ha)'),
                                    ' (No area data)'
                                )
                            )
                        ELSE 
                            CASE COALESCE(f.Crop, 0)
                                WHEN 1 THEN 'Coconut'
                                WHEN 2 THEN 'Cacao'
                                WHEN 3 THEN 'Rice'
                                WHEN 4 THEN 'Corn'
                                WHEN 5 THEN 'Livestock'
                                WHEN 6 THEN 'Fish'
                                WHEN 7 THEN 'HVC'
                                WHEN 8 THEN 'Industrial Crop'
                                ELSE 'Unknown'
                            END
                    END
                    ORDER BY c.CommodityName 
                    SEPARATOR ', '
                ), '') AS Commodities
            FROM Farmers f
            LEFT JOIN farmercommodities fc ON f.FarmerID = fc.FarmerID
            LEFT JOIN Commodities c ON fc.CommodityID = c.CommodityID
            LEFT JOIN (
                SELECT 
                    FarmerID,
                    CommodityID,
                    SUM(COALESCE(TotalArea, 0)) as TotalArea
                FROM harvest
                GROUP BY FarmerID, CommodityID
            ) h ON h.FarmerID = f.FarmerID AND h.CommodityID = fc.CommodityID
            LEFT JOIN (
                SELECT 
                    FarmerID,
                    SUM(CASE WHEN TotalArea IS NULL THEN 0 ELSE TotalArea END) as TotalFarmSize
                FROM harvest
                GROUP BY FarmerID
            ) total_area ON total_area.FarmerID = f.FarmerID
            GROUP BY f.FarmerID, f.FirstName, f.LastName, f.MiddleName, f.Address, 
                     f.Barangay, f.ContactInfo, f.FarmSize, f.RegistrationDate, f.Crop,
                     total_area.TotalFarmSize
            ORDER BY f.LastName, f.FirstName";

        using (var conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
        {
            await conn.OpenAsync();
            using (var adapter = new MySqlDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Update DataGridView on the UI thread
                Invoke(new Action(() =>
                {
                    farmersGrid.DataSource = dt;
                    ConfigureFarmersGridColumns();
                }));
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error loading farmers data: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}


        private void ConfigureFarmersGridColumns()
        {
            // Enable filtering and sorting
            farmersGrid.FilterStringChanged += FarmersGrid_FilterStringChanged;
            farmersGrid.SortStringChanged += FarmersGrid_SortStringChanged;
            farmersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Configure column widths and styles
            farmersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                
            if (farmersGrid.Columns["First Name"] != null)
            {
                farmersGrid.Columns["First Name"].Width = 180;
                farmersGrid.Columns["First Name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            if (farmersGrid.Columns["Last Name"] != null)
            {
                farmersGrid.Columns["Last Name"].Width = 180;
                farmersGrid.Columns["Last Name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            if (farmersGrid.Columns["Middle Name"] != null)
            {
                farmersGrid.Columns["Middle Name"].Width = 180;
                farmersGrid.Columns["Middle Name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            if (farmersGrid.Columns["Birthdate"] != null)
            {
                farmersGrid.Columns["Birthdate"].Width = 120;
                farmersGrid.Columns["Birthdate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                farmersGrid.Columns["Birthdate"].SortMode = DataGridViewColumnSortMode.Programmatic;
                farmersGrid.Columns["Birthdate"].DefaultCellStyle.NullValue = string.Empty;
            }

            if (farmersGrid.Columns["Address"] != null)
            {
                farmersGrid.Columns["Address"].Width = 285;
                farmersGrid.Columns["Address"].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            if (farmersGrid.Columns["Barangay"] != null)
            {
                farmersGrid.Columns["Barangay"].Width = 200;
                farmersGrid.Columns["Barangay"].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            if (farmersGrid.Columns["Contact Info"] != null)
            {
                farmersGrid.Columns["Contact Info"].Width = 120;
                farmersGrid.Columns["Contact Info"].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            if (farmersGrid.Columns["Total Farm Size"] != null)
            {
                farmersGrid.Columns["Total Farm Size"].Width = 120;
                farmersGrid.Columns["Total Farm Size"].DefaultCellStyle.Format = "N2";
                farmersGrid.Columns["Total Farm Size"].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            if (farmersGrid.Columns["Registration Date"] != null)
            {
                farmersGrid.Columns["Registration Date"].Width = 150;
                farmersGrid.Columns["Registration Date"].DefaultCellStyle.Format = "MM/dd/yyyy";
                farmersGrid.Columns["Registration Date"].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            if (farmersGrid.Columns["Commodities"] != null)
            {
                farmersGrid.Columns["Commodities"].Width = 320;
                farmersGrid.Columns["Commodities"].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            // Hide the Crop column if it exists
            if (farmersGrid.Columns["Crop"] != null)
                farmersGrid.Columns["Crop"].Visible = false;

            // Make sure we're using a DataView for proper filtering and sorting
            if (farmersGrid.DataSource is DataTable dt)
            {
                farmersGrid.DataSource = new DataView(dt);
            }

            // Add column click event handler for sorting
            farmersGrid.ColumnHeaderMouseClick += FarmersGrid_ColumnHeaderMouseClick;
        }

        private void FarmersGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (farmersGrid.DataSource is DataView dataView)
            {
                string columnName = farmersGrid.Columns[e.ColumnIndex].DataPropertyName;
                string currentSort = dataView.Sort;

                // Check if the column is already being sorted
                if (currentSort == $"{columnName} ASC")
                {
                    dataView.Sort = $"{columnName} DESC";
                }
                else if (currentSort == $"{columnName} DESC")
                {
                    dataView.Sort = string.Empty; // Clear sorting
                }
                else
                {
                    dataView.Sort = $"{columnName} ASC";
                }
            }
        }



        private void FarmersGrid_FilterStringChanged(object sender, EventArgs e)
            {
                try
            {
                if (farmersGrid.DataSource is DataView dataView)
                {
                    dataView.RowFilter = farmersGrid.FilterString;
                    // Update the total count in the label
                    if (farmersGrid.Parent != null && farmersGrid.Parent.Controls.Find("lblFarmerCount", true).FirstOrDefault() is Label countLabel)
                    {
                        countLabel.Text = $"Total Farmers: {dataView.Count}";
                    }
                }
                }
                catch (Exception ex)
                {
                MessageBox.Show($"Invalid filter: {ex.Message}", "Filter Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FarmersGrid_SortStringChanged(object sender, EventArgs e)
            {
                try
            {
                if (farmersGrid.DataSource is DataView dataView)
                {
                    dataView.Sort = farmersGrid.SortString;
                }
                }
                catch (Exception ex)
                {
                MessageBox.Show($"Invalid sort: {ex.Message}", "Sort Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void RefreshFarmersData()
        {
            LoadFarmersData();
        }


        private void advancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = farmersGrid.Rows[e.RowIndex];
                string firstName = row.Cells["First Name"].Value.ToString();
                string lastName = row.Cells["Last Name"].Value.ToString();
                ShowFarmerDetails(firstName, lastName);
            }
        }

        private void ShowFarmerDetails(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Invalid farmer name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT FarmerID FROM Farmers 
                                   WHERE FirstName = @FirstName 
                                   AND LastName = @LastName";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        var result = cmd.ExecuteScalar();
                        
                        if (result != null && result != DBNull.Value)
                        {
                            int farmerId = Convert.ToInt32(result);
                            Details detailsForm = new Details(farmerId, this);
                            openDetailsForms.Add(detailsForm);
                            detailsForm.FormClosed += (s, args) => openDetailsForms.Remove(detailsForm);
                            detailsForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Could not find farmer in database.", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading farmer details: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void LoadBarangays()
        {
            var barangays = _dbManager.LoadBarangays();
            foreach (var barangay in barangays)
            {
                
            }
        }

        private void ConfigureDataGridView()
        {
            // Configure DataGridView appearance
            farmersGrid.DefaultCellStyle.BackColor = Color.White;
            farmersGrid.DefaultCellStyle.ForeColor = Color.Black;
            farmersGrid.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            farmersGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
            farmersGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            farmersGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            farmersGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            farmersGrid.RowTemplate.Height = 30; // Increase row height for better readability

            // Find the farmers tab
            TabPage farmersTab = materialTabControl1.TabPages.Cast<TabPage>()
                .FirstOrDefault(tab => tab.Controls.Contains(farmersGrid));

            if (farmersTab != null)
            {
                // Create a panel for buttons
                Panel buttonPanel = new Panel
                {
                    Width = farmersTab.Width - 40, // 20px padding on each side
                    Height = 50,
                    Location = new Point(20, 20)
                };

                // Create search box
                TextBox searchBox = new TextBox
                {
                    Name = "txtSearch",
                    Size = new Size(200, 30),
                    Location = new Point(0, 10)
                };
                

                // Add count label
                    Label farmerCountLabel = new Label
                    {
                        Name = "lblFarmerCount",
                        Text = $"Total Farmers: {(farmersGrid.DataSource as DataTable)?.DefaultView.Count ?? 0}",
                    Location = new Point(searchBox.Right + 20, 15),
                        AutoSize = true,
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 10, FontStyle.Regular)
                    };

                // Create buttons with consistent style
                    MaterialButton printButton = new MaterialButton
                    {
                        Text = "PRINT SELECTED",
                        Size = new Size(150, 40),
                    Location = new Point(farmerCountLabel.Right + 30, 5)
                    };
                    printButton.Click += btnPrintSelected_Click;

                    MaterialButton printByBarangayButton = new MaterialButton
                    {
                        Text = "PRINT BY BARANGAY",
                        Size = new Size(200, 40),
                    Location = new Point(printButton.Right + 10, 5)
                    };
                    printByBarangayButton.Click += PrintByBarangayButton_Click;

                    MaterialButton addFarmerButton = new MaterialButton
                    {
                        Text = "ADD FARMER",
                        Size = new Size(150, 40),
                    Location = new Point(printByBarangayButton.Right + 10, 5)
                    };
                    addFarmerButton.Click += AddFarmerButton_Click;

                    MaterialButton addNoteButton = new MaterialButton
                    {
                        Text = "ADD NOTE",
                        Size = new Size(150, 40),
                    Location = new Point(addFarmerButton.Right + 10, 5)
                    };
                    addNoteButton.Click += AddNoteButton_Click;

                MaterialButton importButton = new MaterialButton
                {
                    Text = "IMPORT",
                    Size = new Size(150, 40),
                    Location = new Point(addNoteButton.Right + 10, 5)
                };
                importButton.Click += btn_batch_import_Click;

                // Initialize export button
                MaterialButton exportButton = new MaterialButton
                {
                    Text = "EXPORT",
                    Size = new Size(150, 40),
                    Location = new Point(importButton.Right + 10, 5),
                    Type = MaterialButton.MaterialButtonType.Contained,
                    UseAccentColor = false
                };

                exportButton.Click += (s, ev) => {
                    TabPage existingExportTab = null;
                    foreach (TabPage tab in materialTabControl1.TabPages)
                    {
                        if (tab.Text == "Export Data")
                        {
                            existingExportTab = tab;
                            break;
                        }
                    }

                    if (existingExportTab == null)
                    {
                        existingExportTab = new TabPage
                        {
                            Text = "Export Data",
                            BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White
                        };
                        materialTabControl1.TabPages.Add(existingExportTab);
                        InitializeExportTab(existingExportTab);
                    }

                    materialTabControl1.SelectedTab = existingExportTab;
                };

                // Add controls to button panel
                buttonPanel.Controls.AddRange(new Control[] { 
                    searchBox, 
                    farmerCountLabel, 
                        printButton, 
                        printByBarangayButton, 
                        addFarmerButton, 
                    addNoteButton,
                    importButton,
                    exportButton
                });

                // Add button panel to tab
                farmersTab.Controls.Add(buttonPanel);

                // Configure DataGridView position and size
                farmersGrid.Location = new Point(20, buttonPanel.Bottom + 10);
                farmersGrid.Size = new Size(farmersTab.Width - 40, farmersTab.Height - farmersGrid.Top - 20);
                farmersGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

                // Add event handlers for count updates
                farmersGrid.DataSourceChanged += (s, e) => UpdateFarmerCount(farmerCountLabel);
                farmersGrid.DataBindingComplete += (s, e) => UpdateFarmerCount(farmerCountLabel);

                    // Bring buttons to front
                foreach (Control button in buttonPanel.Controls.OfType<MaterialButton>())
                    {
                        button.BringToFront();
                }

                // Make sure the search box is properly connected
                if (searchBox != null)
                {
                    searchBox.TextChanged -= txtSearch_TextChanged; // Remove any existing handlers
                    searchBox.TextChanged += txtSearch_TextChanged; // Add the handler
                }
            }
        }

        // Add new method for updating count
        private void UpdateFarmerCount(Label countLabel)
        {
            if (countLabel != null && farmersGrid.DataSource is DataView dataView)
            {
                countLabel.Text = $"Total Farmers: {dataView.Count}";
                countLabel.BringToFront();
            }
        }

        private void PrintByBarangayButton_Click(object sender, EventArgs e)
        {
            using (Form selectBarangayForm = new Form())
            {
                selectBarangayForm.Text = "Select Barangay to Print";
                selectBarangayForm.Size = new Size(400, 150);
                selectBarangayForm.StartPosition = FormStartPosition.CenterParent;
                selectBarangayForm.BackColor = Color.FromArgb(50, 50, 50);
                selectBarangayForm.ForeColor = Color.White;

                ComboBox barangayComboBox = new ComboBox
                {
                    Location = new Point(20, 20),
                    Size = new Size(340, 30),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                LoadBarangaysToComboBox(barangayComboBox);

                MaterialButton printButton = new MaterialButton
                {
                    Text = "PRINT",
                    Location = new Point(20, 60),
                    Size = new Size(100, 36)
                };

                printButton.Click += (s, ev) =>
                {
                    if (barangayComboBox.SelectedItem != null)
                    {
                        string selectedBarangay = barangayComboBox.SelectedItem.ToString();
                        PrintFarmersByBarangay(selectedBarangay);
                        selectBarangayForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please select a barangay.", "Selection Required", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                selectBarangayForm.Controls.AddRange(new Control[] { barangayComboBox, printButton });
                selectBarangayForm.ShowDialog();
            }
        }

        private void PrintFarmersByBarangay(string barangay)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT FirstName, LastName 
                FROM Farmers 
                WHERE Barangay = @Barangay
                ORDER BY FirstName, LastName";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Barangay", barangay);

                    List<(string FirstName, string LastName)> farmerNames = new List<(string FirstName, string LastName)>();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            farmerNames.Add((
                                reader["FirstName"].ToString(),
                                reader["LastName"].ToString()
                            ));
                        }
                    }

                    if (farmerNames.Count > 0)
                    {
                        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"FarmersByBarangay_{barangay}.pdf");
                        GeneratePdf(farmerNames, fileName);
                    }
                    else
                    {
                        MessageBox.Show($"No farmers found in {barangay}.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void AddNoteButton_Click(object sender, EventArgs e)
        {
            if (farmersGrid.SelectedCells.Count == 0 && farmersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a farmer to add a note.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row;
            if (farmersGrid.SelectedRows.Count > 0)
            {
                row = farmersGrid.SelectedRows[0];
            }
            else
            {
                int rowIndex = farmersGrid.SelectedCells[0].RowIndex;
                row = farmersGrid.Rows[rowIndex];
            }

            string firstName = GetCellValue(row, "First Name");
            string lastName = GetCellValue(row, "Last Name");

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Could not retrieve farmer information.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string farmerName = $"{firstName} {lastName}";

            using (Form noteForm = new Form())
            {
                noteForm.Text = $"Add Note for {farmerName}";
                noteForm.Size = new Size(500, 300);
                noteForm.StartPosition = FormStartPosition.CenterParent;
                noteForm.BackColor = Color.FromArgb(50, 50, 50);
                noteForm.ForeColor = Color.White;

                Label noteLabel = new Label
                {
                    Text = "Note:",
                    Location = new Point(20, 20),
                    Size = new Size(100, 20),
                    ForeColor = Color.White
                };

                TextBox noteTextBox = new TextBox
                {
                    Location = new Point(20, 50),
                    Size = new Size(440, 150),
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical
                };

                MaterialButton saveButton = new MaterialButton
                {
                    Text = "SAVE NOTE",
                    Location = new Point(20, 210),
                    Size = new Size(150, 36)
                };

                saveButton.Click += (s, ev) =>
                {
                    if (!string.IsNullOrWhiteSpace(noteTextBox.Text))
                    {
                        SaveFarmerNote(firstName, lastName, noteTextBox.Text);
                        noteForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a note.", "Empty Note", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                noteForm.Controls.AddRange(new Control[] { noteLabel, noteTextBox, saveButton });
                noteForm.ShowDialog();
            }
        }

        private void SaveFarmerNote(string firstName, string lastName, string note)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    // First get the farmer's ID from their name
                    string getFarmerIdQuery = @"
                        SELECT FarmerID 
                        FROM Farmers 
                        WHERE FirstName = @FirstName 
                        AND LastName = @LastName";

                    int farmerId;
                    using (MySqlCommand cmd = new MySqlCommand(getFarmerIdQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        var result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Could not find farmer in database.", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        farmerId = Convert.ToInt32(result);
                    }

                    // Format the note with timestamp and ensure it starts on a new line
                    string formattedNote = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {note}";

                    // Now save the note
                    string insertNoteQuery = @"
                        INSERT INTO FarmerNotes (FarmerID, Note, DateAdded) 
                        VALUES (@FarmerID, @Note, @DateAdded)";

                    using (MySqlCommand cmd = new MySqlCommand(insertNoteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@FarmerID", farmerId);
                        cmd.Parameters.AddWithValue("@Note", formattedNote);
                        cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Note added successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving note: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddFarmerButton_Click(object sender, EventArgs e)
        {
            using (Form addFarmerForm = new Form())
            {
                addFarmerForm.Text = "Add New Farmer";
                addFarmerForm.Size = new Size(500, 600);
                addFarmerForm.AutoScroll = true;
                addFarmerForm.StartPosition = FormStartPosition.CenterScreen;

                // Set form colors based on theme
                Color formBackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White;
                Color formForeColor = isDarkMode ? Color.White : Color.Black;
                addFarmerForm.BackColor = formBackColor;

                // Create a container panel for all controls
                Panel mainPanel = new Panel
                {
                    AutoScroll = true,
                    Dock = DockStyle.Fill,
                    BackColor = formBackColor
                };

                // Create input fields with larger labels
                Label firstNameLabel = new Label { Text = "First Name:", Location = new Point(30, 20), Size = new Size(110, 25), ForeColor = formForeColor, Font = new Font("Segoe UI", 9.5f) };
                MaterialTextBox firstNameBox = new MaterialTextBox { Location = new Point(140, 20), Size = new Size(320, 50) };

                Label lastNameLabel = new Label { Text = "Last Name:", Location = new Point(30, 90), Size = new Size(110, 25), ForeColor = formForeColor, Font = new Font("Segoe UI", 9.5f) };
                MaterialTextBox lastNameBox = new MaterialTextBox { Location = new Point(140, 80), Size = new Size(320, 50) };

                Label middleNameLabel = new Label { Text = "Middle Name:", Location = new Point(30, 150), Size = new Size(110, 25), ForeColor = formForeColor, Font = new Font("Segoe UI", 9.5f) };
                MaterialTextBox middleNameBox = new MaterialTextBox { Location = new Point(140, 140), Size = new Size(320, 50) };

                Label birthdateLabel = new Label { Text = "Birthdate:", Location = new Point(30, 200), Size = new Size(110, 25), ForeColor = formForeColor, Font = new Font("Segoe UI", 9.5f) };
                DateTimePicker birthdatePicker = new DateTimePicker { 
                    Location = new Point(140, 200), 
                    Size = new Size(320, 50), 
                    Format = DateTimePickerFormat.Short,
                    ShowCheckBox = true,
                    Checked = false
                };

                Label addressLabel = new Label { Text = "Address:", Location = new Point(30, 270), Size = new Size(110, 25), ForeColor = formForeColor, Font = new Font("Segoe UI", 9.5f) };
                MaterialTextBox addressBox = new MaterialTextBox { Location = new Point(140, 260), Size = new Size(320, 50) };

                Label barangayLabel = new Label { Text = "Barangay:", Location = new Point(30, 330), Size = new Size(110, 25), ForeColor = formForeColor, Font = new Font("Segoe UI", 9.5f) };
                MaterialComboBox barangayBox = new MaterialComboBox { Location = new Point(140, 320), Size = new Size(320, 50) };
                LoadBarangaysToComboBox(barangayBox); // Load barangays from database

                Label contactLabel = new Label { Text = "Contact Info:", Location = new Point(30, 390), Size = new Size(110, 25), ForeColor = formForeColor, Font = new Font("Segoe UI", 9.5f) };
                MaterialTextBox contactBox = new MaterialTextBox { Location = new Point(140, 380), Size = new Size(320, 50) };

                Label farmSizeLabel = new Label { Text = "Farm Size:", Location = new Point(30, 440), Size = new Size(110, 25), ForeColor = formForeColor, Font = new Font("Segoe UI", 9.5f) };
                NumericUpDown farmSizeBox = new NumericUpDown { Location = new Point(140, 440), Size = new Size(320, 50), DecimalPlaces = 2, Maximum = 1000 };

                Label registrationLabel = new Label { Text = "Registration:", Location = new Point(30, 500), Size = new Size(110, 25), ForeColor = formForeColor, Font = new Font("Segoe UI", 9.5f) };
                DateTimePicker registrationDate = new DateTimePicker { Location = new Point(140, 500), Size = new Size(320, 50) };

                Label commoditiesLabel = new Label { Text = "Commodities:", Location = new Point(30, 570), Size = new Size(110, 25), ForeColor = formForeColor, Font = new Font("Segoe UI", 9.5f) };
                Panel commoditiesPanel = new Panel { 
                    Location = new Point(140, 560), 
                    Size = new Size(320, 200), 
                    AutoScroll = true,
                    BackColor = formBackColor
                };

                // Dictionary to store area inputs for each commodity
                var commodityAreaInputs = new Dictionary<string, NumericUpDown>();

                // Load commodities
                using (MySqlConnection conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT CommodityID, CommodityName FROM Commodities ORDER BY CommodityName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int yPos = 0;
                            while (reader.Read())
                            {
                                string commodityName = reader["CommodityName"].ToString();
                                
                                // Create checkbox with theme-aware colors
                                CheckBox checkbox = new CheckBox
                                {
                                    Text = commodityName,
                                    Location = new Point(10, yPos),
                                    Size = new Size(150, 25),
                                    ForeColor = formForeColor
                                };

                                // Create and configure NumericUpDown for area
                                NumericUpDown areaInput = new NumericUpDown
                                {
                                    Location = new Point(170, yPos),
                                    Size = new Size(100, 25),
                                    DecimalPlaces = 2,
                                    Minimum = 0,
                                    Maximum = 1000,
                                    Enabled = false,
                                    BackColor = formBackColor,
                                    ForeColor = formForeColor
                                };

                                // Enable/disable area input based on checkbox
                                checkbox.CheckedChanged += (checkSender, checkArgs) =>
                                {
                                    areaInput.Enabled = checkbox.Checked;
                                    if (!checkbox.Checked)
                                        areaInput.Value = 0;
                                };

                                commoditiesPanel.Controls.Add(checkbox);
                                commoditiesPanel.Controls.Add(areaInput);
                                commodityAreaInputs[commodityName] = areaInput;
                                yPos += 30;
                            }
                        }
                    }
                }

                MaterialButton saveButton = new MaterialButton
                {
                    Text = "SAVE",
                    Location = new Point(392, 780),
                    Size = new Size(150, 40)
                };

                saveButton.Click += (s, ev) =>
                {
                    // Get selected commodities and their areas
                    var selectedCommodities = commodityAreaInputs
                        .Where(kvp => commoditiesPanel.Controls.OfType<CheckBox>()
                            .Any(cb => cb.Text == kvp.Key && cb.Checked))
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value);

                    if (ValidateFarmerInput(firstNameBox.Text, lastNameBox.Text, addressBox.Text,
                        barangayBox.Text, contactBox.Text, farmSizeBox.Value, selectedCommodities.Count > 0))
                    {
                        using (MySqlConnection conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                        {
                            conn.Open();
                            using (MySqlTransaction transaction = conn.BeginTransaction())
                            {
                                try
                                {
                                    // Insert farmer
                                    string insertFarmerQuery = @"
                                        INSERT INTO Farmers (FirstName, LastName, MiddleName, Address, Barangay, 
                                          ContactInfo, FarmSize, RegistrationDate, Birthdate, Crop)
                                        VALUES (@FirstName, @LastName, @MiddleName, @Address, @Barangay, 
                                                @ContactInfo, @FarmSize, @RegistrationDate, @Birthdate, 
                                                (SELECT CommodityID FROM Commodities WHERE CommodityName = @FirstCommodity));
                                        SELECT LAST_INSERT_ID();";

                                    MySqlCommand cmd = new MySqlCommand(insertFarmerQuery, conn, transaction);
                                    cmd.Parameters.AddWithValue("@FirstName", firstNameBox.Text);
                                    cmd.Parameters.AddWithValue("@LastName", lastNameBox.Text);
                                    cmd.Parameters.AddWithValue("@MiddleName", middleNameBox.Text);
                                    cmd.Parameters.AddWithValue("@Address", addressBox.Text);
                                    cmd.Parameters.AddWithValue("@Barangay", barangayBox.Text);
                                    cmd.Parameters.AddWithValue("@ContactInfo", contactBox.Text);
                                    cmd.Parameters.AddWithValue("@FarmSize", farmSizeBox.Value);
                                    cmd.Parameters.AddWithValue("@RegistrationDate", registrationDate.Value);
                                    cmd.Parameters.AddWithValue("@Birthdate", birthdatePicker.Checked ? (object)birthdatePicker.Value : DBNull.Value);

                                    // Add the first selected commodity as the main crop
                                    var firstSelectedCommodity = selectedCommodities.Keys.FirstOrDefault();
                                    cmd.Parameters.AddWithValue("@FirstCommodity", firstSelectedCommodity ?? (object)DBNull.Value);

                                    int farmerId = Convert.ToInt32(cmd.ExecuteScalar());

                                    // Insert commodities and harvest records
                                    foreach (var commodity in selectedCommodities)
                                    {
                                        // Get commodity ID
                                        string getCommodityIdQuery = "SELECT CommodityID FROM Commodities WHERE CommodityName = @CommodityName";
                                        cmd = new MySqlCommand(getCommodityIdQuery, conn, transaction);
                                        cmd.Parameters.AddWithValue("@CommodityName", commodity.Key);
                                        object result = cmd.ExecuteScalar();
                                        
                                        if (result != null)
                                        {
                                            int commodityId = Convert.ToInt32(result);

                                            // Insert into FarmerCommodities
                                            string insertCommodityQuery = @"
                                                INSERT INTO FarmerCommodities (FarmerID, CommodityID) 
                                                VALUES (@FarmerID, @CommodityID)";
                                            cmd = new MySqlCommand(insertCommodityQuery, conn, transaction);
                                            cmd.Parameters.AddWithValue("@FarmerID", farmerId);
                                            cmd.Parameters.AddWithValue("@CommodityID", commodityId);
                                            cmd.ExecuteNonQuery();

                                            // Insert into Harvest table with area if specified
                                            if (commodity.Value > 0)
                                            {
                                                string insertHarvestQuery = @"
                                                    INSERT INTO Harvest (FarmerID, CommodityID, HarvestDate, TotalArea)
                                                    VALUES (@FarmerID, @CommodityID, @HarvestDate, @TotalArea)";
                                                cmd = new MySqlCommand(insertHarvestQuery, conn, transaction);
                                                cmd.Parameters.AddWithValue("@FarmerID", farmerId);
                                                cmd.Parameters.AddWithValue("@CommodityID", commodityId);
                                                cmd.Parameters.AddWithValue("@HarvestDate", registrationDate.Value);
                                                cmd.Parameters.AddWithValue("@TotalArea", commodity.Value);
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                    }

                                    transaction.Commit();
                                    MessageBox.Show("Farmer added successfully!", "Success",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    addFarmerForm.Close();
                                    LoadFarmersData();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    MessageBox.Show($"Error adding farmer: {ex.Message}", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                };

                // Add all controls to the main panel
                mainPanel.Controls.AddRange(new Control[] {
                    firstNameLabel, firstNameBox,
                    lastNameLabel, lastNameBox,
                    middleNameLabel, middleNameBox,
                    birthdateLabel, birthdatePicker,
                    addressLabel, addressBox,
                    barangayLabel, barangayBox,
                    contactLabel, contactBox,
                    farmSizeLabel, farmSizeBox,
                    registrationLabel, registrationDate,
                    commoditiesLabel, commoditiesPanel,
                    saveButton
                });

                // Add the main panel to the form
                addFarmerForm.Controls.Add(mainPanel);

                addFarmerForm.ShowDialog();
            }
        }

        private void LoadBarangaysToComboBox(ComboBox comboBox)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT DISTINCT Barangay FROM Farmers ORDER BY Barangay";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader["Barangay"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading barangays: " + ex.Message);
                }
            }
        }

        private bool ValidateFarmerInput(string firstName, string lastName, string address, 
            string barangay, string contact, decimal farmSize, object selectedCrop)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Please enter both first and last name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please enter an address.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(barangay))
            {
                MessageBox.Show("Please select a barangay.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(contact))
            {
                MessageBox.Show("Please enter contact information.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (farmSize <= 0)
            {
                MessageBox.Show("Farm size must be greater than 0.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (selectedCrop == null)
            {
                MessageBox.Show("Please select a crop.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void SaveNewFarmer(string firstName, string lastName, string address, 
            string barangay, string contact, decimal farmSize, DateTime registrationDate, int cropId)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();

                    // Check for existing records with the same details
                    string checkQuery = @"SELECT COUNT(*) FROM Farmers 
                WHERE FirstName = @FirstName 
                AND LastName = @LastName 
                AND Address = @Address 
                AND Barangay = @Barangay 
                AND ContactInfo = @ContactInfo 
                AND FarmSize = @FarmSize 
                AND RegistrationDate = @RegistrationDate 
                AND Crop = @Crop";

                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@FirstName", firstName);
                    checkCmd.Parameters.AddWithValue("@LastName", lastName);
                    checkCmd.Parameters.AddWithValue("@Address", address);
                    checkCmd.Parameters.AddWithValue("@Barangay", barangay);
                    checkCmd.Parameters.AddWithValue("@ContactInfo", contact);
                    checkCmd.Parameters.AddWithValue("@FarmSize", farmSize);
                    checkCmd.Parameters.AddWithValue("@RegistrationDate", registrationDate);
                    checkCmd.Parameters.AddWithValue("@Crop", cropId);

                    int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    // Check for duplicate FirstName and LastName only
                    string checkNameQuery = @"SELECT COUNT(*) FROM Farmers 
                WHERE FirstName = @FirstName 
                AND LastName = @LastName";

                    MySqlCommand checkNameCmd = new MySqlCommand(checkNameQuery, connection);
                    checkNameCmd.Parameters.AddWithValue("@FirstName", firstName);
                    checkNameCmd.Parameters.AddWithValue("@LastName", lastName);

                    int nameCount = Convert.ToInt32(checkNameCmd.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        // Case: All fields match (farmer already exists)
                        MessageBox.Show("A farmer with the same details already exists!", "Duplicate Found",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (nameCount > 0)
                    {
                        // Case: Only FirstName and LastName match (farmer with the same name exists)
                        MessageBox.Show("A farmer with the same First Name and Last Name already exists!",
                            "Duplicate Name Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // Case: No duplicates, so save the new farmer
                    string query = @"INSERT INTO Farmers 
                        (FirstName, LastName, Address, Barangay, ContactInfo, FarmSize, RegistrationDate, Crop) 
                        VALUES 
                        (@FirstName, @LastName, @Address, @Barangay, @ContactInfo, @FarmSize, @RegistrationDate, @Crop)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Barangay", barangay);
                    cmd.Parameters.AddWithValue("@ContactInfo", contact);
                    cmd.Parameters.AddWithValue("@FarmSize", farmSize);
                    cmd.Parameters.AddWithValue("@RegistrationDate", registrationDate);
                    cmd.Parameters.AddWithValue("@Crop", cropId);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Farmer added successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving farmer: " + ex.Message, "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void RemoveFarmerButton_Click(object sender, EventArgs e)
        {
            string firstName = "";
            string lastName = "";
            string farmerName = "";

            // Check for selected rows first
            if (farmersGrid.SelectedRows.Count > 0)
            {
                firstName = farmersGrid.SelectedRows[0].Cells["FirstName"].Value.ToString();
                lastName = farmersGrid.SelectedRows[0].Cells["LastName"].Value.ToString();
                farmerName = $"{firstName} {lastName}";
            }
            // If no rows selected, check for selected cells
            else if (farmersGrid.SelectedCells.Count > 0)
            {
                int rowIndex = farmersGrid.SelectedCells[0].RowIndex;
                firstName = farmersGrid.Rows[rowIndex].Cells["FirstName"].Value.ToString();
                lastName = farmersGrid.Rows[rowIndex].Cells["LastName"].Value.ToString();
                farmerName = $"{firstName} {lastName}";
            }

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Please select a farmer to remove.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to remove {farmerName}?\nThis action cannot be undone.", 
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                RemoveFarmer(firstName, lastName);
                LoadFarmersData(); // Refresh the grid
            }
        }

        private void RemoveFarmer(string firstName, string lastName)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    
                    // First get the farmer's ID
                    string getFarmerIdQuery = @"SELECT FarmerID FROM Farmers 
                                             WHERE FirstName = @FirstName 
                                             AND LastName = @LastName";
                    
                    int farmerId;
                    using (MySqlCommand cmd = new MySqlCommand(getFarmerIdQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        var result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Could not find farmer in database.", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        farmerId = Convert.ToInt32(result);
                    }
                    
                    // Remove related records in Harvest table
                    string deleteHarvestQuery = "DELETE FROM Harvest WHERE FarmerID = @FarmerID";
                    using (MySqlCommand cmd = new MySqlCommand(deleteHarvestQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@FarmerID", farmerId);
                        cmd.ExecuteNonQuery();
                    }

                    // Remove related records in FarmerNotes table
                    string deleteNotesQuery = "DELETE FROM FarmerNotes WHERE FarmerID = @FarmerID";
                    using (MySqlCommand cmd = new MySqlCommand(deleteNotesQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@FarmerID", farmerId);
                        cmd.ExecuteNonQuery();
                    }

                    // Then remove the farmer
                    string deleteFarmerQuery = "DELETE FROM Farmers WHERE FarmerID = @FarmerID";
                    using (MySqlCommand cmd = new MySqlCommand(deleteFarmerQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@FarmerID", farmerId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Farmer removed successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing farmer: " + ex.Message, "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadBarangayData(string barangay)
        {
            string query = @"
        SELECT c.CommodityName, SUM(h.Quantity) AS TotalQuantity
        FROM Harvest h
        JOIN farmers f ON h.FarmerID = f.FarmerID
        JOIN commodities c ON h.CommodityID = c.CommodityID
        WHERE f.Barangay = @Barangay
        GROUP BY c.CommodityName";

            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Barangay", barangay);
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                var data = new List<CommodityData>();
                while (reader.Read())
                {
                    data.Add(new CommodityData
                    {
                        CommodityName = reader["CommodityName"].ToString(),
                        TotalQuantity = Convert.ToDouble(reader["TotalQuantity"])
                    });
                }

            }
        }


        


        public class CommodityData
        {
            public string CommodityName { get; set; }
            public double TotalQuantity { get; set; }
        }





        private void LoadCommodities()
        {
            string query = "SELECT CommodityID, CommodityName FROM commodities";
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    comboBoxCommodities.DataSource = dt;
                    comboBoxCommodities.DisplayMember = "CommodityName";
                    comboBoxCommodities.ValueMember = "CommodityID";

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void ComboBoxCommodities_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedCommodityID = (int)comboBoxCommodities.SelectedValue;
            LoadCommodityData(selectedCommodityID);
        }
        private void LoadCommodityData(int commodityID)
        {
            string query = @"
        SELECT 
            DATE_FORMAT(HarvestDate, '%Y-%m') AS Month, 
            SUM(Quantity) AS TotalQuantity 
        FROM harvest 
        WHERE CommodityID = @CommodityID 
        GROUP BY DATE_FORMAT(HarvestDate, '%Y-%m') 
        ORDER BY Month";

            List<KeyValuePair<string, double>> dataPoints = new List<KeyValuePair<string, double>>();

            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CommodityID", commodityID);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string month = reader["Month"].ToString();
                        double totalQuantity = Convert.ToDouble(reader["TotalQuantity"]);
                        dataPoints.Add(new KeyValuePair<string, double>(month, totalQuantity));
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            UpdateChart(dataPoints);
        }
        private void UpdateChart(List<KeyValuePair<string, double>> dataPoints)
        {
            cartesianChart.Series.Clear();

            var foregroundBrush = new System.Windows.Media.SolidColorBrush(
                isDarkMode ? System.Windows.Media.Color.FromRgb(255, 255, 255) : 
                            System.Windows.Media.Color.FromRgb(0, 0, 0));

            var series = new LiveCharts.Wpf.ColumnSeries
            {
                Title = "Total Quantity",
                Values = new LiveCharts.ChartValues<double>(),
                DataLabels = true,
                FontSize = 12,
                Foreground = foregroundBrush,
                LabelPoint = point => point.Y.ToString("N0")
            };

            var labels = new List<string>();

            foreach (var point in dataPoints)
            {
                series.Values.Add(point.Value);
                labels.Add(point.Key);
            }

            cartesianChart.Series.Add(series);

            cartesianChart.AxisX.Clear();
            cartesianChart.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Month",
                Labels = labels,
                Foreground = foregroundBrush,
                Separator = new Separator { Step = 1 }
            });

            cartesianChart.AxisY.Clear();
            cartesianChart.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Total Quantity",
                Foreground = foregroundBrush,
                LabelFormatter = value => value.ToString("N0")
            });

            // Apply theme colors
            UpdateChartColors(cartesianChart, isDarkMode);
        }



        private void LoadFarmerRegistrationChart()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                SELECT COALESCE(Barangay, 'Unknown') as Barangay, COUNT(*) AS TotalFarmers
                FROM farmers
                GROUP BY Barangay
                ORDER BY TotalFarmers DESC";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        MySqlDataReader reader = command.ExecuteReader();
                        var barangayLabels = new List<string>();
                        var farmerCounts = new ChartValues<int>();

                        while (reader.Read())
                        {
                            string barangay = reader.IsDBNull(reader.GetOrdinal("Barangay")) ? "Unknown" : reader.GetString("Barangay");
                            int totalFarmers = reader.IsDBNull(reader.GetOrdinal("TotalFarmers")) ? 0 : reader.GetInt32("TotalFarmers");
                            
                            barangayLabels.Add(barangay);
                            farmerCounts.Add(totalFarmers);
                        }

                        if (cartesianChart1 != null)
                        {
                            // Clear existing series and axes
                            cartesianChart1.Series.Clear();
                            cartesianChart1.AxisX.Clear();
                            cartesianChart1.AxisY.Clear();

                            var foregroundBrush = new System.Windows.Media.SolidColorBrush(
                                isDarkMode ? System.Windows.Media.Color.FromRgb(255, 255, 255) : 
                                           System.Windows.Media.Color.FromRgb(0, 0, 0));

                            cartesianChart1.Series = new SeriesCollection
                            {
                                new ColumnSeries
                                {
                                    Title = "Farmers per Barangay",
                                    Values = farmerCounts,
                                    DataLabels = true,
                                    FontSize = 12,
                                    Foreground = foregroundBrush,
                                    LabelPoint = point => point.Y.ToString("N0")
                                }
                            };

                            cartesianChart1.AxisX.Add(new Axis
                            {
                                Title = "Barangay",
                                Labels = barangayLabels.ToArray(),
                                Foreground = foregroundBrush,
                                Separator = new LiveCharts.Wpf.Separator { Step = 1 }
                            });

                            cartesianChart1.AxisY.Add(new Axis
                            {
                                Title = "Number of Farmers",
                                MinValue = 0,
                                Foreground = foregroundBrush,
                                LabelFormatter = value => value.ToString("N0")
                            });

                            // Apply theme colors
                            UpdateChartColors(cartesianChart1, isDarkMode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading farmer registration chart: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void materialFloatingActionButton1_Click(object sender, EventArgs e)
        {
            materialTabControl1.SelectedIndex = 2;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time_txt.Text = DateTime.Now.ToLongTimeString();
            date_txt.Text = DateTime.Now.ToLongDateString();
        }


        private void materialFloatingActionButton2_Click_1(object sender, EventArgs e)
        {
            materialTabControl1.SelectedIndex = 1;
            LoadCount();
        }

        private void LoadCount()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM farmers";
                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();
                        int count = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        txt_farmerCount.Text = $"Total Count: {count}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading farmer count: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (farmersGrid.DataSource is DataView dataView)
            {
                try
                {
                    string searchText = ((TextBox)sender).Text.Trim();
                    if (string.IsNullOrEmpty(searchText))
                    {
                        dataView.RowFilter = string.Empty;
                    }
                    else
                    {
                        searchText = searchText.Replace("'", "''"); // Escape single quotes
                        string filterExpression = $"(ISNULL([First Name], '') LIKE '%{searchText}%' OR " +
                                       $"ISNULL([Last Name], '') LIKE '%{searchText}%' OR " +
                                       $"ISNULL([Middle Name], '') LIKE '%{searchText}%' OR " +
                                       $"ISNULL([Address], '') LIKE '%{searchText}%' OR " +
                                       $"ISNULL([Barangay], '') LIKE '%{searchText}%' OR " +
                                       $"ISNULL([Contact Info], '') LIKE '%{searchText}%' OR " +
                                       $"ISNULL([Commodities], '') LIKE '%{searchText}%')";
                
                        dataView.RowFilter = filterExpression;
                    }

                    // Update farmer count
                    if (farmersGrid.Parent?.Controls.Find("lblFarmerCount", true).FirstOrDefault() is Label countLabel)
                    {
                        countLabel.Text = $"Total Farmers: {dataView.Count}";
                    }

                    // Force grid to refresh
                    farmersGrid.Refresh();
                }
                catch (Exception ex)
                {
                    // If there's an error in the filter, clear it
                    dataView.RowFilter = string.Empty;
                    farmersGrid.Refresh();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            (farmersGrid.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
        }


        public class HarvestDataItem
        {
            public string CommodityName { get; set; }
            public decimal TotalQuantity { get; set; }
        }

        private void InitializePredictionTab(TabPage Prediction)
        {
            // Main container panel with scrolling
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };
            Prediction.Controls.Add(mainPanel);

            // Input Section
            GroupBox inputGroup = new GroupBox
            {
                Text = "Prediction Parameters",
                Location = new Point(20, 20),
                Size = new Size(1800, 100),
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular)
            };

            // Commodity Selection
            MaterialLabel commodityLabel = new MaterialLabel
            {
                Text = "Select Commodity:",
                Location = new Point(40, 40),
                Size = new Size(150, 30),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 50, 50),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular)
            };
            MaterialComboBox commodityComboBox = new MaterialComboBox
            {
                Location = new Point(200, 35),
                Width = 400,
                Hint = "Select Commodity",
                BackColor = Color.FromArgb(50, 50, 50),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular)
            };

            // Season Selection
            MaterialLabel seasonLabel = new MaterialLabel
            {
                Text = "Select Season:",
                Location = new Point(700, 40),
                Size = new Size(150, 30),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 50, 50),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular)
            };
            MaterialComboBox seasonComboBox = new MaterialComboBox
            {
                Location = new Point(860, 35),
                Width = 400,
                Hint = "Select Season",
                BackColor = Color.FromArgb(50, 50, 50),
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular)
            };
            seasonComboBox.Items.AddRange(new string[] { "Dry Season (December-May)", "Wet Season (June-November)" });

            inputGroup.Controls.AddRange(new Control[] { commodityLabel, commodityComboBox, seasonLabel, seasonComboBox });
            mainPanel.Controls.Add(inputGroup);

            // Create a FlowLayoutPanel for the middle section
            FlowLayoutPanel middleSection = new FlowLayoutPanel
            {
                Location = new Point(20, 140),
                Size = new Size(1800, 500),
                AutoSize = false,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.LeftToRight
            };

            // Prediction Results Chart
            GroupBox chartGroup = new GroupBox
            {
                Text = "Yield Prediction Analysis",
                Size = new Size(1100, 400),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Margin = new Padding(0, 0, 20, 0)
            };

            LiveCharts.WinForms.CartesianChart predictionChart = new LiveCharts.WinForms.CartesianChart
            {
                Location = new Point(20, 30),
                Size = new Size(1060, 350)
            };
            chartGroup.Controls.Add(predictionChart);
            middleSection.Controls.Add(chartGroup);

            // Recommendations Panel
            GroupBox recommendationsGroup = new GroupBox
            {
                Text = "Recommendations",
                Size = new Size(600, 400),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White
            };

            MaterialLabel recommendationsLabel = new MaterialLabel
            {
                Text = "Best Planting Time:\n\n" +
                      "Expected Yield Range:\n\n" +
                      "Risk Factors:\n\n" +
                      "Success Probability:",
                Location = new Point(20, 30),
                Size = new Size(560, 350),
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular)
            };
            recommendationsGroup.Controls.Add(recommendationsLabel);
            middleSection.Controls.Add(recommendationsGroup);

            mainPanel.Controls.Add(middleSection);

            // Results Grid - Update formatting
            DataGridView resultsGrid = new DataGridView
            {
                Location = new Point(20, 680),
                Size = new Size(1800, 200),
                BackgroundColor = Color.White,
                ForeColor = Color.Black,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    ForeColor = Color.Black,
                    BackColor = Color.White,
                    SelectionForeColor = Color.White,
                    SelectionBackColor = Color.Blue
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    ForeColor = Color.Black,
                    BackColor = Color.LightGray,
                    Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
                },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true
            };

            // Format the grid after data binding
            resultsGrid.DataBindingComplete += (sender, e) =>
            {
                if (resultsGrid.Columns["Month"] != null)
                {
                    resultsGrid.Columns["Month"].DefaultCellStyle.Format = "MMMM";
                    resultsGrid.Columns["Month"].DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.InvariantCulture;
                }
                if (resultsGrid.Columns["AvgYield"] != null)
                {
                    resultsGrid.Columns["AvgYield"].DefaultCellStyle.Format = "N2";
                    resultsGrid.Columns["AvgYield"].HeaderText = "Average Yield (pieces)";
                }
                if (resultsGrid.Columns["AvgRevenue"] != null)
                {
                    resultsGrid.Columns["AvgRevenue"].HeaderText = "Average Revenue (₱)";
                    resultsGrid.Columns["AvgRevenue"].DefaultCellStyle.Format = "₱#,##0.00";
                    // Remove the right alignment to match other columns
                    resultsGrid.Columns["AvgRevenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
            };

            mainPanel.Controls.Add(resultsGrid);

            // Generate Prediction Button - Moved up
            MaterialButton generateButton = new MaterialButton
            {
                Text = "GENERATE PREDICTION",
                Location = new Point(20, 900),  // Moved up
                Size = new Size(200, 40)
            };
            generateButton.Click += (sender, e) => GeneratePrediction(
                commodityComboBox.Text,
                seasonComboBox.Text,
                predictionChart,
                resultsGrid,
                recommendationsLabel
            );
            mainPanel.Controls.Add(generateButton);

            // Load commodities
            LoadCommoditiesForPrediction(commodityComboBox);

            // Add Compare button next to Generate Prediction
            MaterialButton compareButton = new MaterialButton
            {
                Text = "COMPARE COMMODITIES",
                Location = new Point(240, 900),  // Position it next to the Generate button
                Size = new Size(200, 40)
            };
            compareButton.Click += (sender, e) => OpenComparisonTab();
            mainPanel.Controls.Add(compareButton);
        }

        private void LoadCommoditiesForPrediction(MaterialComboBox comboBox)
        {
            string query = "SELECT DISTINCT CommodityName FROM Commodities ORDER BY CommodityName";
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader["CommodityName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading commodities: " + ex.Message);
                }
            }
        }

        private void GeneratePrediction(string commodity, string season, 
            LiveCharts.WinForms.CartesianChart chart, DataGridView grid, MaterialLabel recommendations)
        {
            if (string.IsNullOrEmpty(commodity))
            {
                MessageBox.Show("Please select a commodity first.", "Missing Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(season))
            {
                MessageBox.Show("Please select a season first.", "Missing Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"
                WITH MonthData AS (
                    SELECT 
                        YEAR(h.HarvestDate) as Year,
                        MONTH(h.HarvestDate) as MonthNum,
                        CASE MONTH(h.HarvestDate)
                            WHEN 1 THEN 'January'
                            WHEN 2 THEN 'February'
                            WHEN 3 THEN 'March'
                            WHEN 4 THEN 'April'
                            WHEN 5 THEN 'May'
                            WHEN 6 THEN 'June'
                            WHEN 7 THEN 'July'
                            WHEN 8 THEN 'August'
                            WHEN 9 THEN 'September'
                            WHEN 10 THEN 'October'
                            WHEN 11 THEN 'November'
                            WHEN 12 THEN 'December'
                        END as Month,
                        h.Quantity
                    FROM Harvest h
                    JOIN Commodities c ON h.CommodityID = c.CommodityID
                    WHERE c.CommodityName = @Commodity
                    AND (
                        -- For Dry Season (Dec-May)
                        (@Season = 'Dry Season (December-May)' AND (MONTH(h.HarvestDate) = 12 OR MONTH(h.HarvestDate) <= 5))
                        OR
                        -- For Wet Season (June-November)
                        (@Season = 'Wet Season (June-November)' AND (MONTH(h.HarvestDate) >= 6 AND MONTH(h.HarvestDate) <= 11))
                    )
                )
                SELECT 
                    Year,
                    MonthNum,
                    Month,
                    COALESCE(AVG(Quantity), 0) as AvgYield
                FROM MonthData
                GROUP BY Year, MonthNum, Month
                ORDER BY Year, MonthNum";

            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Commodity", commodity);
                    cmd.Parameters.AddWithValue("@Season", season);

                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show($"No data available for {commodity} during {season}.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Remove the MonthNum column before binding to grid
                    if (dt.Columns.Contains("MonthNum"))
                    {
                        dt.Columns.Remove("MonthNum");
                    }

                    UpdatePredictionChart(chart, dt);
                    grid.DataSource = dt;
                    GenerateRecommendations(dt, recommendations, season);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating prediction: " + ex.Message);
                }
            }
        }

        private void UpdatePredictionChart(LiveCharts.WinForms.CartesianChart chart, DataTable data)
        {
            chart.Series.Clear();
            chart.AxisX.Clear();
            chart.AxisY.Clear();

            var yields = new ChartValues<double>();
            var labels = new List<string>();

            foreach (DataRow row in data.Rows)
            {
                yields.Add(Convert.ToDouble(row["AvgYield"]));
                labels.Add($"{row["Year"]}-{row["Month"]}");
            }

            // Create series with proper styling
            var columnSeries = new ColumnSeries
                {
                    Title = "Historical Yield",
                    Values = yields,
                    DataLabels = true,
                FontSize = 12,
                LabelPoint = point => point.Y.ToString("N0"),
                Fill = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromArgb(230, 33, 150, 243)), // Material Blue with opacity
                Foreground = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(0, 0, 0)) // Black text for data labels
            };

            chart.Series = new SeriesCollection { columnSeries };

            // Configure X-axis with proper text styling
            chart.AxisX.Add(new Axis
            {
                Title = "Time Period",
                Labels = labels,
                Foreground = new System.Windows.Media.SolidColorBrush(isDarkMode ? 
                    System.Windows.Media.Color.FromRgb(255, 255, 255) : 
                    System.Windows.Media.Color.FromRgb(0, 0, 0)),
                Separator = new Separator
                {
                    Step = 1,
                    IsEnabled = false
                }
            });

            // Configure Y-axis with proper text styling
            chart.AxisY.Add(new Axis
            {
                Title = "Average Yield",
                LabelFormatter = value => value.ToString("N0"),
                Foreground = new System.Windows.Media.SolidColorBrush(isDarkMode ? 
                    System.Windows.Media.Color.FromRgb(255, 255, 255) : 
                    System.Windows.Media.Color.FromRgb(0, 0, 0))
            });

            // Apply current theme colors
            UpdateChartColors(chart, isDarkMode);
        }





        private void InitializeSettingsTab()
        {
            // Settings tab layout
            Panel settingsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(50, 50, 50)
            };
            Settings.Controls.Add(settingsPanel);

            // Theme Settings Group
            GroupBox themeSettingsGroup = new GroupBox
            {
                Text = "Theme Settings",
                Location = new Point(20, 20),
                Size = new Size(600, 150),  // Increased height to fit both controls
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular),
                BackColor = Color.FromArgb(50, 50, 50)
            };

            // Theme toggle switch
            MaterialSwitch themeToggle = new MaterialSwitch
            {
                Text = isDarkMode ? "Dark Mode" : "Light Mode",
                Location = new Point(20, 40),
                Checked = isDarkMode,
                Size = new Size(150, 36)
            };

            themeToggle.CheckedChanged += (sender, e) =>
            {
                isDarkMode = themeToggle.Checked;
                var materialSkinManager = MaterialSkinManager.Instance;
                
                if (isDarkMode)
                {
                    materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Green200, TextShade.WHITE);
                    themeToggle.Text = "Dark Mode";
                }
                else
                {
                    materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey200, Primary.BlueGrey300, Primary.BlueGrey100, Accent.Green200, TextShade.BLACK);
                    themeToggle.Text = "Light Mode";
                }

                // Update controls colors
                UpdateControlColors(isDarkMode);
                
                // Update all open detail forms
                UpdateThemeForAllForms();
            };

            // Add Logout Button
            MaterialButton logoutButton = new MaterialButton
            {
                Text = "LOGOUT",
                Location = new Point(20, 90),
                Size = new Size(150, 36),
                Type = MaterialButton.MaterialButtonType.Contained
            };

            logoutButton.Click += (sender, e) =>
            {
                DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    this.Hide();
                    Login loginForm = new Login();
                    loginForm.Show();
                    this.Close();
                }
            };

            themeSettingsGroup.Controls.Add(themeToggle);
            themeSettingsGroup.Controls.Add(logoutButton);
            settingsPanel.Controls.Add(themeSettingsGroup);

            // Admin Controls Group - Only visible for admin users
            if (userRole.ToLower() == "admin")
            {
                GroupBox adminControlsGroup = new GroupBox
                {
                    Text = "Admin Controls",
                    Location = new Point(20, 140),
                    Size = new Size(600, 600),
                    ForeColor = Color.White,
                    Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular),
                    BackColor = Color.FromArgb(50, 50, 50)
                };

                MaterialButton manageUsersButton = new MaterialButton
                {
                    Text = "MANAGE USERS",
                    Location = new Point(20, 40),
                    Size = new Size(200, 36)
                };
                manageUsersButton.Click += (sender, e) =>
                {
                    ManageUsers userForm = new ManageUsers();
                    userForm.Show();
                };

                MaterialButton backupButton = new MaterialButton
                {
                    Text = "BACKUP DATABASE",
                    Location = new Point(20, 100),
                    Size = new Size(200, 36)
                };

                MaterialButton restoreButton = new MaterialButton
                {
                    Text = "RESTORE DATABASE",
                    Location = new Point(20, 160),
                    Size = new Size(200, 36)
                };

                backupButton.Click += (sender, e) =>
                {
                    BackupDatabase();
                };

                restoreButton.Click += (sender, e) =>
                {
                    RestoreDatabase();
                };

                adminControlsGroup.Controls.AddRange(new Control[] {
                    manageUsersButton,
                    backupButton,
                    restoreButton
                });

                settingsPanel.Controls.Add(adminControlsGroup);
            }

            // Batch Import Button
            MaterialButton btn_batch_import = new MaterialButton
            {
                Text = "BATCH IMPORT",
                Location = new Point(20, 20),
                Size = new Size(150, 40)
            };
            btn_batch_import.Click += btn_batch_import_Click;

            // Export Button
            MaterialButton btn_export = new MaterialButton
            {
                Text = "EXPORT DATA",
                Location = new Point(190, 20), // Position it right after the import button
                Size = new Size(150, 40)
            };
            btn_export.Click += (sender, e) => {
                // Find or create export tab
                TabPage exportTab = null;
                foreach (TabPage tab in materialTabControl1.TabPages)
                {
                    if (tab.Text == "Export Data")
                    {
                        exportTab = tab;
                        break;
                    }
                }

                if (exportTab == null)
                {
                    exportTab = new TabPage
                    {
                        Text = "Export Data",
                        BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White
                    };
                    materialTabControl1.TabPages.Add(exportTab);
                    InitializeExportTab(exportTab);
                }

                materialTabControl1.SelectedTab = exportTab;
            };

            // Add buttons to settings panel
            Settings.Controls.Add(btn_batch_import);
            Settings.Controls.Add(btn_export);
        }

        private void BackupDatabase()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*",
                FilterIndex = 1,
                Title = "Select Backup Location",
                FileName = $"OMA_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string backupPath = saveFileDialog.FileName;
                    using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                using (StreamWriter writer = new StreamWriter(backupPath, false, Encoding.UTF8))
                                {
                                writer.WriteLine("-- Database Backup for OMA");
                                writer.WriteLine($"-- Date: {DateTime.Now}");
                                writer.WriteLine("SET FOREIGN_KEY_CHECKS=0;");
                                    writer.WriteLine("SET SQL_MODE = 'NO_AUTO_VALUE_ON_ZERO';");
                                    writer.WriteLine("SET NAMES utf8mb4;");

                                    string dbName = connection.Database;
                                    writer.WriteLine($"USE `{dbName}`;");

                                    // Get all tables
                                    var tables = new List<string>();
                                    using (var cmd = new MySqlCommand("SHOW FULL TABLES WHERE Table_Type = 'BASE TABLE'", connection))
                                    using (var reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            tables.Add(reader.GetString(0));
                                        }
                                    }

                                    foreach (string tableName in tables)
                                    {
                                    // Get create table SQL
                                        using (var cmd = new MySqlCommand($"SHOW CREATE TABLE `{tableName}`", connection))
                                        using (var reader = cmd.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                                writer.WriteLine($"\n-- Table structure for `{tableName}`");
                                            writer.WriteLine($"DROP TABLE IF EXISTS `{tableName}`;");
                                                writer.WriteLine($"{reader.GetString(1)};");
                                            }
                                        }

                                        // Get column information including generated columns
                                        var columns = new List<(string Name, string Type, bool IsGenerated)>();
                                        using (var cmd = new MySqlCommand(
                                            $@"SELECT COLUMN_NAME, COLUMN_TYPE, EXTRA 
                                       FROM INFORMATION_SCHEMA.COLUMNS 
                                       WHERE TABLE_SCHEMA = '{dbName}' 
                                       AND TABLE_NAME = '{tableName}'", connection))
                                        using (var reader = cmd.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                columns.Add((
                                                    reader.GetString(0),
                                                    reader.GetString(1),
                                                    reader.GetString(2).Contains("GENERATED")
                                                ));
                                            }
                                        }

                                        // Create INSERT statement with only non-generated columns
                                        var nonGeneratedColumns = columns
                                            .Where(c => !c.IsGenerated)
                                            .Select(c => $"`{c.Name}`")
                                            .ToList();

                                        if (nonGeneratedColumns.Any())
                                        {
                                            writer.WriteLine($"\n-- Dumping data for table `{tableName}`");

                                            // Get data excluding generated columns
                                            string columnList = string.Join(", ", nonGeneratedColumns);
                                            string selectQuery = $"SELECT {columnList} FROM `{tableName}`";

                                            using (var cmd = new MySqlCommand(selectQuery, connection))
                                            using (var reader = cmd.ExecuteReader())
                                            {
                                        while (reader.Read())
                                        {
                                                    var values = new List<string>();
                                            for (int i = 0; i < reader.FieldCount; i++)
                                            {
                                                        if (reader.IsDBNull(i))
                                                        {
                                                            values.Add("NULL");
                                                        }
                                                else
                                                {
                                                            var value = reader.GetValue(i);
                                                            var columnType = columns[i].Type.ToLower();

                                                            if (value is byte[] bytes)
                                                            {
                                                                values.Add($"0x{BitConverter.ToString(bytes).Replace("-", "")}");
                                                            }
                                                            else if (columnType.Contains("blob") || columnType.Contains("binary"))
                                                            {
                                                                var blobValue = (byte[])reader.GetValue(i);
                                                                values.Add($"0x{BitConverter.ToString(blobValue).Replace("-", "")}");
                                                            }
                                                            else if (value is string)
                                                            {
                                                                values.Add($"'{MySqlHelper.EscapeString(value.ToString())}'");
                                                            }
                                                            else if (value is DateTime dt)
                                                            {
                                                                values.Add($"'{dt:yyyy-MM-dd HH:mm:ss}'");
                                                            }
                                                            else if (value is bool b)
                                                            {
                                                                values.Add(b ? "1" : "0");
                                                            }
                                                            else
                                                            {
                                                                values.Add(value.ToString());
                                                            }
                                                        }
                                                    }

                                                    writer.WriteLine($"INSERT INTO `{tableName}` ({columnList}) VALUES ({string.Join(", ", values)});");
                                                }
                                            }
                                        }
                                    }

                                    writer.WriteLine("\nSET FOREIGN_KEY_CHECKS=1;");
                                }

                                transaction.Commit();
                                MessageBox.Show("Database backup completed successfully!", "Backup Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during backup: {ex.Message}", "Backup Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RestoreDatabase()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*",
                FilterIndex = 1,
                Title = "Select Backup File to Restore"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string restorePath = openFileDialog.FileName;

                    // Validate backup file
                    if (!ValidateBackupFile(restorePath))
                    {
                        MessageBox.Show("Invalid backup file format.", "Restore Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                    {
                            connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                // Read and execute script in chunks
                                var script = File.ReadAllText(restorePath);
                                var statements = SplitSqlStatements(script);

                                foreach (var statement in statements)
                                {
                                    if (!string.IsNullOrWhiteSpace(statement))
                                    {
                                        using (var cmd = new MySqlCommand(statement, connection, transaction))
                                        {
                                            cmd.CommandTimeout = 300; // 5 minutes timeout per statement
                                        cmd.ExecuteNonQuery();
                                    }
                                    }
                                }
                                RefreshAllData();

                                transaction.Commit();
                                MessageBox.Show("Database restored successfully!", "Restore Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during restore: {ex.Message}", "Restore Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateBackupFile(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    string firstLine = reader.ReadLine();
                    // Verify it's our backup format
                    return firstLine?.StartsWith("-- Database Backup for OMA") ?? false;
                }
            }
            catch
            {
                return false;
            }
        }

        private IEnumerable<string> SplitSqlStatements(string script)
        {
            var statements = new List<string>();
            var statement = new StringBuilder();
            bool inString = false;
            char stringChar = '"';

            for (int i = 0; i < script.Length; i++)
            {
                char c = script[i];

                if (c == '\'' || c == '"')
                {
                    if (!inString)
                    {
                        inString = true;
                        stringChar = c;
                    }
                    else if (c == stringChar)
                    {
                        inString = false;
                    }
                }

                statement.Append(c);

                if (c == ';' && !inString)
                {
                    statements.Add(statement.ToString().Trim());
                    statement.Clear();
                }
            }

            if (statement.Length > 0)
            {
                statements.Add(statement.ToString().Trim());
            }

            return statements;
        }

        private void farmersGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // Add new methods for printing farmer details
        private void btnPrintSelected_Click(object sender, EventArgs e)
        {
            PrintSelectedFarmers();
        }

        private void PrintSelectedFarmers()
        {
            if (farmersGrid.SelectedCells.Count == 0 && farmersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one farmer to print.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<(string FirstName, string LastName)> farmersToPrint = new List<(string FirstName, string LastName)>();

            // Collect selected rows
            foreach (DataGridViewRow row in farmersGrid.SelectedRows)
            {
                farmersToPrint.Add((
                    row.Cells["First Name"].Value.ToString(),
                    row.Cells["Last Name"].Value.ToString()
                ));
            }

            // Generate and preview the PDF
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SelectedFarmersReport.pdf");
            GeneratePdf(farmersToPrint, fileName);
        }



        private void PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Set up fonts and margins
            System.Drawing.Font headerFont = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            System.Drawing.Font titleFont = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            System.Drawing.Font normalFont = new System.Drawing.Font("Arial", 10);
            System.Drawing.Font boldFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            float pageWidth = e.MarginBounds.Width;
            float pageHeight = e.MarginBounds.Height;

            Pen linePen = new Pen(Color.Black, 1);
            int lineSpacing = 10; // Spacing between lines

            // Adjust currentY for header
            float currentY = topMargin;

            // Print header only on the first page
            if (currentFarmerIndex == 0)
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string logoPath = Path.Combine(appDirectory, "Assets", "logo.png");
                System.Drawing.Image logo = null;

                try
                {
                    logo = System.Drawing.Image.FromFile(logoPath);
                }
                catch (Exception ex)
                {
                    e.Graphics.DrawString("Logo not found.", normalFont, Brushes.Red, leftMargin, topMargin);
                    Console.WriteLine($"Error loading logo: {ex.Message}");
                }

                if (logo != null)
                {
                    float logoSize = 100;
                    e.Graphics.DrawImage(logo, leftMargin + 20, topMargin + 10, logoSize, logoSize);
                }

                float textX = leftMargin + 140;
                e.Graphics.DrawString("Republic of the Philippines", headerFont, Brushes.Black, textX, topMargin + 20);
                e.Graphics.DrawString("Province of Palawan", titleFont, Brushes.Black, textX, topMargin + 50);
                e.Graphics.DrawString("Municipality of Brooke's Point", titleFont, Brushes.Black, textX, topMargin + 80);
                e.Graphics.DrawString("Office of the Municipal Agriculturist", titleFont, Brushes.Black, textX, topMargin + 110);

                currentY += 160; // Add space for the header

                // Separator line
                e.Graphics.DrawLine(linePen, leftMargin, currentY, leftMargin + pageWidth, currentY);
                currentY += 10;

                // Report title
                e.Graphics.DrawString("Farmer Details Report", titleFont, Brushes.Black, leftMargin + (pageWidth / 2) - 90, currentY);
                currentY += titleFont.Height + 10;
                e.Graphics.DrawString($"Generated on: {DateTime.Now:MMMM dd, yyyy}", normalFont, Brushes.Black, leftMargin, currentY);
                currentY += normalFont.Height + 20;
            }

            // Print farmer details
            while (currentFarmerIndex < farmersToPrintNames.Count)
            {
                var (firstName, lastName) = farmersToPrintNames[currentFarmerIndex];

                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
    SELECT f.*, 
           GROUP_CONCAT(DISTINCT CONCAT(DATE_FORMAT(h.HarvestDate, '%Y-%m-%d'), ': ', CAST(h.Quantity AS CHAR), ' ', c.CommodityName) 
           ORDER BY h.HarvestDate DESC SEPARATOR '\n') as HarvestDetails
    FROM Farmers f
    LEFT JOIN Harvest h ON f.FarmerID = h.FarmerID
    LEFT JOIN Commodities c ON h.CommodityID = c.CommodityID
    WHERE f.FirstName = @FirstName AND f.LastName = @LastName
    GROUP BY f.FarmerID, f.FirstName, f.LastName, f.Address, f.Barangay, f.ContactInfo, f.FarmSize, f.RegistrationDate, f.Crop";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Calculate the required height for the farmer's details
                                float requiredHeight = CalculateRequiredHeight(reader, titleFont, normalFont);

                                // Check if the current farmer fits in the remaining space
                                if (currentY + requiredHeight > e.MarginBounds.Bottom)
                                {
                                    // Move to the next page
                                    e.HasMorePages = true;
                                    return;
                                }

                                // Draw farmer header
                                e.Graphics.FillRectangle(Brushes.LightGray, leftMargin, currentY, pageWidth, titleFont.Height + 5);
                                e.Graphics.DrawString($"Farmer: {firstName} {lastName}", titleFont, Brushes.Black, leftMargin + 5, currentY + 2);
                                currentY += titleFont.Height + lineSpacing;

                                // Print farmer details
                                string[] details = new string[]
                                {
                            $"Address: {reader["Address"]}",
                            $"Barangay: {reader["Barangay"]}",
                            $"Contact Info: {reader["ContactInfo"]}",
                            $"Farm Size: {reader["FarmSize"]} hectares",
                            $"Registration Date: {Convert.ToDateTime(reader["RegistrationDate"]).ToString("MMMM dd, yyyy")}"
                                };

                                foreach (string detail in details)
                                {
                                    e.Graphics.DrawString(detail, normalFont, Brushes.Black, leftMargin + 10, currentY);
                                    currentY += normalFont.Height + lineSpacing / 2;
                                }

                                // Print harvest history
                                if (!reader.IsDBNull(reader.GetOrdinal("HarvestDetails")))
                                {
                                    e.Graphics.DrawString("Harvest History:", boldFont, Brushes.Black, leftMargin + 10, currentY);
                                    currentY += boldFont.Height + 5;

                                    string[] harvests = reader["HarvestDetails"].ToString().Split('\n');
                                    foreach (string harvest in harvests)
                                    {
                                        e.Graphics.DrawString($"• {harvest}", normalFont, Brushes.Black, leftMargin + 30, currentY);
                                        currentY += normalFont.Height + lineSpacing / 2;
                                    }
                                }

                                currentY += 30; // Add space before the next farmer
                                currentFarmerIndex++;
                            }
                        }
                    }
                }
            }

            e.HasMorePages = false;
        }

        private float CalculateRequiredHeight(MySqlDataReader reader, Font titleFont, Font normalFont)
        {
            float height = 0;

            // Basic info height
            height += titleFont.Height + 10; // Farmer header
            height += (normalFont.Height + 5) * 5; // Details: Address, Barangay, etc.

            // Harvest history height if available
            if (!reader.IsDBNull(reader.GetOrdinal("HarvestDetails")))
            {
                height += 10; // Extra spacing
                height += titleFont.Height + 5; // "Harvest History:" header
                string[] harvests = reader["HarvestDetails"].ToString().Split('\n');
                height += (normalFont.Height + 5) * harvests.Length; // Each harvest line
            }

            height += 30; // Space before the next farmer
            return height;
        }


        private void GeneratePdf(List<(string FirstName, string LastName)> farmersToPrintNames, string fileName)
        {
            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 40, 40, 50, 50); // Margins
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    // Fonts
                    var headerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD);
                    var titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD);
                    var normalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);
                    var boldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD);

                    // Add Header (Logo + Text)
                    PdfPTable combinedHeaderTable = new PdfPTable(2) { WidthPercentage = 100 };
                    combinedHeaderTable.SetWidths(new float[] { 1f, 4f }); // Adjust logo and text widths

                    // Define Base Directory and Logo Path
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string logoPath = Path.Combine(appDirectory, "Assets", "logo.png");

                    // Create Final Header Table
                    // Create Final Header Table
                    PdfPTable finalHeaderTable = new PdfPTable(2) { WidthPercentage = 100 };
                    finalHeaderTable.SetWidths(new float[] { 1, 70f });
 // Adjust column widths

                    // Add Logo Cell
                    if (File.Exists(logoPath))
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                        logo.ScaleAbsolute(70, 70);
                        PdfPCell logoCell = new PdfPCell(logo)
                        {
                            Border = PdfPCell.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE // Center vertically
                        };
                        finalHeaderTable.AddCell(logoCell);
                    }
                    else
                    {
                        finalHeaderTable.AddCell(new PdfPCell(new Phrase("")) { Border = PdfPCell.NO_BORDER });
                    }

                    // Add Text Content Cell
                    PdfPCell headerTextCell = new PdfPCell
                    {
                        Border = PdfPCell.NO_BORDER,
                        PaddingLeft = 10, // Add some padding for spacing between the logo and text
                        HorizontalAlignment = Element.ALIGN_LEFT, // Align text to the left
                        VerticalAlignment = Element.ALIGN_MIDDLE // Center vertically relative to the logo
                    };

                    // Add Header Text with Paragraph Alignment
                    headerTextCell.AddElement(new Paragraph("Republic of the Philippines",
                        new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD))
                    {
                        Alignment = Element.ALIGN_CENTER
                    });
                    headerTextCell.AddElement(new Paragraph("Province of Palawan",
                        new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14))
                    {
                        Alignment = Element.ALIGN_CENTER
                    });
                    headerTextCell.AddElement(new Paragraph("Municipality of Brooke's Point",
                        new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD))
                    {
                        Alignment = Element.ALIGN_CENTER
                    });
                    headerTextCell.AddElement(new Paragraph("Office of the Municipal Agriculturist",
                        new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12))
                    {
                        Alignment = Element.ALIGN_CENTER
                    });

                    // Add the Text Cell to the Table
                    finalHeaderTable.AddCell(headerTextCell);

                    // Add the Header Table to the Document
                    pdfDoc.Add(finalHeaderTable);
                    pdfDoc.Add(new Paragraph("\n")); // Add space after the header



                    // Add Report Title
                    var titleParagraph = new Paragraph("Farmer Details Report", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 10
                    };
                    pdfDoc.Add(titleParagraph);
                    pdfDoc.Add(new Paragraph($"Generated on: {DateTime.Now:MMMM dd, yyyy}", normalFont) { SpacingAfter = 20 });

                    // Iterate over farmers
                    foreach (var (firstName, lastName) in farmersToPrintNames)
                    {
                        using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                        {
                            connection.Open();
                            string query = @"
SELECT f.*, 
       GROUP_CONCAT(DISTINCT CONCAT(DATE_FORMAT(h.HarvestDate, '%Y-%m-%d'), ': ', CAST(h.Quantity AS CHAR), ' ', c.CommodityName) 
       ORDER BY h.HarvestDate DESC SEPARATOR '\n') as HarvestDetails
FROM Farmers f
LEFT JOIN Harvest h ON f.FarmerID = h.FarmerID
LEFT JOIN Commodities c ON h.CommodityID = c.CommodityID
WHERE f.FirstName = @FirstName AND f.LastName = @LastName
GROUP BY f.FarmerID";

                            using (MySqlCommand cmd = new MySqlCommand(query, connection))
                            {
                                cmd.Parameters.AddWithValue("@FirstName", firstName);
                                cmd.Parameters.AddWithValue("@LastName", lastName);

                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        // Farmer Name with Background
                                        PdfPCell farmerNameCell = new PdfPCell(new Phrase($"Farmer Name: {firstName} {lastName}", boldFont))
                                        {
                                            BackgroundColor = BaseColor.LIGHT_GRAY,
                                            Padding = 5,
                                            Colspan = 2,
                                            HorizontalAlignment = Element.ALIGN_LEFT
                                        };

                                        // Farmer Details Table
                                        PdfPTable farmerTable = new PdfPTable(2)
                                        {
                                            WidthPercentage = 100,
                                            KeepTogether = true // Prevents table from splitting across pages
                                        };
                                        farmerTable.AddCell(farmerNameCell);

                                        farmerTable.AddCell(new PdfPCell(new Phrase("Address:", boldFont)) { Padding = 5 });
                                        farmerTable.AddCell(new PdfPCell(new Phrase($"{reader["Address"]}", normalFont)) { Padding = 5 });

                                        farmerTable.AddCell(new PdfPCell(new Phrase("Barangay:", boldFont)) { Padding = 5 });
                                        farmerTable.AddCell(new PdfPCell(new Phrase($"{reader["Barangay"]}", normalFont)) { Padding = 5 });

                                        farmerTable.AddCell(new PdfPCell(new Phrase("Contact Info:", boldFont)) { Padding = 5 });
                                        farmerTable.AddCell(new PdfPCell(new Phrase($"{reader["ContactInfo"]}", normalFont)) { Padding = 5 });

                                        farmerTable.AddCell(new PdfPCell(new Phrase("Farm Size:", boldFont)) { Padding = 5 });
                                        farmerTable.AddCell(new PdfPCell(new Phrase($"{reader["FarmSize"]} hectares", normalFont)) { Padding = 5 });

                                        farmerTable.AddCell(new PdfPCell(new Phrase("Registration Date:", boldFont)) { Padding = 5 });
                                        farmerTable.AddCell(new PdfPCell(new Phrase($"{Convert.ToDateTime(reader["RegistrationDate"]).ToString("MMMM dd, yyyy")}", normalFont)) { Padding = 5 });

                                        // Harvest History
                                        if (!reader.IsDBNull(reader.GetOrdinal("HarvestDetails")))
                                        {
                                            farmerTable.AddCell(new PdfPCell(new Phrase("Harvest History:", boldFont)) { Padding = 5, Colspan = 2 });
                                            string[] harvests = reader["HarvestDetails"].ToString().Split('\n');
                                            foreach (var harvest in harvests)
                                            {
                                                farmerTable.AddCell(new PdfPCell(new Phrase($"• {harvest}", normalFont)) { Padding = 5, Colspan = 2 });
                                            }
                                        }

                                        // Add farmer table to the document
                                        pdfDoc.Add(farmerTable);
                                        pdfDoc.Add(new Paragraph("\n")); // Space between farmers
                                    }
                                }
                            }
                        }
                    }

                    pdfDoc.Close();
                }

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(fileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadYearComboBoxData()
        {
            string query = "SELECT DISTINCT YEAR(HarvestDate) AS Year FROM Harvest ORDER BY Year"; // Adjust the query as needed

            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                var years = new List<int>();
                while (reader.Read())
                {
                    years.Add(reader.GetInt32("Year"));
                }

                
            }
        }

        private void LoadMonthComboBoxData()
        {
            string query = "SELECT DISTINCT MONTH(HarvestDate) AS Month FROM Harvest ORDER BY Month"; // Adjust the query as needed

            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                var months = new List<string>();
                while (reader.Read())
                {
                    int monthNumber = reader.GetInt32("Month");
                    months.Add(new DateTime(1, monthNumber, 1).ToString("MMMM")); // Convert month number to month name
                }

                
            }
        }

        private void UpdateThemeForAllForms()
        {
            foreach (Details detailsForm in openDetailsForms.ToList())
            {
                detailsForm.UpdateTheme();
            }
        }

        private void UpdateControlColors(bool isDark)
        {
            Color backColor = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(240, 240, 240);
            Color foreColor = isDark ? Color.White : Color.Black;
            Color chartBackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            Color chartForeColor = isDark ? Color.White : Color.Black;

            // Update Settings tab controls
            foreach (Control control in Settings.Controls)
            {
                UpdateControlAndChildren(control, backColor, foreColor);
            }

            // Update Prediction tab controls
            foreach (Control control in Prediction.Controls)
            {
                UpdateControlAndChildren(control, backColor, foreColor);
                
                // Handle GroupBox controls specifically in Prediction tab
                if (control is GroupBox groupBox)
                {
                    groupBox.BackColor = backColor;
                    groupBox.ForeColor = foreColor;
                    
                    foreach (Control innerControl in groupBox.Controls)
                    {
                        if (innerControl is MaterialLabel || innerControl is Label)
                        {
                            innerControl.ForeColor = foreColor;
                            innerControl.BackColor = backColor;
                        }
                        else if (innerControl is LiveCharts.WinForms.CartesianChart chart)
                        {
                            UpdateChartColors(chart, isDark);
                        }
                    }
                }
            }

            // Update Comparison tab if it exists
            if (comparisonTab != null)
            {
                foreach (Control control in comparisonTab.Controls)
                {
                    UpdateControlAndChildren(control, backColor, foreColor);
                    
                    // Handle GroupBox controls specifically in Comparison tab
                    if (control is GroupBox groupBox)
                    {
                        groupBox.BackColor = backColor;
                        groupBox.ForeColor = foreColor;
                        
                        foreach (Control innerControl in groupBox.Controls)
                        {
                            if (innerControl is MaterialLabel || innerControl is Label)
                            {
                                innerControl.ForeColor = foreColor;
                                innerControl.BackColor = backColor;
                            }
                            else if (innerControl is LiveCharts.WinForms.CartesianChart chart)
                            {
                                UpdateChartColors(chart, isDark);
                            }
                        }
                    }
                    else if (control is Panel panel)
                    {
                        panel.BackColor = backColor;
                        foreach (Control panelControl in panel.Controls)
                        {
                            if (panelControl is GroupBox panelGroupBox)
                            {
                                panelGroupBox.BackColor = backColor;
                                panelGroupBox.ForeColor = foreColor;
                                foreach (Control groupBoxControl in panelGroupBox.Controls)
                                {
                                    if (groupBoxControl is LiveCharts.WinForms.CartesianChart chart)
                                    {
                                        UpdateChartColors(chart, isDark);
                                    }
                                    else if (groupBoxControl is MaterialLabel || groupBoxControl is Label)
                                    {
                                        groupBoxControl.ForeColor = foreColor;
                                        groupBoxControl.BackColor = backColor;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Update DataGridView colors
            if (farmersGrid != null)
            {
                farmersGrid.DefaultCellStyle.BackColor = isDark ? Color.FromArgb(60, 60, 60) : Color.White;
                farmersGrid.DefaultCellStyle.ForeColor = isDark ? Color.White : Color.Black;
                farmersGrid.DefaultCellStyle.SelectionBackColor = isDark ? Color.FromArgb(100, 100, 100) : Color.Blue;
                farmersGrid.DefaultCellStyle.SelectionForeColor = Color.White;
                farmersGrid.ColumnHeadersDefaultCellStyle.BackColor = isDark ? Color.FromArgb(45, 45, 45) : Color.Navy;
                farmersGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                farmersGrid.EnableHeadersVisualStyles = false;
                farmersGrid.BackgroundColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            }

            // Update all charts
            UpdateChartColors(cartesianChart, isDark);
            UpdateChartColors(cartesianChart1, isDark);
            
            // Force refresh of all charts
            cartesianChart?.Update(true, true);
            cartesianChart1?.Update(true, true);

            // Find and update all charts in the application
            foreach (TabPage tab in materialTabControl1.TabPages)
            {
                var charts = FindAllCharts(tab);
                foreach (var chart in charts)
                {
                    UpdateChartColors(chart, isDark);
                    chart.Update(true, true);
                }
            }
        }

        private List<LiveCharts.WinForms.CartesianChart> FindAllCharts(Control control)
        {
            var charts = new List<LiveCharts.WinForms.CartesianChart>();
            
            if (control is LiveCharts.WinForms.CartesianChart chart)
            {
                charts.Add(chart);
            }
            
            foreach (Control child in control.Controls)
            {
                charts.AddRange(FindAllCharts(child));
            }
            
            return charts;
        }

        private void UpdateChartColors(LiveCharts.WinForms.CartesianChart chart, bool isDark)
        {
            if (chart == null) return;

            Color foreColor = isDark ? Color.White : Color.Black;
            Color backColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            Color gridLinesColor = isDark ? Color.FromArgb(100, 100, 100) : Color.FromArgb(200, 200, 200);
            Color seriesColor = Color.FromArgb(33, 150, 243); // Material Blue color

            chart.BackColor = backColor;

            // Update axis colors and styling
            foreach (var axis in chart.AxisX)
            {
                var foregroundBrush = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(foreColor.R, foreColor.G, foreColor.B));
                var gridLinesBrush = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(gridLinesColor.R, gridLinesColor.G, gridLinesColor.B));

                axis.Foreground = foregroundBrush;
                axis.Separator.Stroke = gridLinesBrush;
                axis.Separator.StrokeThickness = 0.5;

                if (axis.Title != null)
                {
                    var titleContent = axis.Title.ToString().Trim();
                    if (!string.IsNullOrEmpty(titleContent))
                    {
                        axis.ShowLabels = true;
                        axis.Title = titleContent;
                        axis.Foreground = foregroundBrush;
                    }
                }
            }

            foreach (var axis in chart.AxisY)
            {
                var foregroundBrush = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(foreColor.R, foreColor.G, foreColor.B));
                var gridLinesBrush = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(gridLinesColor.R, gridLinesColor.G, gridLinesColor.B));

                axis.Foreground = foregroundBrush;
                axis.Separator.Stroke = gridLinesBrush;
                axis.Separator.StrokeThickness = 0.5;

                if (axis.Title != null)
                {
                    var titleContent = axis.Title.ToString().Trim();
                    if (!string.IsNullOrEmpty(titleContent))
                    {
                        axis.ShowLabels = true;
                        axis.Title = titleContent;
                        axis.Foreground = foregroundBrush;
                    }
                }
            }

            // Update series colors and styling
            if (chart.Series != null)
            {
                foreach (var series in chart.Series)
                {
                    if (series is LiveCharts.Wpf.ColumnSeries columnSeries)
                    {
                        // Set data labels with contrasting color
                        columnSeries.DataLabels = true;
                        columnSeries.FontSize = 12;
                        columnSeries.LabelPoint = point => point.Y.ToString("N0");
                        
                        // Set foreground color for labels based on theme
                        columnSeries.Foreground = new System.Windows.Media.SolidColorBrush(
                            System.Windows.Media.Color.FromRgb(foreColor.R, foreColor.G, foreColor.B));

                        // Set column fill color with opacity
                        columnSeries.Fill = new System.Windows.Media.SolidColorBrush(
                            System.Windows.Media.Color.FromArgb(230, seriesColor.R, seriesColor.G, seriesColor.B));
                    }
                }
            }

            // Force chart refresh
            chart.Update(true, true);
        }

        private void UpdateControlAndChildren(Control control, Color backColor, Color foreColor)
        {
            if (control is GroupBox || control is Panel)
            {
                control.BackColor = backColor;
                control.ForeColor = foreColor;

                foreach (Control child in control.Controls)
                {
                    UpdateControlAndChildren(child, backColor, foreColor);
                }
            }
            else if (control is Label || control is MaterialLabel)
            {
                control.ForeColor = foreColor;
            }
            else if (control is TextBox)
            {
                control.BackColor = isDarkMode ? Color.FromArgb(70, 70, 70) : Color.White;
                control.ForeColor = foreColor;
            }
            else if (control is MaterialButton)
            {
                // MaterialButtons will handle their own colors through the MaterialSkin theme
            }
        }

        private void OpenComparisonTab()
        {
            // If comparison tab already exists, just select it
            if (comparisonTab != null && materialTabControl1.TabPages.Contains(comparisonTab))
            {
                materialTabControl1.SelectedTab = comparisonTab;
                return;
            }

            // Create new comparison tab
            comparisonTab = new TabPage("Commodity Comparison");
            materialTabControl1.TabPages.Add(comparisonTab);
            materialTabControl1.SelectedTab = comparisonTab;
            InitializeComparisonTab(comparisonTab);
        }

        private void InitializeComparisonTab(TabPage ComparisonTab)
        {
            // Configure theme colors based on isDarkMode field
            Color backColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White;
            Color foreColor = isDarkMode ? Color.White : Color.Black;

            ComparisonTab.BackColor = backColor;
            
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = backColor
            };
            ComparisonTab.Controls.Add(mainPanel);

            // Input Section
            GroupBox inputGroup = new GroupBox
            {
                Text = "Comparison Parameters",
                Location = new Point(20, 20),
                Size = new Size(1800, 100),
                BackColor = backColor,
                ForeColor = foreColor,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            // Year Selection
            MaterialLabel yearLabel = new MaterialLabel
            {
                Text = "Select Year:",
                Location = new Point(40, 40),
                Size = new Size(150, 30),
                ForeColor = foreColor,
                BackColor = backColor
            };

            MaterialComboBox yearComboBox = new MaterialComboBox
            {
                Location = new Point(200, 35),
                Width = 400,
                Hint = "Select Year"
            };

            // Load available years
            LoadYearsForComparison(yearComboBox);

            inputGroup.Controls.AddRange(new Control[] { yearLabel, yearComboBox });
            mainPanel.Controls.Add(inputGroup);

            // Chart Section
            GroupBox chartGroup = new GroupBox
            {
                Text = "Commodity Comparison",
                Location = new Point(20, 140),
                Size = new Size(1800, 500),
                BackColor = backColor,
                ForeColor = foreColor
            };

            Panel chartPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = backColor,
                Padding = new Padding(20)
            };
            chartGroup.Controls.Add(chartPanel);

            LiveCharts.WinForms.CartesianChart comparisonChart = new LiveCharts.WinForms.CartesianChart
            {
                Location = new Point(0, 0),
                Size = new Size(1760, 450),
                BackColor = backColor,
                Dock = DockStyle.Fill
            };

            // Set chart theme colors
            comparisonChart.BackColor = backColor;
            comparisonChart.ForeColor = foreColor;

            chartPanel.Controls.Add(comparisonChart);
            mainPanel.Controls.Add(chartGroup);

            // Initialize chart axes
            comparisonChart.AxisX = new LiveCharts.Wpf.AxesCollection();
            comparisonChart.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Month",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                Separator = new LiveCharts.Wpf.Separator
                {
                    Stroke = new System.Windows.Media.SolidColorBrush(
                        isDarkMode ? System.Windows.Media.Color.FromRgb(200, 200, 200) : System.Windows.Media.Color.FromRgb(50, 50, 50))
                },
                Foreground = new System.Windows.Media.SolidColorBrush(
                        isDarkMode ? System.Windows.Media.Colors.White : System.Windows.Media.Colors.Black)
            });

            comparisonChart.AxisY = new LiveCharts.Wpf.AxesCollection();
            comparisonChart.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Total Harvest (kg)",
                Separator = new LiveCharts.Wpf.Separator
                {
                    Stroke = new System.Windows.Media.SolidColorBrush(
                        isDarkMode ? System.Windows.Media.Color.FromRgb(200, 200, 200) : System.Windows.Media.Color.FromRgb(50, 50, 50))
                },
                Foreground = new System.Windows.Media.SolidColorBrush(
                        isDarkMode ? System.Windows.Media.Colors.White : System.Windows.Media.Colors.Black)
            });

            // Analysis Section
            GroupBox analysisGroup = new GroupBox
            {
                Text = "Comparison Analysis",
                Location = new Point(20, 660),
                Size = new Size(1800, 200),
                BackColor = backColor,
                ForeColor = foreColor
            };

            MaterialLabel analysisLabel = new MaterialLabel
            {
                Location = new Point(20, 30),
                Size = new Size(1760, 150),
                ForeColor = foreColor,
                BackColor = backColor,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            analysisGroup.Controls.Add(analysisLabel);
            mainPanel.Controls.Add(analysisGroup);

            // Generate Button
            MaterialButton generateButton = new MaterialButton
            {
                Text = "GENERATE COMPARISON",
                Location = new Point(20, 880),
                Size = new Size(200, 40)
            };

            generateButton.Click += (sender, e) => GenerateYearComparison(
                yearComboBox.Text,
                comparisonChart,
                analysisLabel
            );
            mainPanel.Controls.Add(generateButton);
        }

        private void LoadYearsForComparison(MaterialComboBox comboBox)
        {
            string query = "SELECT DISTINCT YEAR(HarvestDate) as Year FROM Harvest ORDER BY Year DESC";
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader["Year"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading years: " + ex.Message);
                }
            }
        }

        private void GenerateYearComparison(string selectedYear, LiveCharts.WinForms.CartesianChart chart, MaterialLabel analysisLabel)
        {
            if (string.IsNullOrEmpty(selectedYear))
            {
                MessageBox.Show("Please select a year for comparison.", "Missing Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int year = int.Parse(selectedYear);
                if (year > DateTime.Now.Year)
                {
                    MessageBox.Show($"No data available for future year {selectedYear}.", "Future Date", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Clear the chart and analysis
                    chart.Series.Clear();
                    chart.AxisX.Clear();
                    chart.AxisY.Clear();
                    analysisLabel.Text = "";
                    return;
                }

                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    c.CommodityName,
                        COALESCE(SUM(h.Quantity), 0) as TotalYield
                    FROM Commodities c
                    LEFT JOIN Harvest h ON h.CommodityID = c.CommodityID AND YEAR(h.HarvestDate) = @Year
                    GROUP BY c.CommodityName
                    ORDER BY TotalYield DESC";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Year", selectedYear);

                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show($"No data available for year {selectedYear}.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    UpdateYearComparisonChart(chart, dt, selectedYear);
                    GenerateYearComparisonAnalysis(dt, selectedYear, analysisLabel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating comparison: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateYearComparisonChart(LiveCharts.WinForms.CartesianChart chart, DataTable data, string year)
        {
            // Set chart background color immediately
            chart.BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White;

            chart.Series.Clear();
            chart.AxisX.Clear();
            chart.AxisY.Clear();

            var yields = new ChartValues<double>();
            var commodityNames = new List<string>();

            foreach (DataRow row in data.Rows)
            {
                yields.Add(Convert.ToDouble(row["TotalYield"]));
                commodityNames.Add(row["CommodityName"].ToString());
            }

            var foregroundBrush = new System.Windows.Media.SolidColorBrush(
                isDarkMode ? System.Windows.Media.Colors.White : System.Windows.Media.Colors.Black);

            var separatorBrush = new System.Windows.Media.SolidColorBrush(
                isDarkMode ? System.Windows.Media.Color.FromRgb(100, 100, 100) : System.Windows.Media.Color.FromRgb(200, 200, 200));

            chart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = $"Commodity Yields in {year}",
                    Values = yields,
                    DataLabels = true,
                    FontSize = 12,
                    Foreground = foregroundBrush,
                    Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 150, 243)),
                    LabelPoint = point => point.Y.ToString("N0")
                }
            };

            chart.AxisX.Add(new Axis
            {
                Title = "Commodities",
                Labels = commodityNames,
                Foreground = foregroundBrush,
                Separator = new Separator
                {
                    Step = 1,
                    IsEnabled = false,
                    Stroke = separatorBrush
                }
            });

            chart.AxisY.Add(new Axis
            {
                Title = "Total Yield",
                Foreground = foregroundBrush,
                Separator = new Separator
                {
                    Stroke = separatorBrush
                },
                LabelFormatter = value => value.ToString("N0")
            });
        }

        private void GenerateYearComparisonAnalysis(DataTable data, string year, MaterialLabel label)
        {
            var topCommodity = data.Rows[0];
            var bottomCommodity = data.Rows[data.Rows.Count - 1];
            double totalYield = 0;

            foreach (DataRow row in data.Rows)
            {
                totalYield += Convert.ToDouble(row["TotalYield"]);
            }

            double averageYield = totalYield / data.Rows.Count;

            string analysis = 
                $"Year {year} Analysis:\n\n" +
                $"Total Commodities: {data.Rows.Count}\n" +
                $"Total Combined Yield: {totalYield:N0} pieces\n" +
                $"Average Yield per Commodity: {averageYield:N0} pieces\n\n" +
                $"Top Performing Commodity: {topCommodity["CommodityName"]} ({Convert.ToDouble(topCommodity["TotalYield"]):N0} pieces)\n" +
                $"Lowest Performing Commodity: {bottomCommodity["CommodityName"]} ({Convert.ToDouble(bottomCommodity["TotalYield"]):N0} pieces)";

            label.Text = analysis;
        }

        public class ManageUsers : Form
        {
            private DataGridView usersGrid;
            private MaterialButton updateButton;
            private string connectionString = "Server=192.168.1.74;Port=3306;Database=oma;Uid=root;Pwd=root;SslMode=None;AllowPublicKeyRetrieval=True;ConnectionTimeout=60;DefaultCommandTimeout=60;MaximumPoolSize=100;";

            public ManageUsers()
            {
                InitializeManageUsers();
            }

            private void InitializeManageUsers()
            {
                this.Text = "Manage Users";
                this.Size = new Size(800, 600);
                this.StartPosition = FormStartPosition.CenterScreen;

                // Create DataGridView
                usersGrid = new DataGridView
                {
                    Location = new Point(20, 20),
                    Size = new Size(740, 450),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    ReadOnly = true
                };

                // Create Update Button
                updateButton = new MaterialButton
                {
                    Text = "UPDATE USER",
                    Location = new Point(20, 490),
                    Size = new Size(200, 36),
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                };
                updateButton.Click += UpdateButton_Click;

                // Add controls to form
                this.Controls.Add(usersGrid);
                this.Controls.Add(updateButton);

                LoadUsers();
            }

            private void LoadUsers()
            {
                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    try
                    {
                        connection.Open();
                        string query = "SELECT UserID, Username, Password, FirstName, LastName, Email, Role FROM Users";
                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        usersGrid.DataSource = dt;

                        // Hide password column
                        if (usersGrid.Columns["Password"] != null)
                            usersGrid.Columns["Password"].Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            private void UpdateButton_Click(object sender, EventArgs e)
            {
                if (usersGrid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a user to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = usersGrid.SelectedRows[0];
                using (Form updateForm = new Form())
                {
                    updateForm.Text = "Update User";
                    updateForm.Size = new Size(400, 500);
                    updateForm.StartPosition = FormStartPosition.CenterParent;
                    updateForm.BackColor = Color.FromArgb(50, 50, 50);
                    updateForm.ForeColor = Color.White;

                    // Create input fields
                    Label usernameLabel = new Label { Text = "Username:", Location = new Point(20, 20), ForeColor = Color.White };
                    TextBox usernameBox = new TextBox 
                    { 
                        Location = new Point(120, 20), 
                        Width = 200,
                        Text = selectedRow.Cells["Username"].Value.ToString()
                    };

                    Label passwordLabel = new Label { Text = "Password:", Location = new Point(20, 60), ForeColor = Color.White };
                    TextBox passwordBox = new TextBox 
                    { 
                        Location = new Point(120, 60), 
                        Width = 200,
                        UseSystemPasswordChar = true
                    };

                    Label firstNameLabel = new Label { Text = "First Name:", Location = new Point(20, 100), ForeColor = Color.White };
                    TextBox firstNameBox = new TextBox 
                    { 
                        Location = new Point(120, 100), 
                        Width = 200,
                        Text = selectedRow.Cells["FirstName"].Value.ToString()
                    };

                    Label lastNameLabel = new Label { Text = "Last Name:", Location = new Point(20, 140), ForeColor = Color.White };
                    TextBox lastNameBox = new TextBox 
                    { 
                        Location = new Point(120, 140), 
                        Width = 200,
                        Text = selectedRow.Cells["LastName"].Value.ToString()
                    };

                    Label emailLabel = new Label { Text = "Email:", Location = new Point(20, 180), ForeColor = Color.White };
                    TextBox emailBox = new TextBox 
                    { 
                        Location = new Point(120, 180), 
                        Width = 200,
                        Text = selectedRow.Cells["Email"].Value.ToString()
                    };

                    Label roleLabel = new Label { Text = "Role:", Location = new Point(20, 220), ForeColor = Color.White };
                    ComboBox roleBox = new ComboBox 
                    { 
                        Location = new Point(120, 220), 
                        Width = 200,
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };
                    roleBox.Items.AddRange(new string[] { "Admin", "User" });
                    roleBox.SelectedItem = selectedRow.Cells["Role"].Value.ToString();

                    MaterialButton saveButton = new MaterialButton
                    {
                        Text = "SAVE",
                        Location = new Point(120, 280),
                        Size = new Size(150, 40)
                    };

                    saveButton.Click += (s, ev) =>
                    {
                        if (ValidateUserInput(usernameBox.Text, firstNameBox.Text, lastNameBox.Text, emailBox.Text))
                        {
                            UpdateUser(
                                Convert.ToInt32(selectedRow.Cells["UserID"].Value),
                                usernameBox.Text,
                                passwordBox.Text,
                                firstNameBox.Text,
                                lastNameBox.Text,
                                emailBox.Text,
                                roleBox.SelectedItem.ToString()
                            );
                            updateForm.Close();
                            LoadUsers();
                        }
                    };

                    updateForm.Controls.AddRange(new Control[] {
                        usernameLabel, usernameBox,
                        passwordLabel, passwordBox,
                        firstNameLabel, firstNameBox,
                        lastNameLabel, lastNameBox,
                        emailLabel, emailBox,
                        roleLabel, roleBox,
                        saveButton
                    });

                    updateForm.ShowDialog();
                }
            }

            private bool ValidateUserInput(string username, string firstName, string lastName, string email)
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                {
                    MessageBox.Show("First name and last name are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Email is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }

            private void UpdateUser(int userId, string username, string password, string firstName, 
                string lastName, string email, string role)
            {
                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    try
                    {
                        connection.Open();
                        string query;
                        MySqlCommand cmd;

                        if (!string.IsNullOrEmpty(password))
                        {
                            // Update with new password
                            query = @"UPDATE Users SET 
                                    Username = @Username,
                                    Password = @Password,
                                    FirstName = @FirstName,
                                    LastName = @LastName,
                                    Email = @Email,
                                    Role = @Role
                                    WHERE UserID = @UserID";
                            cmd = new MySqlCommand(query, connection);
                            cmd.Parameters.AddWithValue("@Password", password);
                        }
                        else
                        {
                            // Update without changing password
                            query = @"UPDATE Users SET 
                                    Username = @Username,
                                    FirstName = @FirstName,
                                    LastName = @LastName,
                                    Email = @Email,
                                    Role = @Role
                                    WHERE UserID = @UserID";
                            cmd = new MySqlCommand(query, connection);
                        }

                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Role", role);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CreateFarmerNotesTable()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        CREATE TABLE IF NOT EXISTS FarmerNotes (
                            NoteID INT PRIMARY KEY AUTO_INCREMENT,
                            FarmerID INT,
                            Note TEXT,
                            DateAdded DATETIME,
                            FOREIGN KEY (FarmerID) REFERENCES Farmers(FarmerID)
                        )";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating FarmerNotes table: {ex.Message}", "Database Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cartesianChartBarangay_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void GenerateRecommendations(DataTable data, MaterialLabel recommendations, string season)
        {
            try
            {
                if (data == null || data.Rows.Count == 0)
                {
                    recommendations.Text = "No data available for recommendations.";
                    return;
                }

                // Calculate average yield
                double totalYield = 0;
                foreach (DataRow row in data.Rows)
                {
                    totalYield += Convert.ToDouble(row["AvgYield"]);
                }
                double averageYield = totalYield / data.Rows.Count;

                // Find highest and lowest yields
                double highestYield = double.MinValue;
                double lowestYield = double.MaxValue;
                string bestMonth = "";
                string worstMonth = "";

                foreach (DataRow row in data.Rows)
                {
                    double yield = Convert.ToDouble(row["AvgYield"]);
                    string month = row["Month"].ToString();
                    
                    if (yield > highestYield)
                    {
                        highestYield = yield;
                        bestMonth = month;
                    }
                    if (yield < lowestYield)
                    {
                        lowestYield = yield;
                        worstMonth = month;
                    }
                }

                // Calculate success probability based on historical data
                int successfulMonths = 0;
                foreach (DataRow row in data.Rows)
                {
                    if (Convert.ToDouble(row["AvgYield"]) >= averageYield)
                    {
                        successfulMonths++;
                    }
                }
                double successProbability = (double)successfulMonths / data.Rows.Count * 100;

                // Generate recommendations text
                StringBuilder recommendationsText = new StringBuilder();
                recommendationsText.AppendLine($"Best Planting Time: {bestMonth}");
                recommendationsText.AppendLine($"\nExpected Yield Range: {lowestYield:N0} - {highestYield:N0} pieces");
                
                // Risk factors based on season
                recommendationsText.AppendLine("\nRisk Factors:");
                if (season == "Wet Season (June-November)")
                {
                    recommendationsText.AppendLine("• Heavy rainfall and potential flooding");
                    recommendationsText.AppendLine("• Higher humidity and disease risk");
                    recommendationsText.AppendLine("• Reduced sunlight exposure");
                }
                else // Dry Season
                {
                    recommendationsText.AppendLine("• Water scarcity risk");
                    recommendationsText.AppendLine("• High temperature stress");
                    recommendationsText.AppendLine("• Increased pest activity");
                }

                recommendationsText.AppendLine($"\nSuccess Probability: {successProbability:N1}%");

                recommendations.Text = recommendationsText.ToString();
            }
            catch (Exception ex)
            {
                recommendations.Text = "Error generating recommendations: " + ex.Message;
            }
        }

        private void materialFloatingActionButton1_Click_1(object sender, EventArgs e)
        {
            materialTabControl1.SelectedIndex = 2;
        }

        private void btn_batch_import_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Select Excel File for Batch Import";

                if (openFileDialog.ShowDialog() == DialogResult.OK)  
                {
                    try
                    {
                        using (var package = new ExcelPackage(new FileInfo(openFileDialog.FileName)))
                        {
                            // Show progress form
                            Form progressForm = new Form
                            {
                                Text = "Importing Data",
                                Size = new Size(400, 150),
                                StartPosition = FormStartPosition.CenterScreen,
                                FormBorderStyle = FormBorderStyle.FixedDialog,
                                MaximizeBox = false,
                                MinimizeBox = false,
                                ControlBox = false  // Prevent closing the form
                            };

                            ProgressBar progressBar = new ProgressBar
                            {
                                Location = new Point(20, 20),
                                Size = new Size(340, 30),
                                Style = ProgressBarStyle.Marquee,
                                MarqueeAnimationSpeed = 30
                            };

                            Label statusLabel = new Label
                            {
                                Location = new Point(20, 60),
                                Size = new Size(340, 30),
                                Text = "Preparing to import..."
                            };

                            progressForm.Controls.AddRange(new Control[] { progressBar, statusLabel });

                            // Create a background worker
                            BackgroundWorker worker = new BackgroundWorker();
                            worker.WorkerReportsProgress = true;
                            List<string> importResults = new List<string>();

                            worker.DoWork += (s, args) =>
                            {
                                foreach (var worksheet in package.Workbook.Worksheets)
                                {
                                    worker.ReportProgress(0, $"Processing worksheet: {worksheet.Name}");
                                    var table = ConvertWorksheetToDataTable(worksheet);
                                    var importResult = ImportDataTableToDatabase(table, worksheet.Name);
                                    importResults.Add(importResult);
                                }
                            };

                            worker.ProgressChanged += (s, args) =>
                            {
                                statusLabel.Text = args.UserState.ToString();
                            };

                            worker.RunWorkerCompleted += (s, args) =>
                            {
                                progressForm.Close();
                                if (args.Error != null)
                                {
                                    MessageBox.Show($"Error during import: {args.Error.Message}", "Import Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    MessageBox.Show(
                                        string.Join("\n", importResults),
                                        "Import Complete",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                                    // Refresh all data views
                                    RefreshAllData();
                                }
                            };

                            // Start the background worker
                            worker.RunWorkerAsync();
                            progressForm.ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during import: {ex.Message}", "Import Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private DataTable ConvertWorksheetToDataTable(ExcelWorksheet worksheet)
        {
            DataTable dt = new DataTable();
            
            // Read the header row and create columns
            for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
            {
                string columnName = worksheet.Cells[1, col].Text.Trim();
                dt.Columns.Add(columnName);
            }

            // Read the data rows
            for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
            {
                DataRow dataRow = dt.NewRow();
                for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                {
                    var cell = worksheet.Cells[row, col];
                    if (cell == null || cell.Value == null || string.IsNullOrWhiteSpace(cell.Text))
                    {
                        dataRow[col - 1] = DBNull.Value;
                    }
                    else
                    {
                        dataRow[col - 1] = cell.Text.Trim();
                    }
                }
                dt.Rows.Add(dataRow);
            }

            return dt;
        }

        private string ImportDataTableToDatabase(DataTable dt, string sheetName)
        {
            int successCount = 0;
            int errorCount = 0;
            int duplicateCount = 0;
            StringBuilder errorMessages = new StringBuilder();

            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string tableName = DetermineTableName(sheetName, dt);
                        
                        // Get the actual database columns for the table
                        DataTable schemaTable = connection.GetSchema("Columns", new[] { null, null, tableName });
                        var dbColumns = new HashSet<string>(
                            schemaTable.Rows.Cast<DataRow>()
                                .Select(row => row["COLUMN_NAME"].ToString().ToLower())
                        );

                        // Remove FarmerID from required columns for Farmers table
                        if (tableName.ToLower() == "farmers" && dbColumns.Contains("farmerid"))
                        {
                            dbColumns.Remove("farmerid");
                        }

                        // Validate and map columns
                        var validColumns = new List<string>();
                        var validParameters = new List<string>();
                        var columnMappings = new Dictionary<string, string>();

                        foreach (DataColumn col in dt.Columns)
                        {
                            string columnName = col.ColumnName.Trim();
                            string dbColumnName = MapColumnName(columnName, tableName);
                            
                            if (dbColumns.Contains(dbColumnName.ToLower()))
                            {
                                validColumns.Add(dbColumnName);
                                validParameters.Add($"@{columnName}");
                                columnMappings[columnName] = dbColumnName;
                            }
                            else
                            {
                                errorMessages.AppendLine($"Warning: Column '{columnName}' does not exist in table '{tableName}'");
                            }
                        }

                        if (validColumns.Count == 0)
                        {
                            throw new Exception($"No valid columns found for table '{tableName}'");
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            try
                            {
                                bool isDuplicate = false;

                                switch (tableName.ToLower())
                                {
                                    case "farmers":
                                        // Get the first commodity from the row if it exists
                                        string commodityName = null;
                                        if (dt.Columns.Contains("Commodities") && row["Commodities"] != DBNull.Value)
                                        {
                                            commodityName = row["Commodities"].ToString().Split(',')[0].Trim();
                                        }

                                        // Modify the insert query to include Crop
                                        string insertFarmerQuery = @"
                                            INSERT INTO Farmers (
                                                FirstName, LastName, MiddleName, Address, Barangay, 
                                                ContactInfo, FarmSize, RegistrationDate, Birthdate, Crop
                                            ) VALUES (
                                                @FirstName, @LastName, @MiddleName, @Address, @Barangay,
                                                @ContactInfo, @FarmSize, @RegistrationDate, @Birthdate,
                                                (SELECT CommodityID FROM Commodities WHERE CommodityName = @FirstCommodity)
                                            )";

                                        using (MySqlCommand cmd = new MySqlCommand(insertFarmerQuery, connection, transaction))
                                        {
                                            // Add parameters with null checks
                                            cmd.Parameters.AddWithValue("@FirstName", row["FirstName"] ?? DBNull.Value);
                                            cmd.Parameters.AddWithValue("@LastName", row["LastName"] ?? DBNull.Value);
                                            cmd.Parameters.AddWithValue("@MiddleName", 
                                                dt.Columns.Contains("MiddleName") ? (row["MiddleName"] ?? DBNull.Value) : DBNull.Value);
                                            cmd.Parameters.AddWithValue("@Address", row["Address"] ?? DBNull.Value);
                                            cmd.Parameters.AddWithValue("@Barangay", row["Barangay"] ?? DBNull.Value);
                                            cmd.Parameters.AddWithValue("@ContactInfo", 
                                                dt.Columns.Contains("ContactInfo") ? (row["ContactInfo"] ?? DBNull.Value) : DBNull.Value);
                                            
                                            // Handle FarmSize
                                            decimal farmSize = 0;
                                            if (dt.Columns.Contains("FarmSize") && row["FarmSize"] != DBNull.Value)
                                            {
                                                decimal.TryParse(row["FarmSize"].ToString(), out farmSize);
                                            }
                                            cmd.Parameters.AddWithValue("@FarmSize", farmSize);

                                            // Handle dates
                                            DateTime? registrationDate = null;
                                            if (dt.Columns.Contains("RegistrationDate") && row["RegistrationDate"] != DBNull.Value)
                                            {
                                                DateTime.TryParse(row["RegistrationDate"].ToString(), out DateTime regDate);
                                                registrationDate = regDate;
                                            }
                                            cmd.Parameters.AddWithValue("@RegistrationDate", 
                                                registrationDate.HasValue ? (object)registrationDate.Value : DBNull.Value);

                                            DateTime? birthdate = null;
                                            if (dt.Columns.Contains("Birthdate") && row["Birthdate"] != DBNull.Value)
                                            {
                                                DateTime.TryParse(row["Birthdate"].ToString(), out DateTime bDate);
                                                birthdate = bDate;
                                            }
                                            cmd.Parameters.AddWithValue("@Birthdate", 
                                                birthdate.HasValue ? (object)birthdate.Value : DBNull.Value);

                                            // Handle first commodity
                                            cmd.Parameters.AddWithValue("@FirstCommodity", 
                                                !string.IsNullOrEmpty(commodityName) ? (object)commodityName : DBNull.Value);

                                            cmd.ExecuteNonQuery();
                                        }

                                        // If there are commodities, add them to FarmerCommodities
                                        if (!string.IsNullOrEmpty(commodityName))
                                        {
                                            string[] commodities = row["Commodities"].ToString()
                                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(c => c.Trim())
                                                .ToArray();

                                            foreach (string commodity in commodities)
                                            {
                                                string insertCommodityQuery = @"
                                                    INSERT INTO FarmerCommodities (FarmerID, CommodityID)
                                                    SELECT LAST_INSERT_ID(), CommodityID 
                                                    FROM Commodities 
                                                    WHERE CommodityName = @CommodityName";

                                                using (MySqlCommand cmd = new MySqlCommand(insertCommodityQuery, connection, transaction))
                                                {
                                                    cmd.Parameters.AddWithValue("@CommodityName", commodity);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                        break;

                                    case "harvest":
                                        isDuplicate = CheckDuplicateHarvest(connection, transaction, row);
                                        break;

                                    case "commodities":
                                        isDuplicate = CheckDuplicateCommodity(connection, transaction, row);
                                        break;
                                }

                                if (isDuplicate)
                                {
                                    duplicateCount++;
                                    string recordIdentifier = GetRecordIdentifier(tableName, row);
                                    errorMessages.AppendLine($"Duplicate record found: {recordIdentifier}");
                                    continue;
                                }

                                // Validate required fields and data types
                                string validationError = ValidateRowData(tableName, row);
                                if (!string.IsNullOrEmpty(validationError))
                                {
                                    errorCount++;
                                    errorMessages.AppendLine($"Error in row {dt.Rows.IndexOf(row) + 2}: {validationError}");
                                    continue;
                                }

                                // Proceed with insert
                                string insertQuery = $"INSERT INTO {tableName} ({string.Join(", ", validColumns)}) " +
                                                  $"VALUES ({string.Join(", ", validParameters)})";

                                using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection, transaction))
                                {
                                    foreach (DataColumn col in dt.Columns)
                                    {
                                        string columnName = col.ColumnName.Trim();
                                        if (columnMappings.ContainsKey(columnName))
                                        {
                                            object value = row[col];
                                            if (value == DBNull.Value)
                                            {
                                                cmd.Parameters.AddWithValue($"@{columnName}", null);
                                            }
                                            else if (columnName.ToLower().Contains("date"))
                                            {
                                                // Try to parse the date value
                                                    if (DateTime.TryParse(value.ToString(), out DateTime dateValue))
                                                    {
                                                        cmd.Parameters.AddWithValue($"@{columnName}", dateValue);
                                                    }
                                                    else
                                                    {
                                                    throw new Exception($"Invalid date format in column {columnName}");
                                                    }
                                                }
                                                else
                                                {
                                                    cmd.Parameters.AddWithValue($"@{columnName}", value);
                                            }
                                        }
                                    }
                                    cmd.ExecuteNonQuery();
                                    successCount++;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorCount++;
                                errorMessages.AppendLine($"Error in row {dt.Rows.IndexOf(row) + 2}: {ex.Message}");
                            }
                        }

                        transaction.Commit();
                        return $"Sheet: {sheetName}\n" +
                               $"Successful imports: {successCount}\n" +
                               $"Duplicate records: {duplicateCount}\n" +
                               $"Failed imports: {errorCount}\n" +
                               (errorCount > 0 || duplicateCount > 0 ? $"\nDetails:\n{errorMessages}" : "");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Error importing sheet {sheetName}: {ex.Message}");
                    }
                }
            }
        }

        private string MapColumnName(string excelColumn, string tableName)
        {
            // Map Excel column names to database column names
            switch (tableName.ToLower())
            {
                case "harvest":
                    switch (excelColumn.ToLower())
                    {
                        case "commodityid":
                            return "CommodityID";
                        case "farmerid":
                            return "FarmerID";
                        case "harvestdate":
                            return "HarvestDate";
                        case "quantity":
                            return "Quantity";
                        case "totalrevenue":
                            return "TotalRevenue";
                        default:
                            return excelColumn;
                    }

                case "farmers":
                    switch (excelColumn.ToLower())
                    {
                        case "firstname":
                            return "FirstName";
                        case "lastname":
                            return "LastName";
                        case "address":
                            return "Address";
                        case "barangay":
                            return "Barangay";
                        case "contactinfo":
                            return "ContactInfo";
                        case "farmsize":
                            return "FarmSize";
                        case "registrationdate":
                            return "RegistrationDate";
                        case "crop":
                            return "Crop";
                        default:
                            return excelColumn;
                    }

                case "commodities":
                    switch (excelColumn.ToLower())
                    {
                        case "commodityname":
                            return "CommodityName";
                        case "description":
                            return "Description";
                        default:
                            return excelColumn;
                    }

                default:
                    return excelColumn;
            }
        }

        private string DetermineTableName(string sheetName, DataTable dt = null)
        {
            // If we have the DataTable, try to determine the table type from its columns
            if (dt != null)
            {
                var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(col => col.ColumnName.ToLower())
                    .ToList();

                // Check if it's a harvest table
                if (columnNames.Contains("commodityid") || columnNames.Contains("harvestdate"))
                {
                    return "Harvest";
                }
                
                // Check if it's a farmers table
                if (columnNames.Contains("firstname") || columnNames.Contains("lastname"))
                {
                    return "Farmers";
                }
                
                // Check if it's a commodities table
                if (columnNames.Contains("commodityname"))
                {
                    return "Commodities";
                }
            }

            // Fallback to sheet name if we can't determine from columns
            string normalizedSheetName = sheetName?.Trim().ToLower() ?? "";

            switch (normalizedSheetName)
            {
                case "farmers":
                    return "Farmers";
                case "harvest":
                    return "Harvest";
                case "commodities":
                    return "Commodities";
                case "sheet1":
                case "sheet2":
                case "sheet3":
                    throw new Exception(
                        $"Sheet name '{sheetName}' is ambiguous.\n" +
                        "Please rename your sheets to one of:\n" +
                        "- 'Farmers'\n" +
                        "- 'Harvest'\n" +
                        "- 'Commodities'"
                    );
                default:
                    throw new Exception(
                        $"Invalid sheet name: '{sheetName}'\n" +
                        "Expected sheet names are:\n" +
                        "- 'Farmers'\n" +
                        "- 'Harvest'\n" +
                        "- 'Commodities'"
                    );
            }
        }

        private void RefreshAllData()
        {
            // Refresh farmers grid
            LoadFarmersData();
            
            // Refresh farmer count
            LoadCount();
            
            // Refresh farmer registration chart
            LoadFarmerRegistrationChart();
            
            // Refresh commodities data
            LoadCommodities();
            
            // If on prediction tab, refresh current prediction
            if (materialTabControl1.SelectedTab.Text == "Prediction")
            {
                // Find the prediction chart and recommendations label
                var predictionChart = materialTabControl1.SelectedTab.Controls.Find("predictionChart", true).FirstOrDefault() as LiveCharts.WinForms.CartesianChart;
                var recommendationsLabel = materialTabControl1.SelectedTab.Controls.Find("recommendationsLabel", true).FirstOrDefault() as MaterialLabel;
                
                if (predictionChart != null)
                {
                    // Clear current chart data
                    predictionChart.Series.Clear();
                    predictionChart.AxisX.Clear();
                    predictionChart.AxisY.Clear();
                }
            }
            
            // If comparison tab exists and is selected, refresh comparison
            if (comparisonTab != null && materialTabControl1.SelectedTab == comparisonTab)
            {
                var comparisonChart = comparisonTab.Controls.Find("comparisonChart", true).FirstOrDefault() as LiveCharts.WinForms.CartesianChart;
                var analysisLabel = comparisonTab.Controls.Find("analysisLabel", true).FirstOrDefault() as MaterialLabel;
                
                if (comparisonChart != null)
                {
                    // Clear current chart data
                    comparisonChart.Series.Clear();
                    comparisonChart.AxisX.Clear();
                    comparisonChart.AxisY.Clear();
                }
            }
            
            // Refresh the main commodity chart if it exists
            if (cartesianChart != null && comboBoxCommodities.SelectedValue != null)
            {
                LoadCommodityData((int)comboBoxCommodities.SelectedValue);
            }
        }

        private bool CheckDuplicateFarmer(MySqlConnection connection, MySqlTransaction transaction, DataRow row)
        {
            string firstName = row["FirstName"]?.ToString();
            string lastName = row["LastName"]?.ToString();
            string address = row["Address"]?.ToString();
            string barangay = row["Barangay"]?.ToString();
            string contactInfo = row["ContactInfo"]?.ToString();
            
            string query = @"SELECT COUNT(*) FROM Farmers 
                    WHERE FirstName = @FirstName 
                    AND LastName = @LastName 
                    AND Address = @Address 
                    AND Barangay = @Barangay 
                    AND ContactInfo = @ContactInfo";

            using (MySqlCommand cmd = new MySqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Address", address ?? "");
                cmd.Parameters.AddWithValue("@Barangay", barangay ?? "");
                cmd.Parameters.AddWithValue("@ContactInfo", contactInfo ?? "");
                
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private bool CheckDuplicateHarvest(MySqlConnection connection, MySqlTransaction transaction, DataRow row)
        {
            string farmerIdStr = row["FarmerID"]?.ToString();
            string commodityIdStr = row["CommodityID"]?.ToString();
            string harvestDateStr = row["HarvestDate"]?.ToString();
            string quantityStr = row["Quantity"]?.ToString();

            if (!int.TryParse(farmerIdStr, out int farmerId) || 
                !int.TryParse(commodityIdStr, out int commodityId) ||
                !DateTime.TryParse(harvestDateStr, out DateTime harvestDate))
            {
                return false; // Let validation handle this error
            }

            string query = @"SELECT COUNT(*) FROM Harvest 
                    WHERE FarmerID = @FarmerID 
                    AND CommodityID = @CommodityID 
                    AND HarvestDate = @HarvestDate 
                    AND Quantity = @Quantity";

            using (MySqlCommand cmd = new MySqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@FarmerID", farmerId);
                cmd.Parameters.AddWithValue("@CommodityID", commodityId);
                cmd.Parameters.AddWithValue("@HarvestDate", harvestDate);
                cmd.Parameters.AddWithValue("@Quantity", quantityStr);
                
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private bool CheckDuplicateCommodity(MySqlConnection connection, MySqlTransaction transaction, DataRow row)
        {
            string commodityName = row["CommodityName"]?.ToString();
            string description = row["Description"]?.ToString();
            
            string query = @"SELECT COUNT(*) FROM Commodities 
                    WHERE CommodityName = @CommodityName 
                    AND Description = @Description";

            using (MySqlCommand cmd = new MySqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@CommodityName", commodityName);
                cmd.Parameters.AddWithValue("@Description", description ?? "");
                
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private string GetRecordIdentifier(string tableName, DataRow row)
        {
            switch (tableName.ToLower())
            {
                case "farmers":
                    return $"Farmer {row["FirstName"]} {row["LastName"]} from {row["Barangay"]}";
                case "harvest":
                    return $"Harvest record for FarmerID {row["FarmerID"]}, CommodityID {row["CommodityID"]} on {row["HarvestDate"]}";
                case "commodities":
                    return $"Commodity {row["CommodityName"]}";
                default:
                    return "Record";
            }
        }

        private string ValidateRowData(string tableName, DataRow row)
        {
            switch (tableName.ToLower())
            {
                case "farmers":
                    if (string.IsNullOrWhiteSpace(row["FirstName"]?.ToString()) || 
                        string.IsNullOrWhiteSpace(row["LastName"]?.ToString()))
                        return "FirstName and LastName are required";
                    break;

                case "harvest":
                    if (!int.TryParse(row["FarmerID"]?.ToString(), out _))
                        return "Invalid FarmerID format";
                    if (!int.TryParse(row["CommodityID"]?.ToString(), out _))
                        return "Invalid CommodityID format";
                    if (!DateTime.TryParse(row["HarvestDate"]?.ToString(), out _))
                        return "Invalid HarvestDate format";
                    if (!decimal.TryParse(row["Quantity"]?.ToString(), out decimal quantity) || quantity < 0)
                        return "Invalid Quantity";
                    break;

                case "commodities":
                    if (string.IsNullOrWhiteSpace(row["CommodityName"]?.ToString()))
                        return "CommodityName is required";
                    break;
            }
            return string.Empty;
        }

        private void InitializeExportTab(TabPage ExportTab)
        {
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : SystemColors.Control
            };
            ExportTab.Controls.Add(mainPanel);

            // Export Options Group
            GroupBox optionsGroup = new GroupBox
            {
                Text = "Export Options",
                Location = new Point(20, 20),
                Size = new Size(1800, 250),
                BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : SystemColors.Control,
                ForeColor = isDarkMode ? Color.White : Color.Black,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            // Data Type Selection
            MaterialLabel dataTypeLabel = new MaterialLabel
            {
                Text = "Select Data to Export:",
                Location = new Point(40, 60), // Adjusted position
                Size = new Size(200, 30), // Increased width
                ForeColor = isDarkMode ? Color.White : Color.Black
            };

            MaterialComboBox dataTypeComboBox = new MaterialComboBox
            {
                Location = new Point(250, 55), // Adjusted position
                Width = 400,
                Hint = "Select Data Type"
            };
            dataTypeComboBox.Items.AddRange(new string[] { 
                "All Data", 
                "Farmers by Barangay", 
                "Harvest Data by Year",
                "Commodities"
            });

            // Filter Options
            MaterialLabel filterLabel = new MaterialLabel
            {
                Text = "Filter By:",
                Location = new Point(40, 120), // Adjusted position
                Size = new Size(200, 30), // Increased width
                ForeColor = isDarkMode ? Color.White : Color.Black
            };

            MaterialComboBox filterComboBox = new MaterialComboBox
            {
                Location = new Point(250, 115), // Adjusted position
                Width = 400,
                Hint = "Select Filter",
                Enabled = false
            };

            // Export Format
            MaterialLabel formatLabel = new MaterialLabel
            {
                Text = "Export Format:",
                Location = new Point(40, 180), // Adjusted position
                Size = new Size(200, 30), // Increased width
                ForeColor = isDarkMode ? Color.White : Color.Black
            };

            MaterialComboBox formatComboBox = new MaterialComboBox
            {
                Location = new Point(250, 175), // Adjusted position
                Width = 400,
                Hint = "Select Format"
            };
            formatComboBox.Items.AddRange(new string[] { "Excel", "PDF" });

            // Add controls to options group
            optionsGroup.Controls.AddRange(new Control[] { 
                dataTypeLabel, dataTypeComboBox,
                filterLabel, filterComboBox,
                formatLabel, formatComboBox
            });
            mainPanel.Controls.Add(optionsGroup);

            // Preview Section
            GroupBox previewGroup = new GroupBox
            {
                Text = "Data Preview",
                Location = new Point(20, 290),
                Size = new Size(1800, 450),
                BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : SystemColors.Control,
                ForeColor = isDarkMode ? Color.White : Color.Black
            };

            DataGridView previewGrid = new DataGridView
            {
                Location = new Point(20, 30),
                Size = new Size(1760, 350),
                BackgroundColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White,
                    ForeColor = isDarkMode ? Color.White : Color.Black,
                    SelectionBackColor = isDarkMode ? Color.FromArgb(75, 75, 75) : Color.LightBlue,
                    SelectionForeColor = isDarkMode ? Color.White : Color.Black
                },
                RowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White,
                    ForeColor = isDarkMode ? Color.White : Color.Black,
                    SelectionBackColor = isDarkMode ? Color.FromArgb(75, 75, 75) : Color.LightBlue,
                    SelectionForeColor = isDarkMode ? Color.White : Color.Black
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = isDarkMode ? Color.FromArgb(45, 45, 45) : Color.WhiteSmoke,
                    ForeColor = isDarkMode ? Color.White : Color.Black,
                    SelectionBackColor = isDarkMode ? Color.FromArgb(75, 75, 75) : Color.LightBlue,
                    SelectionForeColor = isDarkMode ? Color.White : Color.Black
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = isDarkMode ? Color.FromArgb(45, 45, 45) : Color.FromArgb(0, 0, 139),
                    ForeColor = Color.White,
                    SelectionBackColor = isDarkMode ? Color.FromArgb(75, 75, 75) : Color.FromArgb(0, 0, 160),
                    Font = new Font("Segoe UI", 9.75F, FontStyle.Bold)
                },
                EnableHeadersVisualStyles = false,
                GridColor = isDarkMode ? Color.FromArgb(60, 60, 60) : Color.LightGray
            };

            // Add event handler for theme changes
            dataTypeComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (dataTypeComboBox.SelectedItem != null)
                {
                    UpdatePreviewGrid(dataTypeComboBox.Text, filterComboBox.Text, previewGrid);
                }
            };

            // Add event handler for theme changes
            materialTabControl1.SelectedIndexChanged += (s, e) =>
            {
                if (materialTabControl1.SelectedTab == ExportTab)
                {
                    UpdateGridTheme(previewGrid);
                }
            };

            // Export Button in Preview Section
            MaterialButton previewExportButton = new MaterialButton
            {
                Text = "EXPORT DATA",
                Location = new Point(20, 390), // Position below the grid
                Size = new Size(200, 40)
            };

            previewExportButton.Click += (sender, e) =>
            {
                if (string.IsNullOrEmpty(dataTypeComboBox.Text))
                {
                    MessageBox.Show("Please select data type to export.", "Export Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(formatComboBox.Text))
                {
                    MessageBox.Show("Please select export format.", "Export Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ExportData(
                    dataTypeComboBox.Text,
                    filterComboBox.Text,
                    formatComboBox.Text,
                    previewGrid
                );
            };

            previewGroup.Controls.Add(previewGrid);
            previewGroup.Controls.Add(previewExportButton);
            mainPanel.Controls.Add(previewGroup);

            // Export Button
            MaterialButton exportButton = new MaterialButton
            {
                Text = "EXPORT DATA",
                Location = new Point(20, 660),
                Size = new Size(200, 40)
            };

            mainPanel.Controls.Add(exportButton);

            // Event Handlers
            dataTypeComboBox.SelectedIndexChanged += (sender, e) =>
            {
                filterComboBox.Items.Clear();
                filterComboBox.Enabled = true;

                switch (dataTypeComboBox.Text)
                {
                    case "Farmers by Barangay":
                        LoadBarangaysToComboBox(filterComboBox);
                        break;
                    case "Harvest Data by Year":
                        LoadYearsToComboBox(filterComboBox);
                        break;
                    default:
                        filterComboBox.Enabled = false;
                        break;
                }

                UpdatePreviewGrid(dataTypeComboBox.Text, filterComboBox.Text, previewGrid);
            };

            filterComboBox.SelectedIndexChanged += (sender, e) =>
            {
                UpdatePreviewGrid(dataTypeComboBox.Text, filterComboBox.Text, previewGrid);
            };

            exportButton.Click += (sender, e) =>
            {
                if (string.IsNullOrEmpty(dataTypeComboBox.Text))
                {
                    MessageBox.Show("Please select data type to export.", "Export Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(formatComboBox.Text))
                {
                    MessageBox.Show("Please select export format.", "Export Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ExportData(
                    dataTypeComboBox.Text,
                    filterComboBox.Text,
                    formatComboBox.Text,
                    previewGrid
                );
            };
        }

        private void LoadYearsToComboBox(MaterialComboBox comboBox)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT DISTINCT YEAR(HarvestDate) as Year FROM Harvest ORDER BY Year DESC";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBox.Items.Add(reader["Year"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading years: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePreviewGrid(string dataType, string filter, DataGridView grid)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();
                    DataTable dt = new DataTable();

                    if (dataType == "All Data")
                    {
                        // Create combined DataTable with all necessary columns
                        dt.Columns.AddRange(new DataColumn[] {
                            new DataColumn("Data Type", typeof(string)),
                            new DataColumn("First Name", typeof(string)),
                            new DataColumn("Last Name", typeof(string)),
                            new DataColumn("Address", typeof(string)),
                            new DataColumn("Barangay", typeof(string)),
                            new DataColumn("Contact Info", typeof(string)),
                            new DataColumn("Farm Size", typeof(decimal)),
                            new DataColumn("Registration Date", typeof(string)),
                            new DataColumn("Commodity Name", typeof(string)),
                            new DataColumn("Quantity", typeof(decimal)),
                            new DataColumn("Price Per Unit", typeof(decimal)),
                            new DataColumn("Total Revenue", typeof(decimal)),
                            new DataColumn("Harvest Date", typeof(string)),
                            new DataColumn("Unit Of Measurement", typeof(string))
                        });

                        var queries = GetExportQueries(dataType, filter);

                        // Add Farmers data
                        using (MySqlCommand cmd = new MySqlCommand(queries["Farmers"], conn))
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable farmersTable = new DataTable();
                            adapter.Fill(farmersTable);
                            foreach (DataRow row in farmersTable.Rows)
                            {
                                DataRow newRow = dt.NewRow();
                                newRow["Data Type"] = "Farmer";
                                newRow["First Name"] = row["FirstName"];
                                newRow["Last Name"] = row["LastName"];
                                newRow["Address"] = row["Address"];
                                newRow["Barangay"] = row["Barangay"];
                                newRow["Contact Info"] = row["ContactInfo"];
                                newRow["Farm Size"] = row["FarmSize"];
                                newRow["Registration Date"] = row["RegistrationDate"];
                                dt.Rows.Add(newRow);
                            }
                        }

                        // Add Harvests data
                        using (MySqlCommand cmd = new MySqlCommand(queries["Harvests"], conn))
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable harvestsTable = new DataTable();
                            adapter.Fill(harvestsTable);
                            foreach (DataRow row in harvestsTable.Rows)
                            {
                                DataRow newRow = dt.NewRow();
                                newRow["Data Type"] = "Harvest";
                                string[] farmerName = row["FarmerName"].ToString().Split(' ');
                                if (farmerName.Length >= 2)
                                {
                                    newRow["First Name"] = farmerName[0];
                                    newRow["Last Name"] = farmerName[1];
                                }
                                newRow["Commodity Name"] = row["CommodityName"];
                                newRow["Quantity"] = row["Quantity"];
                                newRow["Price Per Unit"] = row["PricePerUnit"];
                                newRow["Total Revenue"] = row["TotalRevenue"];
                                newRow["Harvest Date"] = row["HarvestDate"];
                                dt.Rows.Add(newRow);
                            }
                        }

                        // Add Commodities data
                        using (MySqlCommand cmd = new MySqlCommand(queries["Commodities"], conn))
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable commoditiesTable = new DataTable();
                            adapter.Fill(commoditiesTable);
                            foreach (DataRow row in commoditiesTable.Rows)
                            {
                                DataRow newRow = dt.NewRow();
                                newRow["Data Type"] = "Commodity";
                                newRow["Commodity Name"] = row["CommodityName"];
                                newRow["Unit Of Measurement"] = row["UnitOfMeasurement"];
                                dt.Rows.Add(newRow);
                            }
                        }
                    }
                    else
                    {
                        var queries = GetExportQueries(dataType, filter);
                        if (queries.Count > 0)
                        {
                            string query = queries["Data"];
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            {
                        adapter.Fill(dt);
                            }
                        }
                    }

                        grid.DataSource = dt;

                        // Adjust column headers for better readability
                        foreach (DataGridViewColumn col in grid.Columns)
                        {
                            col.HeaderText = SplitCamelCase(col.HeaderText);
                        }

                        // Auto-size columns
                        grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating preview: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Dictionary<string, string> GetExportQueries(string dataType, string filter)
        {
            switch (dataType)
            {
                case "All Data":
                    return new Dictionary<string, string>
                    {
                        ["Farmers"] = @"
                            SELECT FirstName, LastName, Address, Barangay, ContactInfo, FarmSize, 
                            DATE_FORMAT(RegistrationDate, '%Y-%m-%d') as RegistrationDate 
                            FROM Farmers 
                            ORDER BY LastName, FirstName",
                        ["Harvests"] = @"
                            SELECT 
                            CONCAT(f.FirstName, ' ', f.LastName) as FarmerName,
                            c.CommodityName,
                            h.Quantity,
                            h.PricePerUnit,
                            h.TotalRevenue,
                            DATE_FORMAT(h.HarvestDate, '%Y-%m-%d') as HarvestDate
                            FROM Harvest h 
                            JOIN Farmers f ON h.FarmerID = f.FarmerID
                            JOIN Commodities c ON h.CommodityID = c.CommodityID
                            ORDER BY h.HarvestDate DESC",
                        ["Commodities"] = @"
                            SELECT CommodityName, CommodityType, UnitOfMeasurement 
                            FROM Commodities 
                            ORDER BY CommodityName"
                    };

                case "Farmers by Barangay":
                    return new Dictionary<string, string>
                    {
                        ["Data"] = $@"SELECT FirstName, LastName, Address, ContactInfo, FarmSize, 
                            DATE_FORMAT(RegistrationDate, '%Y-%m-%d') as RegistrationDate 
                            FROM Farmers 
                            WHERE Barangay = '{filter}'
                            ORDER BY LastName, FirstName"
                    };

                case "Harvest Data by Year":
                    return new Dictionary<string, string>
                    {
                        ["Data"] = $@"SELECT 
                            CONCAT(f.FirstName, ' ', f.LastName) as FarmerName,
                            c.CommodityName,
                            h.Quantity,
                            h.PricePerUnit,
                            h.TotalRevenue,
                            DATE_FORMAT(h.HarvestDate, '%Y-%m-%d') as HarvestDate
                            FROM Harvest h 
                            JOIN Farmers f ON h.FarmerID = f.FarmerID
                            JOIN Commodities c ON h.CommodityID = c.CommodityID
                            WHERE YEAR(h.HarvestDate) = '{filter}'
                            ORDER BY h.HarvestDate"
                    };

                case "Commodities":
                    return new Dictionary<string, string>
                    {
                        ["Data"] = @"SELECT CommodityName, CommodityType, UnitOfMeasurement 
                            FROM Commodities 
                            ORDER BY CommodityName"
                    };

                default:
                    return new Dictionary<string, string>();
            }
        }

        private void ExportData(string dataType, string filter, string format, DataGridView grid)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = format == "Excel" ? 
                    "Excel files (*.xlsx)|*.xlsx" : 
                    "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = $"Export_{dataType.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    if (format == "Excel")
                    {
                        ExportToExcel(grid, saveDialog.FileName, dataType);
                    }
                    else
                    {
                        ExportToPdf(grid, saveDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during export: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ExportToExcel(DataGridView grid, string filePath, string dataType)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Export Data");

                        // Write headers
                        for (int i = 0; i < grid.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1].Value = grid.Columns[i].HeaderText;
                            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                        }

                        // Write data
                        for (int row = 0; row < grid.Rows.Count; row++)
                        {
                            for (int col = 0; col < grid.Columns.Count; col++)
                            {
                                worksheet.Cells[row + 2, col + 1].Value = grid.Rows[row].Cells[col].Value;
                            }
                        }

                        worksheet.Cells.AutoFitColumns();
                        package.Save();
                    }

                    MessageBox.Show("Data exported successfully!", "Export Complete",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Export Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private void ExportToPdf(DataGridView grid, string filePath)
        {
            using (var doc = new iTextSharp.text.Document(PageSize.A4.Rotate()))
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                // Add title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var title = new Paragraph("Data Export Report", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20f;
                doc.Add(title);

                // Create the table
                PdfPTable table = new PdfPTable(grid.Columns.Count);
                table.WidthPercentage = 100;

                // Add headers
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                foreach (DataGridViewColumn column in grid.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                    cell.BackgroundColor = new BaseColor(240, 240, 240);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    table.AddCell(cell);
                }

                // Add data
                var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                foreach (DataGridViewRow row in grid.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value?.ToString() ?? "", cellFont));
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfCell.Padding = 4;
                        table.AddCell(pdfCell);
                    }
                }

                doc.Add(table);
                doc.Close();
            }
        }

        private string SplitCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            
            StringBuilder result = new StringBuilder(input[0].ToString());
            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]) && 
                    (char.IsLower(input[i - 1]) || 
                    (i + 1 < input.Length && char.IsLower(input[i + 1]))))
                {
                    result.Append(" ");
                }
                result.Append(input[i]);
            }
            return result.ToString();
        }

        private void UpdateGridTheme(DataGridView grid)
        {
            grid.BackgroundColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White;
            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White,
                ForeColor = isDarkMode ? Color.White : Color.Black,
                SelectionBackColor = isDarkMode ? Color.FromArgb(75, 75, 75) : Color.LightBlue,
                SelectionForeColor = isDarkMode ? Color.White : Color.Black
            };
            grid.RowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White,
                ForeColor = isDarkMode ? Color.White : Color.Black,
                SelectionBackColor = isDarkMode ? Color.FromArgb(75, 75, 75) : Color.LightBlue,
                SelectionForeColor = isDarkMode ? Color.White : Color.Black
            };
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = isDarkMode ? Color.FromArgb(45, 45, 45) : Color.WhiteSmoke,
                ForeColor = isDarkMode ? Color.White : Color.Black,
                SelectionBackColor = isDarkMode ? Color.FromArgb(75, 75, 75) : Color.LightBlue,
                SelectionForeColor = isDarkMode ? Color.White : Color.Black
            };
            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = isDarkMode ? Color.FromArgb(45, 45, 45) : Color.FromArgb(0, 0, 139),
                ForeColor = Color.White,
                SelectionBackColor = isDarkMode ? Color.FromArgb(75, 75, 75) : Color.FromArgb(0, 0, 160),
                Font = new Font("Segoe UI", 9.75F, FontStyle.Bold)
            };
            grid.GridColor = isDarkMode ? Color.FromArgb(60, 60, 60) : Color.LightGray;
            grid.Refresh();
        }
    }
}

    