// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : PersonController.cs
// Layer   : Presentation layer (console) - CONTROLLER (MVC)
// Summary : CONTROLLER (MVC) - handles the menu actions about people: add, view all, view by role,
// =====================================================================

using MVCConsoleApp.BusinessLogic.Common;
using MVCConsoleApp.BusinessLogic.Services;
using MVCConsoleApp.BusinessLogic.Validation;
using MVCConsoleApp.Data.Entities;
using MVCConsoleApp.Presentation.Views;

namespace MVCConsoleApp.Presentation.Controllers
{
    /// <summary>
    /// CONTROLLER (MVC) - handles the menu actions about people: add, view all, view by role,
    /// edit, delete, search, sort and the recycle bin.
    /// <para>
    /// The controller coordinates the flow: it asks the <see cref="PersonView"/> for input, calls
    /// the Business layer (<see cref="IPersonService"/>, <see cref="ILookupService"/>) and asks the
    /// view to display the result. It contains no SQL and no console code of its own.
    /// </para>
    /// </summary>
    public class PersonController
    {
        private readonly IPersonService _personService;
        private readonly ILookupService _lookupService;
        private readonly PersonView _view;

        /// <summary>Creates the controller with its services and view (injected by Program.cs).</summary>
        public PersonController(IPersonService personService, ILookupService lookupService, PersonView view)
        {
            // Store the injected dependencies. The controller only knows the INTERFACES of the
            // services, not their implementation (abstraction).
            _personService = personService;
            _lookupService = lookupService;
            _view = view;
        }

        // ============================ 1. Add a new record ============================

        /// <summary>Menu option 1: asks for every field, shows a summary, then saves after confirmation.</summary>
        public void AddPerson()
        {
            _view.WriteTitle("Add a new record");
            _view.WriteInfo($"Type '{ConsoleView.CancelKeyword}' at any prompt to return to the main menu.");

            // Common fields - each one is validated immediately with the Business-layer rules.
            PersonRole role = _view.AskRole();
            string name = _view.AskText("Full name", FieldRules.CheckName);
            string telephone = _view.AskText("Telephone", FieldRules.CheckTelephone);
            string email = _view.AskText("Email", value => CheckEmail(value, null));

            // Role-specific fields.
            Person person = AskRoleDetails(role, name, telephone, email, current: null);

            // Show a summary first so the user can check the data before it is saved.
            _view.ShowPersonDetails(person, "Please review the new record");
            if (!_view.Confirm("Save this record?", defaultAnswer: true))
            {
                _view.WriteWarning("The record was not saved.");
                return;
            }

            // The service validates everything again (defence in depth), checks that the email
            // is still unique and saves the record; the view then prints the outcome.
            OperationResult<int> result = _personService.Add(person);
            _view.ShowResult(result);
        }

        // ============================ 2. View all records ============================

        /// <summary>Menu option 2: lists every active record.</summary>
        public void ViewAll()
        {
            // Load every active record from the database and print it as one table.
            _view.ShowPeopleTable(_personService.GetAll(), "All records");
        }

        // ============================ 3. View by role ============================

        /// <summary>Menu option 3: asks for a role and lists only the records of that role.</summary>
        public void ViewByRole()
        {
            _view.WriteTitle("View records by role");
            PersonRole role = _view.AskRole();
            // Only records of the chosen role are loaded (WHERE RoleId = @roleId), and the table
            // shows the role-specific columns of that role.
            _view.ShowRoleTable(_personService.GetByRole(role), role);
        }

        // ============================ 4. Edit a record ============================

        /// <summary>
        /// Menu option 4: edits a record by PersonID. Every prompt shows the current value and
        /// pressing Enter keeps it. The role can be changed as well.
        /// </summary>
        public void EditPerson()
        {
            _view.WriteTitle("Edit a record");
            // AskPersonId only accepts a positive whole number.
            int personId = _view.AskPersonId("edit");

            // Graceful handling of an ID that does not exist.
            Person? existing = _personService.GetById(personId);
            if (existing == null)
            {
                _view.WriteError($"No record with PersonID {personId} was found.");
                return;
            }

            _view.ShowPersonDetails(existing, "Current values");
            _view.WriteInfo("Press Enter to keep the value shown in [brackets].");

            // Each prompt shows the current value in [brackets]; pressing Enter keeps it unchanged.
            string name = _view.AskText("Full name", FieldRules.CheckName, existing.FullName);
            string telephone = _view.AskText("Telephone", FieldRules.CheckTelephone, existing.Telephone);
            // The person may keep his/her own email: personId is excluded from the uniqueness check.
            string email = _view.AskText("Email", value => CheckEmail(value, personId), existing.Email);

            PersonRole role = _view.AskRole(existing.Role);
            if (role != existing.Role)
            {
                _view.WriteWarning($"Role changes from {existing.Role} to {role}: please enter the new {role} details.");
            }

            // When the role is unchanged the current role-specific values are offered as defaults.
            Person updated = AskRoleDetails(role, name, telephone, email, role == existing.Role ? existing : null);
            // The new object must keep the same PersonID so that the right record is updated.
            updated.PersonId = personId;

            _view.ShowPersonDetails(updated, "New values");
            if (!_view.Confirm("Save the changes?", defaultAnswer: true))
            {
                _view.WriteWarning("No changes were saved.");
                return;
            }

            // Validate + save through the Business layer and show the outcome.
            _view.ShowResult(_personService.Update(updated));
        }

        // ============================ 5. Delete a record ============================

        /// <summary>Menu option 5: deletes a record by PersonID after showing it and asking for confirmation.</summary>
        public void DeletePerson()
        {
            _view.WriteTitle("Delete a record");
            int personId = _view.AskPersonId("delete");

            // Look the record up first so a wrong ID is reported clearly and the user can see
            // exactly which record is about to be deleted.
            Person? person = _personService.GetById(personId);
            if (person == null)
            {
                _view.WriteError($"No record with PersonID {personId} was found.");
                return;
            }

            _view.ShowPersonDetails(person, "Record to delete");
            // Confirmation is required by the brief; the default answer is "No" for safety.
            if (!_view.Confirm($"Are you sure you want to delete '{person.FullName}'?", defaultAnswer: false))
            {
                _view.WriteWarning("Deletion cancelled - nothing was deleted.");
                return;
            }

            // Soft delete: the record moves to the recycle bin and can be restored (option 10).
            _view.ShowResult(_personService.Delete(personId));
        }

        // ============================ 6. Search (extension) ============================

        /// <summary>Menu option 6: partial-match search on name or email.</summary>
        public void Search()
        {
            _view.WriteTitle("Search");
            // Partial, case-insensitive match on name OR email (SQL LIKE '%term%').
            string term = _view.AskText("Part of a name or email");
            _view.ShowPeopleTable(_personService.Search(term), $"Results for \"{term}\"");
        }

        // ============================ 7. Sort (extension) ============================

        /// <summary>Menu option 7: lists every record sorted by the chosen field.</summary>
        /// <param name="menuView">View that asks for the sort options.</param>
        public void Sort(MenuView menuView)
        {
            (SortField field, bool descending) = menuView.AskSortOptions();
            // Sorting is done by the Business layer, so the console and Windows Forms sort identically.
            List<Person> sorted = _personService.Sort(_personService.GetAll(), field, descending);
            string direction = descending ? "descending" : "ascending";
            string fieldName = field == SortField.EstimatedCost ? "estimated cost" : field.ToString();
            _view.ShowPeopleTable(sorted, $"All records sorted by {fieldName} ({direction})", showCost: true);
        }

        // ======================== 10. Recycle bin (extension) ========================

        /// <summary>Menu option 10: shows deleted records and lets the user restore or permanently delete them.</summary>
        /// <param name="menuView">View that prints the recycle-bin actions.</param>
        public void RecycleBin(MenuView menuView)
        {
            while (true)
            {
                // Reload the recycle bin every time so the list reflects the last action.
                List<Person> deleted = _personService.GetDeleted();
                _view.ShowDeletedTable(deleted);
                if (deleted.Count == 0)
                {
                    return;
                }

                int choice = menuView.AskRecycleBinChoice();
                if (choice == 0)
                {
                    return;
                }

                // Option 1 = restore, option 2 = permanent delete (which needs a confirmation).
                int personId = _view.AskPersonId(choice == 1 ? "restore" : "permanently delete");
                if (choice == 1)
                {
                    _view.ShowResult(_personService.Restore(personId));
                }
                else if (_view.Confirm($"PersonID {personId} will be removed for ever. Continue?", defaultAnswer: false))
                {
                    _view.ShowResult(_personService.DeletePermanently(personId));
                }
                else
                {
                    _view.WriteWarning("Nothing was deleted.");
                }
            }
        }

        // ============================== Helpers ==============================

        /// <summary>
        /// Validation function for the email prompt: checks the format AND that no other person
        /// already uses the address (business rule: email must be unique).
        /// </summary>
        /// <param name="email">Value typed by the user.</param>
        /// <param name="excludePersonId">When editing, the person's own ID (their current email is allowed).</param>
        /// <returns>null when the email can be used, otherwise the error message.</returns>
        private string? CheckEmail(string email, int? excludePersonId)
        {
            OperationResult result = _personService.CheckEmailAvailable(email, excludePersonId);
            return result.Success ? null : result.Message;
        }

        /// <summary>
        /// Asks for the role-specific fields and builds the matching sub-class of <see cref="Person"/>.
        /// </summary>
        /// <param name="role">Role of the record.</param>
        /// <param name="name">Already accepted name.</param>
        /// <param name="telephone">Already accepted telephone.</param>
        /// <param name="email">Already accepted email.</param>
        /// <param name="current">Current record when editing with the same role (its values become defaults), otherwise null.</param>
        /// <returns>A new <see cref="Performer"/>, <see cref="CrewMember"/> or <see cref="Vendor"/>.</returns>
        private Person AskRoleDetails(PersonRole role, string name, string telephone, string email, Person? current)
        {
            switch (role)
            {
                // Performer: fee + exactly 2 genres (the existing genres are listed as a hint).
                // "old" is null when adding or when the role changed, so no default value is offered.
                case PersonRole.Performer:
                {
                    Performer? old = current as Performer;
                    decimal fee = _view.AskDecimal("Fee (£)", FieldRules.CheckFee, old?.Fee);
                    List<string> genres = _view.AskNameList(
                        "Genres",
                        _lookupService.GetAll(LookupKind.Genre).Select(g => g.Name),
                        BusinessRules.RequiredGenreCount,
                        FieldRules.CheckGenres,
                        old?.Genres);
                    // Build the concrete sub-class; its constructor runs the validating setters.
                    return new Performer(name, telephone, email, fee, genres);
                }

                // Crew: hourly rate, employment type, then weekly hours whose valid range depends
                // on the employment type chosen just before.
                case PersonRole.Crew:
                {
                    CrewMember? old = current as CrewMember;
                    decimal rate = _view.AskDecimal("Hourly rate (£)", FieldRules.CheckHourlyRate, old?.HourlyRate);
                    EmploymentType type = _view.AskEmploymentType(old?.EmploymentType);
                    (int min, int max) = BusinessRules.GetWeeklyHoursRange(type);

                    // Only offer the old hours as default if they are still valid for the chosen type.
                    int? oldHours = old != null && FieldRules.CheckWeeklyHours(type, old.WeeklyHours) == null ? old.WeeklyHours : null;
                    int hours = _view.AskInt($"Weekly hours ({min}-{max})", value => FieldRules.CheckWeeklyHours(type, value), oldHours);
                    return new CrewMember(name, telephone, email, rate, type, hours);
                }

                // Vendor: stall (business) name + exactly 3 product categories.
                default: // PersonRole.Vendor
                {
                    Vendor? old = current as Vendor;
                    string stallName = _view.AskText("Vendor (stall) name", FieldRules.CheckStallName, old?.StallName);
                    List<string> categories = _view.AskNameList(
                        "Product categories",
                        _lookupService.GetAll(LookupKind.ProductCategory).Select(c => c.Name),
                        BusinessRules.RequiredCategoryCount,
                        FieldRules.CheckCategories,
                        old?.ProductCategories);
                    return new Vendor(name, telephone, email, stallName, categories);
                }
            }
        }
    }
}
