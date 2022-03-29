namespace MAV.Web.Helpers
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IImageHelper
    {
        Task<string> UploadImageAsync(
    IFormFile imageFile,
    string nameFile, string folder);

    }
}
