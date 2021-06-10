using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase : AbstractUseCase, IValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase
    {
        public ValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem {nameof(ValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase)}", $"Rabbit - {nameof(ValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase)}");
            var command = mensagemRabbit.ObterObjetoMensagem<ValidaAusenciaParaConciliacaoFrequenciaTurmaCommand>();
            
            return await mediator.Send(command);
        }
    }
}
