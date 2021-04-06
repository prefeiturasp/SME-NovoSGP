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
        [ProducesResponseType(typeof(AEESituacaoEncaminhamentoDto), 200)]
        [ProducesResponseType(typeof(AEESituacaoEncaminhamentoDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSituacoesEncaminhamentos([FromQuery] int anoLetivo, [FromQuery] long dreId, long ueId, [FromServices] IObterEncaminhamentoAEESituacoesUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroDashboardAEEDto() { AnoLetivo = anoLetivo, 
                DreId = dreId, 
                UeId = ueId}));
        }

        [HttpGet("encaminhamentos/deferidos")]
        [ProducesResponseType(typeof(AEETurmaDto), 200)]
        [ProducesResponseType(typeof(AEETurmaDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterEncaminhamentosDeferidos([FromQuery] int anoLetivo, [FromQuery] long dreId, long ueId, [FromServices] IObterEncaminhamentosAEEDeferidosUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroDashboardAEEDto()
            {
                AnoLetivo = anoLetivo,
                DreId = dreId,
                UeId = ueId
            }));
        }

        [HttpGet("planos/situacoes")]
        [ProducesResponseType(typeof(AEESituacaoPlanoDto), 200)]
        [ProducesResponseType(typeof(AEESituacaoPlanoDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterSituacoesPlanos([FromQuery] int anoLetivo, [FromQuery] long dreId, long ueId, [FromServices] IObterPlanoAEESituacoesUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroDashboardAEEDto()
            {
                AnoLetivo = anoLetivo,
                DreId = dreId,
                UeId = ueId
            }));
        }

        [HttpGet("planos/vigentes")]
        [ProducesResponseType(typeof(AEETurmaDto), 200)]
        [ProducesResponseType(typeof(AEETurmaDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterPlanosVigentes([FromQuery] int anoLetivo, [FromQuery] long dreId, long ueId, [FromServices] IObterPlanosAEEVigentesUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroDashboardAEEDto()
            {
                AnoLetivo = anoLetivo,
                DreId = dreId,
                UeId = ueId
            }));
        }

        [HttpGet("planos/acessibilidades")]
        [ProducesResponseType(typeof(AEEAcessibilidadeRetornoDto), 200)]
        [ProducesResponseType(typeof(AEEAcessibilidadeRetornoDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterPlanosAcessibilidades([FromQuery] int anoLetivo, [FromQuery] long dreId, long ueId, [FromServices] IObterPlanosAEEAcessibilidadesUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroDashboardAEEDto()
            {
                AnoLetivo = anoLetivo,
                DreId = dreId,
                UeId = ueId
            }));
        }

        [HttpGet("encaminhamentos/matriculados-srm-paee")]
        [ProducesResponseType(typeof(AEEAcessibilidadeRetornoDto), 200)]
        [ProducesResponseType(typeof(AEEAcessibilidadeRetornoDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterAlunosMatriculadosSRMPAEE([FromQuery] int anoLetivo, [FromQuery] string dreCodigo, string ueCodigo, [FromServices] IObterAlunosMatriculadosSRMPAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroDashboardAEEDto()
            {
                AnoLetivo = anoLetivo,
                DreCodigo = dreCodigo,
                UeCodigo = ueCodigo
            }));
        }
    }
}