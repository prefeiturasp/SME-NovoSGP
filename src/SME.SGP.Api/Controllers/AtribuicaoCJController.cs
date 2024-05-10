﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicoes/cjs")]
    [Authorize("Bearer")]
    [ValidaDto]
    public class AtribuicaoCJController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AtribuicaoCJListaRetornoDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACJ_C, Policy = "Bearer")]
        public async Task<IActionResult> Get([FromQuery]AtribuicaoCJListaFiltroDto atribuicaoCJListaFiltroDto, [FromServices] IListarAtribuicoesCJPorFiltroUseCase useCase)
        {
            return Ok(await useCase.Executar(atribuicaoCJListaFiltroDto));
        }

        [HttpGet("anos-letivos")]
        [ProducesResponseType(typeof(int[]), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACJ_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAnosLetivosAtribuicao([FromServices] IObterAnosLetivosAtribuicaoCJUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpGet("ues/{ueId}/modalidades/{modalidadeId}/turmas/{turmaId}/professores/{professorRf}")]
        [ProducesResponseType(typeof(AtribuicaoCJTitularesRetornoDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACJ_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterAtribuicaoDeProfessores(string ueId, string turmaId,
            string professorRf, Modalidade modalidadeId,[FromQuery] int anoLetivo, [FromServices] IObterProfessoresTitularesECjsUseCase useCase)
        {
            return Ok(await useCase.Executar(ueId, turmaId, professorRf, modalidadeId, anoLetivo));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ACJ_C, Policy = "Bearer")]
        public async Task<IActionResult> Post([FromBody]AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto, [FromServices] ISalvarAtribuicaoCJUseCase useCase)
        {
            await useCase.Executar(atribuicaoCJPersistenciaDto);
            return Ok();
        }
    }
}