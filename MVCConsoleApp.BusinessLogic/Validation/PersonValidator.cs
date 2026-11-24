using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.BusinessLogic.Validation
{
    /// <summary>
    /// BUSINESS LAYER - validates a complete <see cref="Person"/> before it is saved.
    /// <para>
    /// Uses the <b>Template Method</b> design pattern: <see cref="Validate"/> is the fixed
    /// algorithm (check common fields, then role-specific fields). The role-specific step is the
    /// abstract method <see cref="ValidateRoleFields"/>, which each sub-class overrides
    /// (inheritance + polymorphism). <see cref="For(PersonRole)"/> is a small factory that returns
    /// the right validator for a role.
    /// </para>
    /// </summary>
    public abstract class PersonValidator
    {
        /// <summary>
        /// Returns the validator that matches a role.
        /// </summary>
        /// <param name="role">Role of the person to validate.</param>
        /// <returns>A <see cref="PerformerValidator"/>, <see cref="CrewValidator"/> or <see cref="VendorValidator"/>.</returns>
        public static PersonValidator For(PersonRole role)
        {
            switch (role)
            {
                case PersonRole.Performer: return new PerformerValidator();
                case PersonRole.Crew: return new CrewValidator();
                case PersonRole.Vendor: return new VendorValidator();
                default: throw new ArgumentOutOfRangeException(nameof(role), role, "Unknown role.");
            }
        }

        /// <summary>
        /// Validates all fields of a person (the template method - it cannot be overridden).
        /// </summary>
        /// <param name="person">Person to validate.</param>
        /// <returns>A list of error messages; empty when the person is valid.</returns>
        public List<string> Validate(Person person)
        {
            var errors = new List<string>();

            // Step 1: common fields - identical for every role.
            AddIfError(errors, FieldRules.CheckName(person.FullName));
            AddIfError(errors, FieldRules.CheckTelephone(person.Telephone));
            AddIfError(errors, FieldRules.CheckEmailFormat(person.Email));

            // Step 2: role-specific fields - implemented differently by each sub-class.
            ValidateRoleFields(person, errors);
            return errors;
        }

        /// <summary>Role-specific validation step implemented by each sub-class.</summary>
        /// <param name="person">Person to validate (its run-time type matches the validator).</param>
        /// <param name="errors">List to which error messages are added.</param>
        protected abstract void ValidateRoleFields(Person person, List<string> errors);

        /// <summary>Adds a message to the list when it is not null.</summary>
        protected static void AddIfError(List<string> errors, string? error)
        {
            if (error != null)
            {
                errors.Add(error);
            }
        }

        /// <summary>
        /// Casts the person to the type expected by a validator, reporting an error instead of
        /// throwing when the wrong type is passed (e.g. a Vendor given to the CrewValidator).
        /// </summary>
        protected static T? As<T>(Person person, List<string> errors) where T : Person
        {
            if (person is T typed)
            {
                return typed;
            }
            errors.Add($"Expected a {typeof(T).Name} but received a {person.GetType().Name}.");
            return null;
        }
    }

    /// <summary>Validator for performers: fee range and exactly 2 different genres.</summary>
    public class PerformerValidator : PersonValidator
    {
        /// <inheritdoc />
        protected override void ValidateRoleFields(Person person, List<string> errors)
        {
            Performer? performer = As<Performer>(person, errors);
            if (performer == null) return;

            AddIfError(errors, FieldRules.CheckFee(performer.Fee));
            AddIfError(errors, FieldRules.CheckGenres(performer.Genres));
        }
    }

    /// <summary>Validator for crew: hourly rate range and weekly hours that match the employment type.</summary>
    public class CrewValidator : PersonValidator
    {
        /// <inheritdoc />
        protected override void ValidateRoleFields(Person person, List<string> errors)
        {
            CrewMember? crew = As<CrewMember>(person, errors);
            if (crew == null) return;

            AddIfError(errors, FieldRules.CheckHourlyRate(crew.HourlyRate));
            if (!Enum.IsDefined(crew.EmploymentType))
            {
                errors.Add("Employment type must be FullTime or PartTime.");
                return;
            }
            AddIfError(errors, FieldRules.CheckWeeklyHours(crew.EmploymentType, crew.WeeklyHours));
        }
    }

    /// <summary>Validator for vendors: vendor name and exactly 3 different product categories.</summary>
    public class VendorValidator : PersonValidator
    {
        /// <inheritdoc />
        protected override void ValidateRoleFields(Person person, List<string> errors)
        {
            Vendor? vendor = As<Vendor>(person, errors);
            if (vendor == null) return;

            AddIfError(errors, FieldRules.CheckStallName(vendor.StallName));
            AddIfError(errors, FieldRules.CheckCategories(vendor.ProductCategories));
        }
    }
}
