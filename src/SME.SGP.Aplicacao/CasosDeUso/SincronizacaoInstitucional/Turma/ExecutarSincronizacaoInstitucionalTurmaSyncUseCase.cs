using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalTurmaSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalTurmaSyncUseCase
    {
        public ExecutarSincronizacaoInstitucionalTurmaSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var ueId = mensagemRabbit.Mensagem.ToString();
            if (string.IsNullOrEmpty(ueId))
            {
                SentrySdk.CaptureMessage($"Não foi possível iniciar a sincronização das turmas. O codígo da Ue não foi informado", SentryLevel.Error);
            }

            var codigosTurma = await mediator.Send(new ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery(ueId));
            if (!codigosTurma?.Any() ?? true) return true;

            foreach (var codigoTurma in codigosTurma)
            {
                try
                {
                    var mensagemSyncTurma = new MensagemSyncTurmaDto(ueId, codigoTurma);

                    var mensagemParaPublicar = JsonConvert.SerializeObject(mensagemSyncTurma);

                    var publicarFilaIncluirTurma = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizaEstruturaInstitucionalTurmaTratar, mensagemParaPublicar, mensagemRabbit.CodigoCorrelacao, null));
                    if (!publicarFilaIncluirTurma)
                    {
                        var mensagem = $"Não foi possível inserir a turma de codígo : {codigoTurma} na fila de inclusão.";
                        SentrySdk.CaptureMessage(mensagem, SentryLevel.Error);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.AddBreadcrumb($"Não foi possível incluir a turma {codigoTurma} na fila para tratamento", "sincronizacao-institucional", null, null, BreadcrumbLevel.Error);
                    SentrySdk.CaptureException(ex);
                }
            }
            return true;
        }
    }
}
