using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase : IObterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase
    {
        private readonly IMediator mediator;

        public ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe, Modalidade modalidade)
        {
            return await mediator.Send(new ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeQuery(codigoUe, modalidade));
        }
    }
}
