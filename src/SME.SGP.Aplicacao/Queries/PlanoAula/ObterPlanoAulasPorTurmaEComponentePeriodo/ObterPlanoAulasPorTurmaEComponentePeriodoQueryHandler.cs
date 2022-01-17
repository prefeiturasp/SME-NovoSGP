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
        private readonly IRepositorioAula repositorioAula;

        public ObterPlanoAulasPorTurmaEComponentePeriodoQueryHandler(IMediator mediator, IRepositorioAula repositorioAula)
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

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var ehProfessor = usuarioLogado.EhProfessor() || usuarioLogado.EhProfessorCj();

            var codigoRf = ehProfessor ? usuarioLogado.CodigoRf : string.Empty;

            var temPlanoAnual = await ValidarPlanoAnual(turma, periodosEscolaresAulasInicioFim, request.ComponenteCurricularId, usuarioLogado);

            var aulas = ObterAulas(request, turma, periodosEscolaresAulasInicioFim, usuarioLogado, codigoRf,usuarioLogado.EhProfessorCj());

            var planoAulas = await mediator.Send(new ObterPlanosAulaEObjetivosAprendizagemQuery(aulas.Select(s=> s.Id)));

            var planoAulaDto = await MapearParaDto(planoAulas, aulas, temPlanoAnual, request.ComponenteCurricularCodigo, turma.UeId);            

            return planoAulaDto;
        }

        private async Task<bool> ValidarPlanoAnual(Turma turma, IEnumerable<PeriodoEscolar> periodosEscolaresAulasInicioFim, string ComponenteCurricularId, Usuario usuarioLogado)
        {
            DisciplinaDto disciplinaDto = null;
            var temPlanoAnual = new List<long>();

            if (!string.IsNullOrEmpty(ComponenteCurricularId))
            {
                var disciplinasRetorno = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { long.Parse(ComponenteCurricularId) }));
                disciplinaDto = disciplinasRetorno.SingleOrDefault();
            }

            foreach (var periodoEscolar in periodosEscolaresAulasInicioFim)
            {
                var planejamentoAnualPeriodoId = await mediator.Send(new ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery(turma.Id, periodoEscolar.Id, disciplinaDto != null ? disciplinaDto.Id : long.Parse(ComponenteCurricularId)));

                temPlanoAnual.Add(planejamentoAnualPeriodoId);

                if (planejamentoAnualPeriodoId == 0
                    && turma.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year
                    && !usuarioLogado.PerfilAtual.Equals(Perfis.PERFIL_CJ)
                    && !(disciplinaDto != null && disciplinaDto.TerritorioSaber))
                    throw new NegocioException("Não foi possível carregar o plano de aula porque não há plano anual cadastrado");
            }
            return temPlanoAnual.Any(s => s > 0);
        }

        private async Task<IEnumerable<PlanoAulaRetornoDto>> MapearParaDto(IEnumerable<PlanoAulaObjetivosAprendizagemDto> planoAulas, IEnumerable<Aula> aulas, bool temPlanoAnual, string componenteCurricularCodigo, long ueId)
        {
            var planosAulaRetorno = new List<PlanoAulaRetornoDto>();

            var ue = await mediator.Send(new ObterUePorIdQuery(ueId));

            foreach (var plano in planoAulas)
            {
                var atividadeAvaliativa = await mediator.Send(new ObterAtividadeAvaliativaQuery(plano.DataAula.Date, componenteCurricularCodigo, plano.TurmaId, ue.CodigoUe));

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
                    IdAtividadeAvaliativa = atividadeAvaliativa?.Id,
                    PodeLancarNota = atividadeAvaliativa != null && plano.DataAula.Date <= DateTimeExtension.HorarioBrasilia().Date,                    
                    EhReposicao = plano.TipoAula == (int)TipoAula.Reposicao
                });
            }            

            var aulasSemPlanoAula = aulas.Where(a => !planoAulas.Select(b => b.DataAula).Contains(a.DataAula)).Select(aula=> aula);

            planosAulaRetorno.AddRange((from aula in aulasSemPlanoAula select new PlanoAulaRetornoDto()
            { 
                DataAula = aula.DataAula,
                AulaId = aula.Id,
                QtdAulas = aula.Quantidade,
                EhReposicao = aula.EhReposicao()
            }));

            return planosAulaRetorno;
        }

        private PlanoAulaRetornoDto MapearParaDto(PlanoAulaObjetivosAprendizagemDto plano) =>
            plano == null ? null :
            new PlanoAulaRetornoDto()
            {
                Id = plano.Id,
                Descricao = plano.Descricao,
                RecuperacaoAula = plano.RecuperacaoAula,
                LicaoCasa = plano.LicaoCasa,
                AulaId = plano.AulaId,
                QtdAulas = plano.Quantidade,

                Migrado = plano.Migrado,
                CriadoEm = plano.CriadoEm,
                CriadoPor = plano.CriadoPor,
                CriadoRf = plano.CriadoRf,
                AlteradoEm = plano.AlteradoEm,
                AlteradoPor = plano.AlteradoPor,
                AlteradoRf = plano.AlteradoRf,

                ObjetivosAprendizagemComponente = plano.ObjetivosAprendizagemComponente
            };

        private IEnumerable<Aula> ObterAulas(ObterPlanoAulasPorTurmaEComponentePeriodoQuery request, Turma turma, IEnumerable<PeriodoEscolar> periodosEscolaresAulasInicioFim, Usuario usuarioLogado, string codigoRf,bool aulaCj)
        {
            var datasAulas = ObterAulasNosPeriodos(periodosEscolaresAulasInicioFim, turma.AnoLetivo, turma.CodigoTurma, request.ComponenteCurricularCodigo, codigoRf, request.AulaInicio, request.AulaFim,aulaCj);

            var aulasPermitidas = usuarioLogado.ObterAulasQuePodeVisualizar(datasAulas, new string[] { request.ComponenteCurricularCodigo }).Select(a => a.Id);

            return ObterAulasSelecionadas(usuarioLogado, datasAulas, aulasPermitidas);
        }

        private IEnumerable<Aula> ObterAulasSelecionadas(Usuario usuarioLogado, IEnumerable<Aula> datasAulas, IEnumerable<long> aulasPermitidas)
        {
            return datasAulas.Where(da => aulasPermitidas.Contains(da.Id)).Select(s => s);
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

        private IEnumerable<Aula> ObterAulasNosPeriodos(IEnumerable<PeriodoEscolar> periodosEscolares, int anoLetivo, string turmaCodigo, string componenteCurricularCodigo, string usuarioRf, DateTime aulaInicio, DateTime aulaFim,bool aulaCj)
        {
            var lstPeriodosEscolaresIds = periodosEscolares.Select(s=> s.Id).Distinct();

            var lstAulas = repositorioAula.ObterDatasDeAulasPorAnoTurmaEDisciplina(lstPeriodosEscolaresIds, anoLetivo, turmaCodigo, componenteCurricularCodigo, usuarioRf, aulaInicio, aulaFim,aulaCj);

            return lstAulas;
        }

    }
}