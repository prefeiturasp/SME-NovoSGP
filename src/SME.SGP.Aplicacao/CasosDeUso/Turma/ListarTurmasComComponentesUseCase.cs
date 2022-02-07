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

            IEnumerable<long> turmasAbrangencia = null;

            if (filtroTurmaDto.TurmaCodigo == null)
                turmasAbrangencia = await mediator.Send(new ObterCodigosTurmasAbrangenciaPorUeModalidadeAnoQuery(filtroTurmaDto.UeCodigo, filtroTurmaDto.Modalidade.Value, periodo: 0, filtroTurmaDto.ConsideraHistorico,
                                                                                     filtroTurmaDto.AnoLetivo, new int[] { }, desconsideraNovosAnosInfantil: false));

            var anosInfantilDesconsiderar = filtroTurmaDto.Modalidade.Value == Modalidade.EducacaoInfantil ? await mediator.Send(new ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(filtroTurmaDto.AnoLetivo, Modalidade.EducacaoInfantil)) : null;

            var turmasPaginadas = await mediator.Send(new ObterTurmasComComponentesQuery(filtroTurmaDto.UeCodigo,
                                                                                         filtroTurmaDto.DreCodigo,
                                                                                         filtroTurmaDto.TurmaCodigo,
                                                                                         filtroTurmaDto.AnoLetivo,
                                                                                         0,
                                                                                         qtdeRegistrosIgnorados,
                                                                                         filtroTurmaDto.Bimestre,
                                                                                         filtroTurmaDto.Modalidade.Value,
                                                                                         filtroTurmaDto.Semestre,
                                                                                         usuario.EhPerfilProfessor(),
                                                                                         usuario.CodigoRf,
                                                                                         filtroTurmaDto.ConsideraHistorico,
                                                                                         componentesCurricularesDoProfessorCJ,
                                                                                         periodoEscolar.FirstOrDefault().PeriodoInicio,
                                                                                         anosInfantilDesconsiderar != null ? String.Join(",", anosInfantilDesconsiderar) : string.Empty));

            if (turmasPaginadas == null || turmasPaginadas?.Items == null)
                return default;

            var codigosTurmaPaginada = turmasPaginadas.Items.Select(c => c.TurmaCodigo).Distinct().ToArray();
            var componentesCodigos = usuario.EhAdmGestao() ? codigosTurmaPaginada
                                     : turmasAbrangencia != null ? turmasAbrangencia.Select(c => c).ToArray().Intersect(codigosTurmaPaginada).ToArray()
                                     : codigosTurmaPaginada;

            if (turmasAbrangencia!=null)
            {
                var turmasItems = turmasPaginadas.Items.Where(o => turmasAbrangencia.Contains(o.TurmaCodigo));
                turmasPaginadas.Items = turmasItems; 
            }                       

            var componentesRetorno = await mediator.Send(new ObterDescricaoComponentesCurricularesPorIdsQuery(componentesCodigos));

            turmasPaginadas.TotalRegistros = turmasPaginadas.Items != null && turmasPaginadas.Items.Any() ? turmasPaginadas.Items.Count() : 0;
            turmasPaginadas.TotalPaginas = (int)Math.Ceiling((double)turmasPaginadas.TotalRegistros / qtdeRegistros);
            turmasPaginadas.Items = turmasPaginadas.Items.Skip(qtdeRegistrosIgnorados).Take(qtdeRegistros);

            turmasPaginadas = await MapearNomeFiltroTurmas(turmasPaginadas.Items.Select(c => c.TurmaCodigo.ToString()).Distinct().ToArray(),turmasPaginadas);
            var componentes = MapearParaDtoComPaginacao(turmasPaginadas, componentesRetorno);
            var retorno = await MapearParaDtoComPendenciaPaginacao(componentes, filtroTurmaDto.Bimestre, usuario, qtdeRegistros,qtdeRegistrosIgnorados);

            return retorno;
        }

        private async Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> MapearNomeFiltroTurmas(string[] turmasCodigos, PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto> turmasPaginadas)
        {
            var nomesFiltro = await mediator.Send(new ObterTurmasNomeFiltroPorTurmasCodigosQuery(turmasCodigos));
            if (nomesFiltro !=null && nomesFiltro.Any())
            {
                turmasPaginadas.Items.ToList().ForEach(item =>
                {
                    item.NomeFiltro = nomesFiltro?.FirstOrDefault(n => n.TurmaCodigo == item.TurmaCodigo.ToString()).NomeFiltro;
                    item.SerieEnsino = nomesFiltro?.FirstOrDefault(n => n.TurmaCodigo == item.TurmaCodigo.ToString()).SerieEnsino;
                }); 
            }

            return new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>
            {
                Items = turmasPaginadas.Items,
                TotalPaginas = turmasPaginadas.TotalPaginas,
                TotalRegistros = turmasPaginadas.TotalRegistros
            };
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
                NomeTurma = (turmas.Ano == null && turmas.SerieEnsino == null && turmas.NomeFiltro != null) ? turmas.NomeTurmaFiltroFormatado(): turmas.NomeTurmaFormatado(nomeComponente),
                TurmaCodigo = turmas.TurmaCodigo,
                ComponenteCurricularCodigo = turmas.TerritorioSaber ? turmas.ComponenteCurricularTerritorioSaberCodigo : turmas.ComponenteCurricularCodigo,
                Turno = turmas.Turno.ObterNome()
            };
        }

        private async Task<PaginacaoResultadoDto<TurmaComComponenteDto>> MapearParaDtoComPendenciaPaginacao(PaginacaoResultadoDto<TurmaComComponenteDto> turmasComponentes, int bimestre, Usuario usuario, int qtdeRegistros, int qtdeRegistrosIgnorados)
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

            var totalRegistros = itensComPendencias.Any() ? itensComPendencias.Count() : 0;

            return new PaginacaoResultadoDto<TurmaComComponenteDto>
            {
                Items = itensComPendencias,
                TotalPaginas = turmasComponentes.TotalPaginas,
                TotalRegistros = turmasComponentes.TotalRegistros
            };
        }
    }
}
