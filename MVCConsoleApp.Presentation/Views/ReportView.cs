// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : ReportView.cs
// Layer   : Presentation layer (console) - VIEW (MVC)
// Summary : VIEW (MVC) - prints reports and the result of operations.
// =====================================================================

using MVCConsoleApp.BusinessLogic.Services;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.Presentation.Views
{
    /// <summary>
    /// VIEW (MVC) - prints reports and the result of operations.
    /// </summary>
    public class ReportView : ConsoleView
    {
        /// <summary>Prints a "group by" report (genres or categories).</summary>
        /// <param name="title">Report title.</param>
        /// <param name="groupLabel">Name of the first column, e.g. "Genre".</param>
        /// <param name="lines">Report lines.</param>
        public void ShowGroupReport(string title, string groupLabel, IReadOnlyList<GroupSummary> lines)
        {
            WriteTitle(title);
            if (lines.Count == 0)
            {
                WriteWarning("No data.");
                return;
            }

            var rows = lines
                // Convert each report line into the text cells of one table row.
                .Select(l => (IReadOnlyList<string>)new List<string> { l.GroupName, l.Count.ToString(), l.Members })
                .ToList();
            // Columns: group name, number of people, and the names of those people.
            WriteTable(new[] { groupLabel, "Count", "Members" }, rows);
        }

        /// <summary>Prints the cost-estimate report; the TOTAL line is highlighted.</summary>
        public void ShowCostReport(IReadOnlyList<CostSummaryLine> lines)
        {
            WriteTitle("Cost estimates by role");
            var rows = lines
                .Select(l => (IReadOnlyList<string>)new List<string>
                {
                    // Money is formatted with Person.FormatMoney so it looks the same everywhere.
                    l.Role, l.Count.ToString(), Person.FormatMoney(l.TotalCost), Person.FormatMoney(l.AverageCost), l.Basis
                })
                .ToList();
            WriteTable(new[] { "Role", "People", "Total cost", "Average", "Basis" }, rows);
        }
    }
}
