using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorAnoUseCase : IObterModalidadesPorAnoUseCase
    {
        private readonly IMediator mediator;

        public ObterModalidadesPorAnoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<EnumeradoRetornoDto>> Executar(int anoLetivo, bool consideraHistorico, bool consideraNovasModalidades)
        {
            var login = await mediator.Send(new ObterLoginAtualQuery());
            var perfil = await mediator.Send(new ObterPerfilAtualQuery());
            var modadlidadesQueSeraoIgnoradas = await mediator.Send(new ObterNovasModalidadesPorAnoQuery(anoLetivo, consideraNovasModalidades));
            return await mediator.Send(new ObterModalidadesPorAnoQuery(anoLetivo, consideraHistorico, login, perfil, modadlidadesQueSeraoIgnoradas));
        }
    }
}