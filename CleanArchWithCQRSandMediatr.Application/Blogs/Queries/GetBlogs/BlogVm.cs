using CleanArchWithCQRSandMediatr.Application.Common.Mappings;
using CleanArchWithCQRSandMediatr.Domain.Entity;

namespace CleanArchWithCQRSandMediatr.Application.Blogs.Queries.GetBlogs;

public class BlogVm : IMapFrom<Blog>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }

}
