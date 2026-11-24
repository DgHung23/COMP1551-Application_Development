using MVCConsoleApp.BusinessLogic.Common;
using MVCConsoleApp.BusinessLogic.Validation;
using MVCConsoleApp.Data;
using MVCConsoleApp.Data.Entities;
using MVCConsoleApp.Data.Repositories;

namespace MVCConsoleApp.BusinessLogic.Services
{
    /// <summary>Fields that a list of people can be sorted by (extension feature).</summary>
    public enum SortField
    {
        /// <summary>Sort by PersonID.</summary>
        PersonId = 1,

        /// <summary>Sort alphabetically by full name.</summary>
        Name = 2,

        /// <summary>Sort by role (Performer, Crew, Vendor), then by name.</summary>
        Role = 3,

        /// <summary>Sort alphabetically by email.</summary>
        Email = 4,

        /// <summary>Sort by estimated cost to the festival.</summary>
        EstimatedCost = 5
    }

    /// <summary>
    /// BUSINESS LAYER - contract of the person service used by both user interfaces
    /// (console controllers and Windows Forms).
    /// </summary>
    public interface IPersonService
    {
        /// <summary>Returns all active people.</summary>
        List<Person> GetAll();

        /// <summary>Returns all active people with the given role.</summary>
        List<Person> GetByRole(PersonRole role);

        /// <summary>Returns an active person by ID, or null when not found.</summary>
        Person? GetById(int personId);

        /// <summary>Partial-match search on name or email.</summary>
        List<Person> Search(string term);

        /// <summary>Returns the people in the recycle bin.</summary>
        List<Person> GetDeleted();

        /// <summary>Returns a sorted copy of a list of people.</summary>
        List<Person> Sort(IEnumerable<Person> people, SortField field, bool descending);

        /// <summary>Checks that an email has a valid format and is not used by another person.</summary>
        OperationResult CheckEmailAvailable(string email, int? excludePersonId = null);

        /// <summary>Validates and adds a new person. On success the result carries the new PersonID.</summary>
        OperationResult<int> Add(Person person);

        /// <summary>Validates and updates an existing person (identified by PersonId).</summary>
        OperationResult Update(Person person);

        /// <summary>Moves a person to the recycle bin (soft delete).</summary>
        OperationResult Delete(int personId);

        /// <summary>Restores a person from the recycle bin.</summary>
        OperationResult Restore(int personId);

        /// <summary>Permanently deletes a person that is in the recycle bin.</summary>
        OperationResult DeletePermanently(int personId);
    }

    /// <summary>
    /// BUSINESS LAYER - application logic for people.
    /// <para>
    /// All business rules are enforced here before anything reaches the database:
    /// field validation (via <see cref="PersonValidator"/>), unique email, exactly 2 genres /
    /// 3 categories, weekly-hours range, and graceful handling of IDs that do not exist.
    /// The service talks to the database only through <see cref="IPersonRepository"/>.
    /// </para>
    /// </summary>
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;

        /// <summary>Creates the service with the real SQL Server repository.</summary>
        public PersonService() : this(new PersonRepository())
        {
        }

        /// <summary>
        /// Creates the service with any repository implementation (dependency injection).
        /// Unit tests pass an in-memory fake here.
        /// </summary>
        /// <param name="repository">Repository used to store people.</param>
        public PersonService(IPersonRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // =============================== Queries ===============================

        /// <inheritdoc />
        public List<Person> GetAll()
        {
            return _repository.GetAll();
        }

        /// <inheritdoc />
        public List<Person> GetByRole(PersonRole role)
        {
            return _repository.GetByRole(role);
        }

        /// <inheritdoc />
        public Person? GetById(int personId)
        {
            // IDs are always positive, so there is no need to ask the database for 0 or -5.
            return personId > 0 ? _repository.GetById(personId) : null;
        }

        /// <inheritdoc />
        public List<Person> Search(string term)
        {
            // An empty search term simply means "show everything".
            return string.IsNullOrWhiteSpace(term) ? _repository.GetAll() : _repository.Search(term);
        }

        /// <inheritdoc />
        public List<Person> GetDeleted()
        {
            return _repository.GetDeleted();
        }

        /// <inheritdoc />
        public List<Person> Sort(IEnumerable<Person> people, SortField field, bool descending)
        {
            // Pick the key used for ordering. Every key ends with PersonId as a tie-breaker so
            // the order is always stable and predictable.
            IOrderedEnumerable<Person> ordered = field switch
            {
                SortField.Name => people.OrderBy(p => p.FullName, StringComparer.CurrentCultureIgnoreCase),
                SortField.Role => people.OrderBy(p => p.Role).ThenBy(p => p.FullName, StringComparer.CurrentCultureIgnoreCase),
                SortField.Email => people.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase),
                SortField.EstimatedCost => people.OrderBy(p => p.EstimateCost()),   // polymorphic call
                _ => people.OrderBy(p => p.PersonId)
            };
            List<Person> result = ordered.ThenBy(p => p.PersonId).ToList();

            if (descending)
            {
                result.Reverse();
            }
            return result;
        }

        // ============================ Email uniqueness ============================

        /// <inheritdoc />
        public OperationResult CheckEmailAvailable(string email, int? excludePersonId = null)
        {
            string? formatError = FieldRules.CheckEmailFormat(email);
            if (formatError != null)
            {
                return OperationResult.Fail(formatError);
            }

            Person? owner = _repository.GetByEmail(email);
            if (owner == null || owner.PersonId == excludePersonId)
            {
                return OperationResult.Ok("Email is available.");
            }

            // Tell the user exactly where the email is used, including the recycle bin.
            string where = owner.IsDeleted ? "a record in the recycle bin" : "another record";
            return OperationResult.Fail($"The email '{email.Trim()}' is already used by {where} (PersonID {owner.PersonId}).");
        }

        // ============================ Add / Update ============================

        /// <inheritdoc />
        public OperationResult<int> Add(Person person)
        {
            if (person == null)
            {
                return OperationResult<int>.Fail("No record was provided.");
            }

            // 1) Validate every field with the validator that matches the role (polymorphism).
            List<string> errors = PersonValidator.For(person.Role).Validate(person);

            // 2) Business rule: a new person cannot be added if the email already exists.
            if (errors.Count == 0)
            {
                OperationResult emailCheck = CheckEmailAvailable(person.Email);
                if (!emailCheck.Success)
                {
                    errors.Add(emailCheck.Message);
                }
            }

            if (errors.Count > 0)
            {
                return OperationResult<int>.Fail("The record was not added.", errors);
            }

            // 3) Persist. The UNIQUE constraint is a safety net if two users add the same email at once.
            try
            {
                int newId = _repository.Create(person);
                return OperationResult<int>.Ok(newId, $"{person.Role} '{person.FullName}' added with PersonID {newId}.");
            }
            catch (DataAccessException ex) when (ex.IsDuplicateKey)
            {
                return OperationResult<int>.Fail("The record was not added.", new[] { "This email address already exists." });
            }
        }

        /// <inheritdoc />
        public OperationResult Update(Person person)
        {
            if (person == null)
            {
                return OperationResult.Fail("No record was provided.");
            }

            // 1) Graceful handling of an ID that does not exist.
            if (GetById(person.PersonId) == null)
            {
                return NotFound(person.PersonId);
            }

            // 2) Validate the new values and make sure the email is not used by SOMEONE ELSE.
            List<string> errors = PersonValidator.For(person.Role).Validate(person);
            if (errors.Count == 0)
            {
                OperationResult emailCheck = CheckEmailAvailable(person.Email, person.PersonId);
                if (!emailCheck.Success)
                {
                    errors.Add(emailCheck.Message);
                }
            }
            if (errors.Count > 0)
            {
                return OperationResult.Fail("The record was not updated.", errors);
            }

            // 3) Persist (the repository returns false if the record vanished in the meantime).
            try
            {
                return _repository.Update(person)
                    ? OperationResult.Ok($"PersonID {person.PersonId} was updated successfully.")
                    : NotFound(person.PersonId);
            }
            catch (DataAccessException ex) when (ex.IsDuplicateKey)
            {
                return OperationResult.Fail("The record was not updated.", new[] { "This email address already exists." });
            }
        }

        // ========================== Delete / Restore ==========================

        /// <inheritdoc />
        public OperationResult Delete(int personId)
        {
            Person? person = GetById(personId);
            if (person == null)
            {
                return NotFound(personId);
            }

            return _repository.SoftDelete(personId)
                ? OperationResult.Ok($"{person.Role} '{person.FullName}' (PersonID {personId}) was deleted. It can be restored from the recycle bin.")
                : NotFound(personId);
        }

        /// <inheritdoc />
        public OperationResult Restore(int personId)
        {
            Person? person = personId > 0 ? _repository.GetById(personId, includeDeleted: true) : null;
            if (person == null || !person.IsDeleted)
            {
                return OperationResult.Fail($"The recycle bin does not contain PersonID {personId}.");
            }

            return _repository.Restore(personId)
                ? OperationResult.Ok($"'{person.FullName}' (PersonID {personId}) was restored.")
                : OperationResult.Fail($"PersonID {personId} could not be restored.");
        }

        /// <inheritdoc />
        public OperationResult DeletePermanently(int personId)
        {
            Person? person = personId > 0 ? _repository.GetById(personId, includeDeleted: true) : null;
            if (person == null || !person.IsDeleted)
            {
                return OperationResult.Fail($"The recycle bin does not contain PersonID {personId}. Delete the record first.");
            }

            return _repository.HardDelete(personId)
                ? OperationResult.Ok($"'{person.FullName}' (PersonID {personId}) was permanently deleted.")
                : OperationResult.Fail($"PersonID {personId} could not be deleted.");
        }

        /// <summary>Standard "ID not found" result used by edit and delete.</summary>
        private static OperationResult NotFound(int personId)
        {
            return OperationResult.Fail($"No record with PersonID {personId} was found.");
        }
    }
}
