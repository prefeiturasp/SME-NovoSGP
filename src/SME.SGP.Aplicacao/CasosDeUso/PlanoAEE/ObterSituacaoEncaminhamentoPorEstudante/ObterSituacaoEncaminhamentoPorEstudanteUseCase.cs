using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterSituacaoEncaminhamentoPorEstudanteUseCase : AbstractUseCase, IObterSituacaoEncaminhamentoPorEstudanteUseCase
    {
        public ObterSituacaoEncaminhamentoPorEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<SituacaoEncaminhamentoPorEstudanteDto> Executar(string codigoAluno)
        {
            return await mediator.Send(new ObterSituacaoEncaminhamentoAEEPorEstudanteQuery(codigoAluno));
        }
    }
}
