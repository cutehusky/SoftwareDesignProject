namespace CommonDTO;

public class ActionResponse<T>
{
    public required T Result { get; init; }
}