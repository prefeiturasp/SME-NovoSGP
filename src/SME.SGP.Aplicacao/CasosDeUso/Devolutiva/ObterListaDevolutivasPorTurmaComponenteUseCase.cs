using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaDevolutivasPorTurmaComponenteUseCase : AbstractUseCase, IObterListaDevolutivasPorTurmaComponenteUseCase
    {
        public ObterListaDevolutivasPorTurmaComponenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<DevolutivaResumoDto>> Executar(FiltroListagemDevolutivaDto param)
            => await mediator.Send(new ObterListaDevolutivasPorTurmaComponenteQuery(param.TurmaCodigo, param.ComponenteCurricularCodigo, param.DataReferencia));
    }
}
