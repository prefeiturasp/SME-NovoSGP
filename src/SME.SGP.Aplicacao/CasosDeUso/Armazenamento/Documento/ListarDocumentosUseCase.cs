using MediatR;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarDocumentosUseCase : AbstractUseCase, IListarDocumentosUseCase
    {
        public ListarDocumentosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<DocumentoResumidoDto>> Executar(FiltroListagemDocumentosDto filtro)
        {
            return await mediator.Send(new ObterDocumentosPorDreUeETipoEClassificacaoQuery(filtro.DreId, filtro.UeId, filtro.TipoDocumentoId, filtro.ClassificacaoId, filtro.AnoLetivo));
        }
    }
}
