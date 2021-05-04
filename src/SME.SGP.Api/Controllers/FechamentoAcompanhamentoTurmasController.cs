using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return  Ok(listaPaginada);
        }
    }
}
