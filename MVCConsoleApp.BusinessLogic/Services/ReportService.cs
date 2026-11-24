using System.Globalization;
using System.Text;
using MVCConsoleApp.BusinessLogic.Common;
using MVCConsoleApp.Data.Entities;
using MVCConsoleApp.Data.Repositories;

namespace MVCConsoleApp.BusinessLogic.Services
{
    /// <summary>One line of the cost-estimate report: totals for one role.</summary>
    public class CostSummaryLine
    {
        /// <summary>Gets or sets the role name (or "TOTAL" for the grand total line).</summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>Gets or sets the number of active people with this role.</summary>
        public int Count { get; set; }

        /// <summary>Gets or sets the sum of the estimated costs.</summary>
        public decimal TotalCost { get; set; }

        /// <summary>Gets the average cost per person (0 when there is nobody).</summary>
        public decimal AverageCost => Count == 0 ? 0m : decimal.Round(TotalCost / Count, 2);

        /// <summary>Gets or sets an explanation of how the cost is calculated.</summary>
        public string Basis { get; set; } = string.Empty;
    }

    /// <summary>
    /// BUSINESS LAYER - contract for the report and export extension features.
    /// </summary>
    public interface IReportService
    {
        /// <summary>Performers grouped by genre.</summary>
        List<GroupSummary> GetPerformersByGenre();

        /// <summary>Vendors grouped by product category.</summary>
        List<GroupSummary> GetVendorsByCategory();

        /// <summary>Estimated cost per role plus a grand total.</summary>
        List<CostSummaryLine> GetCostSummary();

        /// <summary>Writes people to a CSV file. On success the result carries the full file path.</summary>
        OperationResult<string> ExportToCsv(IEnumerable<Person> people, string? filePath = null);
    }

    /// <summary>
    /// BUSINESS LAYER - reports (group by genre / category, cost estimates) and CSV export.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IPersonService _personService;

        /// <summary>Creates the service with the real repositories.</summary>
        public ReportService() : this(new ReportRepository(), new PersonService())
        {
        }

        /// <summary>Creates the service with injected dependencies (used by unit tests).</summary>
        public ReportService(IReportRepository reportRepository, IPersonService personService)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        /// <inheritdoc />
        public List<GroupSummary> GetPerformersByGenre()
        {
            return _reportRepository.GetPerformersByGenre();
        }

        /// <inheritdoc />
        public List<GroupSummary> GetVendorsByCategory()
        {
            return _reportRepository.GetVendorsByCategory();
        }

        /// <inheritdoc />
        public List<CostSummaryLine> GetCostSummary()
        {
            List<Person> people = _personService.GetAll();

            // Explanation of the cost basis for each role (shown in the report).
            var basis = new Dictionary<PersonRole, string>
            {
                [PersonRole.Performer] = "Agreed performance fee",
                [PersonRole.Crew] = "Hourly rate x weekly hours (1 festival week)",
                [PersonRole.Vendor] = "Vendors pay for their pitch - no staffing cost"
            };

            // One line per role. EstimateCost() is POLYMORPHIC: the same call returns the fee for
            // a Performer, rate x hours for a CrewMember and 0 for a Vendor.
            List<CostSummaryLine> lines = Enum.GetValues<PersonRole>()
                .Select(role => new CostSummaryLine
                {
                    Role = role.ToString(),
                    Count = people.Count(p => p.Role == role),
                    TotalCost = people.Where(p => p.Role == role).Sum(p => p.EstimateCost()),
                    Basis = basis[role]
                })
                .ToList();

            // Grand total line.
            lines.Add(new CostSummaryLine
            {
                Role = "TOTAL",
                Count = people.Count,
                TotalCost = people.Sum(p => p.EstimateCost()),
                Basis = "Estimated total cost of people for the festival"
            });
            return lines;
        }

        /// <inheritdoc />
        public OperationResult<string> ExportToCsv(IEnumerable<Person> people, string? filePath = null)
        {
            List<Person> list = people?.ToList() ?? new List<Person>();
            if (list.Count == 0)
            {
                return OperationResult<string>.Fail("There are no records to export.");
            }

            // Default location: Documents\FestivalExports\people_yyyyMMdd_HHmmss.csv
            if (string.IsNullOrWhiteSpace(filePath))
            {
                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FestivalExports");
                filePath = Path.Combine(folder, $"people_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            }

            try
            {
                string? directory = Path.GetDirectoryName(Path.GetFullPath(filePath));
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var csv = new StringBuilder();
                csv.AppendLine("PersonID,Name,Role,Telephone,Email,Fee,Genres,HourlyRate,EmploymentType,WeeklyHours,VendorName,ProductCategories,EstimatedCost");
                foreach (Person person in list)
                {
                    csv.AppendLine(string.Join(",", BuildCsvFields(person).Select(EscapeCsv)));
                }

                // UTF-8 WITH a byte-order mark so Excel shows accented names (e.g. Vietnamese) correctly.
                File.WriteAllText(filePath, csv.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
                return OperationResult<string>.Ok(Path.GetFullPath(filePath), $"{list.Count} record(s) exported to {Path.GetFullPath(filePath)}");
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException)
            {
                return OperationResult<string>.Fail("The CSV file could not be written.", new[] { ex.Message });
            }
        }

        /// <summary>Builds the 13 CSV columns for one person; columns of other roles stay empty.</summary>
        private static IEnumerable<string> BuildCsvFields(Person person)
        {
            CultureInfo invariant = CultureInfo.InvariantCulture;
            string fee = "", genres = "", rate = "", type = "", hours = "", stall = "", categories = "";

            // Pattern matching on the run-time type to fill the role-specific columns.
            switch (person)
            {
                case Performer performer:
                    fee = performer.Fee.ToString("0.00", invariant);
                    genres = string.Join("; ", performer.Genres);
                    break;
                case CrewMember crew:
                    rate = crew.HourlyRate.ToString("0.00", invariant);
                    type = crew.EmploymentType.ToString();
                    hours = crew.WeeklyHours.ToString(invariant);
                    break;
                case Vendor vendor:
                    stall = vendor.StallName;
                    categories = string.Join("; ", vendor.ProductCategories);
                    break;
            }

            return new[]
            {
                person.PersonId.ToString(invariant), person.FullName, person.RoleName, person.Telephone, person.Email,
                fee, genres, rate, type, hours, stall, categories,
                person.EstimateCost().ToString("0.00", invariant)
            };
        }

        /// <summary>
        /// Escapes one CSV value (RFC 4180): values containing a comma, quote or new line are
        /// wrapped in quotes and inner quotes are doubled. Values starting with = + - @ are
        /// prefixed with ' to stop spreadsheet formula injection.
        /// </summary>
        private static string EscapeCsv(string value)
        {
            value ??= string.Empty;
            if (value.Length > 0 && "=+-@".IndexOf(value[0]) >= 0 && !decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
            {
                value = "'" + value;
            }
            bool needsQuotes = value.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0;
            return needsQuotes ? "\"" + value.Replace("\"", "\"\"") + "\"" : value;
        }
    }
}
