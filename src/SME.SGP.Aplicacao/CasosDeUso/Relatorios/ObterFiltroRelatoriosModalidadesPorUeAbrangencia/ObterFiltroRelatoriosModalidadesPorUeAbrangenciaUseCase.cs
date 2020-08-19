using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeAbrangenciaUseCase : IObterFiltroRelatoriosModalidadesPorUeAbrangenciaUseCase
    {
        private readonly IMediator mediator;

        public ObterFiltroRelatoriosModalidadesPorUeAbrangenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe)
        {
            return await mediator.Send(new ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery(codigoUe));
        }

    }
}
