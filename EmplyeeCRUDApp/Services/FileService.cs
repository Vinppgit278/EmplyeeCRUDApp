namespace EmplyeeCRUDApp.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        public FileService(IWebHostEnvironment env) { _env = env; }


        public string SaveFile(IFormFile file, string uploadsFolder)
        {
            if (file == null || file.Length == 0) return null;
            var uploads = Path.Combine(_env.WebRootPath, uploadsFolder);
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(uploads, fileName);
            using (var fs = new FileStream(fullPath, FileMode.Create)) { file.CopyTo(fs); }
            return Path.Combine(uploadsFolder, fileName).Replace("\\", "/");
        }


        public void DeleteFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            var fullPath = Path.Combine(_env.WebRootPath, path);
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
    }
}
