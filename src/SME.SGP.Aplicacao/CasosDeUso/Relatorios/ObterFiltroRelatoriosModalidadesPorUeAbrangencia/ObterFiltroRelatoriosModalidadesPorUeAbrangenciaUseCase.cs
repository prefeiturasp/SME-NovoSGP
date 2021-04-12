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
        public async Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe, bool consideraNovasModalidades)
        {
            var login = await mediator.Send(new ObterLoginAtualQuery());
            var perfil = await mediator.Send(new ObterPerfilAtualQuery());
            var modadlidadesQueSeraoIgnoradas = await mediator.Send(new ObterNovasModalidadesPorAnoQuery(DateTime.Now.Year, consideraNovasModalidades));
            return await mediator.Send(new ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery(codigoUe, login, perfil, modadlidadesQueSeraoIgnoradas));
        }
    }
}
