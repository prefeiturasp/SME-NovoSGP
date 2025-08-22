using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [Route("api/v1/importar-arquivo")]
    [ApiController]
    [Authorize("Bearer")]
    public class ImportarArquivoController : ControllerBase
    {
        [HttpPost("ideb")]
        [ProducesResponseType(typeof(RetornoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 400)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ImportarArquivoIdeb(IFormFile arquivo, [FromServices] ICasoDeUsoImportacaoArquivoIdeb casoDeUsoImportacaoArquivo)
        {
            return Ok(await casoDeUsoImportacaoArquivo.Executar(arquivo));
        }

    //    [HttpPost]
    //    [ProducesResponseType(typeof(RetornoDto), 200)]
    //    [ProducesResponseType(typeof(RetornoBaseDto), 400)]
    //    [ProducesResponseType(typeof(RetornoBaseDto), 500)]
    //    public async Task<IActionResult> ImportarArquivoIDEB(IFormFile arquivo,
    //[FromServices] ICasoDeUsoImportacaoArquivo casoDeUsoImportacaoArquivo)
    //    {
    //        return Ok(await casoDeUsoImportacaoArquivo.Executar(arquivo));
    //    }
    }
}
