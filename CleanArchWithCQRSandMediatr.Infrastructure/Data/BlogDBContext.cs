using CleanArchWithCQRSandMediatr.Domain.Entity;

using Microsoft.EntityFrameworkCore;

namespace CleanArchWithCQRSandMediatr.Infrastructure.Data;

public class BlogDBContext : DbContext
{
    public BlogDBContext(DbContextOptions<BlogDBContext> dbContextOptions) : base(dbContextOptions)
    {


    }
    public DbSet<Blog> Blogs => Set<Blog>();

}
