// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : FormPerson.cs
// Layer   : Presentation layer (Windows Forms user interface)
// Summary : Windows Forms user interface - main window.
// =====================================================================

using MVCConsoleApp.BusinessLogic.Common;
using MVCConsoleApp.BusinessLogic.Services;
using MVCConsoleApp.BusinessLogic.Validation;
using MVCConsoleApp.Data;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.WindowsForm
{
    /// <summary>
    /// Windows Forms user interface - main window.
    /// <para>
    /// A second Presentation layer that re-uses EXACTLY the same Business and Data layers as the
    /// console application. This shows the benefit of the 3-tier architecture: no business rule or
    /// SQL statement had to be written twice. The form only collects input, calls the services and
    /// shows the results.
    /// </para>
    /// Left side: details of one person (add / edit). Right side: list with role filter, search,
    /// column sorting, recycle bin, reports and CSV export.
    /// </summary>
    public partial class FormPerson : Form
    {
        // Business-layer services shared with the console application.
        private readonly IPersonService _personService = new PersonService();
        private readonly ILookupService _lookupService = new LookupService();
        private readonly IReportService _reportService = new ReportService();

        // PersonID of the record being edited, or null when the form is in "new record" mode.
        private int? _editingPersonId;

        // The list currently shown in the grid (used by sorting and CSV export).
        private List<Person> _currentList = new List<Person>();

        // Current sort column / direction (toggled by clicking a column header).
        private SortField _sortField = SortField.PersonId;
        private bool _sortDescending;

        // True while the code fills the form, so selection events are ignored.
        private bool _isLoading;

        /// <summary>Creates the form, prepares the controls and loads the data.</summary>
        public FormPerson()
        {
            // Designer-generated controls first, then our own set-up, then the data.
            InitializeComponent();

            InitializeComboBoxes();
            InitializeDataGridView(gridViewPersonList);
            LoadLookups();
            ClearForm();
            LoadPeople();
        }

        /// <summary>
        /// When the window appears the grid automatically selects its first row; undo that so the
        /// form starts in "new record" mode.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ClearForm();
            SetStatus($"{_currentList.Count} record(s). Click a row to edit it, or fill in the form and click Save to add a new record.");
        }

        // ====================================================================
        //  Initialisation
        // ====================================================================

        /// <summary>Fills the role, employment-type and filter combo boxes from the enums.</summary>
        private void InitializeComboBoxes()
        {
            // The role and employment-type lists come straight from the enums, so they can
            // never contain a value the rest of the application does not understand.
            cbRole.DataSource = Enum.GetValues<PersonRole>();
            cbEmploymentType.DataSource = Enum.GetValues<EmploymentType>();

            // Filter: "All roles" + every role.
            // The filter adds an "All roles" entry in front of the three roles.
            cbFilterRole.Items.Add("All roles");
            foreach (PersonRole role in Enum.GetValues<PersonRole>())
            {
                cbFilterRole.Items.Add(role);
            }
            _isLoading = true;
            cbFilterRole.SelectedIndex = 0;
            _isLoading = false;

            // Numeric limits come from the Business layer so the UI and the rules always agree.
            nudFee.Minimum = BusinessRules.MinFee;
            nudFee.Maximum = BusinessRules.MaxFee;
            nudHourlyRate.Minimum = BusinessRules.MinHourlyRate;
            nudHourlyRate.Maximum = BusinessRules.MaxHourlyRate;
            labelGenres.Text = $"Genres (tick exactly {BusinessRules.RequiredGenreCount})";
            labelCategories.Text = $"Product categories (tick exactly {BusinessRules.RequiredCategoryCount})";
            labelHoursHint.Text = $"FullTime: {BusinessRules.FullTimeMinHours}-{BusinessRules.FullTimeMaxHours} h, " +
                                  $"PartTime: {BusinessRules.PartTimeMinHours}-{BusinessRules.PartTimeMaxHours} h";
        }

        /// <summary>Creates the grid columns (same approach as the original template).</summary>
        private void InitializeDataGridView(DataGridView gridView)
        {
            // Columns are defined explicitly: each is bound to a property of Person and the
            // Tag remembers which SortField is used when its header is clicked.
            gridView.AutoGenerateColumns = false;
            gridView.Columns.Clear();
            AddColumn(gridView, "PersonId", "ID", 6, SortField.PersonId);
            AddColumn(gridView, "FullName", "Full Name", 16, SortField.Name);
            AddColumn(gridView, "RoleName", "Role", 9, SortField.Role);
            AddColumn(gridView, "Email", "Email", 22, SortField.Email);
            AddColumn(gridView, "Telephone", "Phone", 12, null);
            AddColumn(gridView, "RoleDetails", "Role details", 30, null);       // polymorphic summary
            AddColumn(gridView, "EstimatedCostText", "Est. cost", 10, SortField.EstimatedCost);
            AddColumn(gridView, "DeletedAt", "Deleted at", 14, null);
            gridView.Columns["DeletedAt"]!.Visible = false;
            gridView.Columns["DeletedAt"]!.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
        }

        /// <summary>Adds one text column bound to a property of <see cref="Person"/>.</summary>
        /// <param name="sortField">Field used when the header is clicked, or null if the column is not sortable.</param>
        private static void AddColumn(DataGridView gridView, string property, string header, float weight, SortField? sortField)
        {
            // SortMode "Programmatic" lets the form decide how to sort (through the service).
            gridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = property,
                HeaderText = header,
                DataPropertyName = property,
                FillWeight = weight,
                Tag = sortField,
                SortMode = sortField.HasValue ? DataGridViewColumnSortMode.Programmatic : DataGridViewColumnSortMode.NotSortable
            });
        }

        /// <summary>Loads the genre and category lists into the checked list boxes, keeping the ticks.</summary>
        private void LoadLookups()
        {
            try
            {
                // Remember what is ticked, reload the lists, then tick the same names again.
                List<string> checkedGenres = GetCheckedNames(clbGenres);
                List<string> checkedCategories = GetCheckedNames(clbCategories);

                FillCheckedList(clbGenres, _lookupService.GetAll(LookupKind.Genre), checkedGenres);
                FillCheckedList(clbCategories, _lookupService.GetAll(LookupKind.ProductCategory), checkedCategories);
            }
            catch (DataAccessException ex)
            {
                ShowDatabaseError(ex);
            }
        }

        // ====================================================================
        //  Loading the list (view all / by role / search / recycle bin)
        // ====================================================================

        /// <summary>
        /// Reloads the grid according to the current filters: recycle bin, search text and role.
        /// </summary>
        private void LoadPeople()
        {
            try
            {
                // Read the current filter settings from the controls.
                bool showDeleted = chkShowDeleted.Checked;
                string term = txbSearch.Text.Trim();
                // null means "All roles" (the first, text-only entry of the filter).
                PersonRole? role = cbFilterRole.SelectedItem as PersonRole?;

                List<Person> people;
                // Choose the right service call: recycle bin, search, one role or everything.
                if (showDeleted)
                {
                    people = _personService.GetDeleted();
                }
                else if (term.Length > 0)
                {
                    people = _personService.Search(term);
                }
                else
                {
                    people = role.HasValue ? _personService.GetByRole(role.Value) : _personService.GetAll();
                }

                // The role filter also applies to search results and to the recycle bin.
                if (role.HasValue && (showDeleted || term.Length > 0))
                {
                    people = people.Where(p => p.Role == role.Value).ToList();
                }

                // Keep the user's chosen sort order when the list is reloaded.
                _currentList = _personService.Sort(people, _sortField, _sortDescending);
                BindGrid();

                string what = showDeleted ? "deleted record(s) in the recycle bin" : "record(s)";
                SetStatus($"{_currentList.Count} {what}.");
            }
            catch (DataAccessException ex)
            {
                ShowDatabaseError(ex);
            }
        }

        /// <summary>Binds <see cref="_currentList"/> to the grid and updates the sort glyph.</summary>
        private void BindGrid()
        {
            _isLoading = true;
            // Re-binding (null first) forces the grid to refresh completely.
            gridViewPersonList.DataSource = null;
            gridViewPersonList.DataSource = _currentList;
            gridViewPersonList.Columns["DeletedAt"]!.Visible = chkShowDeleted.Checked;
            gridViewPersonList.ClearSelection();

            // Show the sort arrow only on the column that is currently sorted.
            foreach (DataGridViewColumn column in gridViewPersonList.Columns)
            {
                column.HeaderCell.SortGlyphDirection = column.Tag is SortField field && field == _sortField
                    ? (_sortDescending ? SortOrder.Descending : SortOrder.Ascending)
                    : SortOrder.None;
            }
            _isLoading = false;
        }

        // ====================================================================
        //  Form <-> Person
        // ====================================================================

        /// <summary>Resets the form to "new record" mode.</summary>
        private void ClearForm()
        {
            _isLoading = true;
            _editingPersonId = null;
            // Reset every input to its default value.
            labelPersonId.Text = "PersonID: (new record)";
            labelTitle.Text = "Person Form";
            txbName.Text = string.Empty;
            txbPhone.Text = string.Empty;
            txbEmail.Text = string.Empty;
            cbRole.SelectedItem = PersonRole.Performer;
            nudFee.Value = nudFee.Minimum;
            SetCheckedNames(clbGenres, Enumerable.Empty<string>());
            nudHourlyRate.Value = Math.Max(nudHourlyRate.Minimum, 12m);
            cbEmploymentType.SelectedItem = EmploymentType.FullTime;
            txbStallName.Text = string.Empty;
            SetCheckedNames(clbCategories, Enumerable.Empty<string>());
            btnDelete.Enabled = false;
            gridViewPersonList.ClearSelection();
            gridViewPersonList.CurrentCell = null;
            _isLoading = false;

            ShowRolePanel(PersonRole.Performer);
            UpdateWeeklyHoursRange();
            txbName.Focus();
        }

        /// <summary>Copies a person into the form controls (edit mode).</summary>
        private void ShowPerson(Person person)
        {
            _isLoading = true;
            _editingPersonId = person.PersonId;
            // Common fields first.
            labelPersonId.Text = $"PersonID: {person.PersonId} (editing)";
            txbName.Text = person.FullName;
            txbPhone.Text = person.Telephone;
            txbEmail.Text = person.Email;
            cbRole.SelectedItem = person.Role;

            // Pattern matching fills the controls of the person's own role.
            switch (person)
            {
                case Performer performer:
                    nudFee.Value = Math.Clamp(performer.Fee, nudFee.Minimum, nudFee.Maximum);
                    SetCheckedNames(clbGenres, performer.Genres);
                    break;
                case CrewMember crew:
                    nudHourlyRate.Value = Math.Clamp(crew.HourlyRate, nudHourlyRate.Minimum, nudHourlyRate.Maximum);
                    cbEmploymentType.SelectedItem = crew.EmploymentType;
                    UpdateWeeklyHoursRange();
                    nudWeeklyHours.Value = Math.Clamp(crew.WeeklyHours, (int)nudWeeklyHours.Minimum, (int)nudWeeklyHours.Maximum);
                    break;
                case Vendor vendor:
                    txbStallName.Text = vendor.StallName;
                    SetCheckedNames(clbCategories, vendor.ProductCategories);
                    break;
            }
            btnDelete.Enabled = true;
            _isLoading = false;

            ShowRolePanel(person.Role);
        }

        /// <summary>
        /// Builds a Performer, CrewMember or Vendor from the controls.
        /// Entity setters may throw <see cref="ArgumentException"/> for impossible values (e.g. an
        /// empty name); the caller turns that into a friendly message.
        /// </summary>
        private Person BuildPersonFromForm()
        {
            // Read the common fields; the role decides which sub-class is created below.
            string name = txbName.Text.Trim();
            string phone = txbPhone.Text.Trim();
            string email = txbEmail.Text.Trim();
            // The selected role decides which concrete class is created (switch expression).
            PersonRole role = (PersonRole)cbRole.SelectedItem!;

            Person person = role switch
            {
                PersonRole.Performer => new Performer(name, phone, email, nudFee.Value, GetCheckedNames(clbGenres)),
                PersonRole.Crew => new CrewMember(name, phone, email, nudHourlyRate.Value,
                                                  (EmploymentType)cbEmploymentType.SelectedItem!, (int)nudWeeklyHours.Value),
                _ => new Vendor(name, phone, email, txbStallName.Text.Trim(), GetCheckedNames(clbCategories))
            };

            // In edit mode the object keeps the PersonID of the record being edited.
            if (_editingPersonId.HasValue)
            {
                person.PersonId = _editingPersonId.Value;
            }
            return person;
        }

        /// <summary>Shows only the panel that belongs to the selected role.</summary>
        private void ShowRolePanel(PersonRole role)
        {
            // The three panels are stacked at the same position; only one is visible.
            panelPerformer.Visible = role == PersonRole.Performer;
            panelCrew.Visible = role == PersonRole.Crew;
            panelVendor.Visible = role == PersonRole.Vendor;
        }

        /// <summary>Applies the FullTime / PartTime weekly-hours range (from BusinessRules) to the numeric box.</summary>
        private void UpdateWeeklyHoursRange()
        {
            if (cbEmploymentType.SelectedItem is EmploymentType type)
            {
                (int min, int max) = BusinessRules.GetWeeklyHoursRange(type);
                decimal current = nudWeeklyHours.Value;

                // Widen first, then narrow, so setting Minimum/Maximum never conflicts with Value.
                // Temporarily allow every value, set a value inside the new range, then narrow it.
                nudWeeklyHours.Minimum = 1;
                nudWeeklyHours.Maximum = 168;
                nudWeeklyHours.Value = Math.Clamp(current, min, max);
                nudWeeklyHours.Minimum = min;
                nudWeeklyHours.Maximum = max;
                labelWeeklyHours.Text = $"Weekly hours ({min}-{max})";
            }
        }

        // ====================================================================
        //  Event handlers - details panel
        // ====================================================================

        /// <summary>Changing the role shows the matching role-specific panel.</summary>
        private void cbRole_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbRole.SelectedItem is PersonRole role)
            {
                ShowRolePanel(role);
            }
        }

        /// <summary>Changing the employment type updates the allowed weekly-hours range.</summary>
        private void cbEmploymentType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateWeeklyHoursRange();
        }

        /// <summary>"New" clears the form so a new record can be entered.</summary>
        private void btnNew_Click(object? sender, EventArgs e)
        {
            ClearForm();
            SetStatus("New record: fill in the details and click Save.");
        }

        /// <summary>Add (new mode) or update (edit mode) the record shown in the form.</summary>
        private void btnSave_Click(object? sender, EventArgs e)
        {
            // 1) Check the text fields with the Business-layer field rules so that the user sees
            //    every problem at once (the entity setters would stop at the first empty value).
            var errors = new List<string?>
            {
                FieldRules.CheckName(txbName.Text),
                FieldRules.CheckTelephone(txbPhone.Text),
                FieldRules.CheckEmailFormat(txbEmail.Text),
                cbRole.SelectedItem is PersonRole.Vendor ? FieldRules.CheckStallName(txbStallName.Text) : null
            };
            List<string> messages = errors.Where(error => error != null).Select(error => error!).ToList();
            if (messages.Count > 0)
            {
                MessageBox.Show(OperationResult.Fail("Please correct the following:", messages).ToString(),
                                "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2) Build the entity, then 3) let the service apply every business rule and save.
            Person person;
            try
            {
                person = BuildPersonFromForm();
            }
            catch (ArgumentException ex)
            {
                // Raised by an entity setter (encapsulation), e.g. "Name cannot be empty."
                MessageBox.Show(ex.Message, "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // New record -> Add; existing record -> Update. Both return an OperationResult.
                OperationResult result;
                int savedId;
                if (_editingPersonId.HasValue)
                {
                    result = _personService.Update(person);
                    savedId = _editingPersonId.Value;
                }
                else
                {
                    OperationResult<int> addResult = _personService.Add(person);
                    result = addResult;
                    savedId = addResult.Value;
                }

                // Business-rule failures (duplicate email, wrong number of genres ...) are listed.
                if (!result.Success)
                {
                    MessageBox.Show(result.ToString(), "Please correct the following", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show(result.Message, "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadLookups();   // a new genre/category may have been created
                LoadPeople();
                SelectPerson(savedId);
            }
            catch (DataAccessException ex)
            {
                ShowDatabaseError(ex);
            }
        }

        /// <summary>Soft-deletes the record being edited, after confirmation.</summary>
        private void btnDelete_Click(object? sender, EventArgs e)
        {
            // Delete is only possible while a record is loaded in the form (edit mode).
            if (!_editingPersonId.HasValue)
            {
                return;
            }

            int id = _editingPersonId.Value;
            // Deleting requires confirmation; "No" is the default button for safety.
            DialogResult answer = MessageBox.Show(
                $"Are you sure you want to delete '{txbName.Text}' (PersonID {id})?\n\nThe record will be moved to the recycle bin.",
                "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            RunAndReport(() => _personService.Delete(id));
            ClearForm();
        }

        /// <summary>Opens the genre list manager (based on the template's role form), then reloads the genres.</summary>
        private void btnManageGenres_Click(object? sender, EventArgs e)
        {
            using (var form = new FormLookup(LookupKind.Genre))
            {
                form.ShowDialog(this);
            }
            LoadLookups();   // genres may have been added or removed
        }

        /// <summary>Opens the product-category list manager, then reloads the categories.</summary>
        private void btnManageCategories_Click(object? sender, EventArgs e)
        {
            using (var form = new FormLookup(LookupKind.ProductCategory))
            {
                form.ShowDialog(this);
            }
            LoadLookups();
        }

        // ====================================================================
        //  Event handlers - list
        // ====================================================================

        /// <summary>
        /// Clicking a row loads it into the form for editing.
        /// <para>
        /// Records are loaded ONLY on an explicit user action (mouse click or keyboard navigation),
        /// never on SelectionChanged: the grid selects its first row by itself whenever it receives
        /// the focus, which would silently switch the form to "edit" mode for the wrong person and
        /// make the next Save overwrite that record.
        /// </para>
        /// </summary>
        private void gridViewPersonList_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                LoadRowIntoForm(gridViewPersonList.Rows[e.RowIndex]);
            }
        }

        /// <summary>Moving through the list with the keyboard also loads the current row.</summary>
        private void gridViewPersonList_KeyUp(object? sender, KeyEventArgs e)
        {
            Keys[] navigationKeys = { Keys.Up, Keys.Down, Keys.PageUp, Keys.PageDown, Keys.Home, Keys.End };
            if (navigationKeys.Contains(e.KeyCode) && gridViewPersonList.CurrentRow != null)
            {
                LoadRowIntoForm(gridViewPersonList.CurrentRow);
            }
        }

        /// <summary>Shows the person of a grid row in the details panel (not while viewing the recycle bin).</summary>
        private void LoadRowIntoForm(DataGridViewRow row)
        {
            if (_isLoading || chkShowDeleted.Checked || row.DataBoundItem is not Person person)
            {
                return;
            }
            ShowPerson(person);
            SetStatus($"Editing PersonID {person.PersonId}. Change the values and click Save, or click New.");
        }

        /// <summary>Clicking a header sorts by that column; clicking again reverses the order.</summary>
        private void gridViewPersonList_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (gridViewPersonList.Columns[e.ColumnIndex].Tag is not SortField field)
            {
                return;
            }
            // Clicking the same column again reverses the order; a new column starts ascending.
            _sortDescending = field == _sortField && !_sortDescending;
            _sortField = field;
            _currentList = _personService.Sort(_currentList, _sortField, _sortDescending);
            BindGrid();
        }

        /// <summary>Changing the role filter reloads the list ("View by role").</summary>
        private void cbFilterRole_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!_isLoading)
            {
                LoadPeople();
            }
        }

        /// <summary>Runs the partial-match search on name / email.</summary>
        private void btnSearch_Click(object? sender, EventArgs e) => LoadPeople();

        /// <summary>Clears the search box and shows the full list again.</summary>
        private void btnClearSearch_Click(object? sender, EventArgs e)
        {
            txbSearch.Text = string.Empty;
            LoadPeople();
        }

        /// <summary>Pressing Enter in the search box runs the search.</summary>
        private void txbSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadPeople();
            }
        }

        /// <summary>Switches between the active records and the recycle bin.</summary>
        private void chkShowDeleted_CheckedChanged(object? sender, EventArgs e)
        {
            // In recycle-bin mode the details panel is disabled and the restore buttons enabled.
            bool recycleBin = chkShowDeleted.Checked;
            btnRestore.Enabled = recycleBin;
            btnDeletePermanently.Enabled = recycleBin;
            grpDetails.Enabled = !recycleBin;
            labelPersonList.Text = recycleBin ? "Recycle Bin" : "Person List";
            ClearForm();
            LoadPeople();
        }

        /// <summary>Restores the selected record from the recycle bin.</summary>
        private void btnRestore_Click(object? sender, EventArgs e)
        {
            // Restore works on the row selected in the recycle-bin list.
            Person? person = GetSelectedPerson();
            if (person != null)
            {
                RunAndReport(() => _personService.Restore(person.PersonId));
            }
        }

        /// <summary>Permanently deletes the selected record from the recycle bin, after confirmation.</summary>
        private void btnDeletePermanently_Click(object? sender, EventArgs e)
        {
            Person? person = GetSelectedPerson();
            if (person == null)
            {
                return;
            }
            DialogResult answer = MessageBox.Show(
                $"Permanently delete '{person.FullName}' (PersonID {person.PersonId})?\n\nThis cannot be undone.",
                "Confirm permanent delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (answer == DialogResult.Yes)
            {
                RunAndReport(() => _personService.DeletePermanently(person.PersonId));
            }
        }

        /// <summary>Opens the reports dialog (genres, categories, cost estimates).</summary>
        private void btnReports_Click(object? sender, EventArgs e)
        {
            // Modal dialog: the main window waits until the reports window is closed.
            using (var form = new FormReport(_reportService))
            {
                form.ShowDialog(this);
            }
        }

        /// <summary>Exports the records currently shown in the grid to a CSV file chosen by the user.</summary>
        private void btnExport_Click(object? sender, EventArgs e)
        {
            // Let the user choose where to save; only the records currently shown are exported.
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV files (*.csv)|*.csv";
                dialog.FileName = $"people_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                OperationResult<string> result = _reportService.ExportToCsv(_currentList, dialog.FileName);
                MessageBox.Show(result.ToString(), result.Success ? "Export complete" : "Export failed",
                                MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
        }

        /// <summary>Reloads the lookup lists and the records from the database.</summary>
        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            LoadLookups();
            LoadPeople();
        }

        // ====================================================================
        //  Helpers
        // ====================================================================

        /// <summary>Runs a service call, shows its result and reloads the list.</summary>
        private void RunAndReport(Func<OperationResult> action)
        {
            try
            {
                // Show success or failure with a matching icon, then refresh the list.
                OperationResult result = action();
                MessageBox.Show(result.ToString(), result.Success ? "Done" : "Not possible",
                                MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                LoadPeople();
            }
            catch (DataAccessException ex)
            {
                ShowDatabaseError(ex);
            }
        }

        /// <summary>Returns the person in the selected grid row, or asks the user to select one.</summary>
        private Person? GetSelectedPerson()
        {
            if (gridViewPersonList.SelectedRows.Count > 0 && gridViewPersonList.SelectedRows[0].DataBoundItem is Person person)
            {
                return person;
            }
            MessageBox.Show("Please select a record in the list first.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        /// <summary>Selects the grid row of a person (after saving) so the user sees the result.</summary>
        private void SelectPerson(int personId)
        {
            // Find the row whose bound Person has the requested ID.
            foreach (DataGridViewRow row in gridViewPersonList.Rows)
            {
                if (row.DataBoundItem is Person person && person.PersonId == personId)
                {
                    // Select the saved row and show it in the form so the user sees the result.
                    gridViewPersonList.CurrentCell = row.Cells[0];
                    row.Selected = true;
                    LoadRowIntoForm(row);
                    return;
                }
            }
        }

        /// <summary>Shows a database problem in a message box and in the status bar.</summary>
        private void ShowDatabaseError(DataAccessException ex)
        {
            // Tell the user what to check instead of showing a technical stack trace.
            SetStatus("Database error - see message.");
            MessageBox.Show(ex.Message + "\n\nCheck that the SQL Server Docker container is running and that " +
                            "Database/databasescheme.sql has been executed.",
                            "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>Writes a message in the status bar.</summary>
        private void SetStatus(string message) => lblStatus.Text = message;

        /// <summary>Fills a checked list box with lookup items and re-ticks the given names.</summary>
        private static void FillCheckedList(CheckedListBox list, IEnumerable<LookupItem> items, IEnumerable<string> checkedNames)
        {
            // Rebuild the list from the database, then restore the previous ticks.
            list.Items.Clear();
            foreach (LookupItem item in items)
            {
                list.Items.Add(item.Name);
            }
            SetCheckedNames(list, checkedNames);
        }

        /// <summary>Ticks exactly the given names (case-insensitive) and unticks every other item.</summary>
        private static void SetCheckedNames(CheckedListBox list, IEnumerable<string> names)
        {
            // A HashSet gives fast, case-insensitive "is this name ticked?" checks.
            var wanted = new HashSet<string>(names, StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < list.Items.Count; i++)
            {
                list.SetItemChecked(i, wanted.Contains(list.Items[i]?.ToString() ?? string.Empty));
            }
        }

        /// <summary>Returns the names of the ticked items.</summary>
        private static List<string> GetCheckedNames(CheckedListBox list)
        {
            // CheckedItems contains the ticked names (the list stores plain strings).
            return list.CheckedItems.Cast<object>().Select(item => item.ToString() ?? string.Empty).ToList();
        }
    }
}
