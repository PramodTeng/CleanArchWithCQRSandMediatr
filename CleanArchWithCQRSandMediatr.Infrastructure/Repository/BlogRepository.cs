using CleanArchWithCQRSandMediatr.Domain.Entity;
using CleanArchWithCQRSandMediatr.Domain.Interface;
using CleanArchWithCQRSandMediatr.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace CleanArchWithCQRSandMediatr.Infrastructure.Repository;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDBContext _blogDBContext;

    public BlogRepository(BlogDBContext blogDBContext)
    {
        _blogDBContext = blogDBContext;
    }

    public async Task<Blog> CreateAsync(Blog blog)
    {
        await _blogDBContext.Blogs.AddAsync(blog);
        await _blogDBContext.SaveChangesAsync();

        return blog;
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _blogDBContext.Blogs
            .Where(blog => blog.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<List<Blog>> GetAllBlogsAsync()
    {
        return await _blogDBContext.Blogs
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Blog?> GetByIdAsync(int id)
    {
        return await _blogDBContext.Blogs.AsNoTracking()
            .FirstOrDefaultAsync(blog => blog.Id == id);
    }

    public async Task<int> UpdateAsync(int id, Blog blog)
    {
        return await _blogDBContext.Blogs
            .Where(model => model.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(m => m.Id, blog.Id)
                .SetProperty(m => m.Name, blog.Name)
                .SetProperty(m => m.Author, blog.Author)
                .SetProperty(m => m.Description, blog.Description)
            );
    }
}