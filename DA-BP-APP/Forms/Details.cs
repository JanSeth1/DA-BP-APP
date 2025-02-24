using LiveCharts.WinForms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MaterialSkin;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Forms.Integration;

namespace DA_BP_APP
{
    public partial class Details : MaterialForm
    {
        private LiveCharts.WinForms.PieChart revenueChart;
        private LiveCharts.WinForms.PieChart areaChart;
        private Panel panelChartContainer;
        private decimal totalRevenue;
        private decimal totalArea;

        private int farmerID;
        private Form1 mainForm;
        private MaterialSkinManager materialSkinManager;
        private DataGridView harvestDataGridView;
        private Label totalRevenueLabel; // Label to display total revenue
        private Label totalAreaLabel; // Label to display total area harvested
        private TableLayoutPanel statsPanel; // Panel to hold statistics

        public Details(int farmerID, Form1 mainForm)
        {
            InitializeComponent();

            // Ensure panel exists for charts
            panelChartContainer = new Panel();
            panelChartContainer.Dock = DockStyle.Right;
            panelChartContainer.Size = new Size(400, 400);
            this.Controls.Add(panelChartContainer);

            // Initialize charts
            revenueChart = new LiveCharts.WinForms.PieChart();
            areaChart = new LiveCharts.WinForms.PieChart();

            revenueChart.Size = new Size(300, 300);
            revenueChart.Location = new Point(10, 10);
            revenueChart.Visible = true;

            areaChart.Size = new Size(300, 300);
            areaChart.Location = new Point(10, 320);
            areaChart.Visible = true;

            panelChartContainer.Controls.Add(revenueChart);
            panelChartContainer.Controls.Add(areaChart);

            // Attach Form Load event
            this.Load += DetailsForm_Load;
            this.farmerID = farmerID;
            this.mainForm = mainForm;

            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            UpdateTheme();

            // Initialize the DataGridView for harvest details
            harvestDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true
            };

            // Add the DataGridView to the main panel or form
            Controls.Add(harvestDataGridView);

            // Initialize statistics panel
            InitializeStatisticsPanel();

            // Load data
            LoadFarmerDetails();
            LoadHarvestDetails(); // Load harvest details for the selected farmer
            LoadFarmerStatistics(); // Load statistics for the farmer
        }

        private void DetailsForm_Load(object sender, EventArgs e)
        {
            MessageBox.Show($"Revenue: {totalRevenue}, Area: {totalArea}");
            UpdateRevenueChart(totalRevenue);
            UpdateAreaChart(totalArea);
        }

        private void InitializeStatisticsPanel()
        {
            statsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(10),
                Margin = new Padding(0)
            };

            // Add labels for total revenue and area
            totalRevenueLabel = new Label
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Text = "Total Revenue: $0.00" // Default text
            };

            totalAreaLabel = new Label
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Text = "Total Area Harvested: 0 ha" // Default text
            };

            // Add labels to the panel
            statsPanel.Controls.Add(new Label { Text = "Total Revenue:", Font = new Font("Arial", 10, FontStyle.Regular) }, 0, 0);
            statsPanel.Controls.Add(totalRevenueLabel, 1, 0);
            statsPanel.Controls.Add(new Label { Text = "Total Area Harvested:", Font = new Font("Arial", 10, FontStyle.Regular) }, 0, 1);
            statsPanel.Controls.Add(totalAreaLabel, 1, 1);

            // Add the stats panel to the form
            Controls.Add(statsPanel);
        }

        private void InitializeCharts()
        {
            panelChartContainer = new Panel
            {
                Dock = DockStyle.Right,
                Size = new Size(400, 400)
            };
            this.Controls.Add(panelChartContainer);

            revenueChart = new LiveCharts.WinForms.PieChart
            {
                Size = new Size(300, 300),
                Location = new Point(10, 10),
                Visible = true
            };

            areaChart = new LiveCharts.WinForms.PieChart
            {
                Size = new Size(300, 300),
                Location = new Point(10, 320),
                Visible = true
            };

            panelChartContainer.Controls.Add(revenueChart);
            panelChartContainer.Controls.Add(areaChart);
            this.Load += DetailsForm_Load;
        }

        public void UpdateTheme()
        {
            bool isDarkMode = materialSkinManager.Theme == MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = isDarkMode
                ? new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Green200, TextShade.WHITE)
                : new ColorScheme(Primary.BlueGrey200, Primary.BlueGrey300, Primary.BlueGrey100, Accent.Green200, TextShade.BLACK);

            // Update commodities list box colors
            var commoditiesListBox = Controls.Find("commoditiesListBox", true).FirstOrDefault() as CheckedListBox;
            if (commoditiesListBox != null)
            {
                commoditiesListBox.BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White;
                commoditiesListBox.ForeColor = isDarkMode ? Color.White : Color.Black;
            }

            // Update notes panel colors
            var notesPanel = Controls.Find("notesPanel", true).FirstOrDefault() as Panel;
            if (notesPanel != null)
            {
                notesPanel.BackColor = isDarkMode ? Color.FromArgb(45, 45, 45) : Color.White;
                foreach (Control noteContainer in notesPanel.Controls)
                {
                    if (noteContainer is FlowLayoutPanel flowPanel)
                    {
                        foreach (Control note in flowPanel.Controls)
                        {
                            if (note is Panel panel)
                            {
                                panel.BackColor = isDarkMode ? Color.FromArgb(60, 60, 60) : Color.FromArgb(240, 240, 240);
                                foreach (Control label in panel.Controls)
                                {
                                    if (label is MaterialLabel materialLabel)
                                    {
                                        materialLabel.ForeColor = isDarkMode ? Color.White : Color.Black;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void LoadFarmerDetails()
        {
            Controls.Clear(); // Clear existing controls

            // Main Panel
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };
            Controls.Add(mainPanel);

            // Header Label
            var headerLabel = new MaterialLabel
            {
                Text = "Farmer Details",
                Font = new Font("Roboto", 20, FontStyle.Bold),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 60
            };
            mainPanel.Controls.Add(headerLabel);

            // Layout Panel for form fields
            var layoutPanel = CreateFarmerDetailLayout();
            mainPanel.Controls.Add(layoutPanel);

            // Populate Controls with Farmer Data
            PopulateFarmerData(layoutPanel);
        }

        private TableLayoutPanel CreateFarmerDetailLayout()
        {
            bool isDarkMode = materialSkinManager.Theme == MaterialSkinManager.Themes.DARK;
            var layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                Padding = new Padding(10),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); // Label column
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70)); // Input column

            // Add Fields
            AddTextBox(layoutPanel, "First Name:", "txtFirstName");
            AddTextBox(layoutPanel, "Last Name:", "txtLastName");
            AddTextBox(layoutPanel, "Middle Name:", "txtMiddleName");
            AddTextBox(layoutPanel, "Birthdate:", "txtBirthdate");
            AddTextBox(layoutPanel, "Address:", "txtAddress");
            AddTextBox(layoutPanel, "Barangay:", "txtBarangay");
            AddTextBox(layoutPanel, "Contact Info:", "txtContactInfo");
            AddTextBox(layoutPanel, "Farm Size (ha):", "txtFarmSize");
            AddTextBox(layoutPanel, "Registration Date:", "txtRegistrationDate");

            // Add CheckedListBox for Commodities
            layoutPanel.Controls.Add(new MaterialLabel { Text = "Commodities:", AutoSize = true }, 0, layoutPanel.RowCount);
            var commoditiesListBox = new CheckedListBox
            {
                Name = "commoditiesListBox",
                Dock = DockStyle.Fill,
                Height = 150,
                BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.White,
                ForeColor = isDarkMode ? Color.White : Color.Black
            };
            layoutPanel.Controls.Add(commoditiesListBox, 1, layoutPanel.RowCount++);

            // Add Notes Section
            layoutPanel.Controls.Add(new MaterialLabel { Text = "Notes:", AutoSize = true }, 0, layoutPanel.RowCount);
            var notesPanel = new Panel
            {
                Name = "notesPanel",
                Dock = DockStyle.Fill,
                Height = 200,
                AutoScroll = true,
                BackColor = isDarkMode ? Color.FromArgb(45, 45, 45) : Color.White,
                Margin = new Padding(0, 5, 0, 5)
            };
            layoutPanel.Controls.Add(notesPanel, 1, layoutPanel.RowCount++);

            // Add Save Button
            var saveButton = new MaterialButton
            {
                Text = "SAVE",
                Name = "btnSave",
                Dock = DockStyle.Bottom,
                Width = 150,
                Height = 40
            };
            saveButton.Click += btnSave_Click;
            layoutPanel.Controls.Add(saveButton, 1, layoutPanel.RowCount++);

            return layoutPanel;
        }

        private void AddTextBox(TableLayoutPanel layout, string labelText, string controlName)
        {
            // Add label with adjusted positioning
            var label = new MaterialLabel { 
                Text = labelText,
                AutoSize = true,
                Margin = new Padding(5, 15, 0, 0)  // Add top margin to align with textbox
            };

            // Special case for birthdate
            if (controlName == "txtBirthdate")
            {
                label.Margin = new Padding(5, 2, 0, 0);  // Reduced top margin for birthdate label
                var birthdatePicker = new DateTimePicker
                {
                    Name = controlName,
                    Format = DateTimePickerFormat.Short,
                    Size = new Size(400, 25),
                    ShowCheckBox = true,  // Allow null values
                    Checked = false,      // Default to unchecked
                    BackColor = materialSkinManager.Theme == MaterialSkinManager.Themes.DARK ? Color.FromArgb(50, 50, 50) : Color.White,
                    ForeColor = materialSkinManager.Theme == MaterialSkinManager.Themes.DARK ? Color.White : Color.Black,
                    Margin = new Padding(0, 0, 0, 10)  // Add bottom margin
                };
                layout.Controls.Add(label, 0, layout.RowCount);
                layout.Controls.Add(birthdatePicker, 1, layout.RowCount++);
            }
            else
            {
                layout.Controls.Add(label, 0, layout.RowCount);
                layout.Controls.Add(new MaterialTextBox
                {
                    Name = controlName,
                    Hint = $"Enter {labelText}",
                    Size = new Size(400, 50)
                }, 1, layout.RowCount++);
            }
        }

        private void AddComboBox(TableLayoutPanel layout, string labelText, string controlName)
        {
            layout.Controls.Add(new MaterialLabel { Text = labelText, AutoSize = true }, 0, layout.RowCount);
            layout.Controls.Add(new MaterialComboBox
            {
                Name = controlName,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill,
                AutoSize = false,
                Size = new Size(400, 50)
            }, 1, layout.RowCount++);
        }

        private void PopulateFarmerData(TableLayoutPanel layoutPanel)
        {
            bool isDarkMode = materialSkinManager.Theme == MaterialSkinManager.Themes.DARK;
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();

                    // Query to get farmer details and total farm size from harvest records
                    string farmerQuery = @"
SELECT f.FarmerID, f.FirstName, f.LastName, f.MiddleName, f.Address, f.Barangay, f.ContactInfo,
       CASE 
           WHEN (SELECT SUM(CASE WHEN TotalArea IS NULL THEN 0 ELSE TotalArea END) 
                FROM harvest WHERE FarmerID = f.FarmerID) IS NULL THEN 0
           ELSE (SELECT SUM(CASE WHEN TotalArea IS NULL THEN 0 ELSE TotalArea END) 
                FROM harvest WHERE FarmerID = f.FarmerID)
       END as TotalFarmSize,
       DATE_FORMAT(f.RegistrationDate, '%Y-%m-%d') AS RegistrationDate,
       DATE_FORMAT(f.Birthdate, '%Y-%m-%d') AS Birthdate,
       CASE COALESCE(f.Crop, 0)
           WHEN 1 THEN 'Coconut'
           WHEN 2 THEN 'Cacao'
           WHEN 3 THEN 'Rice'
           WHEN 4 THEN 'Corn'
           WHEN 5 THEN 'Live Stock'
           WHEN 6 THEN 'Fish'
           WHEN 7 THEN 'High Value Crops'
           WHEN 8 THEN 'Industrial Crop'
           ELSE NULL
       END AS Crop
FROM Farmers f
WHERE f.FarmerID = @FarmerID";

                    MySqlCommand cmd = new MySqlCommand(farmerQuery, connection);
                    cmd.Parameters.AddWithValue("@FarmerID", farmerID);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    string farmerCrop = null;

                    if (reader.Read())
                    {
                        // Populate basic farmer details
                        SetControlValue(layoutPanel, "txtFirstName", reader["FirstName"].ToString());
                        SetControlValue(layoutPanel, "txtLastName", reader["LastName"].ToString());
                        SetControlValue(layoutPanel, "txtMiddleName", reader["MiddleName"]?.ToString() ?? "");
                        SetControlValue(layoutPanel, "txtBirthdate", reader["Birthdate"]?.ToString() ?? "");
                        SetControlValue(layoutPanel, "txtAddress", reader["Address"].ToString());
                        SetControlValue(layoutPanel, "txtBarangay", reader["Barangay"].ToString());
                        SetControlValue(layoutPanel, "txtContactInfo", reader["ContactInfo"].ToString());
                        SetControlValue(layoutPanel, "txtFarmSize", reader["TotalFarmSize"].ToString());
                        SetControlValue(layoutPanel, "txtRegistrationDate", reader["RegistrationDate"].ToString());

                        farmerCrop = reader["Crop"]?.ToString();
                    }
                    reader.Close();

                    // Get selected commodities first
                    var selectedCommodities = new HashSet<string>();
                    string farmerCommoditiesQuery = @"
SELECT c.CommodityName 
FROM FarmerCommodities fc
JOIN Commodities c ON fc.CommodityID = c.CommodityID
WHERE fc.FarmerID = @FarmerID";

                    MySqlCommand farmerCommoditiesCmd = new MySqlCommand(farmerCommoditiesQuery, connection);
                    farmerCommoditiesCmd.Parameters.AddWithValue("@FarmerID", farmerID);
                    MySqlDataReader farmerCommoditiesReader = farmerCommoditiesCmd.ExecuteReader();

                    while (farmerCommoditiesReader.Read())
                    {
                        selectedCommodities.Add(farmerCommoditiesReader["CommodityName"].ToString());
                    }
                    farmerCommoditiesReader.Close();

                    // Include the crop from the Farmers table if not already added
                    if (!string.IsNullOrEmpty(farmerCrop))
                    {
                        selectedCommodities.Add(farmerCrop);
                    }

                    // Query to get commodities with their areas
                    string commoditiesQuery = @"
SELECT 
    c.CommodityName,
    COALESCE(h.TotalArea, 0) as Area
FROM Commodities c
LEFT JOIN FarmerCommodities fc ON fc.CommodityID = c.CommodityID AND fc.FarmerID = @FarmerID
LEFT JOIN (
    SELECT 
        FarmerID,
        CommodityID,
        SUM(TotalArea) as TotalArea
    FROM harvest 
    WHERE FarmerID = @FarmerID
    GROUP BY FarmerID, CommodityID
) h ON h.FarmerID = fc.FarmerID AND h.CommodityID = c.CommodityID
ORDER BY c.CommodityName";

                    MySqlCommand commoditiesCmd = new MySqlCommand(commoditiesQuery, connection);
                    commoditiesCmd.Parameters.AddWithValue("@FarmerID", farmerID);
                    MySqlDataReader commoditiesReader = commoditiesCmd.ExecuteReader();

                    var commoditiesListBox = layoutPanel.Controls.Find("commoditiesListBox", true).FirstOrDefault() as CheckedListBox;
                    if (commoditiesListBox != null)
                    {
                        commoditiesListBox.Items.Clear();
                        decimal totalArea = 0;

                        while (commoditiesReader.Read())
                        {
                            string commodityName = commoditiesReader["CommodityName"].ToString();
                            decimal area = Convert.ToDecimal(commoditiesReader["Area"]);

                            if (selectedCommodities.Contains(commodityName))
                            {
                                totalArea += area;
                            }

                            string displayText = area > 0 
                                ? $"{commodityName} ({area} ha)"
                                : commodityName;

                            commoditiesListBox.Items.Add(displayText, selectedCommodities.Contains(commodityName));
                        }
                        commoditiesReader.Close();

                        // Update the total farm size to show it's calculated from harvest records
                        SetControlValue(layoutPanel, "txtFarmSize", $"{totalArea} (Total from harvests)");
                    }

                    // Load and display notes
                    string notesQuery = @"
SELECT Note, DateAdded 
FROM FarmerNotes 
WHERE FarmerID = @FarmerID 
ORDER BY DateAdded DESC";

                    MySqlCommand notesCmd = new MySqlCommand(notesQuery, connection);
                    notesCmd.Parameters.AddWithValue("@FarmerID", farmerID);
                    MySqlDataReader notesReader = notesCmd.ExecuteReader();

                    var notesPanel = layoutPanel.Controls.Find("notesPanel", true).FirstOrDefault() as Panel;
                    if (notesPanel != null)
                    {
                        notesPanel.Controls.Clear();
                        var notesFlowPanel = new FlowLayoutPanel
                        {
                            Dock = DockStyle.Fill,
                            AutoScroll = true,
                            FlowDirection = FlowDirection.TopDown,
                            WrapContents = false,
                            Width = notesPanel.Width - 20
                        };
                        notesPanel.Controls.Add(notesFlowPanel);

                        while (notesReader.Read())
                        {
                            var noteContainer = new Panel
                            {
                                AutoSize = true,
                                Margin = new Padding(5),
                                Width = notesFlowPanel.Width - 25,
                                BackColor = isDarkMode ? Color.FromArgb(60, 60, 60) : Color.FromArgb(240, 240, 240)
                            };

                            var dateLabel = new MaterialLabel
                            {
                                Text = Convert.ToDateTime(notesReader["DateAdded"]).ToString("MM/dd/yyyy hh:mm:ss tt"),
                                AutoSize = true,
                                Margin = new Padding(5, 5, 5, 0),
                                Font = new Font("Roboto", 9, FontStyle.Bold),
                                ForeColor = isDarkMode ? Color.White : Color.Black
                            };

                            var noteLabel = new MaterialLabel
                            {
                                Text = notesReader["Note"].ToString(),
                                AutoSize = true,
                                Margin = new Padding(5, 0, 5, 5),
                                Width = noteContainer.Width - 10,
                                ForeColor = isDarkMode ? Color.White : Color.Black
                            };

                            noteContainer.Controls.Add(dateLabel);
                            noteContainer.Controls.Add(noteLabel);
                            noteLabel.Location = new Point(5, dateLabel.Bottom + 5);
                            noteContainer.Height = dateLabel.Height + noteLabel.Height + 15;

                            notesFlowPanel.Controls.Add(noteContainer);
                        }
                    }
                    notesReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading farmer details or commodities: " + ex.Message);
                }
            }
        }

        private void SetControlValue(TableLayoutPanel layoutPanel, string controlName, string value)
        {
            var control = layoutPanel.Controls.OfType<Control>().FirstOrDefault(c => c.Name == controlName);
            if (control is MaterialTextBox textBox)
            {
                textBox.Text = value;
            }
            else if (control is MaterialMultiLineTextBox multiTextBox)
            {
                multiTextBox.Text = value;
            }
            else if (control is DateTimePicker datePicker && controlName == "txtBirthdate")
            {
                if (string.IsNullOrEmpty(value) || value == "DBNull")
                {
                    datePicker.Checked = false;
                }
                else
                {
                    datePicker.Checked = true;
                    if (DateTime.TryParse(value, out DateTime date))
                    {
                        datePicker.Value = date;
                    }
                    else
                    {
                        datePicker.Checked = false;
                    }
                }
            }
        }

        private void SaveFarmerDetails()
        {
            // Confirmation dialog
            var confirmationResult = MessageBox.Show("Are you sure you want to save the farmer details?", 
                                                      "Confirm Save", 
                                                      MessageBoxButtons.YesNo, 
                                                      MessageBoxIcon.Question);

            if (confirmationResult == DialogResult.No)
            {
                return; // Exit the method if the user chooses not to save
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Get the CheckedListBox and first selected commodity
                            var commoditiesListBox = Controls.Find("commoditiesListBox", true).FirstOrDefault() as CheckedListBox;
                            string firstSelectedCommodity = null;
                            if (commoditiesListBox != null && commoditiesListBox.CheckedItems.Count > 0)
                            {
                                string commodityName = commoditiesListBox.CheckedItems[0].ToString();
                                // Extract just the commodity name if it includes area info
                                int bracketIndex = commodityName.IndexOf('(');
                                if (bracketIndex > 0)
                                {
                                    commodityName = commodityName.Substring(0, bracketIndex).Trim();
                                }
                                firstSelectedCommodity = commodityName;
                            }

                            // Update farmer details including the Crop field
                            string updateFarmerQuery = @"
                                UPDATE Farmers
                                SET FirstName = @FirstName,
                                    LastName = @LastName,
                                    MiddleName = @MiddleName,
                                    Birthdate = @Birthdate,
                                    Address = @Address,
                                    Barangay = @Barangay,
                                    ContactInfo = @ContactInfo,
                                    FarmSize = @FarmSize,
                                    RegistrationDate = @RegistrationDate,
                                    Crop = (SELECT CommodityID FROM Commodities WHERE CommodityName = @FirstCommodity)
                                WHERE FarmerID = @FarmerID";

                            MySqlCommand cmd = new MySqlCommand(updateFarmerQuery, connection, transaction);

                            // Get farm size value and parse it correctly
                            string farmSizeText = GetControlValue("txtFarmSize");
                            decimal farmSize = 0;
                            decimal.TryParse(farmSizeText, out farmSize);

                            cmd.Parameters.AddWithValue("@FirstName", GetControlValue("txtFirstName"));
                            cmd.Parameters.AddWithValue("@LastName", GetControlValue("txtLastName"));
                            cmd.Parameters.AddWithValue("@MiddleName", GetControlValue("txtMiddleName"));
                            string birthdate = GetControlValue("txtBirthdate");
                            cmd.Parameters.AddWithValue("@Birthdate", string.IsNullOrEmpty(birthdate) ? DBNull.Value : (object)DateTime.Parse(birthdate));
                            cmd.Parameters.AddWithValue("@Address", GetControlValue("txtAddress"));
                            cmd.Parameters.AddWithValue("@Barangay", GetControlValue("txtBarangay"));
                            cmd.Parameters.AddWithValue("@ContactInfo", GetControlValue("txtContactInfo"));
                            cmd.Parameters.AddWithValue("@FarmSize", farmSize);
                            cmd.Parameters.AddWithValue("@RegistrationDate", GetControlValue("txtRegistrationDate"));
                            cmd.Parameters.AddWithValue("@FarmerID", farmerID);
                            cmd.Parameters.AddWithValue("@FirstCommodity", firstSelectedCommodity ?? (object)DBNull.Value);

                            cmd.ExecuteNonQuery();

                            // Clear existing commodities for this farmer
                            string deleteCommoditiesQuery = "DELETE FROM FarmerCommodities WHERE FarmerID = @FarmerID";
                            cmd = new MySqlCommand(deleteCommoditiesQuery, connection, transaction);
                            cmd.Parameters.AddWithValue("@FarmerID", farmerID);
                            cmd.ExecuteNonQuery();

                            // Reuse the commoditiesListBox variable we already have
                            if (commoditiesListBox != null)
                            {
                                foreach (var item in commoditiesListBox.CheckedItems)
                                {
                                    string commodityName = item.ToString();
                                    // Extract just the commodity name if it includes area info
                                    int bracketIndex = commodityName.IndexOf('(');
                                    if (bracketIndex > 0)
                                    {
                                        commodityName = commodityName.Substring(0, bracketIndex).Trim();
                                    }

                                    // Get commodity ID
                                    string getCommodityIdQuery = "SELECT CommodityID FROM Commodities WHERE CommodityName = @CommodityName";
                                    cmd = new MySqlCommand(getCommodityIdQuery, connection, transaction);
                                    cmd.Parameters.AddWithValue("@CommodityName", commodityName);
                                    object result = cmd.ExecuteScalar();

                                    if (result != null)
                                    {
                                        int commodityId = Convert.ToInt32(result);

                                        // Insert into FarmerCommodities
                                        string insertCommodityQuery = "INSERT INTO FarmerCommodities (FarmerID, CommodityID) VALUES (@FarmerID, @CommodityID)";
                                        cmd = new MySqlCommand(insertCommodityQuery, connection, transaction);
                                        cmd.Parameters.AddWithValue("@FarmerID", farmerID);
                                        cmd.Parameters.AddWithValue("@CommodityID", commodityId);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            transaction.Commit();
                            mainForm.RefreshFarmersData();
                            MessageBox.Show("Farmer details saved successfully.");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving farmer details: " + ex.Message);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFarmerDetails();
        }

        private string GetControlValue(string controlName)
        {
            var control = Controls.Find(controlName, true).FirstOrDefault();
            if (control is MaterialTextBox textBox)
            {
                // For farm size, extract only the numeric value
                if (controlName == "txtFarmSize")
                {
                    string text = textBox.Text;
                    // Remove the "(Total from harvests)" text if present
                    int bracketIndex = text.IndexOf('(');
                    if (bracketIndex > 0)
                    {
                        text = text.Substring(0, bracketIndex).Trim();
                    }
                    // Try to parse the numeric value
                    if (decimal.TryParse(text, out decimal value))
                    {
                        return value.ToString();
                    }
                    return "0"; // Return 0 if parsing fails
                }
                return textBox.Text;
            }
            else if (control is MaterialMultiLineTextBox multiTextBox)
            {
                return multiTextBox.Text;
            }
            else if (control is DateTimePicker datePicker && controlName == "txtBirthdate")
            {
                return datePicker.Checked ? datePicker.Value.ToString("yyyy-MM-dd") : "";
            }
            return string.Empty;
        }

        private void LoadHarvestDetails()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            h.HarvestID,
                            c.CommodityName,
                            h.TotalArea,
                            h.HarvestDate
                        FROM Harvest h
                        JOIN FarmerCommodities fc ON h.CommodityID = fc.CommodityID
                        JOIN Commodities c ON h.CommodityID = c.CommodityID
                        WHERE fc.FarmerID = @FarmerID
                        ORDER BY h.HarvestDate DESC";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FarmerID", farmerID);

                    DataTable harvestData = new DataTable();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(harvestData); // Fill the DataTable with harvest data
                    }

                    // Bind the DataTable to the DataGridView
                    harvestDataGridView.DataSource = harvestData;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading harvest details: " + ex.Message);
                }
            }
        }

        private void LoadFarmerStatistics()
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    string revenueQuery = "SELECT COALESCE(SUM(h.TotalRevenue), 0) AS TotalRevenue FROM Harvest h JOIN FarmerCommodities fc ON h.CommodityID = fc.CommodityID WHERE fc.FarmerID = @FarmerID";
                    MySqlCommand revenueCmd = new MySqlCommand(revenueQuery, connection);
                    revenueCmd.Parameters.AddWithValue("@FarmerID", farmerID);
                    totalRevenue = Convert.ToDecimal(revenueCmd.ExecuteScalar() ?? 0);

                    string areaQuery = "SELECT COALESCE(SUM(h.TotalArea), 0) AS TotalArea FROM Harvest h JOIN FarmerCommodities fc ON h.CommodityID = fc.CommodityID WHERE fc.FarmerID = @FarmerID";
                    MySqlCommand areaCmd = new MySqlCommand(areaQuery, connection);
                    areaCmd.Parameters.AddWithValue("@FarmerID", farmerID);
                    totalArea = Convert.ToDecimal(areaCmd.ExecuteScalar() ?? 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading farmer statistics: " + ex.Message);
                }
            }
        }

        private void UpdateRevenueChart(decimal totalRevenue)
        {
            if (revenueChart == null) return;
            revenueChart.Series.Clear();
            revenueChart.Visible = true;
            revenueChart.Series.Add(new PieSeries { Title = "Total Revenue", Values = new ChartValues<decimal> { totalRevenue > 0 ? totalRevenue : 1000 }, DataLabels = true });
            revenueChart.LegendLocation = LegendLocation.Bottom;
            revenueChart.Update();
            revenueChart.Refresh();
        }

        private void UpdateAreaChart(decimal totalArea)
        {
            if (areaChart == null) return;
            areaChart.Series.Clear();
            areaChart.Visible = true;
            areaChart.Series.Add(new PieSeries { Title = "Total Area Harvested", Values = new ChartValues<decimal> { totalArea > 0 ? totalArea : 10 }, DataLabels = true });
            areaChart.LegendLocation = LegendLocation.Bottom;
            areaChart.Update();
            areaChart.Refresh();
        }
    }
}
