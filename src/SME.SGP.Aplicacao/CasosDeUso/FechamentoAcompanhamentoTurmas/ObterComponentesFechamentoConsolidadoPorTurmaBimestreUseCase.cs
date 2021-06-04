using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
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
            int[] situacoesFechamento = new int[] { filtro.SituacaoFechamento };

            if (filtro.SituacaoFechamento == (int)SituacaoFechamento.NaoIniciado)            
                situacoesFechamento = new int[] { (int)SituacaoFechamento.NaoIniciado, (int)SituacaoFechamento.EmProcessamento };

            var componentes = await mediator.Send(new ObterComponentesFechamentoConsolidadoPorTurmaBimestreQuery(filtro.TurmaId, filtro.Bimestre, situacoesFechamento));
            return componentes;
        }
    }
}
