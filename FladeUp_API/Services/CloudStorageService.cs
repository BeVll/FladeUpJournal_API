using FladeUp_Api.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Drawing;

namespace FladeUp_Api.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly GoogleCredential googleCredential;
        private readonly StorageClient storageClient;
        private readonly string bucketName;

        public CloudStorageService(IConfiguration configuration)
        {
            googleCredential = GoogleCredential.FromFile(configuration.GetValue<string>("GoogleCredentialFile"));
            storageClient = StorageClient.Create(googleCredential);
            bucketName = configuration.GetValue<string>("GoogleCloudStorageBucket");
        }

        //public async Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await imageFile.CopyToAsync(memoryStream);
        //        var dataObject = await storageClient.UploadObjectAsync(bucketName, fileNameForStorage, null, memoryStream);
        //        return dataObject.MediaLink;
    
        //    }
        //}

        public async Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                var bmp = new Bitmap(System.Drawing.Image.FromStream(memoryStream));
                var dir = Directory.GetCurrentDirectory() + "/" + fileNameForStorage;
                bmp.Save(dir);
            }
            return fileNameForStorage;
        }

        public async Task DeleteFileAsync(string fileNameForStorage)
        {
            await storageClient.DeleteObjectAsync(bucketName, fileNameForStorage);
        }
    }
}
