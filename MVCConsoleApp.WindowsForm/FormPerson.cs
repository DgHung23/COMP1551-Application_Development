using MVCConsoleApp.BusinessLogic.Services;
using System.Text.RegularExpressions;

namespace MVCConsoleApp.WindowsForm
{
    public partial class FormPerson : Form
    {
        public FormPerson()
        {
            InitializeComponent();

            LoadRoles();
            InitializeDataGridView(gridViewPersonRoleList);
            InitializeGender();
        }

        private void LoadRoles()
        {
            var controllerPersonRole = new PersonRoleController();
            cbRole.DataSource = controllerPersonRole.GetAll();
            cbRole.DisplayMember = "Name";
            cbRole.ValueMember = "Id";
        }
        private void InitializeGender()
        {
            var genders = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(1, "Male"),
            new KeyValuePair<int, string>(2, "Female"),
            new KeyValuePair<int, string>(3, "Other")
        };

            cbGender.DataSource = genders;
            cbGender.DisplayMember = "Value";   // text shown in the combo box
            cbGender.ValueMember = "Key";       // value you read from SelectedValue
            cbGender.DropDownStyle = ComboBoxStyle.DropDownList; // user can't type free text
        }

        private void btnManageRoles_Click(object sender, EventArgs e)
        {
            var selectedRoleId = cbRole.SelectedValue;

            using (var formPersonRole = new FormPersonRole())
            {
                formPersonRole.ShowDialog(this);
            }

            // Roles may have been added, so reload and keep the previous selection.
            LoadRoles();
            if (selectedRoleId != null)
            {
                cbRole.SelectedValue = selectedRoleId;
            }
        }

        private void InitializeDataGridView(DataGridView gridView)
        {
            gridView.AutoGenerateColumns = false;
            gridView.Columns.Clear();
            gridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Mã Id",
                DataPropertyName = "Id",
            });
            gridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                HeaderText = "Full Name",
                DataPropertyName = "FullName",
            });
            gridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                DataPropertyName = "Email",
            });
            gridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Telephone",
                HeaderText = "Phone",
                DataPropertyName = "Telephone",
            });
            gridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PersonRoleName",
                HeaderText = "Role",
                DataPropertyName = "PersonRoleName",
            });

            var controller = new PersonController();
            gridView.DataSource = null;
            gridView.DataSource = controller.GetAll();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string fullName = txbName.Text.Trim();
            string email = txbEmail.Text.Trim();
            string phone = txbPhone.Text.Trim();

            //=======Input validation===========
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Full Name is required.");
                return;
            }
            // Email and phone are optional, but must be well-formed when filled in.
            if (email.Length > 0 && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email is not valid (example: name@example.com).");
                txbEmail.Focus();
                return;
            }
            if (phone.Length > 0 && !Regex.IsMatch(phone, @"^\+?\d{8,14}$"))
            {
                MessageBox.Show("Phone is not valid. Use 8-14 digits, optionally starting with +.");
                txbPhone.Focus();
                return;
            }
            if (cbRole.SelectedValue == null)
            {
                MessageBox.Show("Role is required. Please create a role first.");
                return;
            }
            int personRoleId = (int)cbRole.SelectedValue;

            //=== Storing in Database ===
            var controllerPerson = new PersonController();
            if (!controllerPerson.Create(fullName, phone, email, personRoleId))
            {
                MessageBox.Show("Failed to save Person.");
                return;
            }
            MessageBox.Show("Person saved successfully.");

            //===========clearing form ==========
            txbName.Text = "";
            txbEmail.Text = "";
            txbPhone.Text = "";
            if (cbRole.Items.Count > 0)
            {
                cbRole.SelectedIndex = 0;
            }

            InitializeDataGridView(gridViewPersonRoleList);
        }


    }
}
