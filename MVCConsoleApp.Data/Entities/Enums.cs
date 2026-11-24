namespace MVCConsoleApp.Data.Entities
{
    /// <summary>
    /// The three user groups managed by the system.
    /// The numeric values are identical to the primary keys of the <c>Roles</c> table,
    /// so an enum value can be stored directly in <c>People.RoleId</c>.
    /// </summary>
    public enum PersonRole
    {
        /// <summary>Artists and DJs.</summary>
        Performer = 1,

        /// <summary>Stage, sound, lighting, security staff and volunteers.</summary>
        Crew = 2,

        /// <summary>Food and merchandise stall holders.</summary>
        Vendor = 3
    }

    /// <summary>
    /// Employment type of a crew member. It determines the allowed range of weekly hours
    /// (FullTime 25-40, PartTime 1-24). Stored as text ('FullTime' / 'PartTime') in the database.
    /// </summary>
    public enum EmploymentType
    {
        /// <summary>Full-time contract (25 to 40 hours per week).</summary>
        FullTime = 1,

        /// <summary>Part-time contract (1 to 24 hours per week).</summary>
        PartTime = 2
    }

    /// <summary>
    /// The two kinds of lookup (reference) lists used by role-specific data:
    /// music genres for performers and product categories for vendors.
    /// </summary>
    public enum LookupKind
    {
        /// <summary>Music genre (table <c>Genres</c>).</summary>
        Genre = 1,

        /// <summary>Product category (table <c>ProductCategories</c>).</summary>
        ProductCategory = 2
    }
}
