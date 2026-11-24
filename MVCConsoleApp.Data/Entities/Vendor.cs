namespace MVCConsoleApp.Data.Entities
{
    /// <summary>
    /// MODEL (MVC) - a vendor (food or merchandise stall). Inherits the common contact fields from
    /// <see cref="Person"/> and adds the stall (business) name and a list of product categories
    /// (one vendor can have more than one product category).
    /// </summary>
    public class Vendor : Person
    {
        /// <summary>Maximum length of <see cref="StallName"/> (matches NVARCHAR(100) in the database).</summary>
        public const int MaxStallNameLength = 100;

        // ===================== Private fields (encapsulation) =====================
        private string _stallName = string.Empty;
        private readonly List<string> _productCategories = new List<string>();

        /// <summary>Creates an empty vendor (used when loading from the database).</summary>
        public Vendor()
        {
        }

        /// <summary>Creates a fully initialised vendor.</summary>
        /// <param name="fullName">Full name of the contact person.</param>
        /// <param name="telephone">Telephone number.</param>
        /// <param name="email">Unique email address.</param>
        /// <param name="stallName">Name of the stall / business.</param>
        /// <param name="productCategories">Product categories sold at the stall.</param>
        public Vendor(string fullName, string telephone, string email, string stallName, IEnumerable<string> productCategories)
            : base(fullName, telephone, email)
        {
            StallName = stallName;
            SetProductCategories(productCategories);
        }

        /// <summary>Gets the role of this object: always <see cref="PersonRole.Vendor"/>.</summary>
        public override PersonRole Role => PersonRole.Vendor;

        /// <summary>
        /// Gets or sets the vendor (stall) name, e.g. "Seoul Street Kitchen".
        /// The setter rejects empty values and values that are too long.
        /// </summary>
        public string StallName
        {
            get => _stallName;
            set => _stallName = RequireText(value, "Vendor name", MaxStallNameLength);
        }

        /// <summary>Gets a read-only view of the vendor's product categories.</summary>
        public IReadOnlyList<string> ProductCategories => _productCategories.AsReadOnly();

        /// <summary>
        /// Replaces the product categories. Blank entries and duplicates (ignoring case) are removed.
        /// The exact number of categories is a business rule checked in the Business layer.
        /// </summary>
        /// <param name="categories">New list of category names.</param>
        public void SetProductCategories(IEnumerable<string> categories)
        {
            _productCategories.Clear();
            _productCategories.AddRange(CleanNameList(categories));
        }

        /// <summary>Polymorphic override: describes the vendor-specific fields.</summary>
        public override IReadOnlyList<KeyValuePair<string, string>> DescribeRoleFields()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Vendor name", StallName),
                new KeyValuePair<string, string>("Categories", string.Join(", ", _productCategories))
            };
        }

        /// <summary>
        /// Polymorphic override: vendors pay the festival for their pitch, they are not paid by it,
        /// therefore their staffing cost to the festival is zero.
        /// </summary>
        public override decimal EstimateCost()
        {
            return 0m;
        }
    }
}
