using CleanArchWithCQRSandMediatr.Domain.Interface;

using MediatR;

namespace CleanArchWithCQRSandMediatr.Application.Blogs.Commands.DeleteBlog;

public class DeleteCommandHandler : IRequestHandler<DeleteBlogCommand, int>
{
    private readonly IBlogRepository _blogRepository;

    public DeleteCommandHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;

    }
    public async Task<int> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        return await _blogRepository.DeleteAsync(request.Id);
    }
}
