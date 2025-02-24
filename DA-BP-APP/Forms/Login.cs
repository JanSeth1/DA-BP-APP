using MaterialSkin.Controls;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MaterialSkin;

namespace DA_BP_APP
{
    public partial class Login : MaterialForm
    {
        public Login()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Green200, TextShade.WHITE);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            materialButton1.Enabled = false;

            string username = txt_username.Text;
            string password = txt_password.Text;


            try
            {
                using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    string stm = "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password AND IsActive = 1";

                    using (MySqlCommand cmd = new MySqlCommand(stm, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null)
                        {
                            string userRole = result.ToString();
                            MessageBox.Show("Login successful!");
                            
                            Invoke((MethodInvoker)delegate
                            {
                                Hide();
                                Form1 form = new Form1(userRole); // Pass the role to Form1
                                form.ShowDialog();
                                this.Close();
                            });
                        }
                        else
                        {
                            MessageBox.Show("Login failed. Invalid credentials.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"MySQL Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            materialButton1.Enabled = true;
        }

        private void chkbox_show_CheckedChanged(object sender, EventArgs e)
        {
            txt_password.UseSystemPasswordChar = !chkbox_show.Checked;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }
    }
}
