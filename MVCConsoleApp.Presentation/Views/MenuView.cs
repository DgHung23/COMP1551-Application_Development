// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : MenuView.cs
// Layer   : Presentation layer (console) - VIEW (MVC)
// Summary : VIEW (MVC) - prints the menus of the console application and reads the user's choice.
// =====================================================================

using MVCConsoleApp.BusinessLogic.Services;

namespace MVCConsoleApp.Presentation.Views
{
    /// <summary>
    /// VIEW (MVC) - prints the menus of the console application and reads the user's choice.
    /// </summary>
    public class MenuView : ConsoleView
    {
        /// <summary>Highest option number of the main menu.</summary>
        public const int MainMenuMaxOption = 10;

        /// <summary>Prints the welcome banner shown once at start-up.</summary>
        public void ShowWelcome()
        {
            // Start with a clean screen and a coloured banner.
            Clear();
            WriteColoured("+----------------------------------------------------------+", ConsoleColor.Magenta);
            WriteColoured("|        MUSIC FESTIVAL OPERATIONS SYSTEM  (3-day event)    |", ConsoleColor.Magenta);
            WriteColoured("|        Performers  -  Crew  -  Vendors                    |", ConsoleColor.Magenta);
            WriteColoured("+----------------------------------------------------------+", ConsoleColor.Magenta);
        }

        /// <summary>Prints the main menu (core features 1-5, extension features 6-10, 0 = exit).</summary>
        public void ShowMainMenu()
        {
            WriteTitle("Main menu");
            // Core features required by the brief.
            Console.WriteLine("  1. Add a new record (Performer / Crew / Vendor)");
            Console.WriteLine("  2. View all records");
            Console.WriteLine("  3. View records by role");
            Console.WriteLine("  4. Edit a record by PersonID");
            Console.WriteLine("  5. Delete a record by PersonID");
            WriteColoured("  ---------------- Extra features ----------------", ConsoleColor.DarkGray);
            // Extension features (extra credit).
            Console.WriteLine("  6. Search by name or email (partial match)");
            Console.WriteLine("  7. Sort records");
            Console.WriteLine("  8. Reports (genres, categories, cost estimates)");
            Console.WriteLine("  9. Export records to CSV");
            Console.WriteLine(" 10. Recycle bin (restore / permanently delete)");
            Console.WriteLine("  0. Exit");
            Console.WriteLine();
        }

        /// <summary>Asks the user to choose how to sort the records.</summary>
        /// <returns>The chosen field and whether the order is descending.</returns>
        public (SortField Field, bool Descending) AskSortOptions()
        {
            WriteTitle("Sort records");
            Console.WriteLine("  1. PersonID");
            Console.WriteLine("  2. Name");
            Console.WriteLine("  3. Role");
            Console.WriteLine("  4. Email");
            Console.WriteLine("  5. Estimated cost");
            // The menu numbers 1-5 are identical to the SortField enum values, so a cast is enough.
            SortField field = (SortField)AskChoice(1, 5, "Sort by");
            // Enter = ascending (the default), y = descending.
            bool descending = Confirm("Descending order?", defaultAnswer: false);
            return (field, descending);
        }

        /// <summary>Prints the report sub-menu and returns the choice (0 = back).</summary>
        public int AskReportChoice()
        {
            WriteTitle("Reports");
            // Three reports are available; the controller calls the matching service method.
            Console.WriteLine("  1. Performers grouped by genre");
            Console.WriteLine("  2. Vendors grouped by product category");
            Console.WriteLine("  3. Cost estimates by role");
            Console.WriteLine("  0. Back to main menu");
            // AskChoice re-asks until a valid option is typed.
            return AskChoice(0, 3);
        }

        /// <summary>Prints the export sub-menu and returns the choice (0 = back).</summary>
        public int AskExportChoice()
        {
            WriteTitle("Export to CSV");
            // Export everything or just one role (the numbers map onto PersonRole values).
            Console.WriteLine("  1. All records");
            Console.WriteLine("  2. Performers only");
            Console.WriteLine("  3. Crew only");
            Console.WriteLine("  4. Vendors only");
            Console.WriteLine("  0. Back to main menu");
            return AskChoice(0, 4);
        }

        /// <summary>Prints the recycle-bin actions and returns the choice (0 = back).</summary>
        public int AskRecycleBinChoice()
        {
            // The list of deleted records has just been printed by PersonView.
            Console.WriteLine("  1. Restore a record");
            Console.WriteLine("  2. Permanently delete a record");
            Console.WriteLine("  0. Back to main menu");
            return AskChoice(0, 2);
        }
    }
}
