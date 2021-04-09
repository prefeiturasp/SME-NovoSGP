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
    public class EnviarSincronizacaoEstruturaInstitucionalTurmasUseCase : AbstractUseCase, IEnviarSincronizacaoEstruturaInstitucionalTurmasUseCase
    {
        public EnviarSincronizacaoEstruturaInstitucionalTurmasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var ueId = JsonConvert.DeserializeObject<string>(mensagemRabbit.Mensagem.ToString());
            if (string.IsNullOrEmpty(ueId))
            {
                SentrySdk.CaptureMessage($"Não foi possível iniciar a sincronização das turmas. O codígo da Ue não foi informado");
            }

            var codigosTurma = await mediator.Send(new ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery(ueId));
            if (!codigosTurma?.Any() ?? true) return true;

            foreach (var codigoTurma in codigosTurma)
            {
                try
                {
                    var publicarFilaIncluirTurma = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizaEstruturaInstitucionalTurma, codigoTurma, Guid.NewGuid(), null, fila: RotasRabbit.SincronizaEstruturaInstitucionalTurma));
                    if (!publicarFilaIncluirTurma)
                    {
                        var mensagem = $"Não foi possível inserir a turma de codígo : {codigoTurma} na fila de inclusão.";
                        SentrySdk.CaptureMessage(mensagem);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }
            return true;
        }
    }
}
