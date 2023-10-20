using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra;
using System.Threading.Tasks;
using System.Collections.Generic;
using SME.SGP.Aplicacao;
using Microsoft.AspNetCore.Http;
using System;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/informes")]
    [Authorize("Bearer")]
    public class InformesController : Controller
    {
        [HttpPost("salvar")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.INF_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromBody] InformesDto informesDto, [FromServices] ISalvarInformesUseCase useCase)
        {
            return Ok(await useCase.Executar(informesDto));
        }

        [HttpGet]
        [Route("grupos-usuarios/perfil/{tipoPerfil}")]
        [ProducesResponseType(typeof(IEnumerable<GruposDeUsuariosDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.INF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterGruposDeUsuarios(
            [FromServices] IObterGruposDeUsuariosUseCase useCase,
            int tipoPerfil)
        {
            return Ok(await useCase.Executar(tipoPerfil));
        }
    }
}
