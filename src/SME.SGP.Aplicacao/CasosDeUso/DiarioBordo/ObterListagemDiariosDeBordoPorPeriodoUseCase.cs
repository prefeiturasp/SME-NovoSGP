using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListagemDiariosDeBordoPorPeriodoUseCase : AbstractUseCase, IObterListagemDiariosDeBordoPorPeriodoUseCase
    {
        public ObterListagemDiariosDeBordoPorPeriodoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoTituloDto>> Executar(FiltroListagemDiarioBordoDto param)
        {
            return await mediator.Send(new ObterListagemDiariosDeBordoPorPeriodoQuery(param.TurmaId, param.ComponenteCurricularId, param.DataInicio, param.DataFim));
        }
    }
}
