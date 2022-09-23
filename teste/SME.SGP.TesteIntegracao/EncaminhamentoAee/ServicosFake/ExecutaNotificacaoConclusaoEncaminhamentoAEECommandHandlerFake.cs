using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee.ServicosFake
{
    public class ExecutaNotificacaoConclusaoEncaminhamentoAEECommandHandlerFake : IRequestHandler<ExecutaNotificacaoConclusaoEncaminhamentoAEECommand, bool>
    {
        private readonly INotificacaoConclusaoEncaminhamentoAEEUseCase useCase;

        public ExecutaNotificacaoConclusaoEncaminhamentoAEECommandHandlerFake(INotificacaoConclusaoEncaminhamentoAEEUseCase useCase)
        {
            this.useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        }

        public async Task<bool> Handle(ExecutaNotificacaoConclusaoEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var dto = new NotificacaoEncaminhamentoAEEDto
            {
                EncaminhamentoAEEId = request.EncaminhamentoAEEId,
                UsuarioRF = request.UsuarioRF,
                UsuarioNome = request.UsuarioNome
            };

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            return await this.useCase.Executar(mensagem);
        }
    }
}
