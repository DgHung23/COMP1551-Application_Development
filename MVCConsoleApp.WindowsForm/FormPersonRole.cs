using MVCConsoleApp.BusinessLogic.Services;

namespace MVCConsoleApp.WindowsForm
{
    public partial class FormPersonRole : Form
    {
        public FormPersonRole()
        {
            InitializeComponent();

            InitializeDataGridView(gridViewPersonRoleList);
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txbName.Text;
            string description = rtxbDescription.Text;


            //=======Input validation===========
            name = name.Trim();
            if (string.IsNullOrEmpty(name) || name == " ")
            {
                MessageBox.Show("Name is required.");
                return;
            }

            //=== Storing in Database ===
            var controllerPersonRole = new PersonRoleController();
            controllerPersonRole.Create(name, description);
            MessageBox.Show("Person Role saved successfully.");

            //===========clearing form ==========
            txbName.Text = "";
            rtxbDescription.Text = "";

            InitializeDataGridView(gridViewPersonRoleList);
        }

    }
}
