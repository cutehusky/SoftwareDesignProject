namespace SoftwareDesignProject.Client.Services;

public interface IFormatChecker
{
    public bool IsValidEmail(string email);
    public bool IsValidUsername(string username);
    public bool IsValidPassword(string password);
    public bool IsValidPluginName(string name);
    public bool IsValidPluginCategory(string category);
    public bool IsValidPluginDescription(string description);
}