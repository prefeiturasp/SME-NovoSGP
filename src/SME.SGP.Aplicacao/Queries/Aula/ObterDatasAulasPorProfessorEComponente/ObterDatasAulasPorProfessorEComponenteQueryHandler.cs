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
    public class ObterDatasAulasPorProfessorEComponenteQueryHandler : IRequestHandler<ObterDatasAulasPorProfessorEComponenteQuery, IEnumerable<DatasAulasDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAula repositorio;
        private readonly IRepositorioTurma repositorioTurma;

        private readonly IRepositorioAula repositorioAula;

        public ObterDatasAulasPorProfessorEComponenteQueryHandler(IMediator mediator, IRepositorioAula repositorio, IRepositorioTurma repositorioTurma, IRepositorioAula repositorioAula)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));

            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<DatasAulasDto>> Handle(ObterDatasAulasPorProfessorEComponenteQuery request, CancellationToken cancellationToken)
        {
            var turma = await ObterTurma(request.TurmaCodigo);
            var tipoCalendarioId = await ObterTipoCalendario(turma);
            var periodosEscolares = await ObterPeriodosEscolares(tipoCalendarioId);
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var datasAulas = ObterAulasNosPeriodos(periodosEscolares, turma.AnoLetivo, turma.CodigoTurma, request.ComponenteCurricularCodigo,string.Empty);

            var aulas = new List<Aula>();
            datasAulas.ToList()
                .ForEach(da => aulas.Add(repositorioAula.ObterPorId(da.IdAula)));

            var aulasPermitidas = usuarioLogado
                .ObterAulasQuePodeVisualizar(aulas, new string[] { request.ComponenteCurricularCodigo })
                .Select(a => a.Id);

            return datasAulas.Where(da => aulasPermitidas.Contains(da.IdAula)).GroupBy(g => g.Data)
                .Select(x => new DatasAulasDto()
                {
                    Data = x.Key,
                    Aulas = x.OrderBy(a => a.AulaCJ).Select(async a => new AulaSimplesDto()
                    {
                        AulaId = a.IdAula,
                        AulaCJ = a.AulaCJ,
                        PodeEditar = (usuarioLogado.EhProfessorCj() && a.AulaCJ) || (!a.AulaCJ && (usuarioLogado.EhProfessor() || usuarioLogado.EhGestorEscolar())),
                        ProfessorRf = a.ProfessorRf,
                        CriadoPor = a.CriadoPor,
                        PossuiFrequenciaRegistrada = await mediator.Send(new ObterAulaPossuiFrequenciaQuery(a.IdAula)),
                        TipoAula = a.TipoAula
                    }).Select(a => a.Result)
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

        private IEnumerable<DataAulasProfessorDto> ObterAulasNosPeriodos(IEnumerable<PeriodoEscolar> periodosEscolares, int anoLetivo, string turmaCodigo, string componenteCurricularCodigo, string professorRf)
        {
            var aulas = repositorio.ObterDatasDeAulasPorAnoTurmaEDisciplina(periodosEscolares.Select(s => s.Id).Distinct(), anoLetivo, turmaCodigo, componenteCurricularCodigo, professorRf, null, null);
            foreach (var periodoEscolar in periodosEscolares)
            {
                foreach (var aula in aulas)
                {
                    yield return new DataAulasProfessorDto
                    {
                        Data = aula.DataAula,
                        IdAula = aula.Id,
                        AulaCJ = aula.AulaCJ,
                        Bimestre = periodoEscolar.Bimestre,
                        ProfessorRf = aula.ProfessorRf,
                        CriadoPor = aula.CriadoPor,
                        TipoAula = aula.TipoAula
                    };
                }
            }
        }

    }
}
