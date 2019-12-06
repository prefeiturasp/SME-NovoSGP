using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces;
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
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.NC_A, Permissao.NC_I, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery]ListaNotasConceitosConsultaDto consultaListaNotasConceitosDto)
        {
            return Ok();
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