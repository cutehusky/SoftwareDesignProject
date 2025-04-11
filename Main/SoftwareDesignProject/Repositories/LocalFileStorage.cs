namespace SoftwareDesignProject.Repositories;

public class LocalFileStorage: IFileStorage
{
    private readonly string _basePath;
    public LocalFileStorage(IHostEnvironment hostEnvironment)
    {
        _basePath = Path.Combine(hostEnvironment.ContentRootPath, "Root");
    }
    
    public async Task<bool> SaveFileAsync(IFormFile file, string fileName, string path)
    {
        Console.WriteLine(file.Name);
        Console.WriteLine(file.Length);

        var filePath = Path.Combine(_basePath, path, fileName);
        try
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> RemoveFile(string fileName, string path)
    {
        var filePath = Path.Combine(_basePath, path, fileName);
        if (!File.Exists(filePath))
            return true;
        try
        {
            File.Delete(filePath);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> BackupFile(string fileName, string path)
    {
        var filePath = Path.Combine(_basePath, path, fileName);
        var backupDir = Path.Combine(_basePath, path, "backup");
        var backupPath = Path.Combine(_basePath, path, "backup", fileName);
        if (!Directory.Exists(backupDir))
            Directory.CreateDirectory(backupDir);
        if (!File.Exists(filePath))
            return false;
        try
        {
            File.Copy(filePath, backupPath, true);
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> RestoreFile(string fileName, string path)
    {
        var filePath = Path.Combine(_basePath, path, fileName);
        var backupPath = Path.Combine(_basePath, path, "backup", fileName);
        if (!File.Exists(backupPath))
            return false;
        try
        {
            File.Copy(backupPath, filePath, true);
            File.Delete(backupPath);
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> RemoveBackup(string fileName, string path)
    {
        var backupPath = Path.Combine(_basePath, path, "backup", fileName);
        if (!File.Exists(backupPath))
            return false;
        try
        {
            File.Delete(backupPath);
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public async Task<FileStream?> GetFile(string fileName, string path)
    {
        var filePath = Path.Combine(_basePath, path, fileName);
        Console.WriteLine("Getting file: " + filePath);
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found: " + filePath);
            return null;
        }
        // add caching here to optimize performance
        return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
    }
}