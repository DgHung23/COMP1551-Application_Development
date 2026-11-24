// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : LookupItem.cs
// Layer   : Data layer - MODEL (MVC): domain entities
// Summary : MODEL (MVC) - one entry of a lookup list: a music genre or a product category.
// =====================================================================

namespace MVCConsoleApp.Data.Entities
{
    /// <summary>
    /// MODEL (MVC) - one entry of a lookup list: a music genre or a product category.
    /// Used to show the available values to the user and to manage the lists in the Windows Forms UI.
    /// </summary>
    public class LookupItem
    {
        /// <summary>Maximum length of a lookup name (matches NVARCHAR(50) in the database).</summary>
        public const int MaxNameLength = 50;

        /// <summary>Gets or sets the primary key (GenreId or CategoryId).</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the display name, e.g. "Rock" or "Street Food".</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets which list this item belongs to.</summary>
        public LookupKind Kind { get; set; }

        /// <summary>
        /// Gets or sets how many active people currently use this item
        /// (an item that is in use cannot be deleted).
        /// </summary>
        public int UsageCount { get; set; }

        /// <summary>Returns the name, so the item displays nicely in list boxes.</summary>
        public override string ToString() => Name;
    }

    /// <summary>
    /// One row of a "group by" report, e.g. Genre = "Indie", Count = 2, Members = "Luna Rivers, The Midnight Owls".
    /// </summary>
    public class GroupSummary
    {
        /// <summary>Gets or sets the group name (genre or category).</summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>Gets or sets the number of active people in the group.</summary>
        public int Count { get; set; }

        /// <summary>Gets or sets a comma-separated list of the people in the group.</summary>
        public string Members { get; set; } = string.Empty;
    }
}
