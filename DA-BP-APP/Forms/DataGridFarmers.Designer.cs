namespace DA_BP_APP
{
    partial class DataGridFarmers
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Update = new System.Windows.Forms.DataGridViewButtonColumn();
            this.farmerIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.farmerFirstNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.farmerContactDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.farmerAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.farmersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.farmersBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.farmerIDDataGridViewTextBoxColumn,
            this.farmerFirstNameDataGridViewTextBoxColumn,
            this.farmerContactDataGridViewTextBoxColumn,
            this.farmerAddressDataGridViewTextBoxColumn,
            this.Update});
            this.dataGridView1.DataSource = this.farmersBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 64);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1178, 394);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Update
            // 
            this.Update.HeaderText = "Update";
            this.Update.Name = "Update";
            // 
            // farmerIDDataGridViewTextBoxColumn
            // 
            this.farmerIDDataGridViewTextBoxColumn.DataPropertyName = "FarmerID";
            this.farmerIDDataGridViewTextBoxColumn.HeaderText = "FarmerID";
            this.farmerIDDataGridViewTextBoxColumn.Name = "farmerIDDataGridViewTextBoxColumn";
            // 
            // farmerFirstNameDataGridViewTextBoxColumn
            // 
            this.farmerFirstNameDataGridViewTextBoxColumn.DataPropertyName = "FarmerFirstName";
            this.farmerFirstNameDataGridViewTextBoxColumn.HeaderText = "FarmerFirstName";
            this.farmerFirstNameDataGridViewTextBoxColumn.Name = "farmerFirstNameDataGridViewTextBoxColumn";
            this.farmerFirstNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // farmerContactDataGridViewTextBoxColumn
            // 
            this.farmerContactDataGridViewTextBoxColumn.DataPropertyName = "FarmerContact";
            this.farmerContactDataGridViewTextBoxColumn.HeaderText = "FarmerContact";
            this.farmerContactDataGridViewTextBoxColumn.Name = "farmerContactDataGridViewTextBoxColumn";
            this.farmerContactDataGridViewTextBoxColumn.Width = 150;
            // 
            // farmerAddressDataGridViewTextBoxColumn
            // 
            this.farmerAddressDataGridViewTextBoxColumn.DataPropertyName = "FarmerAddress";
            this.farmerAddressDataGridViewTextBoxColumn.HeaderText = "FarmerAddress";
            this.farmerAddressDataGridViewTextBoxColumn.Name = "farmerAddressDataGridViewTextBoxColumn";
            this.farmerAddressDataGridViewTextBoxColumn.Width = 300;
            // 
            // farmersBindingSource
            // 
            this.farmersBindingSource.DataSource = typeof(DA_BP_APP.Farmers);
            // 
            // DataGridFarmers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 461);
            this.Controls.Add(this.dataGridView1);
            this.Name = "DataGridFarmers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Amas Farmers";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.farmersBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource farmersBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn farmerIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn farmerFirstNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn farmerLasttNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn farmerContactDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn farmerAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn Update;
    }
}