namespace SoftwareDesignProject.Repositories;

public class LocalFileStorage: IFileStorage
{
    private readonly string _basePath;
    private readonly ILogger<LocalFileStorage> _logger;
    public LocalFileStorage(IHostEnvironment hostEnvironment,
        ILogger<LocalFileStorage> logger)
    {
        _basePath = Path.Combine(hostEnvironment.ContentRootPath, "Root");
        _logger = logger;
        _logger.LogInformation("Base path of File Storage: " + _basePath);
    }
    
    public async Task<bool> SaveFileAsync(IFormFile file, string fileName, string path)
    {
        var filePath = Path.Combine(_basePath, path, fileName);
        
        _logger.LogTrace("Saving file: " + fileName + " to path: " + filePath);
        
        try
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }
        catch (Exception e)
        {
            _logger.LogError("Error saving file: " + e.Message);
            return false;
        }
        return true;
    }

    public async Task<bool> RemoveFile(string fileName, string path)
    {
        var filePath = Path.Combine(_basePath, path, fileName);
        
        _logger.LogTrace("Removing file: " + fileName + " from path: " + filePath);

        if (!File.Exists(filePath))
        {
            _logger.LogWarning("Failed to remove (target file not exist): " + filePath);
            return true;
        }

        try
        {
            File.Delete(filePath);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error removing file: " + e.Message);
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

        _logger.LogTrace("Backing up file: " + fileName + " to path: " + backupPath);
        
        if (!File.Exists(filePath))
        {
            _logger.LogWarning("Failed to backup (target file not exist): " + filePath);
            return false;
        }

        try
        {
            File.Copy(filePath, backupPath, true);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error backing up file: " + e.Message);
            return false;
        }
    }

    public async Task<bool> RestoreFile(string fileName, string path)
    {
        var filePath = Path.Combine(_basePath, path, fileName);
        var backupPath = Path.Combine(_basePath, path, "backup", fileName);
        
        _logger.LogTrace("Restoring file: " + fileName + " from path: " + backupPath);
        
        if (!File.Exists(backupPath))
        {
            _logger.LogWarning("Failed to restore (backup file not exist): " + backupPath);
            return false;
        }

        try
        {
            File.Copy(backupPath, filePath, true);
            File.Delete(backupPath);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error restoring file: " + e.Message);
            return false;
        }
    }

    public async Task<bool> RemoveBackup(string fileName, string path)
    {
        var backupPath = Path.Combine(_basePath, path, "backup", fileName);
        
        _logger.LogTrace("Removing backup file: " + fileName + " from path: " + backupPath);
        
        if (!File.Exists(backupPath))
        {
            _logger.LogWarning("Failed to remove backup (backup file not exist): " + backupPath);
            return false;
        }

        try
        {
            File.Delete(backupPath);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error removing backup file: " + e.Message);
            return false;
        }
    }

    public async Task<FileStream?> GetFile(string fileName, string path)
    {
        var filePath = Path.Combine(_basePath, path, fileName);
        
        _logger.LogTrace("Getting file: " + fileName + " from path: " + filePath);
        
        if (!File.Exists(filePath))
        {
            _logger.LogWarning("Failed to get file (file not exist): " + filePath);
            return null;
        }
        
        // add caching here to optimize performance
        return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
    }

    public List<string> GetFiles(string path,  string searchPattern = "*")
    {
        var directoryPath = Path.Combine(_basePath, path);
        _logger.LogTrace("Getting files from path: " + directoryPath);
        if (!Directory.Exists(directoryPath))
        {
            _logger.LogWarning("Failed to get files (directory not exist): " + directoryPath);
            return new List<string>();
        }
        try
        {
            return Directory.GetFiles(directoryPath, searchPattern).Select(filePath => Path.GetFileName(filePath)!).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting files: " + e.Message);
            return new List<string>();
        }
    }

    public async Task<byte[]> GetFileBytes(string fileName, string path)
    {
        var filePath = Path.Combine(_basePath, path, fileName);
        _logger.LogTrace("Getting file bytes: " + fileName + " from path: " + filePath);
        if (!File.Exists(filePath))
        {
            _logger.LogWarning("Failed to get file bytes (file not exist): " + filePath);
            return [];
        }
        try
        {
            return await File.ReadAllBytesAsync(filePath);
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting file bytes: " + e.Message);
            return [];
        }
    }
}