
using AutoMapper;

using CleanArchWithCQRSandMediatr.Domain.Entity;
using CleanArchWithCQRSandMediatr.Domain.Interface;

using MediatR;

namespace CleanArchWithCQRSandMediatr.Application.Blogs.Commands.UpdateBlog;

public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand, int>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public UpdateBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }
    public async Task<int> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {

        var blogEntity = new Blog()
        {
            Id = request.Id,
            Author = request.Author,
            Name = request.Name,
            Description = request.Description

        };

        return await _blogRepository.UpdateAsync(request.Id, blogEntity);

    }
}
