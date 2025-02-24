using MaterialSkin;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using DA_BP_APP.Models;

namespace DA_BP_APP
{
    public partial class DataGridFarmers : MaterialForm
    {

        public DataGridFarmers()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Green200, TextShade.WHITE);

            // Ensure controls are created before data binding
            if (!this.DesignMode) // Check if not in design mode to avoid designer errors
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM farmers", connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        // ... (Your CRUD operations)
        // ... (Your button click handlers)
    }
}
