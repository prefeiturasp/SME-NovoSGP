using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaDiasNaoLetivosCommandHandler : IRequestHandler<PendenciaDiasNaoLetivosCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;
        public PendenciaDiasNaoLetivosCommandHandler(IMediator mediator, IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<bool> Handle(PendenciaDiasNaoLetivosCommand request, CancellationToken cancellationToken)
        {
            var data = DateTime.Today;

            var modalidades = new List<Modalidade> { Modalidade.Fundamental, Modalidade.Medio, Modalidade.EJA };

            foreach (var modalidade in modalidades)
            {
                var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(modalidade, data.Year, null));

                var periodosEscolares = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

                var aulas = await mediator.Send(new ObterAulasReduzidaPorTipoCalendarioQuery(tipoCalendarioId));

                var diasComEvento = await mediator.Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendarioId));

                var diasComEventosNaoLetivos = diasComEvento.Where(e => e.EhNaoLetivo);

                if (aulas != null)
                {
                    var listaAgrupada = aulas.Where(a => diasComEventosNaoLetivos.Any(d => d.Data == a.Data)).GroupBy(x => new { x.TurmaId, x.DisciplinaId }).ToList();

                    foreach (var turmas in listaAgrupada)
                    {
                        var pendenciaId = await mediator.Send(new ObterPendenciaAulaPorTurmaIdDisciplinaIdQuery(turmas.Key.TurmaId, turmas.Key.DisciplinaId));

                        if (pendenciaId == 0)
                            pendenciaId = await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.AulaNaoLetivo, TipoPendencia.AulaNaoLetivo.Name()));

                        foreach (var aula in turmas)
                        {
                            var pendenciaAulaId = await repositorioPendenciaAula.ObterPendenciaAulaPorAulaId(aula.aulaId);

                            if (pendenciaAulaId > 0)
                            {
                                var pendenciaAula = new PendenciaAula
                                {
                                    AulaId = aula.aulaId,
                                    Motivo = "",
                                    PendenciaId = pendenciaId
                                };
                                await repositorioPendenciaAula.Salvar(pendenciaAula);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
