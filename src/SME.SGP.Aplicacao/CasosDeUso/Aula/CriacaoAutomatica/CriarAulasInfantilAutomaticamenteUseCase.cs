using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
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
            var anoAtual = DateTime.Now.Year;
            var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.Infantil, anoAtual, null));
            if (tipoCalendarioId > 0)
            {
                var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
                if (periodosEscolares != null && periodosEscolares.Any())
                {
                    var diasLetivos = await mediator.Send(new ObterDiasLetivosPorPeriodosEscolaresQuery(periodosEscolares, tipoCalendarioId));

                    var turmas = await mediator.Send(new ObterTurmasInfantilNaoDeProgramaQuery(anoAtual));
                    if (turmas != null && turmas.Any())
                    {
                        var turmasPorUe = turmas.GroupBy(c => c.UeId);

                        foreach (var ue in turmasPorUe)
                        {

                        }
                        
                        SentrySdk.AddBreadcrumb($"Iniciando manutenção para {turmas.Count()} turmas.");
                        for (int pagina = 0; pagina <= turmas.Count(); pagina += 2000)
                        {
                            var lista = turmas.Skip(pagina).Take(2000);
                            if (lista.Any())
                            {
                                var comando = new CriarAulasInfantilAutomaticamenteCommand(diasLetivos.ToList(), lista, tipoCalendarioId);

                                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaCriarAulasInfatilAutomaticamente, comando, Guid.NewGuid(), null));
                            }
                        }
                        SentrySdk.CaptureMessage($"Criação automática de aulas Infantil.");
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
