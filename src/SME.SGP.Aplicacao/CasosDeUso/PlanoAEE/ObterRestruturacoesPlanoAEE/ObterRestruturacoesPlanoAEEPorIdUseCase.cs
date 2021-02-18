using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterRestruturacoesPlanoAEEPorIdUseCase : AbstractUseCase, IObterRestruturacoesPlanoAEEPorIdUseCase
    {
        public ObterRestruturacoesPlanoAEEPorIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<PlanoAEEReestruturacaoDto>> Executar(long planoId)
        {
           return await mediator.Send(new ObterRestruturacoesPlanoAEEQuery(planoId));
        }
    }
}
