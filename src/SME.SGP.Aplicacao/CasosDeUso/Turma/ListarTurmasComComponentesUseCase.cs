using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarTurmasComComponentesUseCase : ConsultasBase, IListarTurmasComComponentesUseCase
    {
        private readonly IMediator mediator;

        public ListarTurmasComComponentesUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator, IUnitOfWork unitOfWork) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<PaginacaoResultadoDto<TurmaComComponenteDto>> Executar(FiltroTurmaDto filtroTurmaDto)
        {
            var resultado = new PaginacaoResultadoDto<TurmaComComponenteDto>();
            int qtdeRegistros = Paginacao.QuantidadeRegistros;
            int qtdeRegistrosIgnorados = Paginacao.QuantidadeRegistrosIgnorados;

            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            var componentesCurricularesDoProfessorCJ = string.Empty;
            if (usuario.EhProfessorCj())
            {
                var atribuicoes = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(null, filtroTurmaDto.TurmaCodigo, string.Empty, 0, usuario.Login, string.Empty, true));
                componentesCurricularesDoProfessorCJ = String.Join(",", atribuicoes.Select(s => s.DisciplinaId.ToString()).Distinct());
            }

            var turmasPaginadas = await mediator.Send(new ObterTurmasComComponentesQuery(filtroTurmaDto.UeCodigo,
                                                                                         filtroTurmaDto.DreCodigo,
                                                                                         filtroTurmaDto.TurmaCodigo,
                                                                                         filtroTurmaDto.AnoLetivo,
                                                                                         qtdeRegistros,
                                                                                         qtdeRegistrosIgnorados,
                                                                                         filtroTurmaDto.Bimestre,
                                                                                         filtroTurmaDto.Modalidade.Value,
                                                                                         filtroTurmaDto.Semestre,
                                                                                         usuario.EhProfessor(),
                                                                                         usuario.CodigoRf,
                                                                                         filtroTurmaDto.ConsideraHistorico,
                                                                                         componentesCurricularesDoProfessorCJ));

            if (turmasPaginadas == null || !turmasPaginadas.Items.Any())
                return default;
            
            var componentesCodigos = turmasPaginadas.Items.Select(c => c.ComponenteCurricularCodigo).Distinct().ToArray();

            var componentesRetorno = await mediator.Send(new ObterDescricaoComponentesCurricularesPorIdsQuery(componentesCodigos));

            var componentes = MapearParaDtoComPaginacao(turmasPaginadas, componentesRetorno);

            return await MapearParaDtoComPendenciaPaginacao(componentes, filtroTurmaDto.Bimestre);
        }

        private PaginacaoResultadoDto<TurmaComComponenteDto> MapearParaDtoComPaginacao(PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto> turmasPaginadas, IEnumerable<ComponenteCurricularSimplesDto> listaComponentes)
        {
            if (turmasPaginadas == null)
                turmasPaginadas = new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>();

            return new PaginacaoResultadoDto<TurmaComComponenteDto>
            {
                Items = MapearEventosParaDto(turmasPaginadas.Items, listaComponentes),
                TotalPaginas = turmasPaginadas.TotalPaginas,
                TotalRegistros = turmasPaginadas.TotalRegistros
            };
        }

        private IEnumerable<TurmaComComponenteDto> MapearEventosParaDto(IEnumerable<RetornoConsultaListagemTurmaComponenteDto> items, IEnumerable<ComponenteCurricularSimplesDto> listaComponentes)
        {
            return items?
                .OrderBy(a => a.NomeTurma)
                .ThenBy(a => a.NomeComponenteCurricular)
                .Select(c => MapearParaDto(c, listaComponentes));
        }

        private TurmaComComponenteDto MapearParaDto(RetornoConsultaListagemTurmaComponenteDto turmas, IEnumerable<ComponenteCurricularSimplesDto> listaComponentes)
        {
            var nomeComponente = listaComponentes.FirstOrDefault(c => c.Id == turmas.ComponenteCurricularCodigo)?.Descricao ?? turmas.NomeComponenteCurricular;
            //var pende = ObtemPendenciasTurma(turmas.TurmaCodigo, turmas.ComponenteCurricularCodigo);
            return turmas == null ? null : new TurmaComComponenteDto
            {
                Id = turmas.Id,
                NomeTurma = turmas.NomeTurmaFormatado(nomeComponente),
                TurmaCodigo = turmas.TurmaCodigo,
                ComponenteCurricularCodigo = turmas.ComponenteCurricularCodigo,
                Turno = turmas.Turno.ObterNome()
            };
        }

        private async Task<PaginacaoResultadoDto<TurmaComComponenteDto>> MapearParaDtoComPendenciaPaginacao(PaginacaoResultadoDto<TurmaComComponenteDto> turmasComponentes, int bimestre)
        {
            foreach (var turmaComponente in turmasComponentes.Items)
            {
                var pendencias = await mediator.Send(new ObterIndicativoPendenciasAulasPorTipoQuery(turmaComponente.TurmaCodigo.ToString(), turmaComponente.ComponenteCurricularCodigo.ToString()));

                var pendenciaFechamento = await mediator.Send(new ObterIndicativoPendenciaFechamentoTurmaDisciplinaQuery(turmaComponente.TurmaCodigo, bimestre, turmaComponente.ComponenteCurricularCodigo));

                turmaComponente.PendenciaDiarioBordo = pendencias.PendenciaDiarioBordo;
                turmaComponente.PendenciaAvaliacoes = pendencias.PendenciaAvaliacoes;
                turmaComponente.PendenciaFrequencia = pendencias.PendenciaFrequencia;
                turmaComponente.PendenciaPlanoAula = pendencias.PendenciaPlanoAula;
                turmaComponente.PendenciaFechamento = pendenciaFechamento;
            }

            return turmasComponentes;
        }
    }
}
