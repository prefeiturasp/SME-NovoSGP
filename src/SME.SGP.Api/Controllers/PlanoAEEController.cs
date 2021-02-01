using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlunoDto = SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar.AlunoDto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/plano-aee")]
    public class PlanoAEEController : ControllerBase
    {

        [HttpGet]
        [Route("situacoes")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public IActionResult ObterSituacoes()
        {
            var situacoes = Enum.GetValues(typeof(SituacaoPlanoAEE))
                        .Cast<SituacaoPlanoAEE>()
                        .Select(d => new { codigo = (int)d, descricao = d.Name() })
                        .ToList();
            return Ok(situacoes);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult ObterEncaminhamentos([FromQuery] FiltroPesquisaEncaminhamentosAEEDto filtro)
        {
            var items = new List<PlanoAEEResumoDto>()
            {
                new PlanoAEEResumoDto() {
                    Id = 1,
                    Numero = 1,
                    Nome = "Aluno 1",
                    Turma = "EF - 1A",
                    Situacao = "Em Andamento"
                }
            };
            var paginacao = new PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>()
            {
                Items = (IEnumerable<EncaminhamentoAEEResumoDto>)items,
            };
            return Ok(paginacao);
        }
    }
}