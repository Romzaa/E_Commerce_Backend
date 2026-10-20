using E_Commerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IAttachmentService
    {
        Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default);
        Result<(Stream stream, string contentType)?> GetFile(string fileName, string folderName);
        bool DeleteFile(string fileName, string folderName);

    }
}
