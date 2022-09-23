using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee.ServicosFake
{
    public class ExecutaNotificacaoEncerramentoEncaminhamentoAEECommandHandlerFake : IRequestHandler<ExecutaNotificacaoEncerramentoEncaminhamentoAEECommand, bool>
    {
        private INotificacaoEncerramentoEncaminhamentoAEEUseCase notificaoUseCase;

        public ExecutaNotificacaoEncerramentoEncaminhamentoAEECommandHandlerFake(INotificacaoEncerramentoEncaminhamentoAEEUseCase notificaoUseCase)
        {
            this.notificaoUseCase = notificaoUseCase;
        }

        public async Task<bool> Handle(ExecutaNotificacaoEncerramentoEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var dto = new NotificacaoEncaminhamentoAEEDto
            {
                EncaminhamentoAEEId = request.EncaminhamentoAEEId,
                UsuarioRF = request.UsuarioRF,
                UsuarioNome = request.UsuarioNome
            };
   
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            return await this.notificaoUseCase.Executar(mensagem);
        }
    }
}
