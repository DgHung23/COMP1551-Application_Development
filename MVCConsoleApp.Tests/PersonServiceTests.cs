// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : PersonServiceTests.cs
// Layer   : Unit tests (xUnit) for the Business layer and the Model
// Summary : Unit tests of the business rules enforced by PersonService.
// =====================================================================

using MVCConsoleApp.BusinessLogic.Services;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.Tests
{
    /// <summary>
    /// Unit tests of the business rules enforced by <see cref="PersonService"/>.
    /// Each test follows Arrange / Act / Assert.
    /// </summary>
    public class PersonServiceTests
    {
        private readonly FakePersonRepository _repository = new FakePersonRepository();
        private readonly PersonService _service;

        /// <summary>
        /// xUnit creates a new instance of this class for EVERY test, so each test starts with an
        /// empty fake repository and tests cannot influence each other.
        /// </summary>
        public PersonServiceTests()
        {
            _service = new PersonService(_repository);
        }

        // ---------- helpers that build VALID people ----------
        // Factory helpers: each returns a VALID object; a test changes only the value it checks.
        private static Performer NewPerformer(string email = "luna@festival.test", params string[] genres) =>
            new Performer("Luna Rivers", "07700900101", email, 12000m, genres.Length == 0 ? new[] { "Pop", "Indie" } : genres);

        private static CrewMember NewCrew(EmploymentType type, int hours, string email = "crew@festival.test") =>
            new CrewMember("Oliver Bennett", "07700900201", email, 18.50m, type, hours);

        private static Vendor NewVendor(string email = "vendor@festival.test", params string[] categories) =>
            new Vendor("Hannah Lee", "07700900301", email, "Seoul Street Kitchen",
                       categories.Length == 0 ? new[] { "Street Food", "Drinks", "Desserts" } : categories);

        // =========================== Add ===========================

        /// <summary>
        /// Method/feature: add. Scenario: valid performer. Expected: succeeds and returns new id.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Add_ValidPerformer_SucceedsAndReturnsNewId()
        {
            // Act: call the code under test.
            var result = _service.Add(NewPerformer());

            // Assert: check the expected outcome.
            Assert.True(result.Success, result.ToString());
            Assert.Equal(1, result.Value);
            Assert.Single(_service.GetAll());
        }

        /// <summary>
        /// Method/feature: add. Scenario: duplicate email. Expected: is rejected, ignoring case.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Add_DuplicateEmail_IsRejected_IgnoringCase()
        {
            // Arrange: prepare the objects and data used by the test.
            _service.Add(NewPerformer("luna@festival.test"));

            // Act: call the code under test.
            var result = _service.Add(NewVendor("LUNA@Festival.TEST"));

            // Assert: check the expected outcome.
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("already used"));
            Assert.Equal(1, _repository.CreateCalls);   // the second record never reached the database
        }

        /// <summary>
        /// Method/feature: add. Scenario: email of deleted record. Expected: is still rejected.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Add_EmailOfDeletedRecord_IsStillRejected()
        {
            // Arrange: prepare the objects and data used by the test.
            int id = _service.Add(NewPerformer("luna@festival.test")).Value;
            _service.Delete(id);

            // Act: call the code under test.
            var result = _service.Add(NewCrew(EmploymentType.FullTime, 30, "luna@festival.test"));

            // Assert: check the expected outcome.
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("recycle bin"));
        }

        /// <summary>
        /// Method/feature: add. Scenario: performer without exactly two genres. Expected: is rejected.
        /// Type: data-driven test (one run per InlineData row).
        /// </summary>
        [Theory]
        [InlineData("Rock")]                // too few
        [InlineData("Rock,Pop,Jazz")]       // too many
        public void Add_PerformerWithoutExactlyTwoGenres_IsRejected(string genreList)
        {
            // Act: call the code under test.
            var result = _service.Add(NewPerformer("p@festival.test", genreList.Split(',')));

            // Assert: check the expected outcome.
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("Exactly 2"));
        }

        /// <summary>
        /// Method/feature: add. Scenario: vendor with two categories. Expected: is rejected.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Add_VendorWithTwoCategories_IsRejected()
        {
            // Act: call the code under test.
            var result = _service.Add(NewVendor("v@festival.test", "Drinks", "Desserts"));

            // Assert: check the expected outcome.
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("Exactly 3"));
        }

        /// <summary>
        /// Method/feature: add. Scenario: crew. Expected: weekly hours must match employment type.
        /// Type: data-driven test (one run per InlineData row).
        /// </summary>
        [Theory]
        [InlineData(EmploymentType.FullTime, 24, false)]
        [InlineData(EmploymentType.FullTime, 25, true)]
        [InlineData(EmploymentType.FullTime, 40, true)]
        [InlineData(EmploymentType.FullTime, 41, false)]
        [InlineData(EmploymentType.PartTime, 1, true)]
        [InlineData(EmploymentType.PartTime, 24, true)]
        [InlineData(EmploymentType.PartTime, 25, false)]
        public void Add_Crew_WeeklyHoursMustMatchEmploymentType(EmploymentType type, int hours, bool expectedSuccess)
        {
            // Act: call the code under test.
            var result = _service.Add(NewCrew(type, hours));

            // Assert: check the expected outcome.
            Assert.Equal(expectedSuccess, result.Success);
        }

        /// <summary>
        /// Method/feature: add. Scenario: invalid email format. Expected: is rejected.
        /// Type: data-driven test (one run per InlineData row).
        /// </summary>
        [Theory]
        [InlineData("not-an-email")]
        [InlineData("missing@domain")]
        public void Add_InvalidEmailFormat_IsRejected(string email)
        {
            // Act: call the code under test.
            var result = _service.Add(NewPerformer(email));

            // Assert: check the expected outcome.
            Assert.False(result.Success);
        }

        // =========================== Edit ===========================

        /// <summary>
        /// Method/feature: update. Scenario: unknown id. Expected: fails gracefully.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Update_UnknownId_FailsGracefully()
        {
            // Arrange: prepare the objects and data used by the test.
            Performer performer = NewPerformer();
            performer.PersonId = 999;

            // Act: call the code under test.
            var result = _service.Update(performer);

            // Assert: check the expected outcome.
            Assert.False(result.Success);
            Assert.Contains("999", result.Message);
        }

        /// <summary>
        /// Method/feature: update. Scenario: can keep own email. Expected: and change role.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Update_CanKeepOwnEmail_AndChangeRole()
        {
            // Arrange: prepare the objects and data used by the test.
            int id = _service.Add(NewPerformer("same@festival.test")).Value;
            CrewMember crew = NewCrew(EmploymentType.PartTime, 10, "same@festival.test");
            crew.PersonId = id;

            // Act: call the code under test.
            var result = _service.Update(crew);

            // Assert: check the expected outcome.
            Assert.True(result.Success, result.ToString());
            Assert.IsType<CrewMember>(_service.GetById(id));
        }

        // ====================== Delete / Restore ======================

        /// <summary>
        /// Method/feature: delete. Scenario: unknown id. Expected: fails gracefully.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Delete_UnknownId_FailsGracefully()
        {
            // Act: call the code under test.
            var result = _service.Delete(42);

            // Assert: check the expected outcome.
            Assert.False(result.Success);
            Assert.Contains("42", result.Message);
        }

        /// <summary>
        /// Method/feature: delete. Scenario: then restore. Expected: record comes back.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Delete_ThenRestore_RecordComesBack()
        {
            // Arrange: prepare the objects and data used by the test.
            int id = _service.Add(NewPerformer()).Value;

            // Assert: check the expected outcome.
            Assert.True(_service.Delete(id).Success);
            Assert.Empty(_service.GetAll());
            Assert.Single(_service.GetDeleted());

            Assert.True(_service.Restore(id).Success);
            Assert.Single(_service.GetAll());
        }

        /// <summary>
        /// Method/feature: delete permanently. Expected: requires record to be in recycle bin.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void DeletePermanently_RequiresRecordToBeInRecycleBin()
        {
            // Arrange: prepare the objects and data used by the test.
            int id = _service.Add(NewPerformer()).Value;

            // Assert: check the expected outcome.
            Assert.False(_service.DeletePermanently(id).Success);   // still active
            _service.Delete(id);
            Assert.True(_service.DeletePermanently(id).Success);
            Assert.Empty(_service.GetDeleted());
        }

        // ====================== Filter / Search / Sort ======================

        /// <summary>
        /// Method/feature: get by role. Expected: returns only that role.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void GetByRole_ReturnsOnlyThatRole()
        {
            // Arrange: prepare the objects and data used by the test.
            _service.Add(NewPerformer("a@festival.test"));
            _service.Add(NewCrew(EmploymentType.FullTime, 30, "b@festival.test"));
            _service.Add(NewVendor("c@festival.test"));

            // Act: call the code under test.
            List<Person> crew = _service.GetByRole(PersonRole.Crew);

            // Assert: check the expected outcome.
            Assert.Single(crew);
            Assert.All(crew, p => Assert.Equal(PersonRole.Crew, p.Role));
        }

        /// <summary>
        /// Method/feature: sort. Scenario: by estimated cost descending. Expected: uses polymorphic cost.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Sort_ByEstimatedCostDescending_UsesPolymorphicCost()
        {
            // Arrange: prepare the objects and data used by the test.
            _service.Add(NewVendor("v@festival.test"));                          // cost 0
            _service.Add(NewCrew(EmploymentType.FullTime, 40, "c@festival.test")); // 18.50 x 40 = 740
            _service.Add(NewPerformer("p@festival.test"));                       // 12,000

            // Act: call the code under test.
            List<Person> sorted = _service.Sort(_service.GetAll(), SortField.EstimatedCost, descending: true);

            // Assert: check the expected outcome.
            Assert.Equal(new[] { PersonRole.Performer, PersonRole.Crew, PersonRole.Vendor }, sorted.Select(p => p.Role));
        }
    }
}
