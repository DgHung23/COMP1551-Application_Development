using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.BusinessLogic.Validation
{
    /// <summary>
    /// BUSINESS LAYER - every business rule constant in ONE place.
    /// <para>
    /// The user interfaces read these values (e.g. to show "25-40 hours" or to set the limits of a
    /// numeric box) instead of hard-coding them, so a rule only ever has to be changed here.
    /// The same limits are also enforced by CHECK constraints in databasescheme.sql
    /// (defence in depth).
    /// </para>
    /// </summary>
    public static class BusinessRules
    {
        // ------------------------------ Performer ------------------------------

        /// <summary>A performer must have exactly this many genres (brief: "exactly 2 genres").</summary>
        public const int RequiredGenreCount = 2;

        /// <summary>Lowest allowed performer fee in pounds.</summary>
        public const decimal MinFee = 0m;

        /// <summary>Highest allowed performer fee in pounds (protects against typing errors).</summary>
        public const decimal MaxFee = 1_000_000m;

        // -------------------------------- Crew --------------------------------

        /// <summary>Lowest allowed hourly rate in pounds (must be more than zero).</summary>
        public const decimal MinHourlyRate = 0.01m;

        /// <summary>Highest allowed hourly rate in pounds.</summary>
        public const decimal MaxHourlyRate = 1_000m;

        /// <summary>Minimum weekly hours for a FullTime crew member.</summary>
        public const int FullTimeMinHours = 25;

        /// <summary>Maximum weekly hours for a FullTime crew member.</summary>
        public const int FullTimeMaxHours = 40;

        /// <summary>Minimum weekly hours for a PartTime crew member.</summary>
        public const int PartTimeMinHours = 1;

        /// <summary>Maximum weekly hours for a PartTime crew member.</summary>
        public const int PartTimeMaxHours = 24;

        // ------------------------------- Vendor -------------------------------

        /// <summary>A vendor must have exactly this many product categories (brief: "exactly 3 categories").</summary>
        public const int RequiredCategoryCount = 3;

        // ------------------------------ Helpers ------------------------------

        /// <summary>Returns the allowed weekly hours range for an employment type.</summary>
        /// <param name="type">FullTime or PartTime.</param>
        /// <returns>A tuple (Min, Max).</returns>
        public static (int Min, int Max) GetWeeklyHoursRange(EmploymentType type)
        {
            return type == EmploymentType.FullTime
                ? (FullTimeMinHours, FullTimeMaxHours)
                : (PartTimeMinHours, PartTimeMaxHours);
        }
    }
}
