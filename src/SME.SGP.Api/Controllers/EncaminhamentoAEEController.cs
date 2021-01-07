using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlunoDto = SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar.AlunoDto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/encaminhamentoaee")]
    public class EncaminhamentoAEEController : ControllerBase
    {

        [HttpGet]
        [Route("situacoes")]
        [ProducesResponseType(typeof(IEnumerable<AlunoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public IActionResult ObterSituacoes()
        {
            var situacoes = Enum.GetValues(typeof(SituacaoAEE))
                        .Cast<SituacaoAEE>()
                        .Select(d => new { codigo = (int)d, descricao = d.Name() })
                        .ToList();
            return Ok(situacoes);
        }
    }
}