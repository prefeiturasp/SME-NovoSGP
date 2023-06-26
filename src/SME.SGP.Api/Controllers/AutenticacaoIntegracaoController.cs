using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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
        [Route("sga/frequencia")]
        [ChaveIntegracaoSgpApi]
        public async Task<IActionResult> ObterGuidAutenticacaoFrequencia(SolicitacaoGuidAutenticacaoFrequenciaSGADto filtroSolicitacaoGuidAutenticacao, [FromServices] IObterGuidAutenticacaoFrequenciaSGA obterGuidAutenticacaoFrequenciaSGA)
        {
            var guidAutenticacaoFrequencia = await obterGuidAutenticacaoFrequenciaSGA.Executar(filtroSolicitacaoGuidAutenticacao);
            return Ok(guidAutenticacaoFrequencia);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(UsuarioAutenticacaoFrequenciaSGARetornoDto), 200)]
        [AllowAnonymous]
        [Route("sga/frequencia/{guid}")]
        public async Task<IActionResult> ObterAutenticacaoFrequencia(Guid guid, [FromServices] IObterAutenticacaoFrequenciaSGA obterAutenticacaoFrequenciaSGA)
        {
            if (guid == Guid.Empty)
                throw new NegocioException("Informe um Guid de autenticação frequência.");

            var retornoAutenticacao = await obterAutenticacaoFrequenciaSGA.Executar(guid);
            if (!retornoAutenticacao.UsuarioAutenticacao.Autenticado)
                return StatusCode(401);

            return Ok(retornoAutenticacao);
        }
    }
    
}
