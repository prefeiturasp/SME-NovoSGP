using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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
            return Ok(await consultasNotasConceitos.ListarNotasConceitos(consultaListaNotasConceitosDto.TurmaCodigo, consultaListaNotasConceitosDto.Bimestre,
                consultaListaNotasConceitosDto.AnoLetivo, consultaListaNotasConceitosDto.DisciplinaCodigo, consultaListaNotasConceitosDto.Modalidade));
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

        [HttpGet("turma/{turmaId}")]
        [ProducesResponseType(typeof(TipoNota), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_A, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> ObterNotaTipo([FromQuery] DateTime dataAvaliacao, long turmaId, [FromServices]IConsultasNotasConceitos  consultasNotasConceitos)
        {
            return Ok(consultasNotasConceitos.ObterNotaTipo(dataAvaliacao, turmaId));
        }
    }
}