namespace RAProductsRegestration.Domain.Common.ValueObjects;

public sealed record AttachmentReference
{
    private AttachmentReference()
    {
    }

    public AttachmentReference(
        Guid attachmentId,
        string fileName,
        string? notes = null)
    {
        if (attachmentId == Guid.Empty)
        {
            throw new ArgumentException(
                "Attachment ID cannot be empty.",
                nameof(attachmentId));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException(
                "File name is required.",
                nameof(fileName));
        }

        AttachmentId = attachmentId;
        FileName = fileName.Trim();
        Notes = Normalize(notes);
    }

    public Guid AttachmentId { get; private init; }

    public string FileName { get; private init; } = null!;

    public string? Notes { get; private init; }

    public AttachmentReference WithNotes(
        string? notes)
    {
        return new AttachmentReference(
            AttachmentId,
            FileName,
            notes);
    }

    private static string? Normalize(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}