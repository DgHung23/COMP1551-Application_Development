// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : FieldRules.cs
// Layer   : Business layer - business rules and validation
// Summary : BUSINESS LAYER - validation of single fields.
// =====================================================================

using System.Text.RegularExpressions;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.BusinessLogic.Validation
{
    /// <summary>
    /// BUSINESS LAYER - validation of single fields.
    /// <para>
    /// Every method returns <c>null</c> when the value is valid, otherwise a user-friendly error
    /// message. The console controllers pass these methods to the input helpers so a wrong value is
    /// rejected immediately and the user is asked again; the <see cref="PersonValidator"/> classes
    /// re-use exactly the same methods to validate a complete record before it is saved.
    /// </para>
    /// </summary>
    public static class FieldRules
    {
        // Basic email shape: something@something.something without spaces.
        private static readonly Regex EmailPattern =
            new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        // Optional leading '+', then 8 to 15 digits (E.164 maximum is 15 digits).
        // Spaces and dashes are removed before the check (see NormaliseTelephone).
        private static readonly Regex TelephonePattern =
            new Regex(@"^\+?\d{8,15}$", RegexOptions.Compiled);

        // Letters (any language, e.g. Vietnamese accents), digits, spaces and . ' - &
        private static readonly Regex NamePattern =
            new Regex(@"^[\p{L}\p{M}\p{N} .'\-&]+$", RegexOptions.Compiled);

        // Genre / category names: letters, digits, spaces and a few symbols.
        // ',' is not allowed (it separates items in the console) and neither is '|'
        // (used internally by the data layer to join lists).
        private static readonly Regex LookupNamePattern =
            new Regex(@"^[\p{L}\p{M}\p{N} &'\-./+]+$", RegexOptions.Compiled);

        /// <summary>Checks a person's full name: required, 2-100 characters, sensible characters only.</summary>
        public static string? CheckName(string? name)
        {
            // Trim first so "   " counts as empty, then check each rule in turn.
            string value = (name ?? string.Empty).Trim();
            if (value.Length == 0) return "Name is required.";
            if (value.Length < 2) return "Name must have at least 2 characters.";
            if (value.Length > Person.MaxNameLength) return $"Name cannot be longer than {Person.MaxNameLength} characters.";
            if (!NamePattern.IsMatch(value)) return "Name can only contain letters, digits, spaces and . ' - &";
            return null;
        }

        /// <summary>Removes spaces, dashes and brackets from a telephone number.</summary>
        public static string NormaliseTelephone(string? telephone)
        {
            return Regex.Replace(telephone ?? string.Empty, @"[\s\-()]", string.Empty);
        }

        /// <summary>Checks a telephone number: required, optional '+', 8-15 digits.</summary>
        public static string? CheckTelephone(string? telephone)
        {
            // Ignore formatting characters, so "07700 900-101" is accepted.
            string value = NormaliseTelephone(telephone);
            if (value.Length == 0) return "Telephone is required.";
            if (!TelephonePattern.IsMatch(value)) return "Telephone must contain 8 to 15 digits (optionally starting with +), e.g. 07700900123.";
            return null;
        }

        /// <summary>Checks the FORMAT of an email address (uniqueness is checked by the service).</summary>
        public static string? CheckEmailFormat(string? email)
        {
            // Required, not too long, and shaped like name@domain.tld.
            string value = (email ?? string.Empty).Trim();
            if (value.Length == 0) return "Email is required.";
            if (value.Length > Person.MaxEmailLength) return $"Email cannot be longer than {Person.MaxEmailLength} characters.";
            if (!EmailPattern.IsMatch(value)) return "Email is not valid (example: name@example.com).";
            return null;
        }

        /// <summary>Checks a performer fee against the allowed range.</summary>
        public static string? CheckFee(decimal fee)
        {
            if (fee < BusinessRules.MinFee || fee > BusinessRules.MaxFee)
            {
                return $"Fee must be between {Person.FormatMoney(BusinessRules.MinFee)} and {Person.FormatMoney(BusinessRules.MaxFee)}.";
            }
            return null;
        }

        /// <summary>Checks a crew hourly rate against the allowed range.</summary>
        public static string? CheckHourlyRate(decimal hourlyRate)
        {
            if (hourlyRate < BusinessRules.MinHourlyRate || hourlyRate > BusinessRules.MaxHourlyRate)
            {
                return $"Hourly rate must be between {Person.FormatMoney(BusinessRules.MinHourlyRate)} and {Person.FormatMoney(BusinessRules.MaxHourlyRate)}.";
            }
            return null;
        }

        /// <summary>
        /// Checks the weekly hours against the sensible range of the employment type:
        /// FullTime 25-40, PartTime 1-24.
        /// </summary>
        public static string? CheckWeeklyHours(EmploymentType type, int weeklyHours)
        {
            // The allowed range depends on the employment type (FullTime 25-40, PartTime 1-24).
            (int min, int max) = BusinessRules.GetWeeklyHoursRange(type);
            if (weeklyHours < min || weeklyHours > max)
            {
                return $"{type} crew must work between {min} and {max} hours per week.";
            }
            return null;
        }

        /// <summary>Checks a vendor (stall) name: required, max 100 characters.</summary>
        public static string? CheckStallName(string? stallName)
        {
            // The vendor name is required and must fit the NVARCHAR(100) column.
            string value = (stallName ?? string.Empty).Trim();
            if (value.Length == 0) return "Vendor name is required.";
            if (value.Length > Vendor.MaxStallNameLength) return $"Vendor name cannot be longer than {Vendor.MaxStallNameLength} characters.";
            return null;
        }

        /// <summary>Checks one genre or category name.</summary>
        public static string? CheckLookupName(string? name)
        {
            // Genre / category names: required, max 50 characters, safe characters only.
            string value = (name ?? string.Empty).Trim();
            if (value.Length == 0) return "Name is required.";
            if (value.Length > LookupItem.MaxNameLength) return $"'{value}' is longer than {LookupItem.MaxNameLength} characters.";
            if (!LookupNamePattern.IsMatch(value)) return $"'{value}' contains invalid characters (commas and | are not allowed).";
            return null;
        }

        /// <summary>Checks the genre list of a performer: exactly <see cref="BusinessRules.RequiredGenreCount"/> different, valid genres.</summary>
        public static string? CheckGenres(IEnumerable<string>? genres)
        {
            return CheckNameList(genres, BusinessRules.RequiredGenreCount, "genre", "genres");
        }

        /// <summary>Checks the category list of a vendor: exactly <see cref="BusinessRules.RequiredCategoryCount"/> different, valid categories.</summary>
        public static string? CheckCategories(IEnumerable<string>? categories)
        {
            return CheckNameList(categories, BusinessRules.RequiredCategoryCount, "product category", "product categories");
        }

        /// <summary>
        /// Shared logic for genres and categories: validates every name, then checks that the
        /// number of DIFFERENT names (ignoring case) is exactly <paramref name="requiredCount"/>.
        /// </summary>
        private static string? CheckNameList(IEnumerable<string>? names, int requiredCount, string itemName, string pluralName)
        {
            List<string> cleaned = (names ?? Enumerable.Empty<string>())
                .Select(name => (name ?? string.Empty).Trim())
                .Where(name => name.Length > 0)
                .ToList();

            // Validate each name first, so the user gets a precise message.
            foreach (string name in cleaned)
            {
                string? error = CheckLookupName(name);
                if (error != null) return error;
            }

            // Count DIFFERENT names ignoring case, so "Rock, rock" is detected as a duplicate.
            int distinctCount = cleaned.Distinct(StringComparer.OrdinalIgnoreCase).Count();
            if (distinctCount != cleaned.Count)
            {
                return $"The same {itemName} was entered more than once.";
            }
            if (distinctCount != requiredCount)
            {
                return $"Exactly {requiredCount} different {pluralName} are required (you entered {distinctCount}).";
            }
            return null;
        }
    }
}
