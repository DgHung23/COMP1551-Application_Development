// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : IPersonRepository.cs
// Layer   : Data Access layer - repositories (all SQL lives here)
// Summary : DATA ACCESS LAYER - contract for storing and loading people.
// =====================================================================

using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.Data.Repositories
{
    /// <summary>
    /// DATA ACCESS LAYER - contract for storing and loading people.
    /// <para>
    /// The Business layer depends on this interface instead of the concrete SQL class
    /// (abstraction / dependency inversion). This makes it possible to unit-test the business
    /// rules with an in-memory fake repository and to swap the database technology later
    /// (e.g. MySQL) without touching the Business or Presentation layers.
    /// </para>
    /// </summary>
    public interface IPersonRepository
    {
        /// <summary>Inserts a new person (common + role-specific data) and returns the new PersonID.</summary>
        int Create(Person person);

        /// <summary>Returns all active (not deleted) people ordered by PersonID.</summary>
        List<Person> GetAll();

        /// <summary>Returns all active people that have the given role.</summary>
        List<Person> GetByRole(PersonRole role);

        /// <summary>Returns the person with the given ID, or null when it does not exist.</summary>
        /// <param name="personId">PersonID to look for.</param>
        /// <param name="includeDeleted">True to also find records that are in the recycle bin.</param>
        Person? GetById(int personId, bool includeDeleted = false);

        /// <summary>Returns the person (active or deleted) that owns the email address, or null.</summary>
        Person? GetByEmail(string email);

        /// <summary>Returns active people whose name or email contains the search term (partial match).</summary>
        List<Person> Search(string term);

        /// <summary>Returns all soft-deleted people (the recycle bin).</summary>
        List<Person> GetDeleted();

        /// <summary>Updates an active person, including a possible change of role. Returns false when not found.</summary>
        bool Update(Person person);

        /// <summary>Moves an active person to the recycle bin. Returns false when not found.</summary>
        bool SoftDelete(int personId);

        /// <summary>Restores a person from the recycle bin. Returns false when not found.</summary>
        bool Restore(int personId);

        /// <summary>Permanently deletes a person that is in the recycle bin. Returns false when not found.</summary>
        bool HardDelete(int personId);
    }
}
