using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DAL.Core.Settings;
using BLL.Helpers;
using BLL.Services.Interfaces;

namespace BLL.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private StorageCredentials _storageCredentials;
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;

        private readonly ILogger<BlobStorageService> _logger;
        private readonly LocalSettings _settings;

        public BlobStorageService(ILogger<BlobStorageService> logger, IOptions<LocalSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;

            _storageCredentials = new StorageCredentials(_settings.AzureBlobStorageConfig.AccountName, _settings.AzureBlobStorageConfig.Key);
            _storageAccount = new CloudStorageAccount(_storageCredentials, useHttps: true);
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }

        public async Task<List<string>> ListFiles(string contaiterName)
        {
            List<string> _imageList = new List<string>();
            BlobContinuationToken blobContinuationToken = null;

            do
            {
                var _blobContainer = _blobClient.GetContainerReference(contaiterName);
                var results = await _blobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);

                blobContinuationToken = results.ContinuationToken;
                foreach (IListBlobItem item in results.Results)
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;
                        _imageList.Add(blob.Name);
                    }
                    else if (item.GetType() == typeof(CloudPageBlob))
                    {
                        CloudPageBlob blob = (CloudPageBlob)item;
                        _imageList.Add(blob.Name);
                    }
                    else if (item.GetType() == typeof(CloudBlobDirectory))
                    {
                        CloudBlobDirectory dir = (CloudBlobDirectory)item;
                        _imageList.Add(dir.Uri.ToString());
                    }
                }
            } while (blobContinuationToken != null);
            return _imageList;
        }

        public async Task<List<T>> UploadFiles<T>(List<IFormFile> files, string contaiterName)
        {
            List<CloudBlockBlob> cloudBlockBlobs = new List<CloudBlockBlob>();

            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        stream.Close();
                        var blobName = Utilities.GenerateFileName(formFile);

                        var _blobContainer = _blobClient.GetContainerReference(contaiterName);
                        await _blobContainer.CreateIfNotExistsAsync();

                        CloudBlockBlob cloudBlockBlob = _blobContainer.GetBlockBlobReference(blobName);

                        cloudBlockBlob.Properties.ContentType = formFile.ContentType;
                        await cloudBlockBlob.UploadFromFileAsync(filePath);

                        cloudBlockBlobs.Add(cloudBlockBlob);
                    }
                }
            }

            return cloudBlockBlobs.Cast<T>().ToList();
        }

        public async Task<string> Delete(List<string> filesname, string contaiterName)
        {
            foreach (var filename in filesname)
            {
                try
                {
                    var _blobContainer = _blobClient.GetContainerReference(contaiterName);
                    if (await _blobContainer.ExistsAsync())
                    {
                        CloudBlob file = _blobContainer.GetBlobReference(filename);
                        if (await file.ExistsAsync())
                        {
                            await file.DeleteAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting blob " + filename);
                }
            }
            return "Success";
        }
    }
}
