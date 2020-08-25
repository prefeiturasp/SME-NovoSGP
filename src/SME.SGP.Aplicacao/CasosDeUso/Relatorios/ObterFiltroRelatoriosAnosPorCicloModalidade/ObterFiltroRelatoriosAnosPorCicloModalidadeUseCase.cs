using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosAnosPorCicloModalidadeUseCase : IObterFiltroRelatoriosAnosPorCicloModalidadeUseCase
    {
        private readonly IMediator mediator;

        public ObterFiltroRelatoriosAnosPorCicloModalidadeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Executar(long cicloId, Modalidade modalidade)
        {
            return await mediator.Send(new ObterFiltroRelatoriosAnosPorCicloModalidadeQuery(cicloId, modalidade));
        }
    }
}
