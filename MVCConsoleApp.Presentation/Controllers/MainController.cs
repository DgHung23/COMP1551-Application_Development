using MVCConsoleApp.Data;
using MVCConsoleApp.Presentation.Views;

namespace MVCConsoleApp.Presentation.Controllers
{
    /// <summary>
    /// CONTROLLER (MVC) - the main menu loop.
    /// <para>
    /// It shows the menu, reads a VALID choice (the view keeps asking until the input is a number
    /// in range) and dispatches it to <see cref="PersonController"/> or <see cref="ReportController"/>.
    /// It is also the single place where unexpected problems are caught, so the application never
    /// crashes: a cancelled operation or a database error is reported and the menu appears again.
    /// </para>
    /// </summary>
    public class MainController
    {
        private readonly MenuView _menuView;
        private readonly PersonController _personController;
        private readonly ReportController _reportController;

        /// <summary>Creates the main controller (injected by Program.cs).</summary>
        public MainController(MenuView menuView, PersonController personController, ReportController reportController)
        {
            _menuView = menuView;
            _personController = personController;
            _reportController = reportController;
        }

        /// <summary>Runs the application until the user chooses 0 (Exit).</summary>
        public void Run()
        {
            _menuView.ShowWelcome();

            while (true)
            {
                try
                {
                    _menuView.ShowMainMenu();
                    int choice = _menuView.AskChoice(0, MenuView.MainMenuMaxOption);

                    if (choice == 0)
                    {
                        if (_menuView.Confirm("Do you really want to exit?", defaultAnswer: true))
                        {
                            _menuView.WriteSuccess("Goodbye!");
                            return;
                        }
                        continue;
                    }

                    Dispatch(choice);
                }
                catch (InputCancelledException)
                {
                    // The user typed "cancel": go back to the menu without saving anything.
                    _menuView.WriteWarning("Operation cancelled - nothing was saved.");
                }
                catch (EndOfInputException)
                {
                    // The input stream was closed (Ctrl+Z or end of a redirected file): exit cleanly.
                    return;
                }
                catch (DataAccessException ex)
                {
                    _menuView.WriteError(ex.Message);
                    _menuView.WriteWarning("Check that the SQL Server Docker container is running and that " +
                                           "Database/databasescheme.sql has been executed.");
                }
                catch (ArgumentException ex)
                {
                    // Raised by an entity setter (encapsulation) if an invalid value slipped through.
                    _menuView.WriteError(ex.Message);
                }

                try
                {
                    _menuView.Pause();
                }
                catch (EndOfInputException)
                {
                    return;
                }
            }
        }

        /// <summary>Calls the controller action that belongs to a menu option.</summary>
        /// <param name="choice">A valid menu option (1-10).</param>
        private void Dispatch(int choice)
        {
            switch (choice)
            {
                case 1: _personController.AddPerson(); break;
                case 2: _personController.ViewAll(); break;
                case 3: _personController.ViewByRole(); break;
                case 4: _personController.EditPerson(); break;
                case 5: _personController.DeletePerson(); break;
                case 6: _personController.Search(); break;
                case 7: _personController.Sort(_menuView); break;
                case 8: _reportController.ShowReports(); break;
                case 9: _reportController.ExportCsv(); break;
                case 10: _personController.RecycleBin(_menuView); break;
            }
        }
    }
}
