using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorTurmaUseCase : AbstractUseCase, IObterPeriodoEscolarPorTurmaUseCase
    {
        public ObterPeriodoEscolarPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<PeriodoEscolarPorTurmaDto>> Executar(long turmaId)
        {
            var periodosEscolares = new List<PeriodoEscolar>();
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

            if (turma == null)
                throw new NegocioException($"Turma [{turmaId}] não localizada!");

            var periodos = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));

            if (periodos.Any())
                periodosEscolares = FiltrarPeriodosCorretos(periodos.ToList());
           
            return periodosEscolares?.Select(async c => new PeriodoEscolarPorTurmaDto
            {
                Bimestre = c.Bimestre,
                Id = c.Id,
                Migrado = c.Migrado,
                PeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, c.Bimestre, turma.AnoLetivo == DateTime.Today.Year, periodosEscolares.FirstOrDefault().TipoCalendarioId)),
            }).Select(_task => _task.Result);
        }

        public List<PeriodoEscolar> FiltrarPeriodosCorretos(List<PeriodoEscolar> periodos)
        {
            var periodosAgrupados = periodos.GroupBy(p => p.TipoCalendarioId).Select(p=> new
            {
                TipoCalendarioId = p.Key,
                QuantidadePeriodos = p.Count()
            });

            if (periodosAgrupados.Count() > 1)
            {
                var tipoCalendarioId = periodosAgrupados.Where(p => p.QuantidadePeriodos > 1).Select(p => p.TipoCalendarioId).FirstOrDefault();
                return periodos.Where(p => p.TipoCalendarioId == tipoCalendarioId).ToList();
            }
            return periodos.ToList();
        }
    }
}
