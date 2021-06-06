using MediatR;
using Sentry;
using Sentry.Protocol;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilAutomaticamenteUseCase : ICriarAulasInfantilAutomaticamenteUseCase
    {
        private readonly IMediator mediator;

        public CriarAulasInfantilAutomaticamenteUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var executarManutencao = await mediator.Send(new ObterExecutarManutencaoAulasInfantilQuery());

            if (!executarManutencao)
            {
                SentrySdk.CaptureMessage($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois seu parâmetro está marcado como não executar", SentryLevel.Warning);
                return false;
            }

            var anoAtual = DateTime.Now.Year;
            var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.Infantil, anoAtual, null));
            if (tipoCalendarioId > 0)
            {
                var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
                if (periodosEscolares != null && periodosEscolares.Any())
                {
                    var diasLetivosENaoLetivos = await mediator.Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendarioId));
                    var diasForaDoPeriodoEscolar = await mediator.Send(new ObterDiasForaDoPeriodoEscolarQuery(periodosEscolares));

                    var turmas = await mediator.Send(new ObterTurmasInfantilNaoDeProgramaQuery(anoAtual));                    
                    if (turmas != null && turmas.Any())
                    {
                        var paginador = 900;
                        for (int pagina = 0; pagina <= turmas.Count(); pagina += paginador)
                        {
                            var lista = turmas.Skip(pagina).Take(paginador);
                            if (lista.Any())
                            {
                                var comando = new CriarAulasInfantilAutomaticamenteCommand(diasLetivosENaoLetivos.ToList(), lista, tipoCalendarioId, diasForaDoPeriodoEscolar);

                                SentrySdk.CaptureMessage($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Iniciando Rotina de manutenção de aulas do Infantil");
                                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaCriarAulasInfatilAutomaticamente, comando, Guid.NewGuid(), null));
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    SentrySdk.CaptureMessage($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não há Período Escolar cadastrado.", SentryLevel.Error);
                }
            }
            else
            {
                SentrySdk.CaptureMessage($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não há Calendário Escolar cadastrado.", SentryLevel.Error);
            }
            return false;
        }
    }
}
