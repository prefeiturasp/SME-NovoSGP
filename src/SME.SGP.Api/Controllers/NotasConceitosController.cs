using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/avaliacoes/notas")]
    [ValidaDto]
    public class NotasConceitosController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(NotasConceitosRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_A, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery]ListaNotasConceitosConsultaDto consultaListaNotasConceitosDto, [FromServices] IConsultasNotasConceitos consultasNotasConceitos)
        {
            return Ok(await consultasNotasConceitos.ListarNotasConceitos(consultaListaNotasConceitosDto));
        }

        [HttpGet("/api/v1/avaliacoes/{atividadeAvaliativaId}/notas/{nota}/arredondamento")]
        [ProducesResponseType(typeof(double), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_A, Permissao.NC_I, Policy = "Bearer")]
        public IActionResult ObterArredondamento(long atividadeAvaliativaId, double nota, [FromServices] IConsultasNotasConceitos consultasNotasConceitos)
        {
            return Ok(consultasNotasConceitos.ObterValorArredondado(atividadeAvaliativaId, nota));
        }

        [HttpGet("turmas/{turmaId}/anos-letivos/{anoLetivo}/tipos")]
        [ProducesResponseType(typeof(TipoNota), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_A, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterNotaTipo(long turmaId, int anoLetivo,[FromQuery]bool consideraHistorico, [FromServices]IConsultasNotasConceitos consultasNotasConceitos)
        {
            return Ok(await consultasNotasConceitos.ObterNotaTipo(turmaId, anoLetivo, consideraHistorico));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_A, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody]NotaConceitoListaDto notaConceitoListaDto, [FromServices]IComandosNotasConceitos comandosNotasConceitos)
        {
            await comandosNotasConceitos.Salvar(notaConceitoListaDto);

            return Ok();
        }
    }
}