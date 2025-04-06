namespace CommonDTO;

public class PaginationList<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
}