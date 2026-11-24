// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : EntityTests.cs
// Layer   : Unit tests (xUnit) for the Business layer and the Model
// Summary : Tests of the Model classes: encapsulation (setters guard their fields), inheritance and
// =====================================================================

using MVCConsoleApp.BusinessLogic.Validation;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.Tests
{
    /// <summary>
    /// Tests of the Model classes: encapsulation (setters guard their fields), inheritance and
    /// polymorphism (each sub-class answers the same call differently).
    /// </summary>
    public class EntityTests
    {
        // ======================== Encapsulation ========================

        /// <summary>
        /// Method/feature: name. Expected: cannot be empty.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Name_CannotBeEmpty()
        {
            // Arrange: prepare the objects and data used by the test.
            var performer = new Performer();
            // Assert: check the expected outcome.
            Assert.Throws<ArgumentException>(() => performer.FullName = "   ");
        }

        /// <summary>
        /// Method/feature: fee. Expected: cannot be negative.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Fee_CannotBeNegative()
        {
            // Arrange: prepare the objects and data used by the test.
            var performer = new Performer();
            // Assert: check the expected outcome.
            Assert.Throws<ArgumentOutOfRangeException>(() => performer.Fee = -1m);
        }

        /// <summary>
        /// Method/feature: hourly rate. Expected: must be positive.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void HourlyRate_MustBePositive()
        {
            // Arrange: prepare the objects and data used by the test.
            var crew = new CrewMember();
            // Assert: check the expected outcome.
            Assert.Throws<ArgumentOutOfRangeException>(() => crew.HourlyRate = 0m);
        }

        /// <summary>
        /// Method/feature: email. Expected: is trimmed and lower cased.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Email_IsTrimmedAndLowerCased()
        {
            // Arrange: prepare the objects and data used by the test.
            var vendor = new Vendor { Email = "  Hannah.LEE@Festival.Test " };
            // Assert: check the expected outcome.
            Assert.Equal("hannah.lee@festival.test", vendor.Email);
        }

        /// <summary>
        /// Method/feature: telephone. Expected: spaces dashes and brackets are removed.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Telephone_SpacesDashesAndBracketsAreRemoved()
        {
            // Arrange: prepare the objects and data used by the test.
            var crew = new CrewMember { Telephone = "(0770) 090-0101" };
            // Assert: check the expected outcome.
            Assert.Equal("07700900101", crew.Telephone);
        }

        /// <summary>
        /// Method/feature: genres. Scenario: are cleaned. Expected: and cannot be changed from outside.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void Genres_AreCleaned_AndCannotBeChangedFromOutside()
        {
            // Arrange: prepare the objects and data used by the test.
            var performer = new Performer("Luna", "07700900101", "l@x.test", 10m, new[] { " Pop ", "pop", "", "Indie" });

            // Assert: check the expected outcome.
            Assert.Equal(new[] { "Pop", "Indie" }, performer.Genres);
            Assert.IsAssignableFrom<IReadOnlyList<string>>(performer.Genres);
        }

        // ================== Inheritance and polymorphism ==================

        /// <summary>
        /// Method/feature: every subclass. Scenario: is a person. Expected: with its own role.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void EverySubclass_IsAPerson_WithItsOwnRole()
        {
            // Arrange: prepare the objects and data used by the test.
            List<Person> people = new List<Person>
            {
                new Performer("A Performer", "07700900101", "a@x.test", 100m, new[] { "Pop", "Rock" }),
                new CrewMember("A Crew", "07700900102", "b@x.test", 10m, EmploymentType.FullTime, 30),
                new Vendor("A Vendor", "07700900103", "c@x.test", "Stall", new[] { "A", "B", "C" })
            };

            // Assert: check the expected outcome.
            Assert.Equal(new[] { PersonRole.Performer, PersonRole.Crew, PersonRole.Vendor }, people.Select(p => p.Role));
        }

        /// <summary>
        /// Method/feature: estimate cost. Expected: is polymorphic.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void EstimateCost_IsPolymorphic()
        {
            // Arrange: prepare the objects and data used by the test.
            Person performer = new Performer("P", "07700900101", "p@x.test", 5000m, new[] { "Pop", "Rock" });
            Person crew = new CrewMember("C", "07700900102", "c@x.test", 12.50m, EmploymentType.PartTime, 20);
            Person vendor = new Vendor("V", "07700900103", "v@x.test", "Stall", new[] { "A", "B", "C" });

            // Assert: check the expected outcome.
            Assert.Equal(5000m, performer.EstimateCost());
            Assert.Equal(250m, crew.EstimateCost());   // 12.50 x 20
            Assert.Equal(0m, vendor.EstimateCost());
        }

        /// <summary>
        /// Method/feature: describe role fields. Expected: is polymorphic.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void DescribeRoleFields_IsPolymorphic()
        {
            // Arrange: prepare the objects and data used by the test.
            Person crew = new CrewMember("C", "07700900102", "c@x.test", 12.50m, EmploymentType.PartTime, 20);

            // Act: call the code under test.
            List<string> labels = crew.DescribeRoleFields().Select(f => f.Key).ToList();

            // Assert: check the expected outcome.
            Assert.Equal(new[] { "Hourly rate", "Employment", "Weekly hours" }, labels);
        }

        // ========================= Validators =========================

        /// <summary>
        /// Method/feature: validator factory. Expected: returns role specific validator.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void ValidatorFactory_ReturnsRoleSpecificValidator()
        {
            // Assert: check the expected outcome.
            Assert.IsType<PerformerValidator>(PersonValidator.For(PersonRole.Performer));
            Assert.IsType<CrewValidator>(PersonValidator.For(PersonRole.Crew));
            Assert.IsType<VendorValidator>(PersonValidator.For(PersonRole.Vendor));
        }

        /// <summary>
        /// Verifies telephone rule.
        /// Type: data-driven test (one run per InlineData row).
        /// </summary>
        [Theory]
        [InlineData("07700900101", true)]
        [InlineData("+447700900101", true)]
        [InlineData("1234567", false)]        // too short
        [InlineData("0770ABC0101", false)]    // letters
        public void TelephoneRule(string telephone, bool valid)
        {
            // Assert: check the expected outcome.
            Assert.Equal(valid, FieldRules.CheckTelephone(telephone) == null);
        }

        /// <summary>
        /// Method/feature: genre names. Expected: cannot contain comma.
        /// Type: single test case.
        /// </summary>
        [Fact]
        public void GenreNames_CannotContainComma()
        {
            // Assert: check the expected outcome.
            Assert.NotNull(FieldRules.CheckLookupName("Rock, Pop"));
        }
    }
}
