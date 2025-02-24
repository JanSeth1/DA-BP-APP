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

namespace DA_BP_APP
{
    public partial class Admin : MaterialForm
    {
        public Admin()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Green200, TextShade.WHITE);
            timer1.Start();
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void materialFloatingActionButton2_Click(object sender, EventArgs e)
        {
            ManageUsers userForm = new ManageUsers();
            userForm.Show();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            time_txt.Text = DateTime.Now.ToLongTimeString();
            date_txt.Text = DateTime.Now.ToLongDateString();
        }

        private void Home_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            time_txt.Text = DateTime.Now.ToLongTimeString();
            date_txt.Text = DateTime.Now.ToLongDateString();
        }

        private void materialFloatingActionButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
