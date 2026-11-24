// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : LookupRepository.cs
// Layer   : Data Access layer - repositories (all SQL lives here)
// Summary : DATA ACCESS LAYER - contract for the two lookup lists (genres and product categories).
// =====================================================================

using Microsoft.Data.SqlClient;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.Data.Repositories
{
    /// <summary>
    /// DATA ACCESS LAYER - contract for the two lookup lists (genres and product categories).
    /// </summary>
    public interface ILookupRepository
    {
        /// <summary>Returns all items of a lookup list with their usage count, ordered by name.</summary>
        List<LookupItem> GetAll(LookupKind kind);

        /// <summary>Returns the item with the given name (case-insensitive), or null.</summary>
        LookupItem? GetByName(LookupKind kind, string name);

        /// <summary>Inserts a new item and returns its generated ID.</summary>
        int Create(LookupKind kind, string name);

        /// <summary>Deletes an item that is not linked to anybody. Returns false when not found.</summary>
        bool Delete(LookupKind kind, int id);
    }

    /// <summary>
    /// DATA ACCESS LAYER - SQL Server implementation of <see cref="ILookupRepository"/>.
    /// The two lookup tables have the same shape, so one class serves both; the table and
    /// column names come from a fixed internal map (never from user input).
    /// </summary>
    public class LookupRepository : RepositoryBase, ILookupRepository
    {
        /// <summary>Table and column names of one lookup list.</summary>
        private sealed record TableInfo(string Table, string IdColumn, string NameColumn, string LinkTable);

        // Fixed mapping LookupKind -> SQL object names.
        private static readonly Dictionary<LookupKind, TableInfo> Tables = new Dictionary<LookupKind, TableInfo>
        {
            [LookupKind.Genre] = new TableInfo("Genres", "GenreId", "GenreName", "PerformerGenres"),
            [LookupKind.ProductCategory] = new TableInfo("ProductCategories", "CategoryId", "CategoryName", "VendorCategories")
        };

        /// <summary>Returns all items of the list with the number of ACTIVE people using each one.</summary>
        /// <param name="kind">Which list to read.</param>
        public List<LookupItem> GetAll(LookupKind kind)
        {
            // Count how many ACTIVE people use each item (LEFT JOIN keeps unused items with 0).
            // Table/column names come from the fixed Tables map, user input only goes in parameters.
            TableInfo t = Tables[kind];
            string sql =
                $"SELECT l.{t.IdColumn} AS Id, l.{t.NameColumn} AS Name, COUNT(p.PersonId) AS UsageCount " +
                $"FROM {t.Table} l " +
                $"LEFT JOIN {t.LinkTable} k ON k.{t.IdColumn} = l.{t.IdColumn} " +
                "LEFT JOIN People p ON p.PersonId = k.PersonId AND p.IsDeleted = 0 " +
                $"GROUP BY l.{t.IdColumn}, l.{t.NameColumn} " +
                $"ORDER BY l.{t.NameColumn}";
            return QueryItems(kind, sql, null, "loading the " + kind + " list");
        }

        /// <summary>Finds an item by name (case-insensitive).</summary>
        /// <param name="kind">Which list to search.</param>
        /// <param name="name">Name to find.</param>
        public LookupItem? GetByName(LookupKind kind, string name)
        {
            TableInfo t = Tables[kind];
            // Case-insensitive comparison so "rock" finds "Rock".
            string sql = $"SELECT {t.IdColumn} AS Id, {t.NameColumn} AS Name, 0 AS UsageCount " +
                         $"FROM {t.Table} WHERE LOWER({t.NameColumn}) = LOWER(@name)";
            return QueryItems(kind, sql, command => command.Parameters.AddWithValue("@name", name.Trim()),
                              "looking up a " + kind).FirstOrDefault();
        }

        /// <summary>Inserts a new lookup item.</summary>
        /// <param name="kind">Which list to add to.</param>
        /// <param name="name">Validated, unique name.</param>
        /// <returns>The generated ID.</returns>
        public int Create(LookupKind kind, string name)
        {
            TableInfo t = Tables[kind];
            try
            {
                using (SqlConnection connection = OpenConnection())
                using (SqlCommand command = new SqlCommand(
                    // OUTPUT INSERTED returns the IDENTITY value generated for the new row.
                    $"INSERT INTO {t.Table} ({t.NameColumn}) OUTPUT INSERTED.{t.IdColumn} VALUES (@name)", connection))
                {
                    command.Parameters.AddWithValue("@name", name.Trim());
                    return (int)command.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                throw Wrap("adding a " + kind, ex);
            }
        }

        /// <summary>
        /// Deletes a lookup item. Only items that are not linked to any person (active or deleted)
        /// are removed; the NOT EXISTS check avoids a foreign-key error.
        /// </summary>
        /// <param name="kind">Which list to delete from.</param>
        /// <param name="id">ID of the item.</param>
        /// <returns>True when deleted; false when not found or still in use.</returns>
        public bool Delete(LookupKind kind, int id)
        {
            TableInfo t = Tables[kind];
            try
            {
                using (SqlConnection connection = OpenConnection())
                using (SqlCommand command = new SqlCommand(
                    // The NOT EXISTS condition makes the DELETE affect 0 rows when the item is still
                    // linked to somebody, so the caller gets "false" instead of a foreign-key error.
                    $"DELETE FROM {t.Table} WHERE {t.IdColumn} = @id " +
                    $"AND NOT EXISTS (SELECT 1 FROM {t.LinkTable} WHERE {t.IdColumn} = @id)", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                throw Wrap("deleting a " + kind, ex);
            }
        }

        /// <summary>Runs a query returning Id, Name and UsageCount columns and maps each row.</summary>
        private List<LookupItem> QueryItems(LookupKind kind, string sql, Action<SqlCommand>? addParameters, string operation)
        {
            var result = new List<LookupItem>();
            try
            {
                using (SqlConnection connection = OpenConnection())
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    addParameters?.Invoke(command);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Map the current row to a LookupItem.
                            result.Add(new LookupItem
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                UsageCount = reader.GetInt32(reader.GetOrdinal("UsageCount")),
                                Kind = kind
                            });
                        }
                    }
                }
            }
            // Report which operation failed without exposing SqlException to upper layers.
            catch (SqlException ex)
            {
                throw Wrap(operation, ex);
            }
            return result;
        }
    }
}
