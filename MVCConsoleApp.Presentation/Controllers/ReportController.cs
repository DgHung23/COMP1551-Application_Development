// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : ReportController.cs
// Layer   : Presentation layer (console) - CONTROLLER (MVC)
// Summary : CONTROLLER (MVC) - handles the report and CSV export menu actions (extension features).
// =====================================================================

using MVCConsoleApp.BusinessLogic.Common;
using MVCConsoleApp.BusinessLogic.Services;
using MVCConsoleApp.Data.Entities;
using MVCConsoleApp.Presentation.Views;

namespace MVCConsoleApp.Presentation.Controllers
{
    /// <summary>
    /// CONTROLLER (MVC) - handles the report and CSV export menu actions (extension features).
    /// </summary>
    public class ReportController
    {
        private readonly IReportService _reportService;
        private readonly IPersonService _personService;
        private readonly ReportView _view;
        private readonly MenuView _menuView;

        /// <summary>Creates the controller with its services and views (injected by Program.cs).</summary>
        public ReportController(IReportService reportService, IPersonService personService, ReportView view, MenuView menuView)
        {
            // Keep references to the collaborators; the controller never creates them itself
            // (dependency injection), which keeps it loosely coupled and easy to test.
            _reportService = reportService;
            _personService = personService;
            _view = view;
            _menuView = menuView;
        }

        /// <summary>Menu option 8: shows the report sub-menu until the user goes back.</summary>
        public void ShowReports()
        {
            // Stay in the report sub-menu so several reports can be viewed one after another.
            while (true)
            {
                int choice = _menuView.AskReportChoice();
                switch (choice)
                {
                    // SQL GROUP BY report: number of performers per genre.
                    case 1:
                        _view.ShowGroupReport("Performers grouped by genre", "Genre", _reportService.GetPerformersByGenre());
                        break;
                    // SQL GROUP BY report: number of vendors per product category.
                    case 2:
                        _view.ShowGroupReport("Vendors grouped by product category", "Category", _reportService.GetVendorsByCategory());
                        break;
                    // Cost estimates are calculated in C# with the polymorphic EstimateCost().
                    case 3:
                        _view.ShowCostReport(_reportService.GetCostSummary());
                        break;
                    default:
                        return; // 0 = back to the main menu
                }
            }
        }

        /// <summary>Menu option 9: exports all records, or one role, to a CSV file.</summary>
        public void ExportCsv()
        {
            // Ask what to export; 0 returns to the main menu without doing anything.
            int choice = _menuView.AskExportChoice();
            if (choice == 0)
            {
                return;
            }

            // Option 1 = everything; options 2-4 map to the roles Performer (1), Crew (2), Vendor (3).
            List<Person> people = choice == 1
                ? _personService.GetAll()
                : _personService.GetByRole((PersonRole)(choice - 1));

            // The Business layer writes the file and returns its full path (or the reason it failed),
            // so the user knows exactly where to find the CSV file.
            OperationResult<string> result = _reportService.ExportToCsv(people);
            _view.ShowResult(result);
        }
    }
}
