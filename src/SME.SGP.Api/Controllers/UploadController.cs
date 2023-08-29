using System;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SME.SGP.Dominio;
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
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;
        public UploadController(IMediator mediator, IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.mediator = mediator;
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
                //Foi adicionado fixo o valor https pois será discutido com a infra o problema de SSL
                //Depois que corrigir, colocar: {Request.Protocol.Split('/')[0].ToLower()}
                var file = files.FirstOrDefault();
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
            return Ok(await mediator.Send(ObterBucketsServicoArmazenamentoQuery.Instance));
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
        public async Task<IActionResult> CopiarServicoArmazenamento(string nomeArquivo,[FromServices] ICopiarServicoArmazenamentoUseCase useCase)
        {
            return Ok(await useCase.Executar(nomeArquivo));
        }
        
        [HttpPost("/servico-armazenamento/excluir")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ExcluirServicoArmazenamento(string nomeArquivo,[FromServices] IExcluirTemporarioServicoArmazenamentoUseCase useCase)
        {
            return Ok(await useCase.Executar(nomeArquivo, configuracaoArmazenamentoOptions.BucketTemp));
        }
        
        [HttpPost("/servico-armazenamento/mover")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> MoverServicoArmazenamento(string nomeArquivo,[FromServices] IMoverServicoArmazenamentoUseCase useCase)
        {
            return Ok(await useCase.Executar(nomeArquivo));
        }
        
        [HttpPost("/servico-armazenamento/obter-url")]
        [ProducesResponseType(typeof(RetornoBaseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> ObterUrlServicoArmazenamento(string nomeArquivo, bool ehPastaTemporaria,[FromServices] IObterServicoArmazenamentoUseCase useCase)
        {
            return Ok(await useCase.Executar(nomeArquivo,ehPastaTemporaria));
        }
    }
}
