using Microsoft.Data.SqlClient;

namespace MVCConsoleApp.Data.Repositories
{
    /// <summary>
    /// DATA ACCESS LAYER - abstract base class shared by every repository.
    /// <para>
    /// It centralises the code that all repositories need (opening a connection, converting
    /// null values, wrapping SQL errors), so the concrete repositories inherit it instead of
    /// repeating it (inheritance used for code re-use).
    /// </para>
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>Connection string read once from <see cref="DbConfig"/>.</summary>
        protected readonly string connectionString;

        /// <summary>Initialises the repository with the configured connection string.</summary>
        protected RepositoryBase()
        {
            connectionString = DbConfig.ConnectionString;
        }

        /// <summary>
        /// Creates and opens a new SQL Server connection. The caller must dispose it
        /// (always used inside a <c>using</c> block). ADO.NET connection pooling makes
        /// opening a connection per operation cheap.
        /// </summary>
        /// <returns>An open connection.</returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Wraps a low-level exception into a <see cref="DataAccessException"/> with a message that
        /// explains which operation failed.
        /// </summary>
        /// <param name="operation">Description of the operation, e.g. "adding a person".</param>
        /// <param name="exception">The original exception.</param>
        /// <returns>The exception to throw.</returns>
        protected static DataAccessException Wrap(string operation, Exception exception)
        {
            return new DataAccessException($"Database error while {operation}: {exception.Message}", exception);
        }

        /// <summary>Converts a .NET string to a database value: empty strings become NULL.</summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>The value, or <see cref="DBNull.Value"/>.</returns>
        protected static object ToDbValue(string? value)
        {
            return string.IsNullOrEmpty(value) ? DBNull.Value : value;
        }

        /// <summary>
        /// Escapes the LIKE wildcard characters (%, _ and [) in user input so that a search for
        /// "100%" looks for the text "100%" instead of treating % as a wildcard.
        /// </summary>
        /// <param name="term">Raw search term.</param>
        /// <returns>The escaped term.</returns>
        protected static string EscapeLike(string term)
        {
            return term.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }
    }
}
