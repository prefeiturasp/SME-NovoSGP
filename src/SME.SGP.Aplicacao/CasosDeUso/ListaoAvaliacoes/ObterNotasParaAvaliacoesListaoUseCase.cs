using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasParaAvaliacoesListaoUseCase : IObterNotasParaAvaliacoesListaoUseCase
    {
        private readonly IMediator mediator;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IServicoEol servicoEOL;
        public ObterNotasParaAvaliacoesListaoUseCase(IMediator mediator, IConsultasDisciplina consultasDisciplina, IServicoEol servicoEOL)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }
        public async Task<NotasConceitosListaoRetornoDto> Executar(ListaNotasConceitosBimestreRefatoradaDto filtro)
        {
            try
            {

                var retorno = new NotasConceitosListaoRetornoDto();

                var turmaCompleta = await mediator
                    .Send(new ObterTurmaComUeEDrePorCodigoQuery(filtro.TurmaCodigo));

                if (turmaCompleta == null)
                    throw new NegocioException("Não foi possível obter a turma.");


                var disciplinasDoProfessorLogado = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(filtro.TurmaCodigo, true);

                if (disciplinasDoProfessorLogado == null || !disciplinasDoProfessorLogado.Any())
                    throw new NegocioException("Não foi possível obter os componentes curriculares do usuário logado.");

                var periodoFechamentoBimestre = await mediator.Send(new ObterPeriodoFechamentoPorTurmaBimestrePeriodoEscolarQuery(turmaCompleta, filtro.Bimestre, filtro.PeriodoEscolarId));

                var periodoInicio = new DateTime(filtro.PeriodoInicioTicks);
                var periodoFim = new DateTime(filtro.PeriodoFimTicks);

                var componentesCurriculares = ObterComponentesCurricularesParaConsulta(filtro.DisciplinaCodigo, disciplinasDoProfessorLogado);
                var atividadesAvaliativaEBimestres = await mediator
                    .Send(new ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery(componentesCurriculares.Select(a => a.ToString()).ToArray(), filtro.TurmaCodigo, periodoInicio, periodoFim));

                var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(filtro.TurmaCodigo));

                if (alunos == null || !alunos.Any())
                    throw new NegocioException("Não foi encontrado alunos para a turma informada");

                var tipoAvaliacaoBimestral = await mediator.Send(new ObterTipoAvaliacaoBimestralQuery());

                retorno.BimestreAtual = filtro.Bimestre;
                retorno.PercentualAlunosInsuficientes = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));

                DateTime? dataUltimaNotaConceitoInserida = null;
                DateTime? dataUltimaNotaConceitoAlterada = null;
                var usuarioRfUltimaNotaConceitoInserida = string.Empty;
                var usuarioRfUltimaNotaConceitoAlterada = string.Empty;
                var nomeAvaliacaoAuditoriaInclusao = string.Empty;
                var nomeAvaliacaoAuditoriaAlteracao = string.Empty;

                AtividadeAvaliativa atividadeAvaliativaParaObterTipoNota = null;

                var bimestreParaAdicionar = new NotasConceitosBimestreListaoRetornoDto()
                {
                    Descricao = $"{filtro.Bimestre}º Bimestre",
                    Numero = filtro.Bimestre,
                    PeriodoInicio = periodoInicio,
                    PeriodoFim = periodoFim
                };
                var listaAlunosDoBimestre = new List<NotasConceitosAlunoListaoRetornoDto>();
                var atividadesAvaliativasdoBimestre = atividadesAvaliativaEBimestres
                                        .Where(a => a.DataAvaliacao.Date >= periodoInicio && periodoFim >= a.DataAvaliacao.Date)
                                        .OrderBy(a => a.DataAvaliacao)
                                        .ToList();

                var alunosIds = alunos.Select(a => a.CodigoAluno).Distinct().ToArray();
                IEnumerable<NotaConceito> notas = null;
                IEnumerable<AusenciaAlunoDto> ausenciasDasAtividadesAvaliativas = null;

                long[] atividadesAvaliativasId = atividadesAvaliativasdoBimestre.Select(a => a.Id)?.Distinct().ToArray() ?? new long[0];
                notas = await mediator.Send(new ObterNotasPorAlunosAtividadesAvaliativasQuery(atividadesAvaliativasId, alunosIds, filtro.DisciplinaCodigo.ToString()));
                var datasDasAtividadesAvaliativas = atividadesAvaliativasdoBimestre.Select(a => a.DataAvaliacao).Distinct().ToArray();
                ausenciasDasAtividadesAvaliativas = await mediator.Send(new ObterAusenciasDaAtividadesAvaliativasQuery(filtro.TurmaCodigo, datasDasAtividadesAvaliativas, filtro.DisciplinaCodigo.ToString(), alunosIds));

                var componentesCurricularesCompletos = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { filtro.DisciplinaCodigo }));
                if (componentesCurricularesCompletos == null || !componentesCurricularesCompletos.Any())
                    throw new NegocioException("Componente curricular informado não encontrado no EOL");

                var componenteReferencia = componentesCurricularesCompletos.FirstOrDefault(a => a.CodigoComponenteCurricular == filtro.DisciplinaCodigo);

                IOrderedEnumerable<AlunoPorTurmaResposta> alunosAtivos = null;
                if (filtro.TurmaHistorico)
                {
                    alunosAtivos = from a in alunos
                                   where a.EstaAtivo(periodoFim) ||
                                         (a.EstaInativo(periodoFim) && a.DataSituacao.Date >= periodoInicio.Date && a.DataSituacao.Date <= periodoFim.Date) &&
                                          (a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido || a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Transferido)
                                   orderby a.NomeValido(), a.NumeroAlunoChamada
                                   select a;
                }
                else
                {
                    alunosAtivos = from a in alunos
                                   where (a.EstaAtivo(periodoFim) ||
                                         (a.EstaInativo(periodoFim) && a.DataSituacao.Date >= periodoInicio.Date)) &&
                                         a.DataMatricula.Date <= periodoFim.Date
                                   orderby a.NomeValido(), a.NumeroAlunoChamada
                                   select a;
                }

                var alunosAtivosCodigos = alunosAtivos.Select(a => a.CodigoAluno).Distinct().ToArray();

                var frequenciasDosAlunos = await mediator
                    .Send(new ObterFrequenciasPorAlunosTurmaCCDataQuery(alunosAtivosCodigos, periodoFim, TipoFrequenciaAluno.PorDisciplina, filtro.TurmaCodigo, filtro.DisciplinaCodigo.ToString()));

                var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(filtro.TurmaCodigo, filtro.DisciplinaCodigo.ToString(), filtro.PeriodoEscolarId));

                foreach (var aluno in alunosAtivos)
                {
                    var notaConceitoAluno = new NotasConceitosAlunoListaoRetornoDto()
                    {
                        Id = aluno.CodigoAluno,
                        Nome = aluno.NomeValido(),
                        NumeroChamada = aluno.NumeroAlunoChamada
                    };

                    var notasAvaliacoes = new List<NotasConceitosNotaAvaliacaoListaoRetornoDto>();
                    foreach (var atividadeAvaliativa in atividadesAvaliativasdoBimestre)
                    {
                        var notaDoAluno = ObterNotaParaVisualizacao(notas, aluno, atividadeAvaliativa);
                        var notaParaVisualizar = string.Empty;

                        if (notaDoAluno != null)
                        {
                            notaParaVisualizar = notaDoAluno.ObterNota();

                            if (!dataUltimaNotaConceitoInserida.HasValue || notaDoAluno.CriadoEm > dataUltimaNotaConceitoInserida.Value)
                            {
                                usuarioRfUltimaNotaConceitoInserida = $"{notaDoAluno.CriadoPor}({notaDoAluno.CriadoRF})";
                                dataUltimaNotaConceitoInserida = notaDoAluno.CriadoEm;
                                nomeAvaliacaoAuditoriaInclusao = atividadeAvaliativa.NomeAvaliacao;
                            }

                            if (notaDoAluno.AlteradoEm.HasValue && (!dataUltimaNotaConceitoAlterada.HasValue || notaDoAluno.AlteradoEm.Value > dataUltimaNotaConceitoAlterada.Value))
                            {
                                usuarioRfUltimaNotaConceitoAlterada = $"{notaDoAluno.AlteradoPor}({notaDoAluno.AlteradoRF})";
                                dataUltimaNotaConceitoAlterada = notaDoAluno.AlteradoEm;
                                nomeAvaliacaoAuditoriaAlteracao = atividadeAvaliativa.NomeAvaliacao;
                            }
                        }

                        var ausente = ausenciasDasAtividadesAvaliativas
                            .Any(a => a.AlunoCodigo == aluno.CodigoAluno && a.AulaData.Date == atividadeAvaliativa.DataAvaliacao.Date);

                        var notaAvaliacao = new NotasConceitosNotaAvaliacaoListaoRetornoDto()
                        {
                            AtividadeAvaliativaId = atividadeAvaliativa.Id,
                            NotaConceito = notaParaVisualizar,
                            Ausente = ausente,
                            PodeEditar = aluno.EstaAtivo(atividadeAvaliativa.DataAvaliacao) ||
                                         (aluno.EstaInativo(atividadeAvaliativa.DataAvaliacao) && atividadeAvaliativa.DataAvaliacao.Date <= aluno.DataSituacao.Date),
                        };

                        notasAvaliacoes.Add(notaAvaliacao);
                    }

                    notaConceitoAluno.PodeEditar =
                            (notasAvaliacoes.Any(na => na.PodeEditar) || (atividadesAvaliativaEBimestres is null || !atividadesAvaliativaEBimestres.Any())) &&
                            (aluno.Inativo == false || (aluno.Inativo && aluno.DataSituacao >= periodoFechamentoBimestre?.InicioDoFechamento.Date));

                    notaConceitoAluno.Marcador = await mediator
                                    .Send(new ObterMarcadorAlunoQuery(aluno, periodoFim, turmaCompleta.EhTurmaInfantil));
                                                    notaConceitoAluno.NotasAvaliacoes = notasAvaliacoes;

                    var frequenciaAluno = frequenciasDosAlunos.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);

                    listaAlunosDoBimestre.Add(notaConceitoAluno);
                }

                foreach (var avaliacao in atividadesAvaliativasdoBimestre)
                {
                    var avaliacaoDoBimestre = new NotasConceitosAvaliacaoListaoRetornoDto()
                    {
                        Id = avaliacao.Id,
                        Data = avaliacao.DataAvaliacao,
                        Descricao = avaliacao.DescricaoAvaliacao,
                        Nome = avaliacao.NomeAvaliacao,
                        EhCJ = avaliacao.EhCj
                    };

                    avaliacaoDoBimestre.EhInterdisciplinar = avaliacao.Categoria.Equals(CategoriaAtividadeAvaliativa.Interdisciplinar);

                    if (componenteReferencia.Regencia)
                    {
                        var atividadeDisciplinas = await ObterDisciplinasAtividadeAvaliativa(avaliacao.Id, avaliacao.EhRegencia);
                        var idsDisciplinas = atividadeDisciplinas?.Select(a => long.Parse(a.DisciplinaId)).ToArray();
                        IEnumerable<DisciplinaDto> disciplinas;
                        if (idsDisciplinas != null && idsDisciplinas.Any())
                            disciplinas = await ObterDisciplinasPorIds(idsDisciplinas);
                        else
                        {
                            disciplinas = await consultasDisciplina
                                .ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(componenteReferencia.CodigoComponenteCurricular,
                                                                                                turmaCompleta.CodigoTurma,
                                                                                                turmaCompleta.TipoTurma == TipoTurma.Programa,
                                                                                                componenteReferencia.Regencia);
                        }
                        var nomesDisciplinas = disciplinas?.Select(d => d.Nome).ToArray();
                        avaliacaoDoBimestre.Disciplinas = nomesDisciplinas;
                    }

                    bimestreParaAdicionar.Avaliacoes.Add(avaliacaoDoBimestre);

                    if (atividadeAvaliativaParaObterTipoNota == null)
                        atividadeAvaliativaParaObterTipoNota = avaliacao;
                }

                bimestreParaAdicionar.Alunos = listaAlunosDoBimestre;
                bimestreParaAdicionar.QtdAvaliacoesBimestrais = atividadesAvaliativasdoBimestre
                    .Count(x => x.TipoAvaliacaoId == tipoAvaliacaoBimestral.Id);

                await ValidaMinimoAvaliacoesBimestrais(componenteReferencia, null, tipoAvaliacaoBimestral, bimestreParaAdicionar, atividadesAvaliativaEBimestres, filtro.Bimestre);
                if (atividadeAvaliativaParaObterTipoNota != null)
                {
                    var notaTipo = await mediator.Send(new ObterTipoNotaPorTurmaQuery(turmaCompleta, new DateTime(filtro.AnoLetivo, 3, 1)));
                    retorno.NotaTipo = notaTipo;
                    ObterValoresDeAuditoria(dataUltimaNotaConceitoInserida, dataUltimaNotaConceitoAlterada, usuarioRfUltimaNotaConceitoInserida, usuarioRfUltimaNotaConceitoAlterada, notaTipo, retorno, nomeAvaliacaoAuditoriaInclusao, nomeAvaliacaoAuditoriaAlteracao);
                }
                else
                {
                    var tipoNota = await mediator.Send(new ObterTipoNotaPorTurmaQuery(turmaCompleta, new DateTime(filtro.AnoLetivo, 3, 1)));
                    retorno.NotaTipo = tipoNota;
                }
                retorno.Bimestres.Add(bimestreParaAdicionar);
                return retorno;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ObterValoresDeAuditoria(DateTime? dataUltimaNotaConceitoInserida, DateTime? dataUltimaNotaConceitoAlterada, string usuarioInseriu, string usuarioAlterou, TipoNota tipoNota, NotasConceitosListaoRetornoDto notasConceitosRetornoDto, string nomeAvaliacaoInclusao, string nomeAvaliacaoAlteracao)
        {
            var tituloNotasOuConceitos = tipoNota == TipoNota.Conceito ? "Conceitos" : "Notas";

            if (dataUltimaNotaConceitoInserida.HasValue)
                notasConceitosRetornoDto.AuditoriaInserido = $"{tituloNotasOuConceitos} da avaliação {nomeAvaliacaoInclusao} inseridos por {usuarioInseriu} em {dataUltimaNotaConceitoInserida.Value.ToString("dd/MM/yyyy")}, às {dataUltimaNotaConceitoInserida.Value.ToString("HH:mm")}.";
            if (dataUltimaNotaConceitoAlterada.HasValue)
                notasConceitosRetornoDto.AuditoriaAlterado = $"{tituloNotasOuConceitos} da avaliação {nomeAvaliacaoAlteracao} alterados por {usuarioAlterou} em {dataUltimaNotaConceitoAlterada.Value.ToString("dd/MM/yyyy")}, às {dataUltimaNotaConceitoAlterada.Value.ToString("HH:mm")}.";
        }
        private async Task ValidaMinimoAvaliacoesBimestrais(DisciplinaDto componenteCurricular, IEnumerable<DisciplinaResposta> disciplinasRegencia, TipoAvaliacao tipoAvaliacaoBimestral, NotasConceitosBimestreListaoRetornoDto bimestreDto, IEnumerable<AtividadeAvaliativa> atividadeAvaliativas, int bimestre)
        {
            var atividadesBimestrais = atividadeAvaliativas.Where(a => a.TipoAvaliacaoId == (long)TipoAvaliacaoCodigo.AvaliacaoBimestral);
            if (componenteCurricular.Regencia)
            {
                var totalDisciplinasRegencia = disciplinasRegencia != null ? disciplinasRegencia.Count() : 0;

                long[] atividadesAvaliativasBimestraisId = atividadesBimestrais.Select(a => a.Id)?.Distinct().ToArray() ?? new long[0];

                var componentesComAtividade = atividadesAvaliativasBimestraisId.Count() > 0 ? await mediator.Send(new ObterTotalAtividadeAvaliativasRegenciaQuery(atividadesAvaliativasBimestraisId)) :
                    new List<ComponentesRegenciaComAtividadeAvaliativaDto>();

                if (componentesComAtividade.Any())
                {
                    var componentesComAtivadadesMinimas = componentesComAtividade.Where(c => c.TotalAtividades >= tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre).Select(c => c.DisciplinaId);

                    var disciplinasObservacao = disciplinasRegencia.Where(a => !componentesComAtivadadesMinimas.Contains(a.CodigoComponenteCurricular.ToString()));

                    if (disciplinasObservacao.Any())
                    {
                        bimestreDto.Observacoes.Add($"O(s) componente(s) curricular(es) [{string.Join(",", disciplinasObservacao.Select(d => d.Nome))}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
                    }
                }
                else
                {
                    var componentesSemAvaliacao = disciplinasRegencia.Select(a => a.Nome);
                    bimestreDto.Observacoes.Add($"O(s) componente(s) curricular(es) [{string.Join(",", componentesSemAvaliacao)}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
                }
            }
            else
            {
                var avaliacoes = atividadesBimestrais.SelectMany(a => a.Disciplinas.Where(b => b.DisciplinaId == componenteCurricular.CodigoComponenteCurricular.ToString()));
                if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                    bimestreDto.Observacoes.Add($"O componente curricular [{componenteCurricular.Nome}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
        }
        private static NotaConceito ObterNotaParaVisualizacao(IEnumerable<NotaConceito> notas, AlunoPorTurmaResposta aluno, AtividadeAvaliativa atividadeAvaliativa)
        {
            var notaDoAluno = notas.FirstOrDefault(a => a.AlunoId == aluno.CodigoAluno && a.AtividadeAvaliativaID == atividadeAvaliativa.Id);

            return notaDoAluno;
        }
        private static List<long> ObterComponentesCurricularesParaConsulta(long disciplinaId, List<DisciplinaDto> disciplinasDoProfessorLogado)
        {
            var disciplinasFilha = disciplinasDoProfessorLogado.Where(d => d.CdComponenteCurricularPai == disciplinaId).ToList();
            var componentesCurriculares = new List<long>();

            if (disciplinasFilha.Any())
            {
                componentesCurriculares.AddRange(disciplinasFilha.Select(a => a.CodigoComponenteCurricular).ToList());
            }
            else
            {
                componentesCurriculares.Add(disciplinaId);
            }

            return componentesCurriculares;
        }
        public async Task<IEnumerable<AtividadeAvaliativaDisciplina>> ObterDisciplinasAtividadeAvaliativa(long avaliacao_id, bool ehRegencia)
        {
            return await mediator.Send(new ObterDisciplinasAtividadeAvaliativaQuery(avaliacao_id, ehRegencia));
        }
        public async Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIds(long[] ids)
        {
            return await mediator.Send(new ObterDisciplinasPorIdsQuery(ids));
        }
    }
}
