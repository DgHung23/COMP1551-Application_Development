// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : FakePersonRepository.cs
// Layer   : Unit tests (xUnit) for the Business layer and the Model
// Summary : In-memory implementation of IPersonRepository used by the unit tests.
// =====================================================================

using MVCConsoleApp.Data.Entities;
using MVCConsoleApp.Data.Repositories;

namespace MVCConsoleApp.Tests
{
    /// <summary>
    /// In-memory implementation of <see cref="IPersonRepository"/> used by the unit tests.
    /// <para>
    /// Because <c>PersonService</c> depends on the INTERFACE, the business rules can be tested
    /// quickly and repeatably without a running SQL Server - a direct benefit of the layered
    /// architecture and of programming against abstractions.
    /// </para>
    /// </summary>
    public class FakePersonRepository : IPersonRepository
    {
        // The "table": every person ever added (deleted ones have IsDeleted = true).
        private readonly List<Person> _people = new List<Person>();
        private int _nextId = 1;

        /// <summary>Number of times Create was called (lets tests prove nothing was saved).</summary>
        public int CreateCalls { get; private set; }

        /// <summary>Adds a person directly, bypassing the service (test set-up helper).</summary>
        public Person Seed(Person person)
        {
            person.PersonId = _nextId++;
            _people.Add(person);
            return person;
        }

        /// <summary>Simulates INSERT: assigns the next ID (like IDENTITY) and stores the object.</summary>
        public int Create(Person person)
        {
            CreateCalls++;
            person.PersonId = _nextId++;
            _people.Add(person);
            return person.PersonId;
        }

        /// <summary>Active people ordered by ID, like "WHERE IsDeleted = 0 ORDER BY PersonId".</summary>
        public List<Person> GetAll() => _people.Where(p => !p.IsDeleted).OrderBy(p => p.PersonId).ToList();

        /// <summary>Active people of one role.</summary>
        public List<Person> GetByRole(PersonRole role) => GetAll().Where(p => p.Role == role).ToList();

        /// <summary>Finds by ID; deleted records only when includeDeleted is true.</summary>
        public Person? GetById(int personId, bool includeDeleted = false) =>
            _people.FirstOrDefault(p => p.PersonId == personId && (includeDeleted || !p.IsDeleted));

        /// <summary>Case-insensitive email lookup over ALL records (active and deleted).</summary>
        public Person? GetByEmail(string email) =>
            _people.FirstOrDefault(p => p.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));

        /// <summary>Partial match on name or email, like SQL LIKE '%term%'.</summary>
        public List<Person> Search(string term) =>
            GetAll().Where(p => p.FullName.Contains(term, StringComparison.OrdinalIgnoreCase)
                             || p.Email.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();

        /// <summary>The recycle bin.</summary>
        public List<Person> GetDeleted() => _people.Where(p => p.IsDeleted).ToList();

        /// <summary>Replaces an active record; returns false when the ID does not exist.</summary>
        public bool Update(Person person)
        {
            int index = _people.FindIndex(p => p.PersonId == person.PersonId && !p.IsDeleted);
            if (index < 0) return false;
            _people[index] = person;   // replaces the object, so a role change is supported
            return true;
        }

        /// <summary>Marks an active record as deleted.</summary>
        public bool SoftDelete(int personId)
        {
            Person? person = GetById(personId);
            if (person == null) return false;
            person.IsDeleted = true;
            person.DeletedAt = DateTime.Now;
            return true;
        }

        /// <summary>Brings a deleted record back.</summary>
        public bool Restore(int personId)
        {
            Person? person = _people.FirstOrDefault(p => p.PersonId == personId && p.IsDeleted);
            if (person == null) return false;
            person.IsDeleted = false;
            person.DeletedAt = null;
            return true;
        }

        /// <summary>Removes a record that is in the recycle bin.</summary>
        public bool HardDelete(int personId) => _people.RemoveAll(p => p.PersonId == personId && p.IsDeleted) > 0;
    }
}
