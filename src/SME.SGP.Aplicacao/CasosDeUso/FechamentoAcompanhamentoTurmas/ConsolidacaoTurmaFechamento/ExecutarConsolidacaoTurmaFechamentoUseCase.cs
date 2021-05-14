using MediatR;
using Newtonsoft.Json;
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

            var disciplinas = await mediator.Send(new ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQuery(consolidacaoTurma.TurmaId, consolidacaoTurma.Bimestre));

            foreach (var disciplina in disciplinas)
            {

                var mensagem = JsonConvert.SerializeObject(new FechamentoConsolidacaoTurmaComponenteBimestreDto(consolidacaoTurma.TurmaId, consolidacaoTurma.Bimestre, disciplina));

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.ConsolidarTurmaFechamentoComponenteTratar, mensagem, mensagemRabbit.CodigoCorrelacao, null, fila: RotasRabbit.ConsolidarTurmaFechamentoComponenteTratar));

            }
            return true;
        }
    }
}
