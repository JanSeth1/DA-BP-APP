namespace DA_BP_APP
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.Home = new System.Windows.Forms.TabPage();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxCommodities = new System.Windows.Forms.ComboBox();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.cartesianChart = new LiveCharts.WinForms.CartesianChart();
            this.materialFloatingActionButton1 = new MaterialSkin.Controls.MaterialFloatingActionButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.materialCard2 = new MaterialSkin.Controls.MaterialCard();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.txt_farmerCount = new MaterialSkin.Controls.MaterialLabel();
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.materialFloatingActionButton2 = new MaterialSkin.Controls.MaterialFloatingActionButton();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.materialCard6 = new MaterialSkin.Controls.MaterialCard();
            this.time_txt = new System.Windows.Forms.Label();
            this.materialFloatingActionButton6 = new MaterialSkin.Controls.MaterialFloatingActionButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.materialCard5 = new MaterialSkin.Controls.MaterialCard();
            this.date_txt = new System.Windows.Forms.Label();
            this.materialFloatingActionButton5 = new MaterialSkin.Controls.MaterialFloatingActionButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.materialCard3 = new MaterialSkin.Controls.MaterialCard();
            this.materialFloatingActionButton3 = new MaterialSkin.Controls.MaterialFloatingActionButton();
            this.Barangays = new System.Windows.Forms.TabPage();
            this.farmersGrid = new ADGV.AdvancedDataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.Prediction = new System.Windows.Forms.TabPage();
            this.Settings = new System.Windows.Forms.TabPage();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.barangay1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barangay2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barangay3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.materialTabControl1.SuspendLayout();
            this.Home.SuspendLayout();
            this.materialCard1.SuspendLayout();
            this.materialCard2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.materialCard6.SuspendLayout();
            this.materialCard5.SuspendLayout();
            this.materialCard3.SuspendLayout();
            this.Barangays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.farmersGrid)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.AllowDrop = true;
            this.materialTabControl1.Controls.Add(this.Home);
            this.materialTabControl1.Controls.Add(this.Barangays);
            this.materialTabControl1.Controls.Add(this.Prediction);
            this.materialTabControl1.Controls.Add(this.Settings);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialTabControl1.ImageList = this.imageList1;
            this.materialTabControl1.Location = new System.Drawing.Point(3, 64);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Multiline = true;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.Padding = new System.Drawing.Point(10, 10);
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(1914, 1013);
            this.materialTabControl1.TabIndex = 0;
            // 
            // Home
            // 
            this.Home.AllowDrop = true;
            this.Home.Controls.Add(this.materialCard1);
            this.Home.Controls.Add(this.materialCard2);
            this.Home.Controls.Add(this.label10);
            this.Home.Controls.Add(this.label9);
            this.Home.Controls.Add(this.pictureBox1);
            this.Home.Controls.Add(this.materialCard6);
            this.Home.Controls.Add(this.materialCard5);
            this.Home.Controls.Add(this.label6);
            this.Home.Controls.Add(this.label5);
            this.Home.Controls.Add(this.materialCard3);
            this.Home.ImageKey = "home.png";
            this.Home.Location = new System.Drawing.Point(4, 45);
            this.Home.Name = "Home";
            this.Home.Padding = new System.Windows.Forms.Padding(3);
            this.Home.Size = new System.Drawing.Size(1906, 964);
            this.Home.TabIndex = 0;
            this.Home.Text = "Home";
            this.Home.UseVisualStyleBackColor = true;
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.label3);
            this.materialCard1.Controls.Add(this.comboBoxCommodities);
            this.materialCard1.Controls.Add(this.materialLabel2);
            this.materialCard1.Controls.Add(this.cartesianChart);
            this.materialCard1.Controls.Add(this.materialFloatingActionButton1);
            this.materialCard1.Controls.Add(this.label1);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(24, 554);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(1795, 328);
            this.materialCard1.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(38, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Commodities";
            // 
            // comboBoxCommodities
            // 
            this.comboBoxCommodities.FormattingEnabled = true;
            this.comboBoxCommodities.Items.AddRange(new object[] {
            "Rice(Palay)",
            "Corn",
            "Coconut",
            "Sugarcane",
            "Banana",
            "Pineapple",
            "Mango",
            "Cacao"});
            this.comboBoxCommodities.Location = new System.Drawing.Point(42, 50);
            this.comboBoxCommodities.Name = "comboBoxCommodities";
            this.comboBoxCommodities.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCommodities.TabIndex = 3;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(39, 50);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(1, 0);
            this.materialLabel2.TabIndex = 2;
            // 
            // cartesianChart
            // 
            this.cartesianChart.Location = new System.Drawing.Point(17, 84);
            this.cartesianChart.Name = "cartesianChart";
            this.cartesianChart.Size = new System.Drawing.Size(1729, 227);
            this.cartesianChart.TabIndex = 0;
            this.cartesianChart.Text = "cartesianChart2";
            // 
            // materialFloatingActionButton1
            // 
            this.materialFloatingActionButton1.Depth = 0;
            this.materialFloatingActionButton1.Icon = ((System.Drawing.Image)(resources.GetObject("materialFloatingActionButton1.Icon")));
            this.materialFloatingActionButton1.ImageIndex = 0;
            this.materialFloatingActionButton1.ImageList = this.imageList2;
            this.materialFloatingActionButton1.Location = new System.Drawing.Point(1690, 22);
            this.materialFloatingActionButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFloatingActionButton1.Name = "materialFloatingActionButton1";
            this.materialFloatingActionButton1.Size = new System.Drawing.Size(56, 56);
            this.materialFloatingActionButton1.TabIndex = 1;
            this.materialFloatingActionButton1.Text = "materialFloatingActionButton1";
            this.materialFloatingActionButton1.UseVisualStyleBackColor = false;
            this.materialFloatingActionButton1.Click += new System.EventHandler(this.materialFloatingActionButton1_Click_1);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "enter.png");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(40, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 23);
            this.label1.TabIndex = 0;
            // 
            // materialCard2
            // 
            this.materialCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard2.Controls.Add(this.materialLabel1);
            this.materialCard2.Controls.Add(this.txt_farmerCount);
            this.materialCard2.Controls.Add(this.cartesianChart1);
            this.materialCard2.Controls.Add(this.materialFloatingActionButton2);
            this.materialCard2.Depth = 0;
            this.materialCard2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard2.Location = new System.Drawing.Point(24, 137);
            this.materialCard2.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard2.Name = "materialCard2";
            this.materialCard2.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard2.Size = new System.Drawing.Size(1795, 389);
            this.materialCard2.TabIndex = 18;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(39, 13);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(137, 19);
            this.materialLabel1.TabIndex = 3;
            this.materialLabel1.Text = "Registered Farmers";
            // 
            // txt_farmerCount
            // 
            this.txt_farmerCount.AutoSize = true;
            this.txt_farmerCount.Depth = 0;
            this.txt_farmerCount.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txt_farmerCount.Location = new System.Drawing.Point(39, 50);
            this.txt_farmerCount.MouseState = MaterialSkin.MouseState.HOVER;
            this.txt_farmerCount.Name = "txt_farmerCount";
            this.txt_farmerCount.Size = new System.Drawing.Size(28, 19);
            this.txt_farmerCount.TabIndex = 2;
            this.txt_farmerCount.Text = "200";
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(17, 84);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(1729, 288);
            this.cartesianChart1.TabIndex = 0;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // materialFloatingActionButton2
            // 
            this.materialFloatingActionButton2.Depth = 0;
            this.materialFloatingActionButton2.Icon = ((System.Drawing.Image)(resources.GetObject("materialFloatingActionButton2.Icon")));
            this.materialFloatingActionButton2.ImageIndex = 0;
            this.materialFloatingActionButton2.ImageList = this.imageList2;
            this.materialFloatingActionButton2.Location = new System.Drawing.Point(1690, 30);
            this.materialFloatingActionButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFloatingActionButton2.Name = "materialFloatingActionButton2";
            this.materialFloatingActionButton2.Size = new System.Drawing.Size(56, 56);
            this.materialFloatingActionButton2.TabIndex = 1;
            this.materialFloatingActionButton2.Text = "materialFloatingActionButton2";
            this.materialFloatingActionButton2.UseVisualStyleBackColor = false;
            this.materialFloatingActionButton2.Click += new System.EventHandler(this.materialFloatingActionButton2_Click_1);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(138, 945);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(246, 20);
            this.label10.TabIndex = 17;
            this.label10.Text = "BROOKE\'S POINT PALAWAN";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(138, 917);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(393, 20);
            this.label9.TabIndex = 16;
            this.label9.Text = "OFFICE OF THE MUNICIPAL AGRICULTURIST";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DA_BP_APP.Properties.Resources.OMA_Logo__Option_4_;
            this.pictureBox1.Location = new System.Drawing.Point(6, 889);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 96);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // materialCard6
            // 
            this.materialCard6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard6.Controls.Add(this.time_txt);
            this.materialCard6.Controls.Add(this.materialFloatingActionButton6);
            this.materialCard6.Depth = 0;
            this.materialCard6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard6.Location = new System.Drawing.Point(535, 22);
            this.materialCard6.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard6.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard6.Name = "materialCard6";
            this.materialCard6.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard6.Size = new System.Drawing.Size(172, 58);
            this.materialCard6.TabIndex = 13;
            // 
            // time_txt
            // 
            this.time_txt.AutoSize = true;
            this.time_txt.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.time_txt.Location = new System.Drawing.Point(61, 19);
            this.time_txt.Name = "time_txt";
            this.time_txt.Size = new System.Drawing.Size(47, 19);
            this.time_txt.TabIndex = 12;
            this.time_txt.Text = "TIME";
            // 
            // materialFloatingActionButton6
            // 
            this.materialFloatingActionButton6.Depth = 0;
            this.materialFloatingActionButton6.Icon = ((System.Drawing.Image)(resources.GetObject("materialFloatingActionButton6.Icon")));
            this.materialFloatingActionButton6.ImageKey = "clock.png";
            this.materialFloatingActionButton6.ImageList = this.imageList1;
            this.materialFloatingActionButton6.Location = new System.Drawing.Point(0, 0);
            this.materialFloatingActionButton6.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFloatingActionButton6.Name = "materialFloatingActionButton6";
            this.materialFloatingActionButton6.Size = new System.Drawing.Size(56, 56);
            this.materialFloatingActionButton6.TabIndex = 5;
            this.materialFloatingActionButton6.Text = "materialFloatingActionButton6";
            this.materialFloatingActionButton6.UseVisualStyleBackColor = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "chart-histogram.png");
            this.imageList1.Images.SetKeyName(1, "home.png");
            this.imageList1.Images.SetKeyName(2, "notification.png");
            this.imageList1.Images.SetKeyName(3, "user.png");
            this.imageList1.Images.SetKeyName(4, "users-alt.png");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "cogwheel.png");
            this.imageList1.Images.SetKeyName(7, "setting.png");
            this.imageList1.Images.SetKeyName(8, "map.png");
            this.imageList1.Images.SetKeyName(9, "calendar.png");
            this.imageList1.Images.SetKeyName(10, "clock.png");
            // 
            // materialCard5
            // 
            this.materialCard5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard5.Controls.Add(this.date_txt);
            this.materialCard5.Controls.Add(this.materialFloatingActionButton5);
            this.materialCard5.Depth = 0;
            this.materialCard5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard5.Location = new System.Drawing.Point(254, 22);
            this.materialCard5.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard5.Name = "materialCard5";
            this.materialCard5.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard5.Size = new System.Drawing.Size(253, 58);
            this.materialCard5.TabIndex = 11;
            // 
            // date_txt
            // 
            this.date_txt.AutoSize = true;
            this.date_txt.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date_txt.Location = new System.Drawing.Point(63, 19);
            this.date_txt.Name = "date_txt";
            this.date_txt.Size = new System.Drawing.Size(49, 19);
            this.date_txt.TabIndex = 12;
            this.date_txt.Text = "DATE";
            // 
            // materialFloatingActionButton5
            // 
            this.materialFloatingActionButton5.Depth = 0;
            this.materialFloatingActionButton5.Icon = ((System.Drawing.Image)(resources.GetObject("materialFloatingActionButton5.Icon")));
            this.materialFloatingActionButton5.ImageKey = "calendar.png";
            this.materialFloatingActionButton5.ImageList = this.imageList1;
            this.materialFloatingActionButton5.Location = new System.Drawing.Point(0, 0);
            this.materialFloatingActionButton5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFloatingActionButton5.Name = "materialFloatingActionButton5";
            this.materialFloatingActionButton5.Size = new System.Drawing.Size(56, 56);
            this.materialFloatingActionButton5.TabIndex = 5;
            this.materialFloatingActionButton5.Text = "materialFloatingActionButton5";
            this.materialFloatingActionButton5.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(85, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "USER:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(156, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "ADMIN";
            // 
            // materialCard3
            // 
            this.materialCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard3.Controls.Add(this.materialFloatingActionButton3);
            this.materialCard3.Depth = 0;
            this.materialCard3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard3.Location = new System.Drawing.Point(24, 22);
            this.materialCard3.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard3.Name = "materialCard3";
            this.materialCard3.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard3.Size = new System.Drawing.Size(211, 58);
            this.materialCard3.TabIndex = 4;
            // 
            // materialFloatingActionButton3
            // 
            this.materialFloatingActionButton3.Depth = 0;
            this.materialFloatingActionButton3.Icon = ((System.Drawing.Image)(resources.GetObject("materialFloatingActionButton3.Icon")));
            this.materialFloatingActionButton3.ImageIndex = 3;
            this.materialFloatingActionButton3.ImageList = this.imageList1;
            this.materialFloatingActionButton3.Location = new System.Drawing.Point(0, 0);
            this.materialFloatingActionButton3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFloatingActionButton3.Name = "materialFloatingActionButton3";
            this.materialFloatingActionButton3.Size = new System.Drawing.Size(56, 56);
            this.materialFloatingActionButton3.TabIndex = 5;
            this.materialFloatingActionButton3.Text = "materialFloatingActionButton3";
            this.materialFloatingActionButton3.UseVisualStyleBackColor = false;
            // 
            // Barangays
            // 
            this.Barangays.Controls.Add(this.farmersGrid);
            this.Barangays.Controls.Add(this.txtSearch);
            this.Barangays.ImageKey = "map.png";
            this.Barangays.Location = new System.Drawing.Point(4, 45);
            this.Barangays.Name = "Barangays";
            this.Barangays.Size = new System.Drawing.Size(1906, 964);
            this.Barangays.TabIndex = 2;
            this.Barangays.Text = "Farmers";
            this.Barangays.UseVisualStyleBackColor = true;
            // 
            // farmersGrid
            // 
            this.farmersGrid.AutoGenerateContextFilters = true;
            this.farmersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.farmersGrid.DateWithTime = false;
            this.farmersGrid.Location = new System.Drawing.Point(853, 204);
            this.farmersGrid.Name = "farmersGrid";
            this.farmersGrid.Size = new System.Drawing.Size(966, 511);
            this.farmersGrid.TabIndex = 109;
            this.farmersGrid.TimeFilter = false;
            this.farmersGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.farmersGrid_CellContentClick);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(853, 153);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(214, 20);
            this.txtSearch.TabIndex = 108;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // Prediction
            // 
            this.Prediction.ImageKey = "calendar.png";
            this.Prediction.Location = new System.Drawing.Point(4, 45);
            this.Prediction.Name = "Prediction";
            this.Prediction.Size = new System.Drawing.Size(1906, 964);
            this.Prediction.TabIndex = 5;
            this.Prediction.Text = "Prediction";
            this.Prediction.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.Settings.ImageKey = "setting.png";
            this.Settings.Location = new System.Drawing.Point(4, 45);
            this.Settings.Name = "Settings";
            this.Settings.Padding = new System.Windows.Forms.Padding(3);
            this.Settings.Size = new System.Drawing.Size(1906, 964);
            this.Settings.TabIndex = 4;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barangay1ToolStripMenuItem,
            this.barangay2ToolStripMenuItem,
            this.barangay3ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(130, 70);
            // 
            // barangay1ToolStripMenuItem
            // 
            this.barangay1ToolStripMenuItem.Name = "barangay1ToolStripMenuItem";
            this.barangay1ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.barangay1ToolStripMenuItem.Text = "Barangay1";
            // 
            // barangay2ToolStripMenuItem
            // 
            this.barangay2ToolStripMenuItem.Name = "barangay2ToolStripMenuItem";
            this.barangay2ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.barangay2ToolStripMenuItem.Text = "Barangay2";
            // 
            // barangay3ToolStripMenuItem
            // 
            this.barangay3ToolStripMenuItem.Name = "barangay3ToolStripMenuItem";
            this.barangay3ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.barangay3ToolStripMenuItem.Text = "Barangay3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.materialTabControl1);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.materialTabControl1;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ofiice of the Municipal Agriculturist Brooke\'s Point";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.materialTabControl1.ResumeLayout(false);
            this.Home.ResumeLayout(false);
            this.Home.PerformLayout();
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.materialCard2.ResumeLayout(false);
            this.materialCard2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.materialCard6.ResumeLayout(false);
            this.materialCard6.PerformLayout();
            this.materialCard5.ResumeLayout(false);
            this.materialCard5.PerformLayout();
            this.materialCard3.ResumeLayout(false);
            this.Barangays.ResumeLayout(false);
            this.Barangays.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.farmersGrid)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage Home;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage Barangays;
        private System.Windows.Forms.ImageList imageList2;
        private MaterialSkin.Controls.MaterialCard materialCard3;
        private System.Windows.Forms.Label label5;
        private MaterialSkin.Controls.MaterialFloatingActionButton materialFloatingActionButton3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage Settings;
        protected internal MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.Label date_txt;
        private MaterialSkin.Controls.MaterialCard materialCard5;
        private MaterialSkin.Controls.MaterialFloatingActionButton materialFloatingActionButton5;
        private System.Windows.Forms.Timer timer1;
        private MaterialSkin.Controls.MaterialCard materialCard6;
        private System.Windows.Forms.Label time_txt;
        private MaterialSkin.Controls.MaterialFloatingActionButton materialFloatingActionButton6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem barangay1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem barangay2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem barangay3ToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private MaterialSkin.Controls.MaterialCard materialCard2;
        private MaterialSkin.Controls.MaterialLabel txt_farmerCount;
        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private MaterialSkin.Controls.MaterialFloatingActionButton materialFloatingActionButton2;
        private MaterialSkin.Controls.MaterialCard materialCard1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxCommodities;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private LiveCharts.WinForms.CartesianChart cartesianChart;
        private MaterialSkin.Controls.MaterialFloatingActionButton materialFloatingActionButton1;
        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabPage Prediction;
        private ADGV.AdvancedDataGridView farmersGrid;
    }
}

