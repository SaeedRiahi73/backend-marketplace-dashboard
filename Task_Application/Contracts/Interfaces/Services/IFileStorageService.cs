using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Application.Contracts.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName, CancellationToken cancellationToken);
        bool DeleteFile(string relativePath);
    }
}
