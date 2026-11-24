namespace MVCConsoleApp.Data
{
    /// <summary>
    /// Exception thrown by the Data Access layer whenever a database operation fails.
    /// <para>
    /// Repositories never write to the console or show message boxes (that is the job of
    /// the Presentation layer). Instead, low-level <c>SqlException</c>s are wrapped into this
    /// single, technology-neutral exception type so that the upper layers can handle every
    /// database problem in one place.
    /// </para>
    /// </summary>
    public class DataAccessException : Exception
    {
        // SQL Server error numbers that indicate a UNIQUE / PRIMARY KEY violation.
        private const int UniqueConstraintViolation = 2627;
        private const int UniqueIndexViolation = 2601;

        /// <summary>Creates the exception with a user-friendly message and the original cause.</summary>
        /// <param name="message">Message describing which operation failed.</param>
        /// <param name="innerException">The original exception (usually a SqlException).</param>
        public DataAccessException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
            // Inspect the inner SqlException (if any) once, so callers can simply read a flag.
            if (innerException is Microsoft.Data.SqlClient.SqlException sqlException)
            {
                SqlErrorNumber = sqlException.Number;
            }
        }

        /// <summary>Gets the SQL Server error number of the original error, or 0 if unknown.</summary>
        public int SqlErrorNumber { get; }

        /// <summary>
        /// Gets a value indicating whether the failure was caused by a duplicate key,
        /// e.g. inserting an email address that already exists.
        /// </summary>
        public bool IsDuplicateKey =>
            SqlErrorNumber == UniqueConstraintViolation || SqlErrorNumber == UniqueIndexViolation;
    }
}
