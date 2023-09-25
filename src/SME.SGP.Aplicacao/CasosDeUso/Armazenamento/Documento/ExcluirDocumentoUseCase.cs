using System;
using System.IO;
using System.Linq;
using MediatR;
using SME.SGP.Dominio;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDocumentoUseCase : AbstractUseCase, IExcluirDocumentoUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        
        public ExcluirDocumentoUseCase(IMediator mediator,IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(long documentoId)
        {
            unitOfWork.IniciarTransacao();
            try
            {
                var entidadeDocumento = await mediator.Send(new ObterDocumentoPorIdQuery(documentoId));

                if (entidadeDocumento.EhNulo())
                    throw new NegocioException(MensagemNegocioDocumento.DOCUMENTO_INFORMADO_NAO_EXISTE);

                var documentosArquivos = await mediator.Send(new ObterDocumentosArquivosPorDocumentoIdQuery(documentoId));

                if (documentosArquivos.NaoEhNulo())
                {
                    var arquivosAntigos =
                        await mediator.Send(
                            new ObterArquivosPorIdsQuery(documentosArquivos.Select(c => c.ArquivoId).ToArray()));

                    foreach (var arquivoAntigo in arquivosAntigos)
                    {
                        await mediator.Send(new ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand(documentoId, arquivoAntigo.Id));
                        await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(arquivoAntigo.Id));
                            
                        var extencao = Path.GetExtension(arquivoAntigo.Nome);
                            
                        var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = $"{arquivoAntigo.Codigo}{extencao}"};
                        await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid()));
                    }
                }

                await mediator.Send(new ExcluirDocumentoPorIdCommand(documentoId));

                unitOfWork.PersistirTransacao();
                
                return true;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
