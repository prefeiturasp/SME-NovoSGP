using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterSecoesPorEtapaDeEncaminhamentoAEEUseCase : IObterSecoesPorEtapaDeEncaminhamentoAEEUseCase
    {
        private readonly IMediator mediator;

        public ObterSecoesPorEtapaDeEncaminhamentoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Executar(long encaminhamentoAeeId)
        {
            return await mediator.Send(new ObterSecoesPorEtapaDeEncaminhamentoQuery(encaminhamentoAeeId));
        }
    }
}
