using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaUseCase
    {
        public ExecutarConsolidacaoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var consolidacaoTurma = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoTurmaDto>();

            if (consolidacaoTurma == null)
            {
                SentrySdk.CaptureMessage($"Não foi possível iniciar a consolidação das turmas. O id da turma e o bimestre não foram informados", Sentry.Protocol.SentryLevel.Error);
                return false;
            }

            if (consolidacaoTurma.TurmaId == 0)
            {
                SentrySdk.CaptureMessage($"Não foi possível iniciar a consolidação das turmas. O id da turma não foi informado", Sentry.Protocol.SentryLevel.Error);
                return false;
            }

            var mensagemParaPublicar = JsonConvert.SerializeObject(consolidacaoTurma);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaConselhoClasseSync, mensagemParaPublicar, mensagemRabbit.CodigoCorrelacao, null));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaFechamentoSync, mensagemParaPublicar, mensagemRabbit.CodigoCorrelacao, null));

            return true;
        }
    }
}
