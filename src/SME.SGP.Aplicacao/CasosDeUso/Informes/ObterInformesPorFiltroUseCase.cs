using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Informes;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInformesPorFiltroUseCase : AbstractUseCase, IObterInformesPorFiltroUseCase
    {
        public ObterInformesPorFiltroUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<PaginacaoResultadoDto<InformeResumoDto>> Executar(InformeFiltroDto filtro)
        {
            return mediator.Send(new ObterInformesPorFiltroQuery(filtro));
        }
    }
}
