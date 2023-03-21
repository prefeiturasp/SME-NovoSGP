using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarDocumentosUseCase : AbstractUseCase, IListarDocumentosUseCase
    {
        public ListarDocumentosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<DocumentoResumidoDto>> Executar(int? anoLetivo, long ueId = 0, long tipoDocumentoId = 0, long classificacaoId = 0)
        {
            return await mediator.Send(new ObterDocumentosPorUeETipoEClassificacaoQuery(ueId, tipoDocumentoId, classificacaoId, anoLetivo));
        }
    }
}
