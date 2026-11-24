namespace MVCConsoleApp.WindowsForm
{
    partial class FormLookup
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
            labelLookupForm = new Label();
            labelLookupList = new Label();
            labelName = new Label();
            txbName = new TextBox();
            btnSave = new Button();
            btnDelete = new Button();
            labelHint = new Label();
            gridViewLookupList = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)gridViewLookupList).BeginInit();
            SuspendLayout();
            //
            // labelLookupForm
            //
            labelLookupForm.AutoSize = true;
            labelLookupForm.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelLookupForm.ForeColor = SystemColors.MenuHighlight;
            labelLookupForm.Location = new Point(20, 20);
            labelLookupForm.Name = "labelLookupForm";
            labelLookupForm.Size = new Size(144, 30);
            labelLookupForm.TabIndex = 0;
            labelLookupForm.Text = "Genre Form";
            //
            // labelLookupList
            //
            labelLookupList.AutoSize = true;
            labelLookupList.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelLookupList.ForeColor = SystemColors.MenuHighlight;
            labelLookupList.Location = new Point(300, 20);
            labelLookupList.Name = "labelLookupList";
            labelLookupList.Size = new Size(124, 30);
            labelLookupList.TabIndex = 1;
            labelLookupList.Text = "Genre List";
            //
            // labelName
            //
            labelName.AutoSize = true;
            labelName.Font = new Font("Segoe UI", 12F);
            labelName.ForeColor = SystemColors.MenuText;
            labelName.Location = new Point(20, 70);
            labelName.Name = "labelName";
            labelName.Size = new Size(52, 21);
            labelName.TabIndex = 2;
            labelName.Text = "Name";
            //
            // txbName
            //
            txbName.Location = new Point(20, 98);
            txbName.MaxLength = 50;
            txbName.Name = "txbName";
            txbName.Size = new Size(250, 23);
            txbName.TabIndex = 3;
            txbName.KeyDown += txbName_KeyDown;
            //
            // btnSave
            //
            btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSave.ForeColor = SystemColors.Highlight;
            btnSave.Location = new Point(20, 135);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(250, 42);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            //
            // btnDelete
            //
            btnDelete.Font = new Font("Segoe UI", 10F);
            btnDelete.Location = new Point(20, 190);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(250, 36);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Delete selected";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            //
            // labelHint
            //
            labelHint.ForeColor = Color.DimGray;
            labelHint.Location = new Point(20, 240);
            labelHint.Name = "labelHint";
            labelHint.Size = new Size(250, 80);
            labelHint.TabIndex = 6;
            labelHint.Text = "An item can only be deleted when no record uses it.";
            //
            // gridViewLookupList
            //
            gridViewLookupList.AllowUserToAddRows = false;
            gridViewLookupList.AllowUserToDeleteRows = false;
            gridViewLookupList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridViewLookupList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridViewLookupList.BackgroundColor = Color.White;
            gridViewLookupList.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.LightSteelBlue;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            gridViewLookupList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            gridViewLookupList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridViewLookupList.EnableHeadersVisualStyles = false;
            gridViewLookupList.Location = new Point(300, 70);
            gridViewLookupList.MultiSelect = false;
            gridViewLookupList.Name = "gridViewLookupList";
            gridViewLookupList.ReadOnly = true;
            gridViewLookupList.RowHeadersVisible = false;
            gridViewLookupList.RowTemplate.Height = 26;
            gridViewLookupList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridViewLookupList.Size = new Size(400, 330);
            gridViewLookupList.TabIndex = 7;
            //
            // FormLookup
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 420);
            Controls.Add(gridViewLookupList);
            Controls.Add(labelHint);
            Controls.Add(btnDelete);
            Controls.Add(btnSave);
            Controls.Add(txbName);
            Controls.Add(labelName);
            Controls.Add(labelLookupList);
            Controls.Add(labelLookupForm);
            MinimizeBox = false;
            Name = "FormLookup";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Manage list";
            ((System.ComponentModel.ISupportInitialize)gridViewLookupList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelLookupForm;
        private Label labelLookupList;
        private Label labelName;
        private TextBox txbName;
        private Button btnSave;
        private Button btnDelete;
        private Label labelHint;
        private DataGridView gridViewLookupList;
    }
}
