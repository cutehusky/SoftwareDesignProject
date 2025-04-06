using CommonDTO;
using MudBlazor;

namespace SoftwareDesignProject.Repositories;

public interface IRepository<T>
{
    public Task<PaginationList<T>> GetAll(int page, int pageSize,   
        string sortBy, SortDirection order, string search);
    public Task<T?> GetById(Guid id);

    public Task<bool> Add(T pluginDto);

    public Task<bool> Remove(Guid id);

    public Task<bool> Update(T dto);
}