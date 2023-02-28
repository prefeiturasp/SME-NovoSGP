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
            if (request.AulasPendencia == null || !request.AulasPendencia.Any()) return request.AulasPendencia;

            var aulas = request.AulasPendencia.ToList();
            var agrupamentoTurmaDisciplina = aulas.GroupBy(aula => new { TurmaCodigo = aula.TurmaId, aula.DisciplinaId, TurmaId = aula.Turma.Id, ModalidadeTipoCalendario = aula.Turma.ModalidadeTipoCalendario });
            foreach (var turmaDisciplina in agrupamentoTurmaDisciplina)
            {
                var periodoEscolarFechamentoEmAberto = (await mediator.Send(new ObterPeriodoEscolarFechamentoEmAbertoQuery(turmaDisciplina.Key.TurmaCodigo, turmaDisciplina.Key.ModalidadeTipoCalendario, DateTimeExtension.HorarioBrasilia().Date)));
                if (periodoEscolarFechamentoEmAberto != null)
                {
                    var situacao = await mediator.Send(new ObterSituacaoFechamentoTurmaComponenteQuery(turmaDisciplina.Key.TurmaId, long.Parse(turmaDisciplina.Key.DisciplinaId), periodoEscolarFechamentoEmAberto.Id));
                    if (situacao != SituacaoFechamento.NaoIniciado)
                        aulas.RemoveAll(aula => aula.TurmaId == turmaDisciplina.Key.TurmaCodigo && aula.DisciplinaId == turmaDisciplina.Key.DisciplinaId);
                }

            }

            return aulas;
        }
    }
}
