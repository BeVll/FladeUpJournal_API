namespace FladeUp_Api.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage);
        Task DeleteFileAsync(string fileNameForStorage);
    }
}
