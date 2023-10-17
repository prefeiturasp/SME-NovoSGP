using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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
                
                if (documento.EhNulo())
                    throw new NegocioException(MensagemNegocioDocumento.DOCUMENTO_INFORMADO_NAO_EXISTE);

                var arquivos = (await mediator.Send(new ObterArquivosPorCodigosQuery(request.ArquivosCodigos), cancellationToken)).ToList();
                
                if (arquivos.EhNulo() || !arquivos.Any())
                    throw new NegocioException(MensagemNegocioDocumento.CODIGOS_ARQUIVOS_INFORMADOS_NAO_ENCONTRADOS);

                var documentosArquivos = (await mediator.Send(new ObterDocumentosArquivosPorDocumentoIdQuery(request.DocumentoId), cancellationToken)).ToList();

                var documentosArquivosExcluir = documentosArquivos.Where(w => !request.ArquivosCodigos.Contains(w.Codigo));

                if (documentosArquivosExcluir.NaoEhNulo() && documentosArquivosExcluir.Any())
                {
                    var arquivosAntigos =
                        await mediator.Send(
                            new ObterArquivosPorIdsQuery(documentosArquivosExcluir.Select(c => c.ArquivoId).ToArray()),
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

                var arquivosNovos = arquivos.Select(s => s.Id).ToArray();
                var arquivosantigos = documentosArquivos.Select(s => s.ArquivoId).ToArray();
                var arquivosNovosParaInserir = arquivosNovos.Except(arquivosantigos);

                foreach (var arquivo in arquivos.Where(w=> arquivosNovosParaInserir.Contains(w.Id)))
                    await repositorioDocumentoArquivo.SalvarAsync(new DocumentoArquivo{DocumentoId = request.DocumentoId, ArquivoId = arquivo.Id}); 

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
