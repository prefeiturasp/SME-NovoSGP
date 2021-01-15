using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarDocumentoCommandHandler : AbstractUseCase, IRequestHandler<AlterarDocumentoCommand, AuditoriaDto>
    {
        private readonly IRepositorioDocumento repositorioDocumento;
        private readonly IUnitOfWork unitOfWork;

        public AlterarDocumentoCommandHandler(IMediator mediator, IRepositorioDocumento repositorioDocumento, IUnitOfWork unitOfWork) : base(mediator)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaDto> Handle(AlterarDocumentoCommand request, CancellationToken cancellationToken)
        {
            unitOfWork.IniciarTransacao();

            try
            {
                long arquivoAntigoId = 0;
                var documento = await mediator.Send(new ObterDocumentoPorIdQuery(request.DocumentoId));
                if (documento == null)
                    throw new NegocioException("Documento informado não existe");

                var arquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(request.CodigoArquivo));
                if (arquivo == null)
                    throw new NegocioException($"O código de arquivo informado não foi encontrado!");

                if (documento.ArquivoId != null)
                    arquivoAntigoId = documento.ArquivoId.GetValueOrDefault();


                documento.ArquivoId = arquivo.Id;

                if (arquivoAntigoId != 0)
                {
                    var arquivoAntigo = await mediator.Send(new ObterArquivoPorIdQuery(arquivoAntigoId));
                    if (arquivoAntigo != null)
                    {
                        await mediator.Send(new ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand(documento.Id, arquivoAntigo.Id));
                        await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(arquivoAntigo.Id));
                        await mediator.Send(new ExcluirArquivoFisicoCommand(arquivoAntigo.Codigo, arquivoAntigo.Tipo, arquivoAntigo.Nome));
                    }
                }

                await repositorioDocumento.SalvarAsync(documento);

                unitOfWork.PersistirTransacao();
                return (AuditoriaDto)documento;
            }
            catch(Exception ex )
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
