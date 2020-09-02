using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashBoardUseCase : AbstractUseCase, IObterDashBoardUseCase
    {
        public ObterDashBoardUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<DashBoard>> Executar()
        {
            return await mediator.Send(new ObterDashBoardPorPerfilQuery());
        }
    }
}
