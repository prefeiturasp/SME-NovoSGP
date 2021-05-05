using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.FechamentoAcompanhamentoTurmas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api
{
    [ApiController]
    [Route("api/v1/fechamentos/acompanhamentos")]
    //[Authorize("Bearer")]
    public class FechamentoAcompanhamentoTurmasController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaTurmas([FromQuery] FiltroAcompanhamentoFechamentoTurmasDto filtro, [FromServices] IObterTurmasFechamentoAcompanhamentoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("turmas/{turmaId}/fechamentos/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(IEnumerable<StatusTotalFechamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaTotalStatusFechamentos(long turmaId, int bimestre)
        {

            var listaStatus = new List<StatusTotalFechamentoDto>() {

                new StatusTotalFechamentoDto()
                {
                    Descricao = StatusFechamento.NaoIniciado.Description(),
                    Quantidade = 10
                },

                new StatusTotalFechamentoDto()
                {
                    Descricao = StatusFechamento.EmAndamento.Description(),
                    Quantidade = 25
                },

                 new StatusTotalFechamentoDto()
                {
                    Descricao = StatusFechamento.Concluido.Description(),
                    Quantidade = 3
                }
            };

            return Ok(listaStatus);
        }
        [HttpGet("turmas/{turmaId}/conselho-classe/bimestres/{bimestre}")]
        [ProducesResponseType(typeof(IEnumerable<StatusTotalFechamentoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.RI_C, Policy = "Bearer")]
        public async Task<IActionResult> ListaTotalStatusConselhosClasse(long turmaId, int bimestre)
        {

            var listaStatus = new List<StatusTotalFechamentoDto>() {

                new StatusTotalFechamentoDto()
                {
                    Descricao = StatusFechamento.NaoIniciado.Description(),
                    Quantidade = 5
                },

                new StatusTotalFechamentoDto()
                {
                    Descricao = StatusFechamento.EmAndamento.Description(),
                    Quantidade = 15
                },

                 new StatusTotalFechamentoDto()
                {
                    Descricao = StatusFechamento.Concluido.Description(),
                    Quantidade = 30
                }
            };

            return Ok(listaStatus);
        }
    }
}
