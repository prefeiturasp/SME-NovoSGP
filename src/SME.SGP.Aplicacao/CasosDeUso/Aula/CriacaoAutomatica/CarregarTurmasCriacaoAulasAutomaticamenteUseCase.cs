using MediatR;
using Sentry;
using Sentry.Protocol;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarTurmasCriacaoAulasAutomaticamenteUseCase : AbstractUseCase, ICarregarTurmasCriacaoAulasAutomaticamenteUseCase
    {
        public CarregarTurmasCriacaoAulasAutomaticamenteUseCase(IMediator mediator) : base(mediator)
        {

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
            var modalidades = new Modalidade[] {
                Modalidade.InfantilPreEscola,
                Modalidade.Fundamental,
                Modalidade.EJA
            };
            var tiposCalendarios = await mediator.Send(new ObterTiposCalendariosAulaAutomaticaPorAnoLetivoEModalidadesQuery(anoAtual, modalidades));
            foreach (var tipoCalendario in tiposCalendarios)
            {
                if (tipoCalendario.TipoCalendarioId > 0)
                {
                    var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.TipoCalendarioId));
                    if (periodosEscolares != null && periodosEscolares.Any())
                    {
                        var diasLetivosENaoLetivos = await mediator.Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendario.TipoCalendarioId));

                        var diasForaDoPeriodoEscolar = await mediator.Send(new ObterDiasForaDoPeriodoEscolarQuery(periodosEscolares));

                        var turmas = new List<Turma>();

                        switch (tipoCalendario.Modalidade)
                        {
                            case Modalidade.InfantilPreEscola:
                                turmas = (List<Turma>)await mediator.Send(new ObterTurmasInfantilNaoDeProgramaQuery(anoAtual));
                                break;
                            case Modalidade.Fundamental:
                                break;
                            case Modalidade.EJA:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return true;
        }
    }
}
