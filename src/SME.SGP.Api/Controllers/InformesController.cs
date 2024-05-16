using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Informes;
using System;
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

        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.INF_I, Policy = "Bearer")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromServices] IUploadDeArquivoUseCase useCase)
        {
            if (file.Length > 0)
                return Ok(await useCase.Executar(file, Dominio.TipoArquivo.Informativo));

            return BadRequest();
        }

        [HttpDelete("arquivo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.INF_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirArquivo([FromQuery] Guid arquivoCodigo, [FromServices] IExcluirArquivoInformeUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivoCodigo));
        }
    }
}
