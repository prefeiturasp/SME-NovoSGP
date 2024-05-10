using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosPorTurmaQueryHandler : IRequestHandler<ObterPareceresConclusivosPorTurmaQuery, IEnumerable<ConselhoClasseParecerConclusivo>>
    {
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorioParecer;
        private readonly IMediator mediator;

        public ObterPareceresConclusivosPorTurmaQueryHandler(
                                        IRepositorioConselhoClasseParecerConclusivo repositorioParecer,
                                        IMediator mediator)
        {
            this.repositorioParecer = repositorioParecer ?? throw new ArgumentNullException(nameof(repositorioParecer));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ConselhoClasseParecerConclusivo>> Handle(ObterPareceresConclusivosPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioParecer.ObterListaPorTurmaIdAsync(request.Turma.Id, await ObterUltimoPeriodoLetivo(request.Turma));

        private async Task<DateTime> ObterUltimoPeriodoLetivo(Turma turma)
        {
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));

            if (tipoCalendario.NaoEhNulo())
            {
                var periodos = (await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendario.Id))).ToList();

                if (periodos.NaoEhNulo() || periodos.Any())
                {
                    return periodos.OrderBy(pl => pl.Bimestre).Last().PeriodoFim;
                }
            }

            return DateTime.Today;
        }
    }
}
