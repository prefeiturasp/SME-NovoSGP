using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacoesDeAtendimentoNAAPAUseCase : AbstractUseCase, IObterObservacoesDeAtendimentoNAAPAUseCase
    {

        public ObterObservacoesDeAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>> Executar(long encaminhamentoNAAPAId)
        {
            var usuarioLohadoRf = await mediator.Send(ObterUsuarioLogadoRFQuery.Instance);
            return await mediator.Send(new ObterObservacaoEncaminhamentosNAAPAQuery(encaminhamentoNAAPAId, usuarioLohadoRf));
        }
    }
}
