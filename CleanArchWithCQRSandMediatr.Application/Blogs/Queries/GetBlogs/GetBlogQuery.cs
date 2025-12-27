using System;

using MediatR;

namespace CleanArchWithCQRSandMediatr.Application.Blogs.Queries.GetBlogs;

public record GetBlogQuery : IRequest<List<BlogVm>>;
