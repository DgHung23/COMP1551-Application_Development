namespace MVCConsoleApp.BusinessLogic.Common
{
    /// <summary>
    /// BUSINESS LAYER - the outcome of an operation that changes data (add, edit, delete ...).
    /// <para>
    /// Instead of throwing exceptions for expected situations (invalid input, ID not found,
    /// duplicate email) the services return an <see cref="OperationResult"/>. The Presentation
    /// layer (console or Windows Forms) then decides how to show the message to the user.
    /// </para>
    /// </summary>
    public class OperationResult
    {
        /// <summary>Creates a result. Use the static factory methods <see cref="Ok"/> and <see cref="Fail(string, IEnumerable{string}?)"/>.</summary>
        protected OperationResult(bool success, string message, IEnumerable<string>? errors)
        {
            Success = success;
            Message = message;
            Errors = (errors ?? Enumerable.Empty<string>()).ToList().AsReadOnly();
        }

        /// <summary>Gets a value indicating whether the operation succeeded.</summary>
        public bool Success { get; }

        /// <summary>Gets a short summary message for the user.</summary>
        public string Message { get; }

        /// <summary>Gets the detailed validation errors (empty when successful).</summary>
        public IReadOnlyList<string> Errors { get; }

        /// <summary>Creates a successful result.</summary>
        /// <param name="message">Message to show the user.</param>
        public static OperationResult Ok(string message) => new OperationResult(true, message, null);

        /// <summary>Creates a failed result.</summary>
        /// <param name="message">Summary of the failure.</param>
        /// <param name="errors">Optional list of detailed errors.</param>
        public static OperationResult Fail(string message, IEnumerable<string>? errors = null)
            => new OperationResult(false, message, errors);

        /// <summary>Returns the message followed by every error on its own line.</summary>
        public override string ToString()
        {
            return Errors.Count == 0
                ? Message
                : Message + Environment.NewLine + string.Join(Environment.NewLine, Errors.Select(error => " - " + error));
        }
    }

    /// <summary>
    /// A result that also carries a value, e.g. the new PersonID after a successful add.
    /// Inherits all members of <see cref="OperationResult"/>.
    /// </summary>
    /// <typeparam name="T">Type of the returned value.</typeparam>
    public class OperationResult<T> : OperationResult
    {
        private OperationResult(bool success, string message, IEnumerable<string>? errors, T? value)
            : base(success, message, errors)
        {
            Value = value;
        }

        /// <summary>Gets the value produced by a successful operation (default when it failed).</summary>
        public T? Value { get; }

        /// <summary>Creates a successful result carrying a value.</summary>
        public static OperationResult<T> Ok(T value, string message) => new OperationResult<T>(true, message, null, value);

        /// <summary>Creates a failed result (no value).</summary>
        public static new OperationResult<T> Fail(string message, IEnumerable<string>? errors = null)
            => new OperationResult<T>(false, message, errors, default);
    }
}
