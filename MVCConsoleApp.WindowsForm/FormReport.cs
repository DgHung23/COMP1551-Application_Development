// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : FormReport.cs
// Layer   : Presentation layer (Windows Forms user interface)
// Summary : Windows Forms dialog that shows the three reports (extension feature) in tabs:
// =====================================================================

using MVCConsoleApp.BusinessLogic.Services;
using MVCConsoleApp.Data;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.WindowsForm
{
    /// <summary>
    /// Windows Forms dialog that shows the three reports (extension feature) in tabs:
    /// performers by genre, vendors by product category and cost estimates by role.
    /// The data comes from the same <see cref="IReportService"/> used by the console application.
    /// </summary>
    public partial class FormReport : Form
    {
        private readonly IReportService _reportService;

        /// <summary>Creates the dialog and loads every report.</summary>
        /// <param name="reportService">Report service shared with the main form.</param>
        public FormReport(IReportService reportService)
        {
            // The service is passed in by the main form (shared instance, no new connection setup).
            InitializeComponent();
            _reportService = reportService;

            // Give the three grids the same look before loading the data.
            foreach (DataGridView grid in new[] { gridGenres, gridCategories, gridCosts })
            {
                StyleGrid(grid);
            }
            LoadReports();
        }

        /// <summary>Loads the data of the three reports into their grids.</summary>
        private void LoadReports()
        {
            try
            {
                // Tabs 1 and 2: "group by" reports produced by SQL GROUP BY queries.
                BindGroupReport(gridGenres, _reportService.GetPerformersByGenre(), "Genre", "Performers");
                BindGroupReport(gridCategories, _reportService.GetVendorsByCategory(), "Category", "Vendors");

                // Cost report: format money values for display.
                // Tab 3: cost estimates; an anonymous type gives display-ready column values.
                gridCosts.DataSource = _reportService.GetCostSummary()
                    .Select(line => new
                    {
                        line.Role,
                        People = line.Count,
                        TotalCost = Person.FormatMoney(line.TotalCost),
                        Average = Person.FormatMoney(line.AverageCost),
                        line.Basis
                    })
                    .ToList();
                // Friendlier header text and a wider column for the explanation.
                gridCosts.Columns["TotalCost"]!.HeaderText = "Total cost";
                gridCosts.Columns["Basis"]!.FillWeight = 200;
                if (gridCosts.Rows.Count > 0)
                {
                    // Highlight the TOTAL line (the last row).
                    gridCosts.Rows[gridCosts.Rows.Count - 1].DefaultCellStyle.Font = new Font(gridCosts.Font, FontStyle.Bold);
                }
            }
            catch (DataAccessException ex)
            {
                MessageBox.Show(ex.Message, "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Binds a "group by" report and renames its columns.</summary>
        private static void BindGroupReport(DataGridView grid, List<GroupSummary> lines, string groupHeader, string membersHeader)
        {
            // Auto-generated columns are renamed to friendly headers below.
            grid.DataSource = lines;
            grid.Columns[nameof(GroupSummary.GroupName)]!.HeaderText = groupHeader;
            grid.Columns[nameof(GroupSummary.Count)]!.FillWeight = 15;
            grid.Columns[nameof(GroupSummary.Members)]!.HeaderText = membersHeader;
            grid.Columns[nameof(GroupSummary.Members)]!.FillWeight = 160;
        }

        /// <summary>Applies the same read-only look as the main list.</summary>
        private static void StyleGrid(DataGridView grid)
        {
            // Reports are read-only: no editing, adding or deleting rows.
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.BackgroundColor = Color.White;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }
    }
}
