using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/autenticacao/integracoes")]
    public class AutenticacaoIntegracaoController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous]
        [Route("frequencia")]
        [ChaveIntegracaoSgpApi]
        public async Task<IActionResult> ObterGuidAutenticacaoFrequencia(SolicitacaoGuidAutenticacaoFrequenciaDto filtroSolicitacaoGuidAutenticacao, [FromServices] IObterGuidAutenticacaoFrequencia obterGuidAutenticacaoFrequenciaSGA)
        {
            var guidAutenticacaoFrequencia = await obterGuidAutenticacaoFrequenciaSGA.Executar(filtroSolicitacaoGuidAutenticacao);
            return Ok(guidAutenticacaoFrequencia);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(UsuarioAutenticacaoFrequenciaRetornoDto), 200)]
        [AllowAnonymous]
        [Route("frequencia/{guid}")]
        public async Task<IActionResult> ObterAutenticacaoFrequencia(Guid guid, [FromServices] IObterAutenticacaoFrequencia obterAutenticacaoFrequenciaSGA)
        {
            if (guid == Guid.Empty)
                throw new NegocioException("Informe um Guid de autenticação frequência.");

            var retornoAutenticacao = await obterAutenticacaoFrequenciaSGA.Executar(guid);
            if (!retornoAutenticacao.UsuarioAutenticacao.Autenticado)
                return StatusCode((int)HttpStatusCode.Unauthorized);

            return Ok(retornoAutenticacao);
        }
    }
    
}
