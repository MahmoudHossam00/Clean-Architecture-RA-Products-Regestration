namespace RAProductsRegestration.Domain.Common.ValueObjects;

public sealed record FulfillmentDeadline
{
    private FulfillmentDeadline()
    {
    }

    public FulfillmentDeadline(
        string description,
        DateOnly deadline,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Description is required.",
                nameof(description));
        }

        Description = description.Trim();
        Deadline = deadline;
        Notes = Normalize(notes);
        FulfillmentDate = null;
        IsFulfilled = false;
    }

    private FulfillmentDeadline(
        string description,
        DateOnly deadline,
        string? notes,
        DateOnly? fulfillmentDate,
        bool isFulfilled)
    {
        Description = description;
        Deadline = deadline;
        Notes = notes;
        FulfillmentDate = fulfillmentDate;
        IsFulfilled = isFulfilled;
    }

    public string Description { get; private init; } = null!;

    public DateOnly Deadline { get; private init; }

    public string? Notes { get; private init; }

    public DateOnly? FulfillmentDate { get; private init; }

    public bool IsFulfilled { get; private init; }

    public FulfillmentDeadline UpdateDetails(
        string description,
        DateOnly deadline,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Description is required.",
                nameof(description));
        }

        return new FulfillmentDeadline(
            description.Trim(),
            deadline,
            Normalize(notes),
            FulfillmentDate,
            IsFulfilled);
    }

    public FulfillmentDeadline Fulfill(
        DateOnly fulfillmentDate)
    {
        return new FulfillmentDeadline(
            Description,
            Deadline,
            Notes,
            fulfillmentDate,
            true);
    }

    public FulfillmentDeadline ResetFulfillment()
    {
        return new FulfillmentDeadline(
            Description,
            Deadline,
            Notes,
            null,
            false);
    }

    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}