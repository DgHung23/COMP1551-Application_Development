using System.Globalization;
using MVCConsoleApp.BusinessLogic.Common;

namespace MVCConsoleApp.Presentation.Views
{
    /// <summary>Thrown when the user types "cancel" at a prompt to abandon the current operation.</summary>
    public class InputCancelledException : Exception
    {
        /// <summary>Creates the exception with a standard message.</summary>
        public InputCancelledException() : base("Operation cancelled by the user.")
        {
        }
    }

    /// <summary>Thrown when the input stream is closed (e.g. Ctrl+Z / end of a redirected file).</summary>
    public class EndOfInputException : Exception
    {
        /// <summary>Creates the exception with a standard message.</summary>
        public EndOfInputException() : base("No more input is available.")
        {
        }
    }

    /// <summary>
    /// VIEW (MVC) - abstract base class of every console view.
    /// <para>
    /// It contains the reusable console input/output code: coloured messages, tables and
    /// SAFE input methods that never crash on bad input - they show an error and ask again
    /// (e.g. letters typed where a number is expected). Concrete views (<see cref="MenuView"/>,
    /// <see cref="PersonView"/>, <see cref="ReportView"/>) inherit these helpers.
    /// </para>
    /// <para>
    /// Views only read and write the console. They contain no SQL and no business rules:
    /// when a value must be validated, the controller passes a validation function that comes
    /// from the Business layer and the view simply calls it.
    /// </para>
    /// </summary>
    public abstract class ConsoleView
    {
        /// <summary>Word the user can type at any prompt to cancel the current operation.</summary>
        public const string CancelKeyword = "cancel";

        // ============================== Output helpers ==============================

        /// <summary>Clears the screen (only when the console is interactive, not redirected).</summary>
        public void Clear()
        {
            if (!Console.IsOutputRedirected)
            {
                try
                {
                    Console.Clear();
                }
                catch (IOException)
                {
                    // Some hosts do not support clearing; ignoring it is harmless.
                }
            }
        }

        /// <summary>Writes a highlighted section title.</summary>
        public void WriteTitle(string title)
        {
            Console.WriteLine();
            WriteColoured(new string('=', title.Length + 8), ConsoleColor.DarkCyan);
            WriteColoured($"    {title.ToUpperInvariant()}", ConsoleColor.Cyan);
            WriteColoured(new string('=', title.Length + 8), ConsoleColor.DarkCyan);
        }

        /// <summary>Writes a success message in green.</summary>
        public void WriteSuccess(string message) => WriteColoured("[OK] " + message, ConsoleColor.Green);

        /// <summary>Writes an error message in red.</summary>
        public void WriteError(string message) => WriteColoured("[!] " + message, ConsoleColor.Red);

        /// <summary>Writes a warning in yellow.</summary>
        public void WriteWarning(string message) => WriteColoured("[i] " + message, ConsoleColor.Yellow);

        /// <summary>Writes an informational line in grey.</summary>
        public void WriteInfo(string message) => WriteColoured(message, ConsoleColor.Gray);

        /// <summary>Waits until the user presses Enter.</summary>
        public void Pause()
        {
            Console.WriteLine();
            WriteColoured("Press Enter to continue...", ConsoleColor.DarkGray);
            Console.ReadLine();
        }

        /// <summary>
        /// Prints the outcome of an operation: green for success, red with every error for failure.
        /// Shared by all controllers so every message looks the same.
        /// </summary>
        public void ShowResult(OperationResult result)
        {
            if (result.Success)
            {
                WriteSuccess(result.Message);
                return;
            }

            WriteError(result.Message);
            foreach (string error in result.Errors)
            {
                WriteColoured("    - " + error, ConsoleColor.Red);
            }
        }

        /// <summary>Writes one line in the given colour and restores the previous colour.</summary>
        protected static void WriteColoured(string text, ConsoleColor colour)
        {
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            Console.WriteLine(text);
            Console.ForegroundColor = previous;
        }

        /// <summary>
        /// Prints a table. Column widths are calculated from the content; the LAST column gets the
        /// remaining width of the console window and long values are shortened with "...".
        /// </summary>
        /// <param name="headers">Column titles.</param>
        /// <param name="rows">Rows; each row must have the same number of cells as headers.</param>
        protected void WriteTable(IReadOnlyList<string> headers, IReadOnlyList<IReadOnlyList<string>> rows)
        {
            const int maxColumnWidth = 32;
            int columns = headers.Count;
            int[] widths = new int[columns];

            // 1) Width of each column = longest value (header or cell), capped at maxColumnWidth.
            for (int c = 0; c < columns; c++)
            {
                int longest = headers[c].Length;
                foreach (IReadOnlyList<string> row in rows)
                {
                    longest = Math.Max(longest, (row[c] ?? string.Empty).Length);
                }
                widths[c] = Math.Min(longest, maxColumnWidth);
            }

            // 2) The last column uses whatever space is left in the window (at least 20 chars).
            int used = widths.Take(columns - 1).Sum() + (columns - 1) * 3 + 2;
            int lastWidth = Math.Max(20, GetWindowWidth() - used - 1);
            int lastLongest = Math.Max(headers[columns - 1].Length, rows.Count == 0 ? 0 : rows.Max(r => (r[columns - 1] ?? string.Empty).Length));
            widths[columns - 1] = Math.Min(lastWidth, Math.Max(lastLongest, headers[columns - 1].Length));

            // 3) Header, separator and rows.
            string separator = " " + string.Join("-+-", widths.Select(w => new string('-', w)));
            WriteColoured(" " + string.Join(" | ", headers.Select((h, i) => Fit(h, widths[i]))), ConsoleColor.Cyan);
            WriteColoured(separator, ConsoleColor.DarkCyan);
            foreach (IReadOnlyList<string> row in rows)
            {
                Console.WriteLine(" " + string.Join(" | ", row.Select((cell, i) => Fit(cell, widths[i]))));
            }
            WriteColoured(separator, ConsoleColor.DarkCyan);
        }

        /// <summary>Pads or shortens a value to exactly <paramref name="width"/> characters.</summary>
        private static string Fit(string? value, int width)
        {
            value ??= string.Empty;
            if (value.Length <= width) return value.PadRight(width);
            return width <= 3 ? value.Substring(0, width) : value.Substring(0, width - 3) + "...";
        }

        /// <summary>Returns the console width, or 120 when it cannot be read (redirected output).</summary>
        private static int GetWindowWidth()
        {
            try
            {
                return Console.IsOutputRedirected ? 120 : Math.Max(80, Console.WindowWidth);
            }
            catch (IOException)
            {
                return 120;
            }
        }

        // =============================== Input helpers ===============================

        /// <summary>
        /// Reads one line. Returns the trimmed text, throws <see cref="InputCancelledException"/> when
        /// the user types "cancel" and <see cref="EndOfInputException"/> when the input is closed.
        /// </summary>
        protected static string ReadLine(string prompt)
        {
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(prompt);
            Console.ForegroundColor = previous;

            string? line = Console.ReadLine();
            if (line == null)
            {
                throw new EndOfInputException();
            }
            line = line.Trim();
            if (line.Equals(CancelKeyword, StringComparison.OrdinalIgnoreCase))
            {
                throw new InputCancelledException();
            }
            return line;
        }

        /// <summary>
        /// Asks for a text value until it passes validation.
        /// </summary>
        /// <param name="label">Field label shown to the user.</param>
        /// <param name="validate">Validation function from the Business layer (null error = valid).</param>
        /// <param name="current">Current value when editing; pressing Enter keeps it.</param>
        /// <returns>The accepted value.</returns>
        public string AskText(string label, Func<string, string?>? validate = null, string? current = null)
        {
            while (true)
            {
                string input = ReadLine(current == null ? $"  {label}: " : $"  {label} [{current}]: ");

                // Edit mode: an empty answer keeps the current value.
                if (input.Length == 0 && current != null)
                {
                    return current;
                }

                string? error = input.Length == 0 ? $"{label} is required." : validate?.Invoke(input);
                if (error == null)
                {
                    return input;
                }
                WriteError(error);
            }
        }

        /// <summary>
        /// Asks for a decimal number (e.g. money) until it is a valid number that passes validation.
        /// Accepts "1200", "1,200.50" and an optional £ sign.
        /// </summary>
        public decimal AskDecimal(string label, Func<decimal, string?>? validate = null, decimal? current = null)
        {
            while (true)
            {
                string shownCurrent = current?.ToString("0.00", CultureInfo.InvariantCulture) ?? string.Empty;
                string input = ReadLine(current == null ? $"  {label}: " : $"  {label} [{shownCurrent}]: ");
                if (input.Length == 0 && current != null)
                {
                    return current.Value;
                }

                // TryParse instead of Parse: invalid text never throws, the user is simply asked again.
                string cleaned = input.Replace("£", string.Empty).Trim();
                if (!decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
                {
                    WriteError($"'{input}' is not a valid number. Example: 1250.50");
                    continue;
                }

                string? error = validate?.Invoke(value);
                if (error == null)
                {
                    return value;
                }
                WriteError(error);
            }
        }

        /// <summary>Asks for a whole number until it is valid and passes validation.</summary>
        public int AskInt(string label, Func<int, string?>? validate = null, int? current = null)
        {
            while (true)
            {
                string input = ReadLine(current == null ? $"  {label}: " : $"  {label} [{current}]: ");
                if (input.Length == 0 && current != null)
                {
                    return current.Value;
                }
                if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                {
                    WriteError($"'{input}' is not a valid whole number.");
                    continue;
                }

                string? error = validate?.Invoke(value);
                if (error == null)
                {
                    return value;
                }
                WriteError(error);
            }
        }

        /// <summary>Asks for a menu option between <paramref name="min"/> and <paramref name="max"/>.</summary>
        public int AskChoice(int min, int max, string prompt = "Choose an option")
        {
            while (true)
            {
                string input = ReadLine($"{prompt} ({min}-{max}): ");
                if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                WriteError($"Invalid choice '{input}'. Please enter a number from {min} to {max}.");
            }
        }

        /// <summary>Asks a yes/no question. Enter returns <paramref name="defaultAnswer"/>.</summary>
        public bool Confirm(string question, bool defaultAnswer = false)
        {
            string hint = defaultAnswer ? "Y/n" : "y/N";
            while (true)
            {
                string input = ReadLine($"{question} ({hint}): ").ToLowerInvariant();
                if (input.Length == 0) return defaultAnswer;
                if (input == "y" || input == "yes") return true;
                if (input == "n" || input == "no") return false;
                WriteError("Please answer y (yes) or n (no).");
            }
        }
    }
}
