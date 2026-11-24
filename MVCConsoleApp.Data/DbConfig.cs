// =====================================================================
// COMP1551 Application Development - Music Festival Operations System
// File    : DbConfig.cs
// Layer   : Data Access layer - configuration and error handling
// Summary : Central place for database configuration used by every repository.
// =====================================================================

namespace MVCConsoleApp.Data
{
    /// <summary>
    /// Central place for database configuration used by every repository.
    /// <para>
    /// The connection string can be overridden without recompiling by setting the
    /// environment variable <c>FESTIVAL_DB_CONNECTION</c>. When the variable is not set
    /// the default value points at the SQL Server 2022 Docker container that is
    /// published on <c>localhost:1433</c>.
    /// </para>
    /// </summary>
    public static class DbConfig
    {
        /// <summary>Name of the optional environment variable that overrides the default connection string.</summary>
        public const string EnvironmentVariableName = "FESTIVAL_DB_CONNECTION";

        /// <summary>Name of the database created by Database/databasescheme.sql.</summary>
        public const string DatabaseName = "MusicFestivalDB";

        // Default connection string for the local Docker container (development only).
        // "Connect Timeout=5" makes the application report a missing database quickly
        // instead of freezing the UI for the default 15 seconds.
        private const string DefaultConnectionString =
            "Server=localhost,1433;Database=" + DatabaseName + ";User Id=sa;Password=Hd112234;" +
            "TrustServerCertificate=True;Connect Timeout=5;";

        /// <summary>
        /// Gets the connection string that repositories should use:
        /// the environment variable when present, otherwise the default.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                string? fromEnvironment = Environment.GetEnvironmentVariable(EnvironmentVariableName);
                return string.IsNullOrWhiteSpace(fromEnvironment) ? DefaultConnectionString : fromEnvironment;
            }
        }
    }
}
