using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasAulasPorProfessorEComponenteQueryHandler : IRequestHandler<ObterDatasAulasPorProfessorEComponenteQuery, IEnumerable<DatasAulasDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAula repositorio;
        private readonly IRepositorioTurma repositorioTurma;

        public ObterDatasAulasPorProfessorEComponenteQueryHandler(IMediator mediator, IRepositorioAula repositorio, IRepositorioTurma repositorioTurma)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<DatasAulasDto>> Handle(ObterDatasAulasPorProfessorEComponenteQuery request, CancellationToken cancellationToken)
        {
            var turma = await ObterTurma(request.TurmaCodigo);

            var tipoCalendarioId = await ObterTipoCalendario(turma);

            var periodosEscolares = await ObterPeriodosEscolares(tipoCalendarioId);

            var datasAulas = ObterAulasNosPeriodos(periodosEscolares, turma.AnoLetivo, turma.CodigoTurma, request.ComponenteCurricularCodigo, request.ProfessorRf, request.EhProfessorCj, request.TemPerfilSupervisorOuDiretor);

            return datasAulas.GroupBy(g => g.Data)
                    .Select(x => new DatasAulasDto()
                    {
                        Data = x.Key,
                        Aulas = x.Select(a => new AulaSimplesDto()
                        {
                            AulaId = a.IdAula,
                            AulaCJ = a.AulaCJ
                        })
                    });
        }

        private async Task<IEnumerable<PeriodoEscolar>> ObterPeriodosEscolares(long tipoCalendarioId)
        {
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendarioId));
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Períodos escolares não localizados para o tipo de calendário da turma");

            return periodosEscolares;
        }

        private async Task<long> ObterTipoCalendario(Turma turma)
        {
            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if (tipoCalendarioId == 0)
                throw new NegocioException("Tipo de calendário não existe para turma selecionada");

            return tipoCalendarioId;
        }

        private async Task<Turma> ObterTurma(string turmaCodigo)
        {
            var turma = await repositorioTurma.ObterPorCodigo(turmaCodigo);
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            return turma;
        }

        private IEnumerable<DataAulasProfessorDto> ObterAulasNosPeriodos(IEnumerable<PeriodoEscolar> periodosEscolares, int anoLetivo, string turmaCodigo, string componenteCurricularCodigo, string professorRf, bool ehProfessorCj, bool temPerfilSupervisorOuDiretor)
        {
            foreach (var periodoEscolar in periodosEscolares)
            {
                foreach (var aula in repositorio.ObterDatasDeAulasPorAnoTurmaEDisciplina(periodoEscolar.Id, anoLetivo, turmaCodigo, componenteCurricularCodigo, professorRf, ehProfessorCj, temPerfilSupervisorOuDiretor))
                {
                    yield return new DataAulasProfessorDto
                    {
                        Data = aula.DataAula,
                        IdAula = aula.Id,
                        AulaCJ = aula.AulaCJ,
                        Bimestre = periodoEscolar.Bimestre
                    };
                }
            }
        }

    }
}
