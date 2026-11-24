// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : LookupService.cs
// Layer   : Business layer - application services
// Summary : BUSINESS LAYER - contract for managing the genre and product-category lists.
// =====================================================================

using MVCConsoleApp.BusinessLogic.Common;
using MVCConsoleApp.BusinessLogic.Validation;
using MVCConsoleApp.Data.Entities;
using MVCConsoleApp.Data.Repositories;

namespace MVCConsoleApp.BusinessLogic.Services
{
    /// <summary>
    /// BUSINESS LAYER - contract for managing the genre and product-category lists.
    /// </summary>
    public interface ILookupService
    {
        /// <summary>Returns every item of the list (with usage counts).</summary>
        List<LookupItem> GetAll(LookupKind kind);

        /// <summary>Validates and adds a new item.</summary>
        OperationResult Add(LookupKind kind, string name);

        /// <summary>Deletes an item that nobody uses.</summary>
        OperationResult Delete(LookupKind kind, int id);
    }

    /// <summary>
    /// BUSINESS LAYER - rules for the lookup lists: names must be valid and unique, and an item
    /// that is still linked to a performer or vendor cannot be deleted.
    /// </summary>
    public class LookupService : ILookupService
    {
        private readonly ILookupRepository _repository;

        /// <summary>Creates the service with the real SQL Server repository.</summary>
        public LookupService() : this(new LookupRepository())
        {
        }

        /// <summary>Creates the service with any repository implementation.</summary>
        /// <param name="repository">Repository used to store lookup items.</param>
        public LookupService(ILookupRepository repository)
        {
            // Fail fast if no repository is supplied.
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc />
        public List<LookupItem> GetAll(LookupKind kind)
        {
            return _repository.GetAll(kind);
        }

        /// <inheritdoc />
        public OperationResult Add(LookupKind kind, string name)
        {
            // 1) The name must be valid (no commas, not too long ...).
            string label = DisplayName(kind);

            string? error = FieldRules.CheckLookupName(name);
            if (error != null)
            {
                return OperationResult.Fail($"The {label} was not added.", new[] { error });
            }
            // 2) Names must be unique (case-insensitive): "rock" and "Rock" are the same genre.
            if (_repository.GetByName(kind, name) != null)
            {
                return OperationResult.Fail($"The {label} '{name.Trim()}' already exists.");
            }

            // 3) Save the new item and report its generated ID.
            int id = _repository.Create(kind, name.Trim());
            return OperationResult.Ok($"{Capitalise(label)} '{name.Trim()}' added (ID {id}).");
        }

        /// <inheritdoc />
        public OperationResult Delete(LookupKind kind, int id)
        {
            string label = DisplayName(kind);
            // Find the item together with its usage count (how many ACTIVE people use it).
            LookupItem? item = _repository.GetAll(kind).FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return OperationResult.Fail($"No {label} with ID {id} was found.");
            }
            // Business rule: an item in use cannot be deleted, otherwise people would lose data.
            if (item.UsageCount > 0)
            {
                return OperationResult.Fail($"'{item.Name}' is used by {item.UsageCount} record(s) and cannot be deleted.");
            }

            // The repository refuses to delete an item that is still linked to a record in the
            // recycle bin, so a false result means "in use" here.
            return _repository.Delete(kind, id)
                ? OperationResult.Ok($"{Capitalise(label)} '{item.Name}' deleted.")
                : OperationResult.Fail($"'{item.Name}' is still linked to a record in the recycle bin and cannot be deleted.");
        }

        /// <summary>Returns a friendly name for a lookup list, e.g. "product category".</summary>
        public static string DisplayName(LookupKind kind)
        {
            return kind == LookupKind.Genre ? "genre" : "product category";
        }

        /// <summary>Upper-cases the first letter of a text.</summary>
        private static string Capitalise(string text)
        {
            return text.Length == 0 ? text : char.ToUpperInvariant(text[0]) + text.Substring(1);
        }
    }
}
