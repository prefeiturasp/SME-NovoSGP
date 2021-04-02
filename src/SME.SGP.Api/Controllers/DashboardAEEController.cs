using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidade;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.EscolaAqui;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dashboard/aee")]
    //[Authorize("Bearer")]
    public class DashboardAEEController : ControllerBase
    {
        [HttpGet("encaminhamentos/situacoes")]
        [ProducesResponseType(typeof(EncaminhamentoAEESituacaoDto), 200)]
        [ProducesResponseType(typeof(EncaminhamentoAEESituacaoDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTotaisAdesao([FromQuery] int ano, [FromQuery] long dreId, long ueId, [FromServices] IObterEncaminhamentoAEESituacoesUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroDashboardAEEDto() { Ano = ano, 
                DreId = dreId, 
                UeId = ueId}));
        }
    }
}