namespace FinanceTracker.Application.Validation;

public sealed class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Validation failed")
    {
        Errors = errors.ToArray();
    }
}