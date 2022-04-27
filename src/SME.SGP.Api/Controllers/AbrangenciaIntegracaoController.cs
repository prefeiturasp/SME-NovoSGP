using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/abrangencias/integracoes")]
    [ChaveIntegracaoSgpApi]
    public class AbrangenciaIntegracaoController : ControllerBase
    {
        [HttpGet("{usuarioRF}/perfis/{usuarioPerfil}/acesso-sondagem")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> PodeAcessarSondagem(string usuarioRF, Guid usuarioPerfil, [FromServices] IUsuarioPossuiAbrangenciaAcessoSondagemUseCase useCase)
        {
            return Ok(await useCase.Executar(usuarioRF, usuarioPerfil));
        }
    }
}