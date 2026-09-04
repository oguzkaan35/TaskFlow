namespace TaskFlow.Web.Models;

public class UserListViewModel
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
