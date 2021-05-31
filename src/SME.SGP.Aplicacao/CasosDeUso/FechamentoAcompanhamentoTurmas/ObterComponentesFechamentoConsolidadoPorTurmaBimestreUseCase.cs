using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase : AbstractUseCase, IObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase
    {
        public ObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<ConsolidacaoTurmaComponenteCurricularDto>> Executar(FiltroComponentesFechamentoConsolidadoDto filtro)
        {
            var componentes = await mediator.Send(new ObterComponentesFechamentoConsolidadoPorTurmaBimestreQuery(filtro.TurmaId, filtro.Bimestre));
            return componentes;
        }
    }
}
