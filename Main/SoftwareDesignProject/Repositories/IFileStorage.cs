namespace SoftwareDesignProject.Repositories;

public interface IFileStorage
{
    public Task<bool> SaveFileAsync(IFormFile file, string fileName, string path);
    public Task<bool> RemoveFile(string fileName, string path);
    public Task<bool> BackupFile(string fileName, string path);
    public Task<bool> RestoreFile(string fileName, string path);
    public Task<bool> RemoveBackup(string fileName, string path);
    public Task<FileStream?> GetFile(string fileName, string path);
    public List<string> GetFiles(string path, string searchPattern = "*");
    public Task<byte[]> GetFileBytes(string fileName, string path);
}