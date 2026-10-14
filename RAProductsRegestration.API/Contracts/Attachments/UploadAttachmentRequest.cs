namespace RAProductsRegestration.API.Contracts.Attachments
{
    public sealed class UploadAttachmentRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}
