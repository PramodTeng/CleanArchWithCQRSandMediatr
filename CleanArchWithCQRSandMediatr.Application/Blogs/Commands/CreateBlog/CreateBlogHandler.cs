using AutoMapper;

using CleanArchWithCQRSandMediatr.Application.Blogs.Queries.GetBlogs;
using CleanArchWithCQRSandMediatr.Domain.Entity;
using CleanArchWithCQRSandMediatr.Domain.Interface;

using MediatR;

namespace CleanArchWithCQRSandMediatr.Application.Blogs.Commands.CreateBlog;

public class CreateBlogHandler : IRequestHandler<CreateBlogCommand, BlogVm>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public CreateBlogHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<BlogVm> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        var blogEntity = new Blog()
        {
            Author = request.Author,
            Name = request.Name,
            Description = request.Description
        };

        var result = await _blogRepository.CreateAsync(blogEntity);

        return _mapper.Map<BlogVm>(result);

    }
}
