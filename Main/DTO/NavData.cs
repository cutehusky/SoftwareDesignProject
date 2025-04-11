namespace CommonDTO;

public class NavData
{
    public IEnumerable<NavItem> NavItems { get; init; } = new List<NavItem>();
    public HashSet<Guid> FavoriteItems { get; init; } = new();
}