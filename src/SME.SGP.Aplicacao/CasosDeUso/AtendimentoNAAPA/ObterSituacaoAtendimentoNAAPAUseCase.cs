using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterSituacaoAtendimentoNAAPAUseCase : IObterSituacaoAtendimentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterSituacaoAtendimentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<SituacaoDto> Executar(long id)
        {
            return await this.mediator.Send(new ObterSituacaoEncaminhamentoNAAPAQuery(id));
        }
    }
}
