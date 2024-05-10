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
            var componentePai = await mediator.Send(new ObterCodigoComponentePaiQuery(param.ComponenteCurricularId));

            return await mediator.Send(new ObterListagemDiariosDeBordoPorPeriodoQuery(param.TurmaId, componentePai, param.ComponenteCurricularId, param.DataInicio, param.DataFim));
        }
    }
}
