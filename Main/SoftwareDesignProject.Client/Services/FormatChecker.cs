using System.Text.RegularExpressions;

namespace SoftwareDesignProject.Client.Services;

public class FormatChecker: IFormatChecker
{
    public bool IsValidEmail(string email)
    {
        const string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex);
    }

    public bool IsValidUsername(string username)
    {
        // Assuming a valid username is between 3 and 20 characters
        const string usernameRegex = @"^[a-zA-Z0-9]{3,20}$";
        return Regex.IsMatch(username, usernameRegex)
            && username != "admin" && username != "Admin" && username != "ADMIN";
    }

    public bool IsValidPassword(string password)
    {
        // Assuming a valid password is at least 8 characters long,
        // contains at least one uppercase letter, 
        // one lowercase letter, and one digit, 
        // and one special character (? < > ! @ # $ % ^ & * ( ) _ + - = { } [ ] ; : ' " , . / < >)
        const string passwordRegex = """^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}[\]:;'",.<>?])[A-Za-z\d!@#$%^&*()_+{}[\]:;'",.<>?]{8,}$""";
        return Regex.IsMatch(password, passwordRegex);
    }

    public bool IsValidPluginName(string name)
    {
        // Assuming a valid name is between 3 and 50 characters
        const string nameRegex = @"^[a-zA-Z0-9\s]{3,50}$";
        return Regex.IsMatch(name, nameRegex);
    }

    public bool IsValidPluginCategory(string category)
    {
        // Assuming a valid category is between 3 and 50 characters
        const string categoryRegex = @"^[a-zA-Z0-9\s]{3,50}$";
        return Regex.IsMatch(category, categoryRegex)
               && category != "My Favorites";
    }

    public bool IsValidPluginDescription(string description)
    {
        // Assuming a valid description is between 3 and 500 characters
        const string descriptionRegex = @"^[a-zA-Z0-9\s.,!?]{3,500}$";
        return Regex.IsMatch(description, descriptionRegex);
    }
}