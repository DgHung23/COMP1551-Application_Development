namespace MVCConsoleApp.WindowsForm
{
    partial class FormPerson
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            gridViewPersonRoleList = new DataGridView();
            btnSave = new Button();
            btnManageRoles = new Button();
            labelEmail = new Label();
            txbName = new TextBox();
            labelFullName = new Label();
            labelPersonList = new Label();
            labelPersonForm = new Label();
            txbEmail = new TextBox();
            txbPhone = new TextBox();
            labelPhone = new Label();
            cbRole = new ComboBox();
            labelRole = new Label();
            cbGender = new ComboBox();
            lbGender = new Label();
            ((System.ComponentModel.ISupportInitialize)gridViewPersonRoleList).BeginInit();
            SuspendLayout();
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
            gridViewPersonRoleList.Location = new Point(448, 113);
            gridViewPersonRoleList.MultiSelect = false;
            gridViewPersonRoleList.Name = "gridViewPersonRoleList";
            gridViewPersonRoleList.ReadOnly = true;
            gridViewPersonRoleList.RowHeadersVisible = false;
            gridViewPersonRoleList.RowHeadersWidth = 72;
            gridViewPersonRoleList.RowTemplate.Height = 30;
            gridViewPersonRoleList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridViewPersonRoleList.Size = new Size(1170, 446);
            gridViewPersonRoleList.TabIndex = 15;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.IndianRed;
            btnSave.Location = new Point(73, 680);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(272, 62);
            btnSave.TabIndex = 14;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnManageRoles
            // 
            btnManageRoles.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnManageRoles.Location = new Point(351, 601);
            btnManageRoles.Name = "btnManageRoles";
            btnManageRoles.Size = new Size(89, 42);
            btnManageRoles.TabIndex = 21;
            btnManageRoles.Text = "ADD";
            btnManageRoles.UseVisualStyleBackColor = true;
            btnManageRoles.Click += btnManageRoles_Click;
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelEmail.ForeColor = SystemColors.MenuText;
            labelEmail.Location = new Point(12, 328);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(83, 38);
            labelEmail.TabIndex = 12;
            labelEmail.Text = "Email";
            // 
            // txbName
            // 
            txbName.Location = new Point(12, 164);
            txbName.Name = "txbName";
            txbName.Size = new Size(381, 35);
            txbName.TabIndex = 11;
            // 
            // labelFullName
            // 
            labelFullName.AutoSize = true;
            labelFullName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelFullName.ForeColor = SystemColors.MenuText;
            labelFullName.Location = new Point(12, 113);
            labelFullName.Name = "labelFullName";
            labelFullName.Size = new Size(143, 38);
            labelFullName.TabIndex = 10;
            labelFullName.Text = "Full Name";
            // 
            // labelPersonList
            // 
            labelPersonList.AutoSize = true;
            labelPersonList.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelPersonList.ForeColor = Color.IndianRed;
            labelPersonList.Location = new Point(922, 30);
            labelPersonList.Name = "labelPersonList";
            labelPersonList.Size = new Size(215, 51);
            labelPersonList.TabIndex = 9;
            labelPersonList.Text = "Person List";
            // 
            // labelPersonForm
            // 
            labelPersonForm.AutoSize = true;
            labelPersonForm.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelPersonForm.ForeColor = Color.IndianRed;
            labelPersonForm.Location = new Point(73, 30);
            labelPersonForm.Name = "labelPersonForm";
            labelPersonForm.Size = new Size(246, 51);
            labelPersonForm.TabIndex = 8;
            labelPersonForm.Text = "Person Form";
            // 
            // txbEmail
            // 
            txbEmail.Location = new Point(12, 383);
            txbEmail.Name = "txbEmail";
            txbEmail.Size = new Size(381, 35);
            txbEmail.TabIndex = 16;
            // 
            // txbPhone
            // 
            txbPhone.Location = new Point(12, 490);
            txbPhone.Name = "txbPhone";
            txbPhone.Size = new Size(381, 35);
            txbPhone.TabIndex = 18;
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelPhone.ForeColor = SystemColors.MenuText;
            labelPhone.Location = new Point(12, 435);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(96, 38);
            labelPhone.TabIndex = 17;
            labelPhone.Text = "Phone";
            // 
            // cbRole
            // 
            cbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRole.FormattingEnabled = true;
            cbRole.Location = new Point(12, 603);
            cbRole.Name = "cbRole";
            cbRole.Size = new Size(333, 38);
            cbRole.TabIndex = 20;
            // 
            // labelRole
            // 
            labelRole.AutoSize = true;
            labelRole.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelRole.ForeColor = SystemColors.MenuText;
            labelRole.Location = new Point(12, 548);
            labelRole.Name = "labelRole";
            labelRole.Size = new Size(71, 38);
            labelRole.TabIndex = 19;
            labelRole.Text = "Role";
            // 
            // cbGender
            // 
            cbGender.FormattingEnabled = true;
            cbGender.Location = new Point(12, 265);
            cbGender.Name = "cbGender";
            cbGender.Size = new Size(381, 38);
            cbGender.TabIndex = 22;
            // 
            // lbGender
            // 
            lbGender.AutoSize = true;
            lbGender.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbGender.ForeColor = SystemColors.MenuText;
            lbGender.Location = new Point(12, 212);
            lbGender.Name = "lbGender";
            lbGender.Size = new Size(108, 38);
            lbGender.TabIndex = 23;
            lbGender.Text = "Gender";
            // 
            // FormPerson
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1663, 794);
            Controls.Add(lbGender);
            Controls.Add(cbGender);
            Controls.Add(btnManageRoles);
            Controls.Add(cbRole);
            Controls.Add(labelRole);
            Controls.Add(txbPhone);
            Controls.Add(labelPhone);
            Controls.Add(txbEmail);
            Controls.Add(gridViewPersonRoleList);
            Controls.Add(btnSave);
            Controls.Add(labelEmail);
            Controls.Add(txbName);
            Controls.Add(labelFullName);
            Controls.Add(labelPersonList);
            Controls.Add(labelPersonForm);
            Name = "FormPerson";
            Text = "FormPerson";
            ((System.ComponentModel.ISupportInitialize)gridViewPersonRoleList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView gridViewPersonRoleList;
        private Button btnSave;
        private Label labelEmail;
        private TextBox txbName;
        private Label labelFullName;
        private Label labelPersonList;
        private Label labelPersonForm;
        private TextBox txbEmail;
        private TextBox txbPhone;
        private Label labelPhone;
        private ComboBox cbRole;
        private Button btnManageRoles;
        private Label labelRole;
        private ComboBox cbGender;
        private Label lbGender;
    }
}