using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardAcompanhamentoAprendizagemUseCase : AbstractUseCase, IObterDashboardAcompanhamentoAprendizagemUseCase
    {
        public ObterDashboardAcompanhamentoAprendizagemUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<DashboardAcompanhamentoAprendizagemDto>> Executar(FiltroDashboardAcompanhamentoAprendizagemDto filtro)
        {
            await mediator.Send(new ObterDashBoardEncaminhamentoAprendizagemQuery());
        }
    }
}
