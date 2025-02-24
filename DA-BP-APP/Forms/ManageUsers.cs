using ADGV;
using MaterialSkin;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DA_BP_APP
{
    public partial class ManageUsers : MaterialForm
    {
        public ManageUsers()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Green200, TextShade.WHITE);
            Load += ManageUsers_Load;
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            LoadUsers();
            ConfigureDataGridView();
        }
        private void LoadUsers()
        {
            string query = "SELECT UserID, Username, Password, FirstName, LastName, Email, Role, IsActive FROM Users";

            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    usersGrid.DataSource = dt;

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

            }
        }

        private void ConfigureDataGridView()
        {
            usersGrid.DefaultCellStyle.BackColor = Color.White;
            usersGrid.DefaultCellStyle.ForeColor = Color.Black;
            usersGrid.DefaultCellStyle.SelectionBackColor = Color.Blue;
            usersGrid.DefaultCellStyle.SelectionForeColor = Color.White;

            usersGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            usersGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            usersGrid.ColumnHeadersDefaultCellStyle.Font = new Font(usersGrid.Font, System.Drawing.FontStyle.Bold);
        }

        //private void materialButton1_Click(object sender, EventArgs e)
        //{
        //    AddUser userForm = new AddUser();
        //    userForm.Show();
        //}

        private void materialButton2_Click(object sender, EventArgs e)
        {
            UpdateUser userForm = new UpdateUser();
            userForm.Show();
        }
    }
}