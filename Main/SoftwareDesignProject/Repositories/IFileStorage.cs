namespace SoftwareDesignProject.Repositories;

public interface IFileStorage
{
    Task<bool> SaveFileAsync(IFormFile file, string fileName, string path);
    Task<bool> RemoveFile(string fileName, string path);
    Task<bool> BackupFile(string fileName, string path);
    Task<bool> RestoreFile(string fileName, string path);
    Task<bool> RemoveBackup(string fileName, string path);
    Task<FileStream?> GetFile(string fileName, string path);
}