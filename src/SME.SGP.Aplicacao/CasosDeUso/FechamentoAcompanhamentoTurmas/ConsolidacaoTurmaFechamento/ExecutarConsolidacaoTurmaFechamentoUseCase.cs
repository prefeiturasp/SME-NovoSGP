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
    public class ExecutarConsolidacaoTurmaFechamentoUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaFechamentoUseCase
    {
        public ExecutarConsolidacaoTurmaFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var consolidacaoTurma = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoTurmaDto>();

            if (consolidacaoTurma == null)
            {
                SentrySdk.CaptureMessage($"Não foi possível iniciar a consolidação do fechamento da turma. O id da turma e o bimestre não foram informados", Sentry.Protocol.SentryLevel.Error);
                return false;
            }

            if (consolidacaoTurma.TurmaId == 0)
            {
                SentrySdk.CaptureMessage($"Não foi possível iniciar a consolidação do fechamento da turma. O id da turma não foi informado", Sentry.Protocol.SentryLevel.Error);
                return false;
            }

            // obter os componentes da turma por id e bimestre
            //var componentesCurriculares = await mediator.Send(new ObterComp)

            return true;
        }
    }
}
