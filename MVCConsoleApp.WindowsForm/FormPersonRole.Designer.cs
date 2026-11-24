using MVCConsoleApp.Data.Services;
namespace MVCConsoleApp.WindowsForm
{
    partial class FormPersonRole
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            labelPersonRoleForm = new Label();
            labelPersonRoleList = new Label();
            labelName = new Label();
            txbName = new TextBox();
            labelDescription = new Label();
            rtxbDescription = new RichTextBox();
            btnSave = new Button();
            gridViewPersonRoleList = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)gridViewPersonRoleList).BeginInit();
            SuspendLayout();
            // 
            // labelPersonRoleForm
            // 
            labelPersonRoleForm.AutoSize = true;
            labelPersonRoleForm.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelPersonRoleForm.ForeColor = SystemColors.MenuHighlight;
            labelPersonRoleForm.Location = new Point(56, 51);
            labelPersonRoleForm.Name = "labelPersonRoleForm";
            labelPersonRoleForm.Size = new Size(335, 51);
            labelPersonRoleForm.TabIndex = 0;
            labelPersonRoleForm.Text = "Person Role Form";
            // 
            // labelPersonRoleList
            // 
            labelPersonRoleList.AutoSize = true;
            labelPersonRoleList.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelPersonRoleList.ForeColor = SystemColors.MenuHighlight;
            labelPersonRoleList.Location = new Point(718, 51);
            labelPersonRoleList.Name = "labelPersonRoleList";
            labelPersonRoleList.Size = new Size(304, 51);
            labelPersonRoleList.TabIndex = 1;
            labelPersonRoleList.Text = "Person Role List";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelName.ForeColor = SystemColors.MenuText;
            labelName.Location = new Point(56, 134);
            labelName.Name = "labelName";
            labelName.Size = new Size(91, 38);
            labelName.TabIndex = 2;
            labelName.Text = "Name";
            // 
            // txbName
            // 
            txbName.Location = new Point(56, 185);
            txbName.Name = "txbName";
            txbName.Size = new Size(381, 35);
            txbName.TabIndex = 3;
            // 
            // labelDescription
            // 
            labelDescription.AutoSize = true;
            labelDescription.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelDescription.ForeColor = SystemColors.MenuText;
            labelDescription.Location = new Point(56, 242);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new Size(158, 38);
            labelDescription.TabIndex = 4;
            labelDescription.Text = "Description";
            // 
            // rtxbDescription
            // 
            rtxbDescription.Location = new Point(56, 305);
            rtxbDescription.Name = "rtxbDescription";
            rtxbDescription.Size = new Size(381, 275);
            rtxbDescription.TabIndex = 5;
            rtxbDescription.Text = "";
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = SystemColors.Highlight;
            btnSave.Location = new Point(117, 613);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(272, 62);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // gridViewPersonRoleList
            // 
            gridViewPersonRoleList.AllowUserToAddRows = false;
            gridViewPersonRoleList.AllowUserToDeleteRows = false;
            gridViewPersonRoleList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridViewPersonRoleList.BackgroundColor = Color.White;
            gridViewPersonRoleList.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.LightSteelBlue;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            gridViewPersonRoleList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            gridViewPersonRoleList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridViewPersonRoleList.EnableHeadersVisualStyles = false;
            gridViewPersonRoleList.Location = new Point(492, 134);
            gridViewPersonRoleList.MultiSelect = false;
            gridViewPersonRoleList.Name = "gridViewPersonRoleList";
            gridViewPersonRoleList.ReadOnly = true;
            gridViewPersonRoleList.RowHeadersVisible = false;
            gridViewPersonRoleList.RowHeadersWidth = 72;
            gridViewPersonRoleList.RowTemplate.Height = 30;
            gridViewPersonRoleList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridViewPersonRoleList.Size = new Size(701, 446);
            gridViewPersonRoleList.TabIndex = 7;
            // 
            // FormPersonRole
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1222, 745);
            Controls.Add(gridViewPersonRoleList);
            Controls.Add(btnSave);
            Controls.Add(rtxbDescription);
            Controls.Add(labelDescription);
            Controls.Add(txbName);
            Controls.Add(labelName);
            Controls.Add(labelPersonRoleList);
            Controls.Add(labelPersonRoleForm);
            Name = "FormPersonRole";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)gridViewPersonRoleList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        public void InitializeDataGridView(DataGridView gridView)
        {
            gridView.Columns.Clear();
            gridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Mã Id",
                DataPropertyName = "Id",
                //Width = 80
            });
            gridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Tên Role",
                DataPropertyName = "Name",
                //Width = 80
            });
            gridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                //Width = 80
            });


            //var controller = new PersonRoleController();
            var service = new PersonRoleServices();
            gridView.DataSource = null;
            gridView.DataSource = service.GetAll();
        }
        #endregion

        private Label labelPersonRoleForm;
        private Label labelPersonRoleList;
        private Label labelName;
        private TextBox txbName;
        private Label labelDescription;
        private RichTextBox rtxbDescription;
        private Button btnSave;
        private DataGridView gridViewPersonRoleList;
    }
}
