using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ea/dashboard")]
    //[Authorize("Bearer")]
    public class DashboardEAController : ControllerBase
    {
        [HttpGet("adesao")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTotaisAdesao([FromQuery] string codigoDre, [FromQuery] string codigoUe, [FromServices] IObterTotaisAdesaoUseCase obterTotaisAdesaoUseCase)
        {
            return Ok(await obterTotaisAdesaoUseCase.Executar(codigoDre, codigoUe));
        }

        [HttpGet("adesao/agrupados")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterTotaisAdesaoAgrupadosPorDre([FromServices] IObterTotaisAdesaoAgrupadosPorDreUseCase obterTotaisAdesaoAgrupadosPorDreUseCase)
        {
            return Ok(await obterTotaisAdesaoAgrupadosPorDreUseCase.Executar());
        }

        [HttpGet("ultimoProcessamento")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterUltimaAtualizacaoPorProcesso([FromQuery] string nomeProcesso, [FromServices] IObterUltimaAtualizacaoPorProcessoUseCase obterUltimaAtualizacaoPorProcessoUseCase)
        {
            return Ok(await obterUltimaAtualizacaoPorProcessoUseCase.Executar(nomeProcesso));
        }

        [HttpGet("comunicados/leitura")]
        [ProducesResponseType(typeof(IEnumerable<DadosDeLeituraDoComunicadoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterDadosDeLeituraDeComunicados([FromQuery] ObterDadosDeLeituraDeComunicadosDto obterDadosDeLeituraDeComunicadosDto, [FromServices] IObterDadosDeLeituraDeComunicadosUseCase obterDadosDeLeituraDeComunicadosUseCase)
        {
            return Ok(await obterDadosDeLeituraDeComunicadosUseCase.Executar(obterDadosDeLeituraDeComunicadosDto));
        }
        
        [HttpGet("comunicados/totais")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterComunicadosTotaisSme([FromQuery] int anoLetivo, [FromQuery] string codigoDre, [FromQuery] string codigoUe, [FromServices] IObterComunicadosTotaisUseCase obterComunicadosTotaisSmeUseCase)
        {
            return Ok(await obterComunicadosTotaisSmeUseCase.Executar(anoLetivo, codigoDre, codigoUe));
        }

        [HttpGet("comunicados/totais/agrupados")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterComunicadosTotaisAgrupadosPorDre([FromQuery] int anoLetivo, [FromServices] IObterComunicadosTotaisAgrupadosPorDreUseCase obterComunicadosTotaisAgrupadosPorDreUseCase)
        {
            return Ok(await obterComunicadosTotaisAgrupadosPorDreUseCase.Executar(anoLetivo));
        }
    }
}