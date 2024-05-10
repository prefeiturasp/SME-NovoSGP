using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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
        public async Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe, bool consideraNovasModalidades, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var login = await mediator.Send(ObterLoginAtualQuery.Instance);
            var perfil = await mediator.Send(ObterPerfilAtualQuery.Instance);

            var modalidadesQueSeraoIgnoradas = await mediator
                .Send(new ObterNovasModalidadesPorAnoQuery(anoLetivo > 0 ? anoLetivo : DateTime.Today.Year, consideraNovasModalidades));

            return await mediator
                .Send(new ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery(codigoUe, login, perfil, modalidadesQueSeraoIgnoradas, consideraHistorico, anoLetivo));
        }
    }
}
