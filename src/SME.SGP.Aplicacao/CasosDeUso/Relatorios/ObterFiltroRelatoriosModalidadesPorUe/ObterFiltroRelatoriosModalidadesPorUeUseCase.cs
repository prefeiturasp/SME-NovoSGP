using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeUseCase : IObterFiltroRelatoriosModalidadesPorUeUseCase
    {
        private readonly IMediator mediator;

        public ObterFiltroRelatoriosModalidadesPorUeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe)
        {
            return await mediator.Send(new ObterFiltroRelatoriosModalidadesPorUeQuery(codigoUe));
        }
    }
}
