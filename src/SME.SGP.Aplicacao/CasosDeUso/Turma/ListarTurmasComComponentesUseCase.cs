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

        public ListarTurmasComComponentesUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
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

            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(filtroTurmaDto.Modalidade.Value, filtroTurmaDto.AnoLetivo, 1));

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
                                                                                         componentesCurricularesDoProfessorCJ,
                                                                                         periodoEscolar.FirstOrDefault().PeriodoInicio));

            if (turmasPaginadas == null || !turmasPaginadas.Items.Any())
                return default;
            
            var componentesCodigos = turmasPaginadas.Items.Select(c => c.ComponenteCurricularCodigo).Distinct().ToArray();

            var componentesRetorno = await mediator.Send(new ObterDescricaoComponentesCurricularesPorIdsQuery(componentesCodigos));

            var componentes = MapearParaDtoComPaginacao(turmasPaginadas, componentesRetorno);

            return await MapearParaDtoComPendenciaPaginacao(componentes, filtroTurmaDto.Bimestre, usuario);
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
            return turmas == null ? null : new TurmaComComponenteDto
            {
                Id = turmas.Id,
                NomeTurma = turmas.NomeTurmaFormatado(nomeComponente),
                TurmaCodigo = turmas.TurmaCodigo,
                ComponenteCurricularCodigo = turmas.ComponenteCurricularCodigo,
                Turno = turmas.Turno.ObterNome()
            };
        }

        private async Task<PaginacaoResultadoDto<TurmaComComponenteDto>> MapearParaDtoComPendenciaPaginacao(PaginacaoResultadoDto<TurmaComComponenteDto> turmasComponentes, int bimestre,Usuario usuario)
        {
            List<TurmaComComponenteDto> itensComPendencias = new List<TurmaComComponenteDto>();

            foreach (var turmaCodigo in turmasComponentes.Items.GroupBy(a => a.TurmaCodigo))
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo.Key.ToString()));

                var ehTurmaInfantil = turma.EhTurmaInfantil;

                var periodoFechamentoIniciado = !ehTurmaInfantil && !usuario.EhProfessorCj() &&
                    await mediator.Send(new PeriodoFechamentoTurmaIniciadoQuery(turma, bimestre, DateTime.Today));

                foreach (var turmaComponente in turmaCodigo)
                {
                    var pendencias = await mediator.Send(new ObterIndicativoPendenciasAulasPorTipoQuery(turmaComponente.ComponenteCurricularCodigo.ToString(),
                                                                                                        turma.CodigoTurma,
                                                                                                        bimestre,
                                                                                                        verificaDiarioBordo: ehTurmaInfantil && !usuario.EhProfessorCjInfantil(),
                                                                                                        verificaAvaliacao: !ehTurmaInfantil,
                                                                                                        verificaPlanoAula: !ehTurmaInfantil,
                                                                                                        verificaFrequencia: !ehTurmaInfantil || !usuario.EhProfessorCjInfantil(),
                                                                                                        professorCj: usuario.EhProfessorCj(),
                                                                                                        professorNaoCj: usuario.EhProfessor(),
                                                                                                        professorRf: usuario.CodigoRf));

                    var possuiFechamento = periodoFechamentoIniciado &&
                        await mediator.Send(new ObterIndicativoPendenciaFechamentoTurmaDisciplinaQuery(turma.Id,
                                                                                                        bimestre,
                                                                                                        turmaComponente.ComponenteCurricularCodigo));

                    turmaComponente.PendenciaDiarioBordo = pendencias.PendenciaDiarioBordo;
                    turmaComponente.PendenciaAvaliacoes = pendencias.PendenciaAvaliacoes;
                    turmaComponente.PendenciaFrequencia = pendencias.PendenciaFrequencia;
                    turmaComponente.PendenciaPlanoAula = pendencias.PendenciaPlanoAula;
                    turmaComponente.PendenciaFechamento = periodoFechamentoIniciado && !possuiFechamento;
                    turmaComponente.PeriodoFechamentoIniciado = periodoFechamentoIniciado;

                    itensComPendencias.Add(turmaComponente);
                }
            }

            return new PaginacaoResultadoDto<TurmaComComponenteDto>
            {
                Items = itensComPendencias,
                TotalPaginas = turmasComponentes.TotalPaginas,
                TotalRegistros = turmasComponentes.TotalRegistros
            };
        }
    }
}
