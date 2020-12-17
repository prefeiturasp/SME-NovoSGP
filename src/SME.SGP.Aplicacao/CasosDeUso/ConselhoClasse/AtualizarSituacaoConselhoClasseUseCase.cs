using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarSituacaoConselhoClasseUseCase : AbstractUseCase, IAtualizarSituacaoConselhoClasseUseCase
    {
        public AtualizarSituacaoConselhoClasseUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var command = mensagem.ObterObjetoMensagem<AtualizaSituacaoConselhoClasseCommand>();

            LogSentry(command, "Inicio");
            await mediator.Send(command);
            LogSentry(command, "Fim");

            return true;
        }

        private void LogSentry(AtualizaSituacaoConselhoClasseCommand command, string mensagem)
        {
            SentrySdk.AddBreadcrumb($"Mensagem AtualizarSituacaoConselhoClasseUseCase : {mensagem} - ConselhoClasseId:{command.ConselhoClasseId}", "Rabbit - AtualizarSituacaoConselhoClasseUseCase");
        }
    }
}
