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
    public class ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQueryHandler : IRequestHandler<ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery, IEnumerable<Aula>>
    {
        private readonly IMediator mediator;

        public ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<Aula>> Handle(ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery request, CancellationToken cancellationToken)
        {
            if (request.AulasPendencia.EhNulo() || !request.AulasPendencia.Any()) return request.AulasPendencia;

            var aulas = request.AulasPendencia.ToList();
            var agrupamentoTurmaDisciplina = aulas.GroupBy(aula => new { TurmaCodigo = aula.TurmaId, aula.DisciplinaId, TurmaId = aula.Turma.Id, ModalidadeTipoCalendario = aula.Turma.ModalidadeTipoCalendario });
            foreach (var turmaDisciplina in agrupamentoTurmaDisciplina)
            {
                var periodoEscolarFechamentoEmAberto = (await mediator.Send(new ObterPeriodoEscolarFechamentoEmAbertoQuery(turmaDisciplina.Key.TurmaCodigo, turmaDisciplina.Key.ModalidadeTipoCalendario, DateTimeExtension.HorarioBrasilia().Date)));
                if (periodoEscolarFechamentoEmAberto.Any())
                {
                    foreach (var periodo in periodoEscolarFechamentoEmAberto)
                    {
                        var existeFechamentoTurmaDisciplinaIniciado = await mediator.Send(new VerificarFechamentoTurmaComponenteEmAndamentoQuery(turmaDisciplina.Key.TurmaId, long.Parse(turmaDisciplina.Key.DisciplinaId), periodo.Id));
                        if (existeFechamentoTurmaDisciplinaIniciado)
                            aulas.RemoveAll(aula => aula.TurmaId == turmaDisciplina.Key.TurmaCodigo && aula.DisciplinaId == turmaDisciplina.Key.DisciplinaId);
                    }
                }

            }

            return aulas;
        }
    }
}
