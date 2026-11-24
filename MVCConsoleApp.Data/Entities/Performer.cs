namespace MVCConsoleApp.Data.Entities
{
    /// <summary>
    /// MODEL (MVC) - a performer (artist or DJ). Inherits the common fields from <see cref="Person"/>
    /// and adds a fee and a list of music genres (one performer can have more than one genre).
    /// </summary>
    public class Performer : Person
    {
        // ===================== Private fields (encapsulation) =====================
        private decimal _fee;

        // The genre list is private; outside code can read it but only change it
        // through SetGenres(), which cleans the data first.
        private readonly List<string> _genres = new List<string>();

        /// <summary>Creates an empty performer (used when loading from the database).</summary>
        public Performer()
        {
        }

        /// <summary>Creates a fully initialised performer.</summary>
        /// <param name="fullName">Full name or stage name.</param>
        /// <param name="telephone">Telephone number.</param>
        /// <param name="email">Unique email address.</param>
        /// <param name="fee">Performance fee in pounds.</param>
        /// <param name="genres">Music genres of the performer.</param>
        public Performer(string fullName, string telephone, string email, decimal fee, IEnumerable<string> genres)
            : base(fullName, telephone, email)
        {
            Fee = fee;
            SetGenres(genres);
        }

        /// <summary>Gets the role of this object: always <see cref="PersonRole.Performer"/>.</summary>
        public override PersonRole Role => PersonRole.Performer;

        /// <summary>
        /// Gets or sets the performance fee. A negative fee is impossible, so the setter rejects it.
        /// The value is rounded to 2 decimal places (pence) to match DECIMAL(12,2) in the database.
        /// </summary>
        public decimal Fee
        {
            get => _fee;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Fee), "Fee cannot be negative.");
                }
                _fee = decimal.Round(value, 2);
            }
        }

        /// <summary>Gets a read-only view of the performer's genres.</summary>
        public IReadOnlyList<string> Genres => _genres.AsReadOnly();

        /// <summary>
        /// Replaces the genres of the performer. Blank entries and duplicates (ignoring case)
        /// are removed. The exact number of genres is a business rule checked in the Business layer.
        /// </summary>
        /// <param name="genres">New list of genre names.</param>
        public void SetGenres(IEnumerable<string> genres)
        {
            _genres.Clear();
            _genres.AddRange(CleanNameList(genres));
        }

        /// <summary>Polymorphic override: describes the performer-specific fields.</summary>
        public override IReadOnlyList<KeyValuePair<string, string>> DescribeRoleFields()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Fee", FormatMoney(Fee)),
                new KeyValuePair<string, string>("Genres", string.Join(", ", _genres))
            };
        }

        /// <summary>Polymorphic override: a performer costs the festival exactly the agreed fee.</summary>
        public override decimal EstimateCost()
        {
            return Fee;
        }
    }
}
