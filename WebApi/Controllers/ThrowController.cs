using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // tira essa controller da documentação do swagger
    public class ThrowController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError() =>
            Problem(); // método para tratar erros, retorna um problema genérico sem detalhes

        [Route("/error-development")]
        public IActionResult HandleErrorDevelopment( // método para tratar erros em desenvolvimento
            [FromServices] IHostEnvironment hostEnvironment) // injetando o ambiente da aplicação
        {
            if(!hostEnvironment.IsDevelopment()) // se não estiver em desenvolvimento
            {
                return NotFound(404); // retorna 404
            }

            var exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>()!; // obtém a feature de tratamento de exceções

            // retorna um problema com os detalhes do erro
            return Problem( 
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message);


        }
        

    }
}
