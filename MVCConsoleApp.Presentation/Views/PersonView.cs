using System.Globalization;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.Presentation.Views
{
    /// <summary>
    /// VIEW (MVC) - everything the user sees or types about people: tables, the details card and
    /// the prompts used when adding or editing a record.
    /// <para>
    /// Thanks to polymorphism the view never needs to know whether it is printing a Performer,
    /// a CrewMember or a Vendor: it calls <see cref="Person.DescribeRoleFields"/> and every
    /// object describes its own role-specific fields.
    /// </para>
    /// </summary>
    public class PersonView : ConsoleView
    {
        // ================================ Output ================================

        /// <summary>
        /// Prints a list of people of mixed roles. The role-specific fields are summarised in one
        /// "Details" column.
        /// </summary>
        /// <param name="people">People to print.</param>
        /// <param name="title">Table title.</param>
        /// <param name="showCost">True to add an "Est. cost" column (used by the sort feature).</param>
        public void ShowPeopleTable(IReadOnlyCollection<Person> people, string title, bool showCost = false)
        {
            WriteTitle(title);
            if (people.Count == 0)
            {
                WriteWarning("No records found.");
                return;
            }

            var headers = new List<string> { "ID", "Name", "Role", "Email", "Telephone" };
            if (showCost) headers.Add("Est. cost");
            headers.Add("Details");

            var rows = new List<IReadOnlyList<string>>();
            foreach (Person person in people)
            {
                var row = new List<string> { person.PersonId.ToString(), person.FullName, person.RoleName, person.Email, person.Telephone };
                if (showCost) row.Add(person.EstimatedCostText);
                row.Add(person.RoleDetails);   // polymorphic summary
                rows.Add(row);
            }

            WriteTable(headers, rows);
            WriteInfo($" {people.Count} record(s).");
        }

        /// <summary>
        /// Prints people that all have the same role. Each role-specific field gets its own column;
        /// the column titles come from <see cref="Person.DescribeRoleFields"/> of the first person.
        /// </summary>
        /// <param name="people">People of one role.</param>
        /// <param name="role">The role (used for the title).</param>
        public void ShowRoleTable(IReadOnlyList<Person> people, PersonRole role)
        {
            WriteTitle($"{role} records");
            if (people.Count == 0)
            {
                WriteWarning($"There are no {role} records.");
                return;
            }

            var headers = new List<string> { "ID", "Name", "Email", "Telephone" };
            headers.AddRange(people[0].DescribeRoleFields().Select(field => field.Key));

            var rows = new List<IReadOnlyList<string>>();
            foreach (Person person in people)
            {
                var row = new List<string> { person.PersonId.ToString(), person.FullName, person.Email, person.Telephone };
                row.AddRange(person.DescribeRoleFields().Select(field => field.Value));
                rows.Add(row);
            }

            WriteTable(headers, rows);
            WriteInfo($" {people.Count} {role} record(s).");
        }

        /// <summary>Prints the recycle bin (soft-deleted people with the date they were deleted).</summary>
        public void ShowDeletedTable(IReadOnlyCollection<Person> people)
        {
            WriteTitle("Recycle bin");
            if (people.Count == 0)
            {
                WriteWarning("The recycle bin is empty.");
                return;
            }

            var headers = new List<string> { "ID", "Name", "Role", "Email", "Deleted at" };
            var rows = people
                .Select(p => (IReadOnlyList<string>)new List<string>
                {
                    p.PersonId.ToString(), p.FullName, p.RoleName, p.Email,
                    p.DeletedAt?.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) ?? string.Empty
                })
                .ToList();
            WriteTable(headers, rows);
            WriteInfo($" {people.Count} deleted record(s).");
        }

        /// <summary>Prints every field of one person as a "card".</summary>
        /// <param name="person">Person to show.</param>
        /// <param name="title">Card title.</param>
        public void ShowPersonDetails(Person person, string title = "Record details")
        {
            WriteTitle(title);
            WriteField("PersonID", person.PersonId == 0 ? "(new)" : person.PersonId.ToString());
            WriteField("Name", person.FullName);
            WriteField("Role", person.RoleName);
            WriteField("Telephone", person.Telephone);
            WriteField("Email", person.Email);

            // Role-specific part: each sub-class describes its own fields (polymorphism).
            foreach (KeyValuePair<string, string> field in person.DescribeRoleFields())
            {
                WriteField(field.Key, field.Value);
            }
            WriteField("Est. cost", person.EstimatedCostText);
        }

        /// <summary>Writes "  Label........: value" with the label in a fixed-width column.</summary>
        private static void WriteField(string label, string value)
        {
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"  {label,-13}: ");
            Console.ForegroundColor = previous;
            Console.WriteLine(value);
        }

        // ================================ Prompts ================================

        /// <summary>Asks for a role. In edit mode Enter keeps <paramref name="current"/>.</summary>
        public PersonRole AskRole(PersonRole? current = null)
        {
            Console.WriteLine("  Role:  1. Performer   2. Crew   3. Vendor");
            while (true)
            {
                string prompt = current == null ? "  Choose role (1-3): " : $"  Choose role (1-3) [{current}]: ";
                string input = ReadLine(prompt);
                if (input.Length == 0 && current != null)
                {
                    return current.Value;
                }
                if (int.TryParse(input, out int value) && Enum.IsDefined(typeof(PersonRole), value))
                {
                    return (PersonRole)value;
                }
                WriteError("Please enter 1 (Performer), 2 (Crew) or 3 (Vendor).");
            }
        }

        /// <summary>Asks for an employment type. In edit mode Enter keeps <paramref name="current"/>.</summary>
        public EmploymentType AskEmploymentType(EmploymentType? current = null)
        {
            Console.WriteLine("  Employment type:  1. FullTime   2. PartTime");
            while (true)
            {
                string prompt = current == null ? "  Choose type (1-2): " : $"  Choose type (1-2) [{current}]: ";
                string input = ReadLine(prompt);
                if (input.Length == 0 && current != null)
                {
                    return current.Value;
                }
                if (input == "1" || input.Equals("fulltime", StringComparison.OrdinalIgnoreCase)) return EmploymentType.FullTime;
                if (input == "2" || input.Equals("parttime", StringComparison.OrdinalIgnoreCase)) return EmploymentType.PartTime;
                WriteError("Please enter 1 (FullTime) or 2 (PartTime).");
            }
        }

        /// <summary>
        /// Asks for a comma-separated list of names (genres or categories). Existing names are shown
        /// as a hint; a new name is accepted and will be added to the list automatically.
        /// </summary>
        /// <param name="label">e.g. "Genres".</param>
        /// <param name="available">Names that already exist (shown as a hint).</param>
        /// <param name="requiredCount">How many names are required.</param>
        /// <param name="validate">Validation function from the Business layer.</param>
        /// <param name="current">Current names when editing; Enter keeps them.</param>
        /// <returns>The accepted list.</returns>
        public List<string> AskNameList(string label, IEnumerable<string> available, int requiredCount,
                                        Func<IEnumerable<string>, string?> validate, IReadOnlyList<string>? current = null)
        {
            WriteInfo($"  Available {label.ToLowerInvariant()}: {string.Join(", ", available)}");
            WriteInfo($"  Enter exactly {requiredCount}, separated by commas (a new name will be added to the list).");

            while (true)
            {
                string currentText = current == null ? string.Empty : string.Join(", ", current);
                string input = ReadLine(current == null ? $"  {label}: " : $"  {label} [{currentText}]: ");
                if (input.Length == 0 && current != null)
                {
                    return current.ToList();
                }

                // Split "Rock, Pop" into ["Rock", "Pop"], ignoring empty pieces.
                List<string> names = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                // Re-use the exact spelling of an existing name ("rock" -> "Rock") to keep the list tidy.
                names = names.Select(name => available.FirstOrDefault(a => a.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? name).ToList();

                string? error = validate(names);
                if (error == null)
                {
                    return names;
                }
                WriteError(error);
            }
        }

        /// <summary>Asks for a PersonID (a positive whole number).</summary>
        /// <param name="action">What the ID is for, e.g. "edit".</param>
        public int AskPersonId(string action)
        {
            return AskInt($"Enter the PersonID to {action}",
                          id => id > 0 ? null : "PersonID must be a positive whole number.");
        }
    }
}
