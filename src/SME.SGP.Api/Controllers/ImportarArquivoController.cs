using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.Boletim;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.FluenciaLeitora;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.Ideb;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.Idep;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/importar-arquivo")]
    [ApiController]
    [Authorize("Bearer")]
    public class ImportarArquivoController : ControllerBase
    {
        [HttpPost("ideb")]
        [ProducesResponseType(typeof(ImportacaoLogRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.IE_I_P_I, Policy = "Bearer")]
        public async Task<IActionResult> ImportarArquivoIdeb([FromForm] IFormFile arquivo, [FromForm] int anoLetivo, [FromServices] IImportacaoArquivoIdebUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivo, anoLetivo));
        }

        [HttpPost("idep")]
        [ProducesResponseType(typeof(ImportacaoLogRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.IE_I_P_I, Policy = "Bearer")]
        public async Task<IActionResult> ImportarArquivoIdep([FromForm] IFormFile arquivo, [FromForm] int anoLetivo, [FromServices] IImportacaoArquivoIdepUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivo, anoLetivo));
        }

        [HttpPost("fluencia-leitora")]
        [ProducesResponseType(typeof(ImportacaoLogRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.IE_I_P_I, Policy = "Bearer")]
        public async Task<IActionResult> ImportarArquivoFluenciaLeitora([FromForm] IFormFile arquivo, [FromForm] int anoLetivo, [FromForm] int tipoAvaliacao, [FromServices] IImportacaoArquivoFluenciaLeitoraUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivo, anoLetivo, tipoAvaliacao));
        }

        [HttpPost("alfabetizacao")]
        [ProducesResponseType(typeof(ImportacaoLogRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.IE_I_P_I, Policy = "Bearer")]
        public async Task<IActionResult> ImportarArquivoAlfabetizacao([FromForm] IFormFile arquivo, [FromForm] int anoLetivo, [FromServices] IImportacaoArquivoAlfabetizacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(arquivo, anoLetivo));
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.IE_I_P_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterImportacaoLog([FromQuery] FiltroPesquisaImportacaoDto filtro, [FromServices] IImportacaoLogUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpGet("falhas")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<ImportacaoLogErroQueryRetornoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.IE_I_P_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterImportacaoLogErros([FromQuery] FiltroPesquisaImportacaoDto filtro, [FromServices] IImportacaoLogErroUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpPost("boletim-idep")]
        [ProducesResponseType(typeof(ImportacaoLogRetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.IE_I_P_I, Policy = "Bearer")]
        public async Task<IActionResult> ImportarBoletins([FromQuery] int ano, [FromForm] IEnumerable<IFormFile> boletins, [FromServices] IBoletimIdepUseCase useCase)
        {
            return Ok(await useCase.Executar(ano, boletins));
        }

        //[HttpPost("boletim-ideb")]
        //[ProducesResponseType(typeof(ImportacaoLogRetornoDto), 200)]
        //[ProducesResponseType(typeof(RetornoBaseDto), 400)]
        //[ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.IE_I_P_I, Policy = "Bearer")]
        //public async Task<IActionResult> ImportarBoletins([FromQuery] int ano, [FromForm] IEnumerable<IFormFile> boletins, [FromServices] IBoletimIdepUseCase useCase)
        //{
        //    return Ok(await useCase.Executar(ano, boletins));
        //}
    }
}
