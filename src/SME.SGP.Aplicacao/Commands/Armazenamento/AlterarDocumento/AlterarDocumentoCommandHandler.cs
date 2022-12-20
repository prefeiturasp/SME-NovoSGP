using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class AlterarDocumentoCommandHandler : AbstractUseCase, IRequestHandler<AlterarDocumentoCommand, AuditoriaDto>
    {
        private readonly IRepositorioDocumento repositorioDocumento;
        private readonly IRepositorioDocumentoArquivo repositorioDocumentoArquivo;
        private readonly IUnitOfWork unitOfWork;

        public AlterarDocumentoCommandHandler(IMediator mediator, 
            IRepositorioDocumento repositorioDocumento, 
            IUnitOfWork unitOfWork, 
            IRepositorioDocumentoArquivo repositorioDocumentoArquivo) : base(mediator)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioDocumentoArquivo = repositorioDocumentoArquivo ?? throw new ArgumentNullException(nameof(repositorioDocumentoArquivo));
        }

        public async Task<AuditoriaDto> Handle(AlterarDocumentoCommand request, CancellationToken cancellationToken)
        {
            unitOfWork.IniciarTransacao();
            try
            {
                var documento = await mediator.Send(new ObterDocumentoPorIdQuery(request.DocumentoId), cancellationToken);
                
                if (documento == null)
                    throw new NegocioException(MensagemNegocioDocumento.DOCUMENTO_INFORMADO_NAO_EXISTE);

                var arquivos = (await mediator.Send(new ObterArquivosPorCodigosQuery(request.ArquivosCodigos), cancellationToken)).ToList();
                
                if (arquivos == null || !arquivos.Any())
                    throw new NegocioException(MensagemNegocioDocumento.CODIGOS_ARQUIVOS_INFORMADOS_NAO_ENCONTRADOS);

                var documentosArquivos = await mediator.Send(new ObterDocumentosArquivosPorDocumentoIdQuery(request.DocumentoId), cancellationToken);

                if (documentosArquivos != null)
                {
                    var arquivosAntigos =
                        await mediator.Send(
                            new ObterArquivosPorIdsQuery(documentosArquivos.Select(c => c.ArquivoId).ToArray()),
                            cancellationToken);

                    foreach (var arquivoAntigo in arquivosAntigos)
                    {
                        await mediator.Send(new ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand(request.DocumentoId, arquivoAntigo.Id), cancellationToken);
                        await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(arquivoAntigo.Id), cancellationToken);
                        
                        var extencao = Path.GetExtension(arquivoAntigo.Nome);
                        
                        var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = $"{arquivoAntigo.Codigo}{extencao}"};
                        await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid()), cancellationToken);
                    }
                }
                
                await repositorioDocumento.SalvarAsync(documento);

                foreach (var documentoArquivo in arquivos.Select(arquivo => new DocumentoArquivo
                         {
                             ArquivoId = arquivo.Id,
                             DocumentoId = request.DocumentoId
                         }))
                {
                    await repositorioDocumentoArquivo.SalvarAsync(documentoArquivo);
                }
                
                unitOfWork.PersistirTransacao();
                
                return (AuditoriaDto)documento;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
