using CleanArchWithCQRSandMediatr.Domain.Entity;
namespace CleanArchWithCQRSandMediatr.Domain.Interface;

public interface IBlogRepository
{

    Task<List<Blog>> GetAllBlogsAsync();
    Task<Blog?> GetByIdAsync(int id);
    Task<Blog> CreateAsync(Blog blog);
    Task<int> UpdateAsync(int id, Blog blog);
    Task<int> DeleteAsync(int id);

}
