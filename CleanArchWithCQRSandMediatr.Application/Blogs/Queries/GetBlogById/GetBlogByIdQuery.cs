using CleanArchWithCQRSandMediatr.Application.Blogs.Queries.GetBlogs;

using MediatR;

namespace CleanArchWithCQRSandMediatr.Application.Blogs.Queries.GetBlogById;

public class GetBlogByIdQuery() : IRequest<BlogVm>
{
    public int BlogId { get; set; }

}
