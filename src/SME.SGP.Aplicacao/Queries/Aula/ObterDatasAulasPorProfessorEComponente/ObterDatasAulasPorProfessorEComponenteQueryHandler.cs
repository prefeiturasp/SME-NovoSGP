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
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var componenteCurricularId = long.Parse(request.ComponenteCurricularCodigo);

            var componentesCurricularesEolProfessor = (await mediator
                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.TurmaCodigo,
                                                                              usuarioLogado.Login,
                                                                              usuarioLogado.PerfilAtual,
                                                                              usuarioLogado.EhProfessorInfantilOuCjInfantil()))).ToList();

            if (usuarioLogado.EhProfessorCj())
            {
                var componentesCurricularesDoProfessorCJ = (await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login)))
                    .Where(cc => cc.TurmaId == turma.CodigoTurma);

                if (componentesCurricularesDoProfessorCJ.Any())
                {
                    componentesCurricularesDoProfessorCJ.ToList().ForEach(ccj =>
                    {
                        var componenteListaProfessor = componentesCurricularesEolProfessor
                            .Any(ccp => ccp.Codigo == ccj.DisciplinaId || ccp.CodigoComponenteTerritorioSaber == ccj.DisciplinaId);

                        /*(string codigoComponente, string professor)[] codigosTerritorioEquivalentes = null;*/
                        var codigoComponenteEquivalente = (long?)null;

                        if (!componenteListaProfessor)
                        {
                            /*codigosTerritorioEquivalentes = mediator
                                .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(ccj.DisciplinaId, turma.CodigoTurma, null)).Result;

                            var componentesConsiderados = codigosTerritorioEquivalentes != null && codigosTerritorioEquivalentes.Any() ? 
                                codigosTerritorioEquivalentes.Select(c => c.codigoComponente).Except(new string[] { ccj.DisciplinaId.ToString() }) : null;

                            if (componentesConsiderados != null && componentesConsiderados.Any())
                                codigoComponenteEquivalente = long.Parse(codigosTerritorioEquivalentes.First().codigoComponente);
                            else*/
                            {
                                codigoComponenteEquivalente = ccj.Modalidade == Modalidade.EducacaoInfantil ?
                                    (mediator.Send(new ObterComponenteCurricularPorIdQuery(ccj.DisciplinaId)).Result)?.CdComponenteCurricularPai : null;
                            }                                
                        }

                        componentesCurricularesEolProfessor.Add(new ComponenteCurricularEol()
                        {
                            Codigo = codigoComponenteEquivalente ?? ccj.DisciplinaId,
                            CodigoComponenteTerritorioSaber = codigoComponenteEquivalente.HasValue ? ccj.DisciplinaId : 0,
                            Professor = ccj.ProfessorRf
                        });
                    });
                }
            }

            componentesCurricularesEolProfessor = componentesCurricularesEolProfessor
                .Where(c => c.Codigo == componenteCurricularId || (c.CodigoComponenteCurricularPai.HasValue && c.CodigoComponenteCurricularPai.Value == componenteCurricularId) || c.CodigoComponenteTerritorioSaber == componenteCurricularId)
                .ToList();

            // códigos disciplinas normais + regência + território
            var codigosComponentesUsuario = componentesCurricularesEolProfessor.Select(c => c.Codigo.ToString())
                .Concat(componentesCurricularesEolProfessor.Where(c => c.Regencia && c.CodigoComponenteCurricularPai.HasValue && c.CodigoComponenteCurricularPai.Value > 0).Select(c => c.CodigoComponenteCurricularPai.Value.ToString()))
                .Concat(componentesCurricularesEolProfessor.Where(c => c.TerritorioSaber).Select(c => c.CodigoComponenteTerritorioSaber.ToString()))
                .Distinct().ToArray();

            var datasAulas = await ObterAulasNosPeriodos(periodosEscolares, turma.AnoLetivo, turma.CodigoTurma, codigosComponentesUsuario, componentesCurricularesEolProfessor.Any(c => c.TerritorioSaber) ? componentesCurricularesEolProfessor.First().Professor : null);

            if (datasAulas == null || !datasAulas.Any())
                return default;

            var ids = datasAulas.Select(a => a.IdAula).Distinct().ToList();
            var aulas = await mediator.Send(new ObterAulasPorIdsQuery(ids));

            var aulasPermitidas = new List<long>();
            bool verificaCJPodeEditar = await VerificaCJPodeEditarRegistroTitular(turma.AnoLetivo);

            if (usuarioLogado.EhProfessorCjInfantil() && verificaCJPodeEditar)
            {
                aulasPermitidas = aulas.Where(a => codigosComponentesUsuario.Contains(a.DisciplinaId))
                                       .Select(a => a.Id).ToList();
            }
            else
            {
                var codigosTerritorio = componentesCurricularesEolProfessor.Where(c => c.TerritorioSaber).Select(c => c.Codigo);
                /*var professoresDesconsiderados = usuarioLogado.EhProfessor() && codigosTerritorio.Any() ?
                    await mediator.Send(new ObterProfessoresAtribuidosPorCodigosComponentesTerritorioQuery(codigosTerritorio.ToArray(), turma.CodigoTurma, usuarioLogado.Login)) : null;*/

                aulasPermitidas = usuarioLogado
                    .ObterAulasQuePodeVisualizar(aulas, componentesCurricularesEolProfessor, null /*professoresDesconsiderados*/).Select(a => a.Id).ToList();
            }

            return datasAulas.Where(da => aulasPermitidas.Contains(da.IdAula)).GroupBy(g => g.Data)
                .Select(x => new DatasAulasDto()
                {
                    Data = x.Key,
                    Aulas = x.OrderBy(a => a.AulaCJ).Select(a => new AulaSimplesDto()
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
                    }).DistinctBy(a => a.AulaId).Select(a => a)
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

                    foreach (var dc in dadosComponentes.ToList())
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
                        else if (professor != null && String.IsNullOrEmpty(professor.ProfessorRf))
                        {
                            var componentesDaTurma = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(turma.CodigoTurma));

                            if (componentesDaTurma != null && componentesDaTurma.Any())
                            {
                                var componenteCorrespondente = componentesDaTurma.FirstOrDefault(c => c.CodigoComponenteTerritorioSaber == dc.CodigoComponenteCurricular);
                                if (componenteCorrespondente != null)
                                    componentesCurricularesDoProfessorCj.Add((componenteCorrespondente.CodigoComponenteTerritorioSaber.ToString(), componenteCorrespondente.CodigoComponenteCurricularPai?.ToString(),
                                        componenteCorrespondente.TerritorioSaber ? componenteCorrespondente.CodigoComponenteCurricular.ToString() : "0"));
                            }
                        }
                        else
                            componentesCurricularesDoProfessorCj.Add((dc.CodigoComponenteCurricular.ToString(), dc.CdComponenteCurricularPai?.ToString(), dc.TerritorioSaber ? dc.CodigoComponenteCurricular.ToString() : "0"));
                    }
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
