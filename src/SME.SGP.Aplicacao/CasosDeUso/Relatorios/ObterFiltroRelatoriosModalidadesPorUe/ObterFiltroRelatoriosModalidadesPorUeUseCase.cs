using MediatR;
using SME.SGP.Dominio;
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
        public async Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe, int anoLetivo, bool consideraHistorico, bool consideraNovasModalidades)
        {
            var login = await mediator.Send(new ObterLoginAtualQuery());
            var perfil = await mediator.Send(new ObterPerfilAtualQuery());
            var modalidadesQueSeraoIgnoradas = await mediator.Send(new ObterNovasModalidadesPorAnoQuery(anoLetivo, consideraNovasModalidades));
            return await mediator.Send(new ObterFiltroRelatoriosModalidadesPorUeQuery(codigoUe, anoLetivo, consideraHistorico, login, perfil, modalidadesQueSeraoIgnoradas));
        }
    }
}
