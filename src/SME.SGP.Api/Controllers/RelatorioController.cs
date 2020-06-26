using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios")]
    public class RelatorioController : ControllerBase
    {
        [HttpGet("{codigoCorrelacao}")]
        public async Task<IActionResult> Download(Guid codigoCorrelacao, [FromServices] IReceberDadosDownloadRelatorioUseCase downloadRelatorioUseCase, [FromServices] ISevicoJasper servicoJasper)
        {
            var (relatorio, contentType, nomeArquivo) = await downloadRelatorioUseCase.Executar(codigoCorrelacao);

            return File(relatorio, contentType, nomeArquivo);
        }
        [HttpGet("conselhos-classe/atas-finais")]
        public async Task<IActionResult> ConselhoClasseAtaFinal([FromQuery] FiltroRelatorioConselhoClasseAtaFinalDto filtroRelatorioConselhoClasseAtaFinalDto, [FromServices] IRelatorioConselhoClasseAtaFinalUseCase relatorioConselhoClasseAtaFinalUseCase)
        {
            return Ok(await relatorioConselhoClasseAtaFinalUseCase.Executar(filtroRelatorioConselhoClasseAtaFinalDto));
        }

    }
}