using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterVersoesPlanoAEEUseCase : AbstractUseCase, IObterVersoesPlanoAEEUseCase
    {
        public ObterVersoesPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<PlanoAEEVersaoDto>> Executar(FiltroVersoesPlanoAEEDto filtro)
        {
            return await mediator.Send(new ObterVersoesPlanoAEEQuery(filtro.PlanoId, filtro.VersaoPlanoId));
        }
    }
}
