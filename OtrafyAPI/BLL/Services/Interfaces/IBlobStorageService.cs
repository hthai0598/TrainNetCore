using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> Delete(List<string> files, string contaiterName);
        Task<List<string>> ListFiles(string contaiterName);
        Task<List<T>> UploadFiles<T>(List<IFormFile> files, string contaiterName);
    }
}
