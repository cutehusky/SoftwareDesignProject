namespace CommonDTO;

public class NavData
{
    public IEnumerable<NavItem> NavItems { get; init; } = new List<NavItem>();
    public IEnumerable<Guid> FavoriteItems { get; init; } = new List<Guid>();
}