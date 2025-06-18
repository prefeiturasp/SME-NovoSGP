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
    public class ObterPlanoAulasPorTurmaEComponentePeriodoQueryHandler : IRequestHandler<ObterPlanoAulasPorTurmaEComponentePeriodoQuery, IEnumerable<PlanoAulaRetornoDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterPlanoAulasPorTurmaEComponentePeriodoQueryHandler(IMediator mediator, IRepositorioAulaConsulta repositorioAula)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<PlanoAulaRetornoDto>> Handle(ObterPlanoAulasPorTurmaEComponentePeriodoQuery request, CancellationToken cancellationToken)
        {
            var turma = await ObterTurma(request.TurmaCodigo);

            var tipoCalendarioId = await ObterTipoCalendario(turma);

            var periodosEscolares = await ObterPeriodosEscolares(tipoCalendarioId);

            var periodosEscolaresAulasInicioFim = periodosEscolares.Where(w => w.DataDentroPeriodo(request.AulaInicio) && w.DataDentroPeriodo(request.AulaFim));

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var ehProfessor = usuarioLogado.EhProfessor() || usuarioLogado.EhProfessorCj();

            var codigoRf = ehProfessor ? usuarioLogado.CodigoRf : string.Empty;

            var temPlanoAnual = await ValidarPlanoAnual(turma, periodosEscolaresAulasInicioFim, request.ComponenteCurricularId, usuarioLogado);

            var componente = await mediator.Send(new ObterComponenteCurricularPorIdQuery(long.Parse(request.ComponenteCurricularId)));
            var componenteCurricularCodigo = !string.IsNullOrWhiteSpace(request.ComponenteCurricularCodigo) ? long.Parse(request.ComponenteCurricularCodigo) : 0;

            var aulas = await repositorioAula.ObterAulasPorDataPeriodo(request.AulaInicio, request.AulaFim, turma.CodigoTurma, new string[] { componenteCurricularCodigo > 0 ? componenteCurricularCodigo.ToString() : request.ComponenteCurricularId }, usuarioLogado.EhProfessorCj());

            var bimestre = periodosEscolaresAulasInicioFim.FirstOrDefault().Bimestre;

            var validaObjetivos = !usuarioLogado.EhSomenteProfessorCj() && turma.ModalidadeCodigo != Modalidade.EJA && turma.ModalidadeCodigo != Modalidade.Medio;

            var planoAulaDto = await MapearParaDto(aulas, temPlanoAnual, validaObjetivos, bimestre, request, turma);

            return planoAulaDto.OrderBy(x => x.DataAula);
        }

        private async Task<bool> ValidarPlanoAnual(Turma turma, IEnumerable<PeriodoEscolar> periodosEscolaresAulasInicioFim, string ComponenteCurricularId, Usuario usuarioLogado)
        {
            DisciplinaDto disciplinaDto = null;
            var temPlanoAnual = new List<long>();

            if (!string.IsNullOrEmpty(ComponenteCurricularId))
            {
                var disciplinasRetorno = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new long[] { long.Parse(ComponenteCurricularId) }, codigoTurma: turma.CodigoTurma));
                disciplinaDto = disciplinasRetorno.FirstOrDefault();
            }

            var codigoComponente = long.Parse(ComponenteCurricularId);

            if (disciplinaDto != null)
                codigoComponente = disciplinaDto?.Id > 0 ? disciplinaDto.Id : disciplinaDto.CodigoComponenteCurricular;

            foreach (var periodoEscolar in periodosEscolaresAulasInicioFim)
            {
                var planejamentoAnualPeriodoId = await mediator.Send(new ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery(turma.Id, periodoEscolar.Id, codigoComponente));

                temPlanoAnual.Add(planejamentoAnualPeriodoId);

                if (planejamentoAnualPeriodoId == 0
                    && turma.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year
                    && !usuarioLogado.PerfilAtual.Equals(Perfis.PERFIL_CJ)
                    && !(disciplinaDto != null && disciplinaDto.TerritorioSaber))
                    throw new NegocioException("Não foi possível carregar o plano de aula porque não há plano anual cadastrado");
            }
            return temPlanoAnual.Any(s => s > 0);
        }

        private async Task<IEnumerable<PlanoAulaRetornoDto>> MapearParaDto(
                                                        IEnumerable<Aula> aulas, 
                                                        bool temPlanoAnual,
                                                        bool validaObjetivos, 
                                                        int bimestre,
                                                        ObterPlanoAulasPorTurmaEComponentePeriodoQuery filtro,
                                                        Turma turma)
        {
            var planosAulaRetorno = new List<PlanoAulaRetornoDto>();
            var planoAulas = await mediator.Send(new ObterPlanosAulaEObjetivosAprendizagemQuery(aulas.Select(s => s.Id)));
            var ue = await mediator.Send(new ObterUePorIdQuery(turma.UeId));
            //var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));
            var ehRegencia = (await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new long[] { long.Parse(filtro.ComponenteCurricularId) }, codigoTurma: turma.CodigoTurma))).FirstOrDefault().Regencia;

            var disciplinaId = (planoAulas.NaoEhNulo() && planoAulas.Any()) ? long.Parse(planoAulas.FirstOrDefault().DisciplinaId) : 0;
            var objetivosAprendizagemComponente = validaObjetivos ? await mediator.Send(new ObterObjetivosPlanoDisciplinaQuery(bimestre,
                                                                                                                               turma.Id,
                                                                                                                               long.Parse(filtro.ComponenteCurricularId),
                                                                                                                               disciplinaId,
                                                                                                                               ehRegencia)) : null;

            var temObjetivosAprendizagemOpcionais = objetivosAprendizagemComponente.EhNulo() || objetivosAprendizagemComponente.Count() == 0;

            foreach (var plano in planoAulas)
            {
                var atividadeAvaliativa = await mediator.Send(new ObterAtividadeAvaliativaQuery(plano.DataAula.Date, filtro.ComponenteCurricularCodigo, plano.TurmaId, ue.CodigoUe));

                planosAulaRetorno.Add(new PlanoAulaRetornoDto()
                {
                    Id = plano.Id,
                    Descricao = plano.Descricao,
                    RecuperacaoAula = plano.RecuperacaoAula,
                    LicaoCasa = plano.LicaoCasa,
                    AulaId = plano.AulaId,
                    AulaCj = plano.AulaCj,
                    QtdAulas = plano.Quantidade,
                    DataAula = plano.DataAula,
                    Migrado = plano.Migrado,
                    CriadoEm = plano.CriadoEm,
                    CriadoPor = plano.CriadoPor,
                    CriadoRf = plano.CriadoRf,
                    AlteradoEm = plano.AlteradoEm,
                    AlteradoPor = plano.AlteradoPor,
                    AlteradoRf = plano.AlteradoRf,
                    PossuiPlanoAnual = temPlanoAnual,
                    ObjetivosAprendizagemComponente = plano.ObjetivosAprendizagemComponente,
                    ObjetivosAprendizagemOpcionais = temObjetivosAprendizagemOpcionais,
                    IdAtividadeAvaliativa = atividadeAvaliativa?.Id,
                    PodeLancarNota = atividadeAvaliativa.NaoEhNulo() && plano.DataAula.Date <= DateTimeExtension.HorarioBrasilia().Date,
                    EhReposicao = plano.TipoAula == (int)TipoAula.Reposicao
                });
            }

            var aulasSemPlanoAula = aulas.Where(a => !planoAulas.Select(b => b.AulaId).Contains(a.Id)).Select(aula => aula);

            planosAulaRetorno.AddRange((from aula in aulasSemPlanoAula
                                        select new PlanoAulaRetornoDto()
                                        {
                                            DataAula = aula.DataAula,
                                            AulaId = aula.Id,
                                            QtdAulas = aula.Quantidade,
                                            AulaCj = aula.AulaCJ,
                                            EhReposicao = aula.EhReposicao(),
                                            ObjetivosAprendizagemOpcionais = temObjetivosAprendizagemOpcionais,
                                        }));

            return planosAulaRetorno;
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
    }
}