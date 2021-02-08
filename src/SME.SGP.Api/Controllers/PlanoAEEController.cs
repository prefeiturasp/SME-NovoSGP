using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
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
        [ProducesResponseType(typeof(PaginacaoResultadoDto<PlanoAEEResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterPlanosAEE([FromQuery] FiltroPlanosAEEDto filtro, [FromServices] IObterPlanosAEEUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpPost("salvar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Salvar([FromBody] PlanoAeeDto planoAeeDto, [FromServices] ISalvarPlanoAEEUseCase usecase)
        {
            return Ok(await usecase.Executar(planoAeeDto));
        }
    }
}