using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CleanArchWithCQRSandMediatr.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _sender;

        protected ISender Mediator => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    }
}
