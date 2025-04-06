namespace SoftwareDesignProject.Repositories;

public interface IRepository<T>
{
    public Task<List<T>> GetAll();
    public Task<T?> GetById(Guid id);

    public Task<bool> Add(T pluginDto);

    public Task<bool> Remove(Guid id);

    public Task<bool> Update(T dto);
}