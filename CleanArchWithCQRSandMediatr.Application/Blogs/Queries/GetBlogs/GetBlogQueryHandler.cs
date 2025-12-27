using AutoMapper;

using CleanArchWithCQRSandMediatr.Domain.Interface;

using MediatR;

namespace CleanArchWithCQRSandMediatr.Application.Blogs.Queries.GetBlogs;

public class GetBlogQueryHandler(IBlogRepository blogRepository, IMapper mapper)
    : IRequestHandler<GetBlogQuery, List<BlogVm>>
{
    public async Task<List<BlogVm>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        var blogs = await blogRepository.GetAllBlogsAsync();
        // var blogList = blogs.Select(x => new BlogVm
        // {
        //     Author = x.Author,
        //     Name = x.Name,
        //     Description = x.Description,
        //     Id = x.Id
        // }).ToList();
        return mapper.Map<List<BlogVm>>(blogs);

    }
}
