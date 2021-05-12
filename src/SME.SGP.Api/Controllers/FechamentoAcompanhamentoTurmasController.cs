using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
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
        public async Task<IActionResult> ListaTurmas([FromQuery] FiltroAcompanhamentoFechamentoTurmasDto filtro)
        {
            var listaPaginada = new PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>()
            {
                TotalPaginas = 10,
                TotalRegistros = 10,
                Items = new List<TurmaAcompanhamentoFechamentoRetornoDto>()
                {
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 123456,
                        Nome = "Turma 1A",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 345689,
                        Nome = "Turma 1B",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 453567,
                        Nome = "Turma 1C",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 3457435,
                        Nome = "Turma 2A",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 765432,
                        Nome = "Turma 2A",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 123456,
                        Nome = "Turma 2B",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 098789,
                        Nome = "Turma 2C",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 434564,
                        Nome = "Turma 3A",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 941267,
                        Nome = "Turma 4A",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                       TurmaId = 342367,
                        Nome = "Turma 4B",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 675456,
                        Nome = "Turma 5A",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 543234,
                        Nome = "Turma 5B",
                    },
                     new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 675456,
                        Nome = "Turma 6A",
                    },
                    new TurmaAcompanhamentoFechamentoRetornoDto()
                    {
                        TurmaId = 543234,
                        Nome = "Turma 6B",
                    }
                }
            };
            return Ok(listaPaginada);
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


        //TODO: REMOVER ANTES DA STORY IR PARA DEV!
        [HttpPost]
        public async Task<IActionResult> TestarFila([FromServices] IExecutarConsolidacaoTurmaConselhoClasseUseCase executarConsolidacaoTurmaConselhoClasseUseCase)
        {

            var obj = new ConsolidacaoTurmaConselhoClasseDto() { Bimestre = 1, TurmaId = 639036 };
            var mensagem = JsonConvert.SerializeObject(obj); 
            var msgRabbit = new MensagemRabbit(mensagem);
            
            await executarConsolidacaoTurmaConselhoClasseUseCase.Executar(msgRabbit);

            return Ok();
        }
    }
}
