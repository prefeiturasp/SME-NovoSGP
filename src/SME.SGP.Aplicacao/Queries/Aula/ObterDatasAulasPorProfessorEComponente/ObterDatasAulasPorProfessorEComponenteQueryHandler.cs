using MailKit;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
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
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance, cancellationToken);
            var componenteCurricularId = long.Parse(request.ComponenteCurricularCodigo);

            var componentesCurricularesEolProfessor = (await mediator
                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.TurmaCodigo,
                                                                              usuarioLogado.Login,
                                                                              usuarioLogado.PerfilAtual,
                                                                              turma.EhTurmaInfantil), cancellationToken)).ToList();

            if (usuarioLogado.EhProfessorCj())
            {
                var componentesCurricularesDoProfessorCJ = (await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login), cancellationToken))
                    .Where(cc => cc.TurmaId == turma.CodigoTurma);

                if (componentesCurricularesDoProfessorCJ.Any())
                {
                    componentesCurricularesDoProfessorCJ.ToList().ForEach(async ccj =>
                    {
                        var componenteListaProfessor = componentesCurricularesEolProfessor
                            .Any(ccp => ccp.Codigo == ccj.DisciplinaId || ccp.CodigoComponenteTerritorioSaber == ccj.DisciplinaId);

                        var codigoComponenteEquivalente = (long?)null;

                        if (!componenteListaProfessor)
                        {
                            codigoComponenteEquivalente = ccj.Modalidade == Modalidade.EducacaoInfantil ?
                                    (mediator.Send(new ObterComponenteCurricularPorIdQuery(ccj.DisciplinaId)).Result)?.CdComponenteCurricularPai : null;                               
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

            if (componenteCurricularId.EhIdComponenteCurricularTerritorioSaberAgrupado())
                componentesCurricularesEolProfessor.AddRange(await mediator.Send(new ObterComponentesTerritorioAgrupamentoCorrelacionadosQuery(new long[] { componenteCurricularId })));


            // códigos disciplinas normais + regência + território
            var codigosComponentesUsuario = componentesCurricularesEolProfessor.Select(c => c.Codigo.ToString())
                .Concat(componentesCurricularesEolProfessor.Where(c => c.Regencia && c.CodigoComponenteCurricularPai.HasValue && c.CodigoComponenteCurricularPai.Value > 0).Select(c => c.CodigoComponenteCurricularPai.Value.ToString()))
                .Concat(componentesCurricularesEolProfessor.Where(c => c.TerritorioSaber).Select(c => c.CodigoComponenteTerritorioSaber.ToString()))
                .Distinct().ToArray();

            var datasAulas = await ObterAulasNosPeriodos(periodosEscolares, turma.AnoLetivo, turma.CodigoTurma, codigosComponentesUsuario);

            if (datasAulas.EhNulo() || !datasAulas.Any())
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
                aulasPermitidas = usuarioLogado
                    .ObterAulasQuePodeVisualizar(aulas, componentesCurricularesEolProfessor).Select(a => a.Id).ToList();
            }

            return datasAulas.Where(da => aulasPermitidas.Contains(da.IdAula)).GroupBy(g => g.Data.Date)
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

        public async Task<bool> VerificaCJPodeEditarRegistroTitular(int anoLetivo)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.CJInfantilPodeEditarAulaTitular, anoLetivo));

            return dadosParametro?.Ativo ?? false;
        }

        private async Task<IEnumerable<PeriodoEscolar>> ObterPeriodosEscolares(long tipoCalendarioId)
        {
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendarioId));
            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
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
            if (turma.EhNulo())
                throw new NegocioException("Turma não encontrada");

            return turma;
        }

        private async Task<IEnumerable<DataAulasProfessorDto>> ObterAulasNosPeriodos(IEnumerable<PeriodoEscolar> periodosEscolares, int anoLetivo, string turmaCodigo, string[] componenteCurricularCodigo)
        {
            var listaDataAulas = new List<DataAulasProfessorDto>();

            var aulas = await repositorioConsulta
                .ObterDatasDeAulasPorAnoTurmaEDisciplinaVerificandoSePossuiFrequenciaAulaRegistrada(periodosEscolares.Select(s => s.Id).Distinct(), anoLetivo, turmaCodigo, componenteCurricularCodigo, null, null, null, false);

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
