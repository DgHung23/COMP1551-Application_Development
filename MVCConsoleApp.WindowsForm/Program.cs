// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : Program.cs
// Layer   : Presentation layer (Windows Forms user interface)
// Summary : Entry point of the Windows Forms version of the Music Festival Operations System.
// =====================================================================

namespace MVCConsoleApp.WindowsForm
{
    /// <summary>
    /// Entry point of the Windows Forms version of the Music Festival Operations System.
    /// It is a second Presentation layer on top of the same Business and Data layers used by
    /// the console application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Last-resort safety net: show unexpected errors instead of closing the application.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) =>
                MessageBox.Show(e.Exception.Message, "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Application.Run(new FormPerson());
        }
    }
}
