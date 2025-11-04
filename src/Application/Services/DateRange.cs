namespace FinanceTracker.Application.Services;

public readonly struct DateRange
{
    public DateOnly From { get; }
    public DateOnly To { get; }

    public DateRange(DateOnly from, DateOnly to)
    {
        if (to < from) throw new ArgumentException("End date cannot be earlier than start date.");
        From = from;
        To = to;
    }
}