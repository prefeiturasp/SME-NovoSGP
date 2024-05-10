using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/calendarios/eventos/integracoes")]
    [ApiController]
    [ChaveIntegracaoSgpApi]
    public class EventosIntegracaoController : ControllerBase
    {
        [HttpGet("liberacao-boletim/turmas/{turmaCodigo}/bimestres")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterBimestresLiberacaoBoletim(string turmaCodigo, [FromServices] IObterBimestresLiberacaoBoletimUseCase obterBimestresLiberacaoBoletimUseCase)
        {
            return Ok(await obterBimestresLiberacaoBoletimUseCase.Executar(turmaCodigo));
        }

        [HttpGet("modalidadesCalendario/{modalidade}/mesAno/{mesAno}")]
        [ChaveIntegracaoSgpApi]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> ObterEventosEscolaAquiPorDreUeTurmaMes(int modalidade, DateTime mesAno, [FromQuery] FiltroEventosEscolaAquiDto filtro, [FromServices] IObterEventosEscolaAquiPorDreUeTurmaMesUseCase useCase)
        {
            filtro.ModalidadeCalendario = modalidade;
            filtro.MesAno = mesAno;
            return Ok(await useCase.Executar(filtro));
        }
    }
}
