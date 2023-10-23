using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Informes;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpDelete("{informeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.INF_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long informeId, [FromServices] IExcluirInformesUseCase useCase)
        {
            return Ok(await useCase.Executar(informeId));
        }

        [HttpGet("{informeId}")]
        [ProducesResponseType(typeof(InformesRespostaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.INF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterInforme(long informeId, [FromServices] IObterInformeUseCase useCase)
        {
            return Ok(await useCase.Executar(informeId));
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<InformeResumoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.INF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterInformes(
                                            [FromQuery] InformeFiltroDto filtro,
                                            [FromServices] IObterInformesPorFiltroUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet]
        [Route("grupos-usuarios/perfil/{tipoPerfil}")]
        [ProducesResponseType(typeof(IEnumerable<GruposDeUsuariosDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.INF_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterGruposDeUsuarios(int tipoPerfil, [FromServices] IObterGruposDeUsuariosUseCase useCase)
        {
            return Ok(await useCase.Executar(tipoPerfil));
        }
    }
}
