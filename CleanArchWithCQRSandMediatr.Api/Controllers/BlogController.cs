using CleanArchWithCQRSandMediatr.Application.Blogs.Commands.CreateBlog;
using CleanArchWithCQRSandMediatr.Application.Blogs.Commands.DeleteBlog;
using CleanArchWithCQRSandMediatr.Application.Blogs.Commands.UpdateBlog;
using CleanArchWithCQRSandMediatr.Application.Blogs.Queries.GetBlogById;
using CleanArchWithCQRSandMediatr.Application.Blogs.Queries.GetBlogs;

using Microsoft.AspNetCore.Mvc;

namespace CleanArchWithCQRSandMediatr.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new GetBlogQuery());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Mediator.Send(new GetBlogByIdQuery() { BlogId = id });
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogCommand command)
        {
            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateBlogCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Mediator.Send(new DeleteBlogCommand() { Id = id });
            if (result == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
