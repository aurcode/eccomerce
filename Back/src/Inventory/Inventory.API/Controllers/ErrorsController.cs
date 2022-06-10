using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inventory.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : Controller
    {
        private readonly ILogger _logger;


        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [Route("/error-development")]
        public IActionResult HandleErrorDevelopment(
    [FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                return NotFound();
            }

            var exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            _logger.LogError(exceptionHandlerFeature.Error, "Unhandled Exception");

            return Problem(
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message);
        }

        [Route("/error")]
        public IActionResult HandleError()
        {
            var exceptionHandlerFeature =
              HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            _logger.LogError(exceptionHandlerFeature.Error, "Unhandled Exception");

            return Problem();
        }
    }
}