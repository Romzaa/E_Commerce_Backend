using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace E_Commerce.Infrastucture.Attachment
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long _maxFileSize = 10 * 1024 * 1024; // 10 MB
        private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        public AttachmentService(IWebHostEnvironment env, ILogger<IAttachmentService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead || fileStream.Length == 0)
                return Result<string>.Fail(new Error("File.Failure", "File stream is empty or unreadable.", ErrorType.Failure));

            if (fileStream.Length > _maxFileSize)
                return Result<string>.Fail(new Error("File.Failure", $"File size exceeds the maximum limit of {_maxFileSize / (1024 * 1024)} MB.", ErrorType.Validation));

            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName))
                return Result<string>.Fail(new Error("File.Failure", "File name or folder name is empty or whitespace.", ErrorType.Validation));

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return Result<string>.Fail(new Error("File.Failure",
                    $"File extension '{extension}' is not allowed. Allowed extensions are: {string.Join(", ", _allowedExtensions)}.", ErrorType.Validation));

            var folderPath = Path.Combine(_env.ContentRootPath, folderName);
            Directory.CreateDirectory(folderPath);

            var storeFileAs = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(folderPath, storeFileAs);

            try
            {
                await using var filestremToWrite = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                await fileStream.CopyToAsync(filestremToWrite, ct);
                return Result<string>.Ok(storeFileAs);
            }
            catch
            {
                return Result<string>.Fail(new Error("File.Failure", "An error occurred while saving the file.", ErrorType.Failure));

            }
        }


           public Result<(Stream stream, string contentType)?> GetFile(string fileName, string folderName)
            {
                if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName))
                    return Result<(Stream stream, string contentType)?>.Fail(new Error("File.Failure", "File name or folder name is empty or whitespace.", ErrorType.Validation));
            var extension = Path.GetExtension(fileName);
            var file = Path.GetFileName(fileName);
            var realFile = $"{file}.{extension}";

            var filePath = Path.Combine(_env.ContentRootPath, folderName, realFile);
                if (!File.Exists(filePath))
                    return Result<(Stream stream, string contentType)?>.Fail(new Error("File.NotFound", "File Path Is Invalid", ErrorType.NotFound));

                var contentType = Path.GetExtension(realFile).ToLowerInvariant() switch
                {

                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    _ => "application/octet-stream"
                };
                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                return Result<(Stream stream, string contentType)?>.Ok((stream, contentType));

            }

        public bool DeleteFile(string fileName, string folderName)
            {
                if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName))
                    return false;

            var extension = Path.GetExtension(fileName);
            var file = Path.GetFileName(fileName);
            var realFile = $"{file}.{extension}";
                try
                {
                    var filePath = Path.Combine(_env.ContentRootPath, folderName, realFile);

                    if (!File.Exists(filePath))
                        return false;

                    File.Delete(filePath);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed To Delete Attachment {fileName}");
                    return false;
                }

            }

       
    }
}