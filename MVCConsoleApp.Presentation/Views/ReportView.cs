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
                .Select(l => (IReadOnlyList<string>)new List<string> { l.GroupName, l.Count.ToString(), l.Members })
                .ToList();
            WriteTable(new[] { groupLabel, "Count", "Members" }, rows);
        }

        /// <summary>Prints the cost-estimate report; the TOTAL line is highlighted.</summary>
        public void ShowCostReport(IReadOnlyList<CostSummaryLine> lines)
        {
            WriteTitle("Cost estimates by role");
            var rows = lines
                .Select(l => (IReadOnlyList<string>)new List<string>
                {
                    l.Role, l.Count.ToString(), Person.FormatMoney(l.TotalCost), Person.FormatMoney(l.AverageCost), l.Basis
                })
                .ToList();
            WriteTable(new[] { "Role", "People", "Total cost", "Average", "Basis" }, rows);
        }
    }
}
