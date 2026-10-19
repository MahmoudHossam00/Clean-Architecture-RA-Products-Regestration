using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using RAProductsRegestration.Application.Common.Interfaces.Files;
using RAProductsRegestration.Application.Common.Models;
using RAProductsRegestration.Application.Features.Attachments.Download;
using RAProductsRegestration.Application.Features.Attachments.Upload;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.API.Controllers;

[ApiController][Authorize]
[Route("api/attachments")]
public sealed class AttachmentsController(
    ISender sender)
    : ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<Result<AttachmentUploadResponse>> Upload(
        IFormFile file,
        CancellationToken ct)
    {
        await using var stream =
            file.OpenReadStream();

        var upload =
            new FileUpload(
                stream,
                file.FileName,
                file.ContentType,
                file.Length
                );

        return await sender.Send(
            new UploadAttachmentCommand(upload),
            ct);
    }


    // =====================================================
    // Download
    // =====================================================

    [HttpGet("{attachmentId:guid}")]
    public async Task<IActionResult> Download(
        Guid attachmentId,
        [FromQuery] string fileName,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new DownloadAttachmentQuery(
                    attachmentId,
                    fileName),
                ct);

        if (result.IsError)
        {
            var error =
                result.TopError!;

            return error.Type switch
            {
                ErrorKind.NotFound =>
                    NotFound(new
                    {
                        error.Code,
                        error.Description
                    }),

                _ =>
                    BadRequest(new
                    {
                        error.Code,
                        error.Description
                    })
            };
        }

        var response =
            result.Value;

        var contentTypeProvider =
            new FileExtensionContentTypeProvider();

        if (!contentTypeProvider.TryGetContentType(
                response.FileName,
                out var contentType))
        {
            contentType =
                "application/octet-stream";
        }

        return File(
            response.Content,
            contentType,
            response.FileName);
    }
}