using MediatR;
using Sentry;
using Sentry.Protocol;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SincronizarUeTurmaAulaRegenciaAutomaticaUseCase : AbstractUseCase, ISincronizarUeTurmaAulaRegenciaAutomaticaUseCase
    {
        public SincronizarUeTurmaAulaRegenciaAutomaticaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosCriacao = mensagemRabbit.ObterObjetoMensagem<DadosCriacaoAulasAutomaticasDto>();

            if (dadosCriacao.TipoCalendarioId > 0)
            {
                if (dadosCriacao.DadosTurmas != null && dadosCriacao.DadosTurmas.Any())
                {
                    var comando = new CriarAulasRegenciaAutomaticamenteCommand(dadosCriacao.Modalidade, dadosCriacao.TipoCalendarioId, dadosCriacao.UeCodigo, dadosCriacao.DiasLetivosENaoLetivos,
                        dadosCriacao.DadosTurmas, dadosCriacao.DiasForaDoPeriodoEscolar);
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizarAulasRegenciaAutomaticamente, comando, Guid.NewGuid(), null));

                }
                return true;
            }
            else
            {
                SentrySdk.CaptureMessage($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não há Calendário Escolar cadastrado.", SentryLevel.Error);
            }
            return false;
        }
    }
}