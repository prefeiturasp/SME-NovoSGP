using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;

namespace SME.SGP.Api.Controllers
{
    /// <summary>
    /// Este controller é temporário, os métodos deverão ser migrados para o
    /// controller de autenticação que está sendo criado em outra história
    /// </summary>
    [ApiController]
    [Route("api/v1/autenticacao")]
    [ValidaDto]
    public class AuthController : ControllerBase
    {
        [HttpPost("recuperar-senha")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult RecuperarSenha(string codigo, [FromServices]IComandoAuth comandoAuth)
        {
            return Ok(comandoAuth.RecuperarSenha(codigo));
        }
    }
}