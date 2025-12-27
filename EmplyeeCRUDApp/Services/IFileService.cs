namespace EmplyeeCRUDApp.Services
{
    public interface IFileService
    {
        string SaveFile(IFormFile file, string uploadsFolder);
        void DeleteFile(string path);
    }
}
