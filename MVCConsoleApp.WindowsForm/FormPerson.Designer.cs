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
            labelTitle = new Label();
            grpDetails = new GroupBox();
            labelPersonId = new Label();
            labelFullName = new Label();
            txbName = new TextBox();
            labelPhone = new Label();
            txbPhone = new TextBox();
            labelEmail = new Label();
            txbEmail = new TextBox();
            labelRole = new Label();
            cbRole = new ComboBox();
            panelPerformer = new Panel();
            labelFee = new Label();
            nudFee = new NumericUpDown();
            labelGenres = new Label();
            clbGenres = new CheckedListBox();
            btnManageGenres = new Button();
            panelCrew = new Panel();
            labelHourlyRate = new Label();
            nudHourlyRate = new NumericUpDown();
            labelEmploymentType = new Label();
            cbEmploymentType = new ComboBox();
            labelWeeklyHours = new Label();
            nudWeeklyHours = new NumericUpDown();
            labelHoursHint = new Label();
            panelVendor = new Panel();
            labelStallName = new Label();
            txbStallName = new TextBox();
            labelCategories = new Label();
            clbCategories = new CheckedListBox();
            btnManageCategories = new Button();
            btnNew = new Button();
            btnSave = new Button();
            btnDelete = new Button();
            labelPersonList = new Label();
            labelFilterRole = new Label();
            cbFilterRole = new ComboBox();
            labelSearch = new Label();
            txbSearch = new TextBox();
            btnSearch = new Button();
            btnClearSearch = new Button();
            chkShowDeleted = new CheckBox();
            gridViewPersonList = new DataGridView();
            btnRestore = new Button();
            btnDeletePermanently = new Button();
            btnReports = new Button();
            btnExport = new Button();
            btnRefresh = new Button();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            grpDetails.SuspendLayout();
            panelPerformer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudFee).BeginInit();
            panelCrew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudHourlyRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWeeklyHours).BeginInit();
            panelVendor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridViewPersonList).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();
            //
            // labelTitle
            //
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelTitle.ForeColor = Color.IndianRed;
            labelTitle.Location = new Point(12, 9);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(229, 30);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Person Form";
            //
            // grpDetails
            //
            grpDetails.Controls.Add(labelPersonId);
            grpDetails.Controls.Add(labelFullName);
            grpDetails.Controls.Add(txbName);
            grpDetails.Controls.Add(labelPhone);
            grpDetails.Controls.Add(txbPhone);
            grpDetails.Controls.Add(labelEmail);
            grpDetails.Controls.Add(txbEmail);
            grpDetails.Controls.Add(labelRole);
            grpDetails.Controls.Add(cbRole);
            grpDetails.Controls.Add(panelPerformer);
            grpDetails.Controls.Add(panelCrew);
            grpDetails.Controls.Add(panelVendor);
            grpDetails.Controls.Add(btnNew);
            grpDetails.Controls.Add(btnSave);
            grpDetails.Controls.Add(btnDelete);
            grpDetails.Font = new Font("Segoe UI", 9F);
            grpDetails.Location = new Point(12, 48);
            grpDetails.Name = "grpDetails";
            grpDetails.Size = new Size(400, 600);
            grpDetails.TabIndex = 1;
            grpDetails.TabStop = false;
            grpDetails.Text = "Person details";
            //
            // labelPersonId
            //
            labelPersonId.AutoSize = true;
            labelPersonId.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelPersonId.ForeColor = Color.SteelBlue;
            labelPersonId.Location = new Point(15, 24);
            labelPersonId.Name = "labelPersonId";
            labelPersonId.Size = new Size(124, 19);
            labelPersonId.TabIndex = 0;
            labelPersonId.Text = "PersonID: (new)";
            //
            // labelFullName
            //
            labelFullName.AutoSize = true;
            labelFullName.Location = new Point(15, 52);
            labelFullName.Name = "labelFullName";
            labelFullName.Size = new Size(61, 15);
            labelFullName.TabIndex = 1;
            labelFullName.Text = "Full name";
            //
            // txbName
            //
            txbName.Location = new Point(15, 70);
            txbName.MaxLength = 100;
            txbName.Name = "txbName";
            txbName.Size = new Size(370, 23);
            txbName.TabIndex = 2;
            //
            // labelPhone
            //
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(15, 100);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(61, 15);
            labelPhone.TabIndex = 3;
            labelPhone.Text = "Telephone";
            //
            // txbPhone
            //
            txbPhone.Location = new Point(15, 118);
            txbPhone.MaxLength = 20;
            txbPhone.Name = "txbPhone";
            txbPhone.Size = new Size(370, 23);
            txbPhone.TabIndex = 4;
            //
            // labelEmail
            //
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(15, 148);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(36, 15);
            labelEmail.TabIndex = 5;
            labelEmail.Text = "Email";
            //
            // txbEmail
            //
            txbEmail.Location = new Point(15, 166);
            txbEmail.MaxLength = 254;
            txbEmail.Name = "txbEmail";
            txbEmail.Size = new Size(370, 23);
            txbEmail.TabIndex = 6;
            //
            // labelRole
            //
            labelRole.AutoSize = true;
            labelRole.Location = new Point(15, 196);
            labelRole.Name = "labelRole";
            labelRole.Size = new Size(30, 15);
            labelRole.TabIndex = 7;
            labelRole.Text = "Role";
            //
            // cbRole
            //
            cbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRole.FormattingEnabled = true;
            cbRole.Location = new Point(15, 214);
            cbRole.Name = "cbRole";
            cbRole.Size = new Size(370, 23);
            cbRole.TabIndex = 8;
            cbRole.SelectedIndexChanged += cbRole_SelectedIndexChanged;
            //
            // panelPerformer
            //
            panelPerformer.Controls.Add(labelFee);
            panelPerformer.Controls.Add(nudFee);
            panelPerformer.Controls.Add(labelGenres);
            panelPerformer.Controls.Add(clbGenres);
            panelPerformer.Controls.Add(btnManageGenres);
            panelPerformer.Location = new Point(15, 248);
            panelPerformer.Name = "panelPerformer";
            panelPerformer.Size = new Size(370, 280);
            panelPerformer.TabIndex = 9;
            //
            // labelFee
            //
            labelFee.AutoSize = true;
            labelFee.Location = new Point(0, 0);
            labelFee.Name = "labelFee";
            labelFee.Size = new Size(46, 15);
            labelFee.TabIndex = 0;
            labelFee.Text = "Fee (£)";
            //
            // nudFee
            //
            nudFee.DecimalPlaces = 2;
            nudFee.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            nudFee.Location = new Point(0, 18);
            nudFee.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudFee.Name = "nudFee";
            nudFee.Size = new Size(180, 23);
            nudFee.TabIndex = 1;
            nudFee.ThousandsSeparator = true;
            //
            // labelGenres
            //
            labelGenres.AutoSize = true;
            labelGenres.Location = new Point(0, 50);
            labelGenres.Name = "labelGenres";
            labelGenres.Size = new Size(150, 15);
            labelGenres.TabIndex = 2;
            labelGenres.Text = "Genres (tick exactly 2)";
            //
            // clbGenres
            //
            clbGenres.CheckOnClick = true;
            clbGenres.FormattingEnabled = true;
            clbGenres.Location = new Point(0, 68);
            clbGenres.Name = "clbGenres";
            clbGenres.Size = new Size(370, 166);
            clbGenres.TabIndex = 3;
            //
            // btnManageGenres
            //
            btnManageGenres.Location = new Point(0, 242);
            btnManageGenres.Name = "btnManageGenres";
            btnManageGenres.Size = new Size(150, 30);
            btnManageGenres.TabIndex = 4;
            btnManageGenres.Text = "Manage genres...";
            btnManageGenres.UseVisualStyleBackColor = true;
            btnManageGenres.Click += btnManageGenres_Click;
            //
            // panelCrew
            //
            panelCrew.Controls.Add(labelHourlyRate);
            panelCrew.Controls.Add(nudHourlyRate);
            panelCrew.Controls.Add(labelEmploymentType);
            panelCrew.Controls.Add(cbEmploymentType);
            panelCrew.Controls.Add(labelWeeklyHours);
            panelCrew.Controls.Add(nudWeeklyHours);
            panelCrew.Controls.Add(labelHoursHint);
            panelCrew.Location = new Point(15, 248);
            panelCrew.Name = "panelCrew";
            panelCrew.Size = new Size(370, 280);
            panelCrew.TabIndex = 10;
            panelCrew.Visible = false;
            //
            // labelHourlyRate
            //
            labelHourlyRate.AutoSize = true;
            labelHourlyRate.Location = new Point(0, 0);
            labelHourlyRate.Name = "labelHourlyRate";
            labelHourlyRate.Size = new Size(94, 15);
            labelHourlyRate.TabIndex = 0;
            labelHourlyRate.Text = "Hourly rate (£)";
            //
            // nudHourlyRate
            //
            nudHourlyRate.DecimalPlaces = 2;
            nudHourlyRate.Location = new Point(0, 18);
            nudHourlyRate.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudHourlyRate.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            nudHourlyRate.Name = "nudHourlyRate";
            nudHourlyRate.Size = new Size(180, 23);
            nudHourlyRate.TabIndex = 1;
            nudHourlyRate.Value = new decimal(new int[] { 12, 0, 0, 0 });
            //
            // labelEmploymentType
            //
            labelEmploymentType.AutoSize = true;
            labelEmploymentType.Location = new Point(0, 50);
            labelEmploymentType.Name = "labelEmploymentType";
            labelEmploymentType.Size = new Size(103, 15);
            labelEmploymentType.TabIndex = 2;
            labelEmploymentType.Text = "Employment type";
            //
            // cbEmploymentType
            //
            cbEmploymentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEmploymentType.FormattingEnabled = true;
            cbEmploymentType.Location = new Point(0, 68);
            cbEmploymentType.Name = "cbEmploymentType";
            cbEmploymentType.Size = new Size(180, 23);
            cbEmploymentType.TabIndex = 3;
            cbEmploymentType.SelectedIndexChanged += cbEmploymentType_SelectedIndexChanged;
            //
            // labelWeeklyHours
            //
            labelWeeklyHours.AutoSize = true;
            labelWeeklyHours.Location = new Point(0, 100);
            labelWeeklyHours.Name = "labelWeeklyHours";
            labelWeeklyHours.Size = new Size(78, 15);
            labelWeeklyHours.TabIndex = 4;
            labelWeeklyHours.Text = "Weekly hours";
            //
            // nudWeeklyHours
            //
            nudWeeklyHours.Location = new Point(0, 118);
            nudWeeklyHours.Maximum = new decimal(new int[] { 40, 0, 0, 0 });
            nudWeeklyHours.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudWeeklyHours.Name = "nudWeeklyHours";
            nudWeeklyHours.Size = new Size(180, 23);
            nudWeeklyHours.TabIndex = 5;
            nudWeeklyHours.Value = new decimal(new int[] { 25, 0, 0, 0 });
            //
            // labelHoursHint
            //
            labelHoursHint.AutoSize = true;
            labelHoursHint.ForeColor = Color.DimGray;
            labelHoursHint.Location = new Point(0, 148);
            labelHoursHint.Name = "labelHoursHint";
            labelHoursHint.Size = new Size(200, 15);
            labelHoursHint.TabIndex = 6;
            labelHoursHint.Text = "FullTime: 25-40 h, PartTime: 1-24 h";
            //
            // panelVendor
            //
            panelVendor.Controls.Add(labelStallName);
            panelVendor.Controls.Add(txbStallName);
            panelVendor.Controls.Add(labelCategories);
            panelVendor.Controls.Add(clbCategories);
            panelVendor.Controls.Add(btnManageCategories);
            panelVendor.Location = new Point(15, 248);
            panelVendor.Name = "panelVendor";
            panelVendor.Size = new Size(370, 280);
            panelVendor.TabIndex = 11;
            panelVendor.Visible = false;
            //
            // labelStallName
            //
            labelStallName.AutoSize = true;
            labelStallName.Location = new Point(0, 0);
            labelStallName.Name = "labelStallName";
            labelStallName.Size = new Size(120, 15);
            labelStallName.TabIndex = 0;
            labelStallName.Text = "Vendor (stall) name";
            //
            // txbStallName
            //
            txbStallName.Location = new Point(0, 18);
            txbStallName.MaxLength = 100;
            txbStallName.Name = "txbStallName";
            txbStallName.Size = new Size(370, 23);
            txbStallName.TabIndex = 1;
            //
            // labelCategories
            //
            labelCategories.AutoSize = true;
            labelCategories.Location = new Point(0, 50);
            labelCategories.Name = "labelCategories";
            labelCategories.Size = new Size(210, 15);
            labelCategories.TabIndex = 2;
            labelCategories.Text = "Product categories (tick exactly 3)";
            //
            // clbCategories
            //
            clbCategories.CheckOnClick = true;
            clbCategories.FormattingEnabled = true;
            clbCategories.Location = new Point(0, 68);
            clbCategories.Name = "clbCategories";
            clbCategories.Size = new Size(370, 166);
            clbCategories.TabIndex = 3;
            //
            // btnManageCategories
            //
            btnManageCategories.Location = new Point(0, 242);
            btnManageCategories.Name = "btnManageCategories";
            btnManageCategories.Size = new Size(170, 30);
            btnManageCategories.TabIndex = 4;
            btnManageCategories.Text = "Manage categories...";
            btnManageCategories.UseVisualStyleBackColor = true;
            btnManageCategories.Click += btnManageCategories_Click;
            //
            // btnNew
            //
            btnNew.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNew.Location = new Point(15, 545);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(110, 40);
            btnNew.TabIndex = 12;
            btnNew.Text = "New";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            //
            // btnSave
            //
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.IndianRed;
            btnSave.Location = new Point(135, 545);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 40);
            btnSave.TabIndex = 13;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            //
            // btnDelete
            //
            btnDelete.Enabled = false;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.Location = new Point(275, 545);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(110, 40);
            btnDelete.TabIndex = 14;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            //
            // labelPersonList
            //
            labelPersonList.AutoSize = true;
            labelPersonList.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelPersonList.ForeColor = Color.IndianRed;
            labelPersonList.Location = new Point(425, 9);
            labelPersonList.Name = "labelPersonList";
            labelPersonList.Size = new Size(125, 30);
            labelPersonList.TabIndex = 2;
            labelPersonList.Text = "Person List";
            //
            // labelFilterRole
            //
            labelFilterRole.AutoSize = true;
            labelFilterRole.Location = new Point(425, 57);
            labelFilterRole.Name = "labelFilterRole";
            labelFilterRole.Size = new Size(33, 15);
            labelFilterRole.TabIndex = 3;
            labelFilterRole.Text = "Role:";
            //
            // cbFilterRole
            //
            cbFilterRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterRole.FormattingEnabled = true;
            cbFilterRole.Location = new Point(464, 54);
            cbFilterRole.Name = "cbFilterRole";
            cbFilterRole.Size = new Size(130, 23);
            cbFilterRole.TabIndex = 4;
            cbFilterRole.SelectedIndexChanged += cbFilterRole_SelectedIndexChanged;
            //
            // labelSearch
            //
            labelSearch.AutoSize = true;
            labelSearch.Location = new Point(610, 57);
            labelSearch.Name = "labelSearch";
            labelSearch.Size = new Size(45, 15);
            labelSearch.TabIndex = 5;
            labelSearch.Text = "Search:";
            //
            // txbSearch
            //
            txbSearch.Location = new Point(661, 54);
            txbSearch.Name = "txbSearch";
            txbSearch.PlaceholderText = "part of a name or email";
            txbSearch.Size = new Size(220, 23);
            txbSearch.TabIndex = 6;
            txbSearch.KeyDown += txbSearch_KeyDown;
            //
            // btnSearch
            //
            btnSearch.Location = new Point(887, 52);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 27);
            btnSearch.TabIndex = 7;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            //
            // btnClearSearch
            //
            btnClearSearch.Location = new Point(968, 52);
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Size = new Size(65, 27);
            btnClearSearch.TabIndex = 8;
            btnClearSearch.Text = "Clear";
            btnClearSearch.UseVisualStyleBackColor = true;
            btnClearSearch.Click += btnClearSearch_Click;
            //
            // chkShowDeleted
            //
            chkShowDeleted.AutoSize = true;
            chkShowDeleted.Location = new Point(1050, 56);
            chkShowDeleted.Name = "chkShowDeleted";
            chkShowDeleted.Size = new Size(116, 19);
            chkShowDeleted.TabIndex = 9;
            chkShowDeleted.Text = "Show recycle bin";
            chkShowDeleted.UseVisualStyleBackColor = true;
            chkShowDeleted.CheckedChanged += chkShowDeleted_CheckedChanged;
            //
            // gridViewPersonList
            //
            gridViewPersonList.AllowUserToAddRows = false;
            gridViewPersonList.AllowUserToDeleteRows = false;
            gridViewPersonList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridViewPersonList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridViewPersonList.BackgroundColor = Color.White;
            gridViewPersonList.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.LightSteelBlue;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            gridViewPersonList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            gridViewPersonList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridViewPersonList.EnableHeadersVisualStyles = false;
            gridViewPersonList.Location = new Point(425, 88);
            gridViewPersonList.MultiSelect = false;
            gridViewPersonList.Name = "gridViewPersonList";
            gridViewPersonList.ReadOnly = true;
            gridViewPersonList.RowHeadersVisible = false;
            gridViewPersonList.RowTemplate.Height = 26;
            gridViewPersonList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridViewPersonList.Size = new Size(810, 512);
            gridViewPersonList.TabIndex = 10;
            gridViewPersonList.ColumnHeaderMouseClick += gridViewPersonList_ColumnHeaderMouseClick;
            gridViewPersonList.CellClick += gridViewPersonList_CellClick;
            gridViewPersonList.KeyUp += gridViewPersonList_KeyUp;
            //
            // btnRestore
            //
            btnRestore.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRestore.Enabled = false;
            btnRestore.Location = new Point(425, 610);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(110, 36);
            btnRestore.TabIndex = 11;
            btnRestore.Text = "Restore";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            //
            // btnDeletePermanently
            //
            btnDeletePermanently.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeletePermanently.Enabled = false;
            btnDeletePermanently.Location = new Point(541, 610);
            btnDeletePermanently.Name = "btnDeletePermanently";
            btnDeletePermanently.Size = new Size(150, 36);
            btnDeletePermanently.TabIndex = 12;
            btnDeletePermanently.Text = "Delete permanently";
            btnDeletePermanently.UseVisualStyleBackColor = true;
            btnDeletePermanently.Click += btnDeletePermanently_Click;
            //
            // btnReports
            //
            btnReports.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnReports.Location = new Point(889, 610);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(110, 36);
            btnReports.TabIndex = 13;
            btnReports.Text = "Reports...";
            btnReports.UseVisualStyleBackColor = true;
            btnReports.Click += btnReports_Click;
            //
            // btnExport
            //
            btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExport.Location = new Point(1005, 610);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(120, 36);
            btnExport.TabIndex = 14;
            btnExport.Text = "Export CSV...";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            //
            // btnRefresh
            //
            btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRefresh.Location = new Point(1131, 610);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(104, 36);
            btnRefresh.TabIndex = 15;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            //
            // statusStrip
            //
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip.Location = new Point(0, 658);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1248, 22);
            statusStrip.TabIndex = 16;
            //
            // lblStatus
            //
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(39, 17);
            lblStatus.Text = "Ready";
            //
            // FormPerson
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 680);
            Controls.Add(labelTitle);
            Controls.Add(grpDetails);
            Controls.Add(labelPersonList);
            Controls.Add(labelFilterRole);
            Controls.Add(cbFilterRole);
            Controls.Add(labelSearch);
            Controls.Add(txbSearch);
            Controls.Add(btnSearch);
            Controls.Add(btnClearSearch);
            Controls.Add(chkShowDeleted);
            Controls.Add(gridViewPersonList);
            Controls.Add(btnRestore);
            Controls.Add(btnDeletePermanently);
            Controls.Add(btnReports);
            Controls.Add(btnExport);
            Controls.Add(btnRefresh);
            Controls.Add(statusStrip);
            MinimumSize = new Size(1100, 700);
            Name = "FormPerson";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Music Festival Operations System";
            grpDetails.ResumeLayout(false);
            grpDetails.PerformLayout();
            panelPerformer.ResumeLayout(false);
            panelPerformer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudFee).EndInit();
            panelCrew.ResumeLayout(false);
            panelCrew.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudHourlyRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWeeklyHours).EndInit();
            panelVendor.ResumeLayout(false);
            panelVendor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridViewPersonList).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitle;
        private GroupBox grpDetails;
        private Label labelPersonId;
        private Label labelFullName;
        private TextBox txbName;
        private Label labelPhone;
        private TextBox txbPhone;
        private Label labelEmail;
        private TextBox txbEmail;
        private Label labelRole;
        private ComboBox cbRole;
        private Panel panelPerformer;
        private Label labelFee;
        private NumericUpDown nudFee;
        private Label labelGenres;
        private CheckedListBox clbGenres;
        private Button btnManageGenres;
        private Panel panelCrew;
        private Label labelHourlyRate;
        private NumericUpDown nudHourlyRate;
        private Label labelEmploymentType;
        private ComboBox cbEmploymentType;
        private Label labelWeeklyHours;
        private NumericUpDown nudWeeklyHours;
        private Label labelHoursHint;
        private Panel panelVendor;
        private Label labelStallName;
        private TextBox txbStallName;
        private Label labelCategories;
        private CheckedListBox clbCategories;
        private Button btnManageCategories;
        private Button btnNew;
        private Button btnSave;
        private Button btnDelete;
        private Label labelPersonList;
        private Label labelFilterRole;
        private ComboBox cbFilterRole;
        private Label labelSearch;
        private TextBox txbSearch;
        private Button btnSearch;
        private Button btnClearSearch;
        private CheckBox chkShowDeleted;
        private DataGridView gridViewPersonList;
        private Button btnRestore;
        private Button btnDeletePermanently;
        private Button btnReports;
        private Button btnExport;
        private Button btnRefresh;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
    }
}
