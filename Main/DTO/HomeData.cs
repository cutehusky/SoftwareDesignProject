namespace CommonDTO;

public class HomeData
{
    public IEnumerable<HomeItem> PluginItems { get; init; } = new List<HomeItem>();
    public HashSet<Guid> FavoriteItems { get; init; } = new();
}