// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : ReportRepository.cs
// Layer   : Data Access layer - repositories (all SQL lives here)
// Summary : DATA ACCESS LAYER - contract for the aggregated "group by" reports (extension feature).
// =====================================================================

using Microsoft.Data.SqlClient;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.Data.Repositories
{
    /// <summary>
    /// DATA ACCESS LAYER - contract for the aggregated "group by" reports (extension feature).
    /// </summary>
    public interface IReportRepository
    {
        /// <summary>Number of active performers per genre, with their names.</summary>
        List<GroupSummary> GetPerformersByGenre();

        /// <summary>Number of active vendors per product category, with their stall names.</summary>
        List<GroupSummary> GetVendorsByCategory();
    }

    /// <summary>
    /// DATA ACCESS LAYER - SQL Server implementation of <see cref="IReportRepository"/>.
    /// The grouping is done by the database (GROUP BY + STRING_AGG), which is faster than
    /// loading every row into memory and grouping in C#.
    /// </summary>
    public class ReportRepository : RepositoryBase, IReportRepository
    {
        /// <summary>Groups active performers by genre (genres nobody plays are listed with 0).</summary>
        public List<GroupSummary> GetPerformersByGenre()
        {
            const string sql =
                // LEFT JOINs keep genres that nobody plays (count 0); the extra condition
                // "p.IsDeleted = 0" makes sure soft-deleted performers are not counted.
                "SELECT g.GenreName AS GroupName, COUNT(p.PersonId) AS [Count], " +
                "       ISNULL(STRING_AGG(p.FullName, ', ') WITHIN GROUP (ORDER BY p.FullName), '') AS Members " +
                "FROM Genres g " +
                "LEFT JOIN PerformerGenres pg ON pg.GenreId = g.GenreId " +
                "LEFT JOIN People p ON p.PersonId = pg.PersonId AND p.IsDeleted = 0 " +
                "GROUP BY g.GenreName " +
                "ORDER BY [Count] DESC, g.GenreName";
            return QuerySummaries(sql, "building the genre report");
        }

        /// <summary>Groups active vendors by product category (unused categories are listed with 0).</summary>
        public List<GroupSummary> GetVendorsByCategory()
        {
            const string sql =
                // Same pattern for categories; STRING_AGG lists the stall names in each group.
                "SELECT c.CategoryName AS GroupName, COUNT(p.PersonId) AS [Count], " +
                "       ISNULL(STRING_AGG(v.StallName, ', ') WITHIN GROUP (ORDER BY v.StallName), '') AS Members " +
                "FROM ProductCategories c " +
                "LEFT JOIN VendorCategories vc ON vc.CategoryId = c.CategoryId " +
                "LEFT JOIN People p  ON p.PersonId = vc.PersonId AND p.IsDeleted = 0 " +
                "LEFT JOIN Vendors v ON v.PersonId = p.PersonId " +
                "GROUP BY c.CategoryName " +
                "ORDER BY [Count] DESC, c.CategoryName";
            return QuerySummaries(sql, "building the category report");
        }

        /// <summary>Executes a report query and maps the GroupName / Count / Members columns.</summary>
        private List<GroupSummary> QuerySummaries(string sql, string operation)
        {
            // Collect the report lines; an empty list means there is nothing to report.
            var result = new List<GroupSummary>();
            try
            {
                // Open the connection, run the query and read every row; the using blocks close
                // the reader and the connection even if an exception happens.
                using (SqlConnection connection = OpenConnection())
                using (SqlCommand command = new SqlCommand(sql, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Map the three columns of the current row to a GroupSummary object.
                        result.Add(new GroupSummary
                        {
                            GroupName = reader.GetString(reader.GetOrdinal("GroupName")),
                            Count = reader.GetInt32(reader.GetOrdinal("Count")),
                            Members = reader.GetString(reader.GetOrdinal("Members"))
                        });
                    }
                }
            }
            // Translate the SQL Server error into the layer's own exception type.
            catch (SqlException ex)
            {
                throw Wrap(operation, ex);
            }
            return result;
        }
    }
}
