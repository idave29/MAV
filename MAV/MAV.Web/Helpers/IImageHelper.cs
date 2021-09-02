namespace MAV.Web.Helpers
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    interface IImageHelper
    {
        Task<string> UploadImageAsync(
    IFormFile imageFile,
    string nameFile, string folder);

    }
}
