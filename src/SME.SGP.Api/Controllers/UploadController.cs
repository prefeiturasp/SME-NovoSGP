using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Api.Controllers
{
    /// <summary>
    /// Controller para upload de arquivos do Jodit
    /// </summary>
    [ApiController]
    [Route("api/v1/arquivos/upload")]
    [Authorize("Bearer")]
    public class UploadController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;
        public UploadController(IMediator mediator, IServicoArmazenamento servicoArmazenamento,IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.mediator = mediator;
            this.servicoArmazenamento = servicoArmazenamento;
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Editor([FromServices] IUploadArquivoEditorUseCase useCase)
        {
            var files = Request.Form.Files;
            if (files != null)
            {
                var file = files.FirstOrDefault();
                string urlBase = Request.Host.Value;
                if (file.Length > 0)
                    return Ok(await useCase.Executar(files.FirstOrDefault(), 
                        $"https://{Request.Host}{Request.PathBase}/{configuracaoArmazenamentoOptions.BucketTemp}", 
                        Dominio.TipoArquivo.Editor));
            }
                
            return BadRequest();
        }
        
        [HttpPost("/servico-armazenamento/obter-buckets")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterBucketsServicoArmazenamento()
        {
            return Ok(await mediator.Send(new ObterBucketsServicoArmazenamentoQuery()));
        }
        
        [HttpPost("/servico-armazenamento/armazenar-temporario")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ArmazenarTemporarioServicoArmazenamento(IFormFile iFromFile)
        {
            if (iFromFile != null) 
                return Ok(await mediator.Send(new ArmazenarArquivoFisicoCommand(iFromFile,Guid.NewGuid().ToString(),TipoArquivo.temp)));
                
            return BadRequest();
        }
        
        [HttpPost("/servico-armazenamento/armazenar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ArmazenarServicoArmazenamento(IFormFile iFromFile)
        {
            if (iFromFile != null) 
                return Ok(await mediator.Send(new ArmazenarArquivoFisicoCommand(iFromFile,iFromFile.FileName,TipoArquivo.Geral)));
                
            return BadRequest();
        }
        
        [HttpPost("/servico-armazenamento/copiar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> CopiarServicoArmazenamento(string nomeArquivo)
        {
            return Ok(await servicoArmazenamento.Copiar(nomeArquivo));
        }
        
        [HttpPost("/servico-armazenamento/excluir")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ExcluirServicoArmazenamento(string nomeArquivo)
        {
            return Ok(await servicoArmazenamento.Excluir(nomeArquivo, configuracaoArmazenamentoOptions.BucketTemp));
        }
        
        [HttpPost("/servico-armazenamento/mover")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> MoverServicoArmazenamento(string nomeArquivo)
        {
            return Ok(await servicoArmazenamento.Mover(nomeArquivo));
        }
        
        [HttpPost("/servico-armazenamento/obter-url")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterUrlServicoArmazenamento(string nomeArquivo, bool ehPastaTemporaria)
        {
            return Ok(await servicoArmazenamento.Obter(nomeArquivo,ehPastaTemporaria));
        }
    }
}
