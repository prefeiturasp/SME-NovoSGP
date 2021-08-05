using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExcluirNotificacoesPorAulaIdUseCase : AbstractUseCase, IExcluirNotificacoesPorAulaIdUseCase
    {
        public ExcluirNotificacoesPorAulaIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            await mediator.Send(new ExcluirNotificacoesDaAulaCommand(filtro.Id));
            return true;
        }
    }
}
