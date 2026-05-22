using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
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

        [HttpGet("{consideraHistorico}/login/{login}/perfis/{perfil}/abrangencia-completa")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAbrangenciaCompleta(bool consideraHistorico, string login, Guid perfil, [FromServices] IObterAbrangenciaCompletaIntegracaoUseCase useCase, [Required][FromQuery] Modalidade modalidade, [Required][FromQuery] int anoLetivo, [FromQuery] int semestre = 0, [FromQuery] string codigoDre = null, [FromQuery] string codigoUe = null, [FromQuery] bool includeTurmas = false)
        {
            return Ok(await useCase.Executar(login, perfil, consideraHistorico, anoLetivo, semestre, modalidade, codigoDre, codigoUe, includeTurmas));
        }
    }
}
