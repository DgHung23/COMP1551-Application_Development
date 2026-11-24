// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : CrewMember.cs
// Layer   : Data layer - MODEL (MVC): domain entities
// Summary : MODEL (MVC) - a crew member (stage, sound, lighting, security or volunteer).
// =====================================================================

namespace MVCConsoleApp.Data.Entities
{
    /// <summary>
    /// MODEL (MVC) - a crew member (stage, sound, lighting, security or volunteer).
    /// Inherits the common fields from <see cref="Person"/> and adds hourly rate,
    /// employment type and weekly hours.
    /// </summary>
    public class CrewMember : Person
    {
        // ===================== Private fields (encapsulation) =====================
        private decimal _hourlyRate;
        private int _weeklyHours;

        /// <summary>Creates an empty crew member (used when loading from the database).</summary>
        public CrewMember()
        {
        }

        /// <summary>Creates a fully initialised crew member.</summary>
        /// <param name="fullName">Full name.</param>
        /// <param name="telephone">Telephone number.</param>
        /// <param name="email">Unique email address.</param>
        /// <param name="hourlyRate">Pay per hour in pounds.</param>
        /// <param name="employmentType">FullTime or PartTime.</param>
        /// <param name="weeklyHours">Contracted hours per week.</param>
        public CrewMember(string fullName, string telephone, string email,
                          decimal hourlyRate, EmploymentType employmentType, int weeklyHours)
            : base(fullName, telephone, email)
        {
            HourlyRate = hourlyRate;
            EmploymentType = employmentType;
            WeeklyHours = weeklyHours;
        }

        /// <summary>Gets the role of this object: always <see cref="PersonRole.Crew"/>.</summary>
        public override PersonRole Role => PersonRole.Crew;

        /// <summary>
        /// Gets or sets the hourly rate. It must be greater than zero; the value is rounded to pence.
        /// </summary>
        public decimal HourlyRate
        {
            get => _hourlyRate;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(HourlyRate), "Hourly rate must be greater than zero.");
                }
                _hourlyRate = decimal.Round(value, 2);
            }
        }

        /// <summary>Gets or sets the employment type (FullTime / PartTime).</summary>
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

        /// <summary>
        /// Gets or sets the weekly hours. The setter only rejects impossible values (0 or less,
        /// more than the 168 hours in a week). The "sensible range" that depends on the employment
        /// type (FullTime 25-40, PartTime 1-24) is a business rule enforced in the Business layer.
        /// </summary>
        public int WeeklyHours
        {
            get => _weeklyHours;
            set
            {
                if (value <= 0 || value > 168)
                {
                    throw new ArgumentOutOfRangeException(nameof(WeeklyHours), "Weekly hours must be between 1 and 168.");
                }
                _weeklyHours = value;
            }
        }

        /// <summary>Polymorphic override: describes the crew-specific fields.</summary>
        public override IReadOnlyList<KeyValuePair<string, string>> DescribeRoleFields()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Hourly rate", FormatMoney(HourlyRate) + "/h"),
                new KeyValuePair<string, string>("Employment", EmploymentType.ToString()),
                new KeyValuePair<string, string>("Weekly hours", WeeklyHours + " h/week")
            };
        }

        /// <summary>
        /// Polymorphic override: a crew member costs hourly rate x weekly hours
        /// (one festival week, which covers the 3 festival days plus set-up and take-down).
        /// </summary>
        public override decimal EstimateCost()
        {
            return HourlyRate * WeeklyHours;
        }
    }
}
