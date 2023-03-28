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
        private readonly IRepositorioAulaConsulta repositorioConsulta;
        private readonly IRepositorioTurmaConsulta repositorioTurma;


        public ObterDatasAulasPorProfessorEComponenteQueryHandler(IMediator mediator, IRepositorioAulaConsulta repositorioConsulta, IRepositorioTurmaConsulta repositorioTurma)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConsulta = repositorioConsulta ?? throw new ArgumentNullException(nameof(repositorioConsulta));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }


        public async Task<IEnumerable<DatasAulasDto>> Handle(ObterDatasAulasPorProfessorEComponenteQuery request, CancellationToken cancellationToken)
        {
            var turma = await ObterTurma(request.TurmaCodigo);
            var tipoCalendarioId = await ObterTipoCalendario(turma);
            var periodosEscolares = await ObterPeriodosEscolares(tipoCalendarioId);
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var componenteCurricularId = long.Parse(request.ComponenteCurricularCodigo);

            IEnumerable<ComponenteCurricularEol> componentesCurricularesEolProfessor = Enumerable.Empty<ComponenteCurricularEol>();

            componentesCurricularesEolProfessor = await mediator
                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.TurmaCodigo,
                                                                              usuarioLogado.CodigoRf,
                                                                              usuarioLogado.PerfilAtual,
                                                                              usuarioLogado.EhProfessorInfantilOuCjInfantil()));

            var componenteCurricularCorrespondente = componentesCurricularesEolProfessor
                .FirstOrDefault(cp => cp.Codigo.Equals(componenteCurricularId) || cp.CodigoComponenteTerritorioSaber.Equals(componenteCurricularId));

            var componenteCurricular = await mediator
                .Send(new ObterComponenteCurricularPorIdQuery(componenteCurricularCorrespondente != null && componenteCurricularCorrespondente.CodigoComponenteTerritorioSaber > 0 ? componenteCurricularCorrespondente.CodigoComponenteTerritorioSaber : componenteCurricularId));

            List<(string codigo, string codigoComponentePai, string codigoTerritorioSaber)> componentesCurricularesDoProfessorCj = new List<(string, string, string)>();

            if (usuarioLogado.EhProfessorCj())
                await DefinirComponentesProfessorCJ(turma, usuarioLogado, componentesCurricularesDoProfessorCj);

            var componenteCorrespondente = (!usuarioLogado.EhProfessorCj() && usuarioLogado.PerfilAtual == Perfis.PERFIL_CJ) && componentesCurricularesEolProfessor != null && componentesCurricularesEolProfessor.Any(x => x.Regencia)
                    ? componentesCurricularesEolProfessor.FirstOrDefault(cp => cp.CodigoComponenteCurricularPai.ToString() == request.ComponenteCurricularCodigo || cp.Codigo.ToString() == componenteCurricular.CdComponenteCurricularPai.ToString())
                    : new ComponenteCurricularEol
                    {
                        Codigo = componenteCurricularCorrespondente?.Codigo ?? (long.TryParse(request.ComponenteCurricularCodigo, out long codigo) ? codigo : 0),
                        CodigoComponenteCurricularPai = componentesCurricularesDoProfessorCj.Any() ? componentesCurricularesDoProfessorCj.Select(c => long.TryParse(c.codigoComponentePai, out long codigoPai) ? codigoPai : 0).FirstOrDefault() : componenteCurricularCorrespondente?.CodigoComponenteCurricularPai ?? 0,
                        CodigoComponenteTerritorioSaber = componentesCurricularesDoProfessorCj.Any() ? componentesCurricularesDoProfessorCj.Select(c => long.TryParse(c.codigoTerritorioSaber, out long codigoTerritorio) ? codigoTerritorio : 0).FirstOrDefault() : componenteCurricularCorrespondente?.CodigoComponenteTerritorioSaber ?? 0
                    };


            var codigoComponentes = new[] { componenteCorrespondente.Regencia ? componenteCorrespondente.CodigoComponenteCurricularPai.ToString() : componenteCorrespondente.Codigo.ToString() };
            if (componenteCorrespondente.CodigoComponenteTerritorioSaber > 0)
                codigoComponentes = codigoComponentes.Append(componenteCorrespondente.CodigoComponenteTerritorioSaber.ToString()).ToArray();

            var datasAulas = await ObterAulasNosPeriodos(periodosEscolares, turma.AnoLetivo, turma.CodigoTurma, codigoComponentes, string.Empty);

            if (datasAulas == null || !datasAulas.Any())
                return default;

            var ids = datasAulas.Select(a => a.IdAula).Distinct().ToList();
            var aulas = await mediator.Send(new ObterAulasPorIdsQuery(ids));

            var aulasPermitidas = new List<long>();
            bool verificaCJPodeEditar = await VerificaCJPodeEditarRegistroTitular(turma.AnoLetivo);

            if (usuarioLogado.EhProfessorCjInfantil() && verificaCJPodeEditar)
            {
                aulasPermitidas = aulas.Where(a => a.DisciplinaId == request.ComponenteCurricularCodigo)
                                       .Select(a => a.Id).ToList();
            }
            else
            {
                IList<(string, string)> codigosComponentes = new List<(string, string)>()
                    { (componenteCorrespondente?.Codigo.ToString() ?? request.ComponenteCurricularCodigo, componenteCorrespondente?.CodigoComponenteTerritorioSaber.ToString() ?? null) };

                aulasPermitidas = usuarioLogado
                    .ObterAulasQuePodeVisualizar(aulas, codigosComponentes)
                    .Select(a => a.Id).ToList();
            }

            return datasAulas.Where(da => aulasPermitidas.Contains(da.IdAula)).GroupBy(g => g.Data)
                .Select(x => new DatasAulasDto()
                {
                    Data = x.Key,
                    Aulas = x.OrderBy(a => a.AulaCJ).Select(async a => new AulaSimplesDto()
                    {
                        AulaId = a.IdAula,
                        AulaCJ = a.AulaCJ,
                        PodeEditar = (usuarioLogado.EhProfessorCj() && a.AulaCJ || usuarioLogado.EhProfessorCjInfantil() && verificaCJPodeEditar)
                        || (!a.AulaCJ && (usuarioLogado.EhProfessor() || usuarioLogado.EhGestorEscolar() || usuarioLogado.EhProfessorPosl()
                        || usuarioLogado.EhProfessorPoed()) || usuarioLogado.EhProfessorPap() || usuarioLogado.EhProfessorPaee()),
                        ProfessorRf = a.ProfessorRf,
                        CriadoPor = a.CriadoPor,
                        PossuiFrequenciaRegistrada = a.PossuiFrequenciaRegistrada,
                        TipoAula = a.TipoAula
                    }).DistinctBy(a => a.Result.AulaId).Select(a => a.Result)
                });
        }

        private async Task DefinirComponentesProfessorCJ(Turma turma, Usuario usuarioLogado, List<(string codigo, string codigoComponentePai, string codigoTerritorioSaber)> componentesCurricularesDoProfessorCj)
        {
            var componentesCurricularesDoProfessorCJ = await mediator
               .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));

            if (componentesCurricularesDoProfessorCJ.Any())
            {
                var dadosComponentes = await mediator
                    .Send(new ObterDisciplinasPorIdsQuery(componentesCurricularesDoProfessorCJ.Select(c => c.DisciplinaId).ToArray()));

                if (dadosComponentes != null && dadosComponentes.Any())
                {
                    var professores = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turma.CodigoTurma));

                    dadosComponentes.ToList().ForEach(dc =>
                    {
                        var professor = dc.TerritorioSaber ? professores.FirstOrDefault(p => p.DisciplinasId.Contains(dc.CodigoComponenteCurricular)) : null;
                        if (professor != null && !String.IsNullOrEmpty(professor.ProfessorRf))
                        {
                            var componentesProfessorAtrelado = mediator
                                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turma.CodigoTurma, professor.ProfessorRf, Perfis.PERFIL_PROFESSOR)).Result;

                            var componenteProfessorAtreladoEquivalente = componentesProfessorAtrelado
                                .FirstOrDefault(c => c.CodigoComponenteTerritorioSaber.Equals(dc.CodigoComponenteCurricular));

                            componentesCurricularesDoProfessorCj
                                .Add((dc.CodigoComponenteCurricular.ToString(), dc.CdComponenteCurricularPai?.ToString(), componenteProfessorAtreladoEquivalente?.Codigo.ToString() ?? dc.CodigoComponenteCurricular.ToString()));
                        }
                        else
                            componentesCurricularesDoProfessorCj.Add((dc.CodigoComponenteCurricular.ToString(), dc.CdComponenteCurricularPai?.ToString(), dc.TerritorioSaber ? dc.CodigoComponenteCurricular.ToString() : "0"));
                    });
                }
            }
        }

        public async Task<bool> VerificaCJPodeEditarRegistroTitular(int anoLetivo)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.CJInfantilPodeEditarAulaTitular, anoLetivo));

            return dadosParametro?.Ativo ?? false;
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
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            return turma;
        }

        private async Task<IEnumerable<DataAulasProfessorDto>> ObterAulasNosPeriodos(IEnumerable<PeriodoEscolar> periodosEscolares, int anoLetivo, string turmaCodigo, string[] componenteCurricularCodigo, string professorRf)
        {
            var listaDataAulas = new List<DataAulasProfessorDto>();

            var aulas = await repositorioConsulta
                .ObterDatasDeAulasPorAnoTurmaEDisciplinaVerificandoSePossuiFrequenciaAulaRegistrada(periodosEscolares.Select(s => s.Id).Distinct(), anoLetivo, turmaCodigo, componenteCurricularCodigo, professorRf, null, null, false);

            foreach (var periodoEscolar in periodosEscolares)
            {
                listaDataAulas.AddRange(aulas.Distinct().Select(aula => new DataAulasProfessorDto
                {
                    Data = aula.DataAula,
                    IdAula = aula.Id,
                    AulaCJ = aula.AulaCJ,
                    Bimestre = periodoEscolar.Bimestre,
                    ProfessorRf = aula.ProfessorRf,
                    CriadoPor = aula.CriadoPor,
                    TipoAula = aula.TipoAula,
                    PossuiFrequenciaRegistrada = aula.PossuiFrequenciaRegistrada
                }));
            }

            return listaDataAulas;
        }

    }
}
