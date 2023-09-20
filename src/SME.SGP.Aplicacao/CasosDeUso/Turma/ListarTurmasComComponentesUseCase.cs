using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
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
        private readonly IConsultasDisciplina consultasDisciplina;

        public ListarTurmasComComponentesUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator, IConsultasDisciplina consultasDisciplina) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
        }
        public async Task<PaginacaoResultadoDto<TurmaComComponenteDto>> Executar(FiltroTurmaDto filtroTurmaDto)
        {
            var resultado = new PaginacaoResultadoDto<TurmaComComponenteDto>();
            int qtdeRegistros = Paginacao.QuantidadeRegistros;
            int qtdeRegistrosIgnorados = Paginacao.QuantidadeRegistrosIgnorados;

            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var componentesCurricularesDoProfessorCJ = string.Empty;
            if (usuario.EhProfessorCj())
            {
                var atribuicoes = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(null, filtroTurmaDto.TurmaCodigo, string.Empty, 0, usuario.Login, string.Empty, true));
                componentesCurricularesDoProfessorCJ = String.Join(",", atribuicoes.Select(s => s.DisciplinaId.ToString()).Distinct());
            }

            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(filtroTurmaDto.Modalidade.Value, filtroTurmaDto.AnoLetivo, 1));

            IEnumerable<long> turmasAbrangencia = null;

            if (filtroTurmaDto.TurmaCodigo.EhNulo())
                turmasAbrangencia = await mediator.Send(new ObterCodigosTurmasAbrangenciaPorUeModalidadeAnoQuery(filtroTurmaDto.UeCodigo, filtroTurmaDto.Modalidade.Value, periodo: 0, filtroTurmaDto.ConsideraHistorico,
                                                                                     filtroTurmaDto.AnoLetivo, new int[] { }, desconsideraNovosAnosInfantil: false));
            else
                turmasAbrangencia = new long[] { long.Parse(filtroTurmaDto.TurmaCodigo) };

            var anosInfantilDesconsiderar = filtroTurmaDto.Modalidade.Value == Modalidade.EducacaoInfantil ? await mediator.Send(new ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(filtroTurmaDto.AnoLetivo, Modalidade.EducacaoInfantil)) : null;

            var turmasPaginadas = new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>();
            var listRetorno = new List<RetornoConsultaListagemTurmaComponenteDto>();

            if (usuario.EhProfessorCj())
            {
                var disciplinas = await consultasDisciplina.ObterDisciplinasPerfilCJ(filtroTurmaDto.TurmaCodigo, usuario.CodigoRf);

                if (filtroTurmaDto.TurmaCodigo.NaoEhNulo())
                {
                    var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(filtroTurmaDto.TurmaCodigo));

                    turmasPaginadas.Items = disciplinas.ToList().Select(d => new RetornoConsultaListagemTurmaComponenteDto()
                    {
                        TurmaCodigo = long.Parse(filtroTurmaDto.TurmaCodigo),
                        Modalidade = filtroTurmaDto.Modalidade.Value,
                        NomeTurma = turma.Nome,
                        NomeFiltro = turma.NomeFiltro,
                        Ano = turma.Ano,
                        ComplementoTurmaEJA = turma.EhEJA() ? turma.SerieEnsino : string.Empty,
                        NomeComponenteCurricular = string.IsNullOrEmpty(d.NomeComponenteInfantil) ? d.Nome : d.NomeComponenteInfantil,
                        ComponenteCurricularCodigo = d.CodigoComponenteCurricular,
                        ComponenteCurricularTerritorioSaberCodigo = d.CodigoComponenteTerritorioSaber ?? 0,
                        Turno = (TipoTurnoEOL)turma.TipoTurno,
                        TerritorioSaber = d.TerritorioSaber,
                        Id = d.Id
                    });
                }
                else
                {
                    if (turmasAbrangencia.Any() || turmasAbrangencia.NaoEhNulo())
                        foreach (var turmaAbrangencia in turmasAbrangencia)
                        {
                            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaAbrangencia.ToString()));

                            turmasPaginadas.Items = disciplinas.ToList().Select(d => new RetornoConsultaListagemTurmaComponenteDto()
                            {
                                TurmaCodigo = long.Parse(turmaAbrangencia.ToString()),
                                Modalidade = filtroTurmaDto.Modalidade.Value,
                                NomeTurma = turma.Nome,
                                NomeFiltro = turma.NomeFiltro,
                                Ano = turma.Ano,
                                ComplementoTurmaEJA = turma.EhEJA() ? turma.SerieEnsino : string.Empty,
                                NomeComponenteCurricular = string.IsNullOrEmpty(d.NomeComponenteInfantil) ? d.Nome : d.NomeComponenteInfantil,
                                ComponenteCurricularCodigo = d.CodigoComponenteCurricular,
                                ComponenteCurricularTerritorioSaberCodigo = d.CodigoComponenteTerritorioSaber ?? 0,
                                Turno = (TipoTurnoEOL)turma.TipoTurno,
                            });

                            listRetorno.AddRange(turmasPaginadas.Items);
                        }
                    turmasPaginadas.Items = listRetorno;
                }
            }
            else if(!filtroTurmaDto.ConsideraHistorico)
            {
                turmasPaginadas = await mediator.Send(new ObterTurmasComComponentesQuery(filtroTurmaDto.UeCodigo,
                                                                                             filtroTurmaDto.DreCodigo,
                                                                                             filtroTurmaDto.TurmaCodigo,
                                                                                             filtroTurmaDto.AnoLetivo,
                                                                                             qtdeRegistros,
                                                                                             qtdeRegistrosIgnorados,
                                                                                             filtroTurmaDto.Bimestre,
                                                                                             filtroTurmaDto.Modalidade.Value,
                                                                                             filtroTurmaDto.Semestre,
                                                                                             usuario.EhPerfilProfessor(),
                                                                                             usuario.CodigoRf,
                                                                                             filtroTurmaDto.ConsideraHistorico,
                                                                                             filtroTurmaDto.Bimestre > 0 ?
                                                                                                periodoEscolar.Where(p => p.Bimestre == (filtroTurmaDto.Bimestre)).FirstOrDefault().PeriodoInicio :
                                                                                                periodoEscolar.FirstOrDefault().PeriodoInicio,
                                                                                             anosInfantilDesconsiderar.NaoEhNulo() ? String.Join(",", anosInfantilDesconsiderar) : string.Empty));
            }
            else
            {
                if (turmasAbrangencia.Any() || turmasAbrangencia != null)
                    foreach (var turmaAbrangencia in turmasAbrangencia)
                    {
                        var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaAbrangencia.ToString()));
                        var disciplinas = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(turma.CodigoTurma));
                        turmasPaginadas.Items = disciplinas.ToList().Select(d => new RetornoConsultaListagemTurmaComponenteDto()
                        {
                            TurmaCodigo = long.Parse(turmaAbrangencia.ToString()),
                            Modalidade = filtroTurmaDto.Modalidade.Value,
                            NomeTurma = turma.Nome,
                            NomeFiltro = turma.NomeFiltro,
                            Ano = turma.Ano,
                            ComplementoTurmaEJA = turma.EhEJA() ? turma.SerieEnsino : string.Empty,
                            NomeComponenteCurricular = string.IsNullOrEmpty(d.NomeComponenteInfantil) ? d.Nome : d.NomeComponenteInfantil,
                            ComponenteCurricularCodigo = d.TerritorioSaber ? d.CodigoComponenteTerritorioSaber.Value : d.CodigoComponenteCurricular,
                            Turno = (TipoTurnoEOL)turma.TipoTurno,
                        });

                        listRetorno.AddRange(turmasPaginadas.Items);
                    }
                turmasPaginadas.Items = listRetorno;
            }

            if (turmasPaginadas.EhNulo() || (turmasPaginadas?.Items).EhNulo() || !turmasPaginadas.Items.Any())
                return default;

            var codigosTurmaPaginada = turmasPaginadas.Items.Select(c => c.TurmaCodigo).Distinct().ToArray();
            var codigosTurmasComponente = usuario.EhAdmGestao() ? codigosTurmaPaginada
                                     : turmasAbrangencia.NaoEhNulo() ? turmasAbrangencia.Select(c => c).ToArray().Intersect(codigosTurmaPaginada).ToArray()
                                     : codigosTurmaPaginada;

            var retornoComponentesTurma = from item in turmasPaginadas.Items.ToList()
                                          join componenteCodigo in codigosTurmasComponente on item.TurmaCodigo equals componenteCodigo
                                          select item.TerritorioSaber && item.Id.HasValue && item.Id.Value > 0 ? item.Id.Value : item.ComponenteCurricularCodigo;

            if (turmasAbrangencia.NaoEhNulo())
            {
                var turmasItems = turmasPaginadas.Items.Where(o => turmasAbrangencia.Contains(o.TurmaCodigo));
                var turmasAgrupadas = turmasItems.GroupBy(x => x.TurmaCodigo);
                var turmasItemsFiltrados = new List<RetornoConsultaListagemTurmaComponenteDto>();
                foreach (var turmas in turmasAgrupadas)
                {
                    turmasItemsFiltrados.AddRange(turmas);
                }
                turmasPaginadas.Items = turmasItemsFiltrados;
            }

            if (filtroTurmaDto.Modalidade == Modalidade.EducacaoInfantil)
                turmasPaginadas = await VerificarAgrupamentoRegencia(turmasPaginadas);

            var componentesRetorno = await mediator.Send(new ObterComponentesCurricularesSimplesPorIdsQuery(retornoComponentesTurma.ToArray()));

            turmasPaginadas.TotalRegistros = turmasPaginadas.Items.NaoEhNulo() && turmasPaginadas.Items.Any() ? turmasPaginadas.Items.Count() : 0;
            turmasPaginadas.TotalPaginas = (int)Math.Ceiling((double)turmasPaginadas.TotalRegistros / qtdeRegistros);
            turmasPaginadas.Items = turmasPaginadas.Items.Skip(qtdeRegistrosIgnorados).Take(qtdeRegistros);

            turmasPaginadas = await MapearNomeFiltroTurmas(turmasPaginadas.Items.Select(c => c.TurmaCodigo.ToString()).Distinct().ToArray(), turmasPaginadas);
            var componentes = MapearParaDtoComPaginacao(turmasPaginadas, componentesRetorno);
            var retorno = await MapearParaDtoComPendenciaPaginacao(componentes, filtroTurmaDto.AnoLetivo, filtroTurmaDto.Bimestre, usuario);

            return retorno;
        }

        private async Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> VerificarAgrupamentoRegencia(PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto> turmasPaginadas)
        {
            var turmasAgrupadasPorAnoNome = turmasPaginadas.Items.GroupBy(t => t.NomeTurma).ToList();

            var listaComAgrupamento = new List<RetornoConsultaListagemTurmaComponenteDto>();
            var listaTurmasMesmaSerie = new List<RetornoConsultaListagemTurmaComponenteDto>();
            foreach (var turmas in turmasAgrupadasPorAnoNome)
            {
                foreach (var turmaAno in turmas)
                {
                    var componenteCurricularPai = await mediator.Send(new ObterCodigoComponentePaiQuery(turmaAno.ComponenteCurricularCodigo));
                    if (!string.IsNullOrEmpty(componenteCurricularPai))
                        turmaAno.ComponenteCurricularPaiCodigo = Convert.ToInt64(componenteCurricularPai);
                    else
                        turmaAno.ComponenteCurricularPaiCodigo = turmaAno.ComponenteCurricularCodigo;

                    listaTurmasMesmaSerie.Add(turmaAno);
                }

                listaComAgrupamento.AddRange(listaTurmasMesmaSerie.DistinctBy(t => t.ComponenteCurricularPaiCodigo).Where(t => t.NomeTurma == turmas.Key));

                listaTurmasMesmaSerie = new List<RetornoConsultaListagemTurmaComponenteDto>();
            }

            turmasPaginadas.Items = listaComAgrupamento;

            return turmasPaginadas;
        }

        private async Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> MapearNomeFiltroTurmas(string[] turmasCodigos, PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto> turmasPaginadas)
        {
            var nomesFiltro = await mediator.Send(new ObterTurmasNomeFiltroPorTurmasCodigosQuery(turmasCodigos));
            if (nomesFiltro.NaoEhNulo() && nomesFiltro.Any())
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
            if (turmasPaginadas.EhNulo())
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
            var componente = listaComponentes.FirstOrDefault(c => c.Id == turmas.ComponenteCurricularCodigo);

            var nomeComponente = componente?.Descricao ?? turmas.NomeComponenteCurricular;
            var componentePermiteLanctoNota = componente?.PermiteLanctoNota ?? false;

            return turmas.EhNulo() ? null : new TurmaComComponenteDto
            {
                Id = turmas.Id.GetValueOrDefault(),
                NomeTurma = turmas.SerieEnsino.EhNulo() && turmas.NomeFiltro.EhNulo() ? turmas.NomeTurmaFormatado(nomeComponente) : turmas.NomeTurmaFiltroFormatado(nomeComponente),
                TurmaCodigo = turmas.TurmaCodigo,
                ComponenteCurricularCodigo = turmas.ComponenteCurricularCodigo,
                Turno = turmas.Turno.ObterNome(),
                LancaNota = componentePermiteLanctoNota
            };
        }

        private async Task<PaginacaoResultadoDto<TurmaComComponenteDto>> MapearParaDtoComPendenciaPaginacao(PaginacaoResultadoDto<TurmaComComponenteDto> turmasComponentes, int anoLetivo, int bimestre, Usuario usuario)
        {
            var itensComPendencias = new List<TurmaComComponenteDto>();

            foreach (var turmaCodigo in turmasComponentes.Items.GroupBy(a => a.TurmaCodigo))
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo.Key.ToString()));

                var ehTurmaInfantil = turma.EhTurmaInfantil;

                var periodoFechamentoIniciado = !ehTurmaInfantil && !usuario.EhProfessorCj() &&
                    await mediator.Send(new PeriodoFechamentoTurmaIniciadoQuery(turma, bimestre, DateTime.Today));

                foreach (var turmaComponente in turmaCodigo)
                {
                    var pendencias = bimestre > 0 ? await mediator.Send(new ObterIndicativoPendenciasAulasPorTipoQuery(turmaComponente.ComponenteCurricularCodigo.ToString(),
                                                                                                        turma.CodigoTurma,
                                                                                                        anoLetivo,
                                                                                                        bimestre,
                                                                                                        verificaDiarioBordo: ehTurmaInfantil && !usuario.EhProfessorCjInfantil(),
                                                                                                        verificaAvaliacao: !ehTurmaInfantil,
                                                                                                        verificaPlanoAula: !ehTurmaInfantil,
                                                                                                        verificaFrequencia: !ehTurmaInfantil || !usuario.EhProfessorCjInfantil(),
                                                                                                        professorCj: usuario.EhProfessorCj(),
                                                                                                        professorNaoCj: usuario.EhProfessor(),
                                                                                                        professorRf: usuario.CodigoRf,
                                                                                                        ehGestor: usuario.EhGestorEscolar())) : null;

                    var possuiFechamento = periodoFechamentoIniciado &&
                        await mediator.Send(new ObterIndicativoPendenciaFechamentoTurmaDisciplinaQuery(turma.Id,
                                                                                                        bimestre,
                                                                                                        turmaComponente.ComponenteCurricularCodigo));

                    turmaComponente.PendenciaDiarioBordo = pendencias?.PendenciaDiarioBordo ?? false;
                    turmaComponente.PendenciaAvaliacoes = pendencias?.PendenciaAvaliacoes ?? false;
                    turmaComponente.PendenciaFrequencia = pendencias?.PendenciaFrequencia ?? false;
                    turmaComponente.PendenciaPlanoAula = pendencias?.PendenciaPlanoAula ?? false;
                    turmaComponente.PendenciaFechamento = periodoFechamentoIniciado && !possuiFechamento && turmaComponente.LancaNota && turma.TipoTurma != TipoTurma.Programa;
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
