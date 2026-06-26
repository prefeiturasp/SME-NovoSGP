using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoNotificacaoInformativoUsuarioUseCase : AbstractUseCase, IExecutarExclusaoNotificacaoInformativoUsuarioUseCase
    {
        public ExecutarExclusaoNotificacaoInformativoUsuarioUseCase(IMediator mediator) : base(mediator)
        { }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var param = mensagem.ObterObjetoMensagem<string>();
            if (string.IsNullOrEmpty(param)) return false;
            var notificacaoId = long.Parse(param);

            return await mediator.Send(new ExcluirNotificacaoPorIdCommand(notificacaoId));
        }
    }
}