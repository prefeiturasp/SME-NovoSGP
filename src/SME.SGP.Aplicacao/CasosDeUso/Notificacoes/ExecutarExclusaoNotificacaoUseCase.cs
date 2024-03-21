using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutarExclusaoNotificacaoUseCase : AbstractUseCase, IExecutarExclusaoNotificacaoUseCase
    {
        public ExecutarExclusaoNotificacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            long.TryParse(param.Mensagem.ToString(), out long notificacaoId);

            if (notificacaoId > 0)
                await mediator.Send(new ExcluirNotificacaoTotalPorIdCommand(notificacaoId));

            return true;
        }
    }
}
