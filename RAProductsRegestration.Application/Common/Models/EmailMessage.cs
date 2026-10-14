namespace RAProductsRegestration.Application.Common.Models
{
    public sealed record EmailMessage(
    IReadOnlyCollection<string> To,
    string Subject,
    string HtmlBody,
    IReadOnlyCollection<string>? Cc = null);
}
