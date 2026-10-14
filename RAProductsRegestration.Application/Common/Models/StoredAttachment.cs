namespace RAProductsRegestration.Application.Common.Models
{
    public sealed record StoredAttachment(
Guid AttachmentId,
string FileName,
string ContentType,
long Length);
}
