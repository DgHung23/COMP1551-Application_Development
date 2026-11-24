namespace MVCConsoleApp.WindowsForm
{
    partial class FormReport
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
            labelTitle = new Label();
            tabReports = new TabControl();
            tabGenres = new TabPage();
            gridGenres = new DataGridView();
            tabCategories = new TabPage();
            gridCategories = new DataGridView();
            tabCosts = new TabPage();
            gridCosts = new DataGridView();
            btnClose = new Button();
            tabReports.SuspendLayout();
            tabGenres.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridGenres).BeginInit();
            tabCategories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCategories).BeginInit();
            tabCosts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCosts).BeginInit();
            SuspendLayout();
            //
            // labelTitle
            //
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelTitle.ForeColor = Color.IndianRed;
            labelTitle.Location = new Point(12, 9);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(98, 30);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Reports";
            //
            // tabReports
            //
            tabReports.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabReports.Controls.Add(tabGenres);
            tabReports.Controls.Add(tabCategories);
            tabReports.Controls.Add(tabCosts);
            tabReports.Location = new Point(12, 50);
            tabReports.Name = "tabReports";
            tabReports.SelectedIndex = 0;
            tabReports.Size = new Size(876, 400);
            tabReports.TabIndex = 1;
            //
            // tabGenres
            //
            tabGenres.Controls.Add(gridGenres);
            tabGenres.Location = new Point(4, 24);
            tabGenres.Name = "tabGenres";
            tabGenres.Padding = new Padding(3);
            tabGenres.Size = new Size(868, 372);
            tabGenres.TabIndex = 0;
            tabGenres.Text = "Performers by genre";
            tabGenres.UseVisualStyleBackColor = true;
            //
            // gridGenres
            //
            gridGenres.Dock = DockStyle.Fill;
            gridGenres.Name = "gridGenres";
            gridGenres.TabIndex = 0;
            //
            // tabCategories
            //
            tabCategories.Controls.Add(gridCategories);
            tabCategories.Location = new Point(4, 24);
            tabCategories.Name = "tabCategories";
            tabCategories.Padding = new Padding(3);
            tabCategories.Size = new Size(868, 372);
            tabCategories.TabIndex = 1;
            tabCategories.Text = "Vendors by category";
            tabCategories.UseVisualStyleBackColor = true;
            //
            // gridCategories
            //
            gridCategories.Dock = DockStyle.Fill;
            gridCategories.Name = "gridCategories";
            gridCategories.TabIndex = 0;
            //
            // tabCosts
            //
            tabCosts.Controls.Add(gridCosts);
            tabCosts.Location = new Point(4, 24);
            tabCosts.Name = "tabCosts";
            tabCosts.Padding = new Padding(3);
            tabCosts.Size = new Size(868, 372);
            tabCosts.TabIndex = 2;
            tabCosts.Text = "Cost estimates";
            tabCosts.UseVisualStyleBackColor = true;
            //
            // gridCosts
            //
            gridCosts.Dock = DockStyle.Fill;
            gridCosts.Name = "gridCosts";
            gridCosts.TabIndex = 0;
            //
            // btnClose
            //
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.OK;
            btnClose.Location = new Point(788, 460);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 32);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            //
            // FormReport
            //
            AcceptButton = btnClose;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(900, 504);
            Controls.Add(btnClose);
            Controls.Add(tabReports);
            Controls.Add(labelTitle);
            MinimizeBox = false;
            Name = "FormReport";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reports";
            tabReports.ResumeLayout(false);
            tabGenres.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridGenres).EndInit();
            tabCategories.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridCategories).EndInit();
            tabCosts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridCosts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitle;
        private TabControl tabReports;
        private TabPage tabGenres;
        private DataGridView gridGenres;
        private TabPage tabCategories;
        private DataGridView gridCategories;
        private TabPage tabCosts;
        private DataGridView gridCosts;
        private Button btnClose;
    }
}
