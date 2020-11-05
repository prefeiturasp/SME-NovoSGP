﻿using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/planos/aulas")]
    [ValidaDto]
    public class PlanoAulaController : ControllerBase
    {
        [HttpPost("migrar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_I, Permissao.PA_A, Policy = "Bearer")]
        public async Task<IActionResult> Migrar(MigrarPlanoAulaDto migrarPlanoAulaDto, [FromServices]IMigrarPlanoAulaUseCase migrarPlanoAula)
        {
            await migrarPlanoAula.Executar(migrarPlanoAulaDto);
            return Ok();
        }

        [HttpGet("{aulaId}")]
        [ProducesResponseType(typeof(PlanoAulaRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPlanoAula(long aulaId, [FromQuery]long turmaId, [FromServices] IObterPlanoAulaUseCase useCase)
        {
            return Ok(await useCase.Executar(new FiltroObterPlanoAulaDto(aulaId, turmaId)));
           
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PDA_I, Permissao.PDA_A, Policy = "Bearer")]
        public async Task<IActionResult> Post(PlanoAulaDto planoAulaDto, [FromServices]ISalvarPlanoAulaUseCase useCase)
        {
            return Ok(await useCase.Executar(planoAulaDto));
        }

        [HttpPost("validar-existente")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.PA_C, Policy = "Bearer")]
        public IActionResult ValidarPlanoAnualExistente(FiltroPlanoAulaExistenteDto filtroPlanoAulaExistenteDto, [FromServices]IConsultasPlanoAula consultasPlanoAula)
        {
            return Ok(consultasPlanoAula.ValidarPlanoAulaExistente(filtroPlanoAulaExistenteDto));
        }
    }
}