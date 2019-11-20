using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicao/esporadica")]
    [ValidaDto]
    public class AtribuicaoEsporadicaController : ControllerBase
    {
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_E, Permissao.AE_I, Policy = "Bearer")]
        public async Task<IActionResult> Excluir([FromServices]IComandosAtribuicaoEsporadica comandosAtribuicaoEsporadica, long id)
        {
            await comandosAtribuicaoEsporadica.Excluir(id);
            return Ok();
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<AtribuicaoEsporadicaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Listar()
        {
            var MOCKLISTAGEM = new PaginacaoResultadoDto<AtribuicaoEsporadicaDto>
            {
                TotalPaginas = 1,
                TotalRegistros = 10,
                Items = new List<AtribuicaoEsporadicaDto>
                {
                    new AtribuicaoEsporadicaDto
                    {
                        AnoLetivo = 2019,
                        DataFim =  DateTime.Now.AddDays(7),
                        DataInicio = DateTime.Now,
                        ProfessorNome = "Caíque Latorre 1",
                        DreId = "1",
                        ProfessorRf = "7777710",
                        UeId = "1",
                        Id = 1
                    },
                    new AtribuicaoEsporadicaDto
                    {
                        AnoLetivo = 2019,
                        DataFim =  DateTime.Now.AddDays(8),
                        DataInicio = DateTime.Now.AddDays(1),
                        DreId = "1",
                        ProfessorNome = "Caíque Latorre 2",
                        ProfessorRf = "7777710",
                        UeId = "1",
                        Id = 2
                    },
                    new AtribuicaoEsporadicaDto
                    {
                        AnoLetivo = 2019,
                        DataFim =  DateTime.Now.AddDays(9),
                        DataInicio = DateTime.Now.AddDays(2),
                        DreId = "1",
                        ProfessorNome = "Caíque Latorre 3",
                        ProfessorRf = "7777710",
                        UeId = "1",
                        Id = 3
                    },
                    new AtribuicaoEsporadicaDto
                    {
                        AnoLetivo = 2019,
                        DataFim =  DateTime.Now.AddDays(10),
                        DataInicio = DateTime.Now.AddDays(3),
                        DreId = "1",
                        ProfessorNome = "Caíque Latorre 4",
                        ProfessorRf = "7777710",
                        UeId = "1",
                        Id = 4
                    },
                    new AtribuicaoEsporadicaDto
                    {
                        AnoLetivo = 2019,
                        DataFim =  DateTime.Now.AddDays(11),
                        DataInicio = DateTime.Now.AddDays(4),
                        DreId = "1",
                        ProfessorNome = "Caíque Latorre 5",
                        ProfessorRf = "7777710",
                        UeId = "1",
                        Id = 5
                    },
                    new AtribuicaoEsporadicaDto
                    {
                        AnoLetivo = 2019,
                        DataFim =  DateTime.Now.AddDays(12),
                        DataInicio = DateTime.Now.AddDays(5),
                        DreId = "1",
                        ProfessorNome = "Caíque Latorre 6",
                        ProfessorRf = "7777710",
                        UeId = "1",
                        Id = 6
                    },
                    new AtribuicaoEsporadicaDto
                    {
                        AnoLetivo = 2019,
                        DataFim =  DateTime.Now.AddDays(13),
                        DataInicio = DateTime.Now.AddDays(6),
                        DreId = "1",
                        ProfessorNome = "Caíque Latorre 7",
                        ProfessorRf = "7777710",
                        UeId = "1",
                        Id = 7
                    },
                    new AtribuicaoEsporadicaDto
                    {
                        AnoLetivo = 2019,
                        DataFim =  DateTime.Now.AddDays(14),
                        DataInicio = DateTime.Now.AddDays(7),
                        DreId = "1",
                        ProfessorNome = "Caíque Latorre 8",
                        ProfessorRf = "7777710",
                        UeId = "1",
                        Id = 8
                    },
                    new AtribuicaoEsporadicaDto
                    {
                        AnoLetivo = 2019,
                        DataFim =  DateTime.Now.AddDays(15),
                        DataInicio = DateTime.Now.AddDays(8),
                        DreId = "1",
                        ProfessorNome = "Caíque Latorre 9",
                        ProfessorRf = "7777710",
                        UeId = "1",
                        Id = 9
                    },
                    new AtribuicaoEsporadicaDto
                    {
                        AnoLetivo = 2019,
                        DataFim =  DateTime.Now.AddDays(16),
                        DataInicio = DateTime.Now.AddDays(9),
                        DreId = "1",
                        ProfessorNome = "Caíque Latorre 10",
                        ProfessorRf = "7777710",
                        UeId = "1",
                        Id = 10
                    }
                }
            };

            return Ok(MOCKLISTAGEM);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AE_A, Permissao.AE_I, Policy = "Bearer")]
        public IActionResult Post([FromBody]AtribuicaoEsporadicaCompletaDto atribuicaoEsporadicaCompletaDto, [FromServices]IComandosAtribuicaoEsporadica comandosAtribuicaoEsporadica)
        {
            comandosAtribuicaoEsporadica.Salvar(atribuicaoEsporadicaCompletaDto);

            return Ok();
        }
    }
}