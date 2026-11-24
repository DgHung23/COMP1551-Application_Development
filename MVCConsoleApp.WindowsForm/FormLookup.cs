// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : FormLookup.cs
// Layer   : Presentation layer (Windows Forms user interface)
// Summary : Windows Forms dialog that manages one lookup list - music genres OR product categories.
// =====================================================================

using MVCConsoleApp.BusinessLogic.Common;
using MVCConsoleApp.BusinessLogic.Services;
using MVCConsoleApp.Data;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.WindowsForm
{
    /// <summary>
    /// Windows Forms dialog that manages one lookup list - music genres OR product categories.
    /// It follows the layout of the original "Person Role" form of the template: input on the
    /// left, list on the right. One form class serves both lists; the <see cref="LookupKind"/>
    /// passed to the constructor decides which one.
    /// </summary>
    public partial class FormLookup : Form
    {
        private readonly ILookupService _lookupService = new LookupService();
        private readonly LookupKind _kind;

        /// <summary>Creates the dialog for one lookup list.</summary>
        /// <param name="kind">Genre or ProductCategory.</param>
        public FormLookup(LookupKind kind)
        {
            // Remember which list this dialog manages; the titles below depend on it.
            InitializeComponent();
            _kind = kind;

            // Titles adapt to the list being managed.
            string title = kind == LookupKind.Genre ? "Genre" : "Product Category";
            Text = $"Manage {title.ToLowerInvariant()} list";
            labelLookupForm.Text = $"{title} Form";
            labelLookupList.Text = $"{title} List";

            InitializeDataGridView(gridViewLookupList);
        }

        /// <summary>Creates the grid columns and loads the items.</summary>
        private void InitializeDataGridView(DataGridView gridView)
        {
            // Explicit columns (same approach as the original template) instead of auto-generated ones.
            gridView.AutoGenerateColumns = false;
            gridView.Columns.Clear();
            gridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", DataPropertyName = "Id", FillWeight = 15 });
            gridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", DataPropertyName = "Name", FillWeight = 55 });
            gridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "UsageCount", HeaderText = "Used by", DataPropertyName = "UsageCount", FillWeight = 30 });
            LoadItems();
        }

        /// <summary>Reloads the items from the database.</summary>
        private void LoadItems()
        {
            try
            {
                // Re-bind the grid to the fresh list (with usage counts).
                gridViewLookupList.DataSource = null;
                gridViewLookupList.DataSource = _lookupService.GetAll(_kind);
            }
            catch (DataAccessException ex)
            {
                MessageBox.Show(ex.Message, "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Adds the name typed in the text box (validation happens in the Business layer).</summary>
        private void btnSave_Click(object? sender, EventArgs e)
        {
            // The Business layer validates the name (required, unique, valid characters).
            Run(() => _lookupService.Add(_kind, txbName.Text), clearInput: true);
        }

        /// <summary>Deletes the selected item after confirmation.</summary>
        private void btnDelete_Click(object? sender, EventArgs e)
        {
            // A row must be selected before anything can be deleted.
            if (gridViewLookupList.SelectedRows.Count == 0 || gridViewLookupList.SelectedRows[0].DataBoundItem is not LookupItem item)
            {
                MessageBox.Show("Please select an item in the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ask for confirmation; the service refuses if the item is still in use.
            if (MessageBox.Show($"Delete '{item.Name}'?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Run(() => _lookupService.Delete(_kind, item.Id), clearInput: false);
            }
        }

        /// <summary>Pressing Enter in the name box saves.</summary>
        private void txbName_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSave_Click(sender, e);
            }
        }

        /// <summary>Runs a service call, shows the result and refreshes the list.</summary>
        private void Run(Func<OperationResult> action, bool clearInput)
        {
            try
            {
                // Run the Business-layer operation (it validates and saves).
                OperationResult result = action();
                if (!result.Success)
                {
                    MessageBox.Show(result.ToString(), "Not possible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // After a successful add, empty the text box ready for the next name.
                if (clearInput)
                {
                    txbName.Text = string.Empty;
                }
                LoadItems();
            }
            catch (DataAccessException ex)
            {
                MessageBox.Show(ex.Message, "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
