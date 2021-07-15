using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasParaAvaliacoesUseCase : IObterNotasParaAvaliacoesUseCase
    {
        private readonly IMediator mediator;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IServicoEol servicoEOL;

        public ObterNotasParaAvaliacoesUseCase(IMediator mediator, IConsultasDisciplina consultasDisciplina, IServicoEol servicoEOL)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasDisciplina = consultasDisciplina;
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<NotasConceitosRetornoDto> Executar(ListaNotasConceitosConsultaRefatoradaDto filtro)
        {
            var turmaCompleta = await mediator
                .Send(new ObterTurmaComUeEDrePorCodigoQuery(filtro.TurmaCodigo));

            if (turmaCompleta == null)
                throw new NegocioException("Não foi possível obter a turma.");

              var disciplinasDoProfessorLogado = await consultasDisciplina
                .ObterComponentesCurricularesPorProfessorETurma(filtro.TurmaCodigo, true);

            if (disciplinasDoProfessorLogado == null || !disciplinasDoProfessorLogado.Any())
                throw new NegocioException("Não foi possível obter os componentes curriculares do usuário logado.");

            var periodoInicio = new DateTime(filtro.PeriodoInicioTicks);
            var periodoFim = new DateTime(filtro.PeriodoFimTicks);

            var componentesCurriculares = ObterComponentesCurricularesParaConsulta(filtro.DisciplinaCodigo, disciplinasDoProfessorLogado);
            var atividadesAvaliativaEBimestres = await mediator
                .Send(new ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery(componentesCurriculares.Select(a => a.ToString()).ToArray(), filtro.TurmaCodigo, periodoInicio, periodoFim));

            var alunos = await mediator
                .Send(new ObterAlunosPorTurmaEAnoLetivoQuery(filtro.TurmaCodigo));

            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");

            var retorno = new NotasConceitosRetornoDto();
            var tipoAvaliacaoBimestral = await mediator.Send(new ObterTipoAvaliacaoBimestralQuery());

            retorno.BimestreAtual = filtro.Bimestre;
            retorno.MediaAprovacaoBimestre = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, DateTime.Today.Year)));
            retorno.MinimoAvaliacoesBimestrais = tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre;
            retorno.PercentualAlunosInsuficientes = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));

            DateTime? dataUltimaNotaConceitoInserida = null;
            DateTime? dataUltimaNotaConceitoAlterada = null;
            var usuarioRfUltimaNotaConceitoInserida = string.Empty;
            var usuarioRfUltimaNotaConceitoAlterada = string.Empty;
            var nomeAvaliacaoAuditoriaInclusao = string.Empty;
            var nomeAvaliacaoAuditoriaAlteracao = string.Empty;

            AtividadeAvaliativa atividadeAvaliativaParaObterTipoNota = null;

            var bimestreParaAdicionar = new NotasConceitosBimestreRetornoDto()
            {
                Descricao = $"{filtro.Bimestre}º Bimestre",
                Numero = filtro.Bimestre,
                PeriodoInicio = periodoInicio,
                PeriodoFim = periodoFim
            };

            var listaAlunosDoBimestre = new List<NotasConceitosAlunoRetornoDto>();

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

            IEnumerable<DisciplinaResposta> disciplinasRegencia = null;
            if (componenteReferencia.Regencia)
            {
                var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

                if (usuario.EhProfessorCj())
                {
                    IEnumerable<DisciplinaDto> disciplinasRegenciaCJ = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(filtro.DisciplinaCodigo, filtro.TurmaCodigo, false, componenteReferencia.Regencia);
                    if (disciplinasRegenciaCJ == null || !disciplinasRegenciaCJ.Any())
                        throw new NegocioException("Não foram encontradas as disciplinas de regência");
                    disciplinasRegencia = MapearParaDto(disciplinasRegenciaCJ);
                }
                else
                {
                    IEnumerable<ComponenteCurricularEol> disciplinasRegenciaEol = await servicoEOL.ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamento(filtro.TurmaCodigo, usuario.CodigoRf, usuario.PerfilAtual);
                    if (disciplinasRegenciaEol == null || !disciplinasRegenciaEol.Any(d => !d.TerritorioSaber && d.Regencia))
                        throw new NegocioException("Não foram encontradas disciplinas de regência no EOL");
                    disciplinasRegencia = MapearParaDto(disciplinasRegenciaEol.Where(d => !d.TerritorioSaber && d.Regencia));
                }
            }

            var fechamentosNotasDaTurma = await mediator
                .Send(new ObterFechamentosPorTurmaPeriodoCCQuery(filtro.PeriodoEscolarId, filtro.TurmaId, filtro.DisciplinaCodigo));

            //Obter alunos ativos
            var alunosAtivos = from a in alunos
                               where (a.EstaAtivo(periodoFim) ||
                                     (a.EstaInativo(periodoFim) && a.DataSituacao.Date >= periodoInicio.Date)) &&
                                     a.DataMatricula.Date <= periodoFim.Date
                               orderby a.NomeValido(), a.NumeroAlunoChamada
                               select a;

            var alunosAtivosCodigos = alunosAtivos
                .Select(a => a.CodigoAluno).Distinct().ToArray();

            var frequenciasDosAlunos = await mediator
                .Send(new ObterFrequenciasPorAlunosTurmaCCDataQuery(alunosAtivosCodigos, periodoFim, TipoFrequenciaAluno.PorDisciplina, filtro.TurmaCodigo, filtro.DisciplinaCodigo.ToString()));

            foreach (var aluno in alunosAtivos)
            {
                var notaConceitoAluno = new NotasConceitosAlunoRetornoDto()
                {
                    Id = aluno.CodigoAluno,
                    Nome = aluno.NomeValido(),
                    NumeroChamada = aluno.NumeroAlunoChamada
                };

                var notasAvaliacoes = new List<NotasConceitosNotaAvaliacaoRetornoDto>();
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

                    var notaAvaliacao = new NotasConceitosNotaAvaliacaoRetornoDto()
                    {
                        AtividadeAvaliativaId = atividadeAvaliativa.Id,
                        NotaConceito = notaParaVisualizar,
                        Ausente = ausente,
                        PodeEditar = aluno.EstaAtivo(atividadeAvaliativa.DataAvaliacao) ||
                                     (aluno.EstaInativo(atividadeAvaliativa.DataAvaliacao) && atividadeAvaliativa.DataAvaliacao.Date <= aluno.DataSituacao.Date)
                    };

                    notasAvaliacoes.Add(notaAvaliacao);
                }

                notaConceitoAluno.PodeEditar = notasAvaliacoes
                    .Any(na => na.PodeEditar) || (atividadesAvaliativaEBimestres is null || !atividadesAvaliativaEBimestres.Any());

                notaConceitoAluno.Marcador = await mediator
                    .Send(new ObterMarcadorAlunoQuery(aluno, periodoFim, turmaCompleta.EhTurmaInfantil));
                notaConceitoAluno.NotasAvaliacoes = notasAvaliacoes;

                var fechamentoTurma = (from ft in fechamentosNotasDaTurma
                                       from fa in ft.FechamentoAlunos
                                       where fa.AlunoCodigo.Equals(aluno.CodigoAluno)
                                       select ft).FirstOrDefault();

                // Carrega Notas do Bimestre
                if (fechamentoTurma != null)
                {
                    bimestreParaAdicionar.FechamentoTurmaId = fechamentoTurma.Id;
                    bimestreParaAdicionar.Situacao = fechamentoTurma.Situacao;
                    retorno.AuditoriaBimestreInserido = $"Nota final do bimestre inserida por {fechamentoTurma.CriadoPor}({fechamentoTurma.CriadoRF}) em {fechamentoTurma.CriadoEm.ToString("dd/MM/yyyy")}, às {fechamentoTurma.CriadoEm.ToString("HH:mm")}.";

                    var notasConceitoBimestre = fechamentoTurma.FechamentoAlunos
                        .Where(a => a.AlunoCodigo == aluno.CodigoAluno)
                        .SelectMany(a => a.FechamentoNotas).ToList();

                    if (notasConceitoBimestre.Any())
                    {
                        var dadosAuditoriaAlteracaoBimestre = notasConceitoBimestre
                            .OrderByDescending(nc => nc.AlteradoEm)
                            .ThenBy(nc => nc.CriadoEm)
                            .Select(nc => new
                            {
                                AlteradoPor = !string.IsNullOrWhiteSpace(nc.AlteradoPor) && !nc.AlteradoRF.Equals(0) ? nc.AlteradoPor : nc.CriadoPor,
                                AlteradoRf = !string.IsNullOrWhiteSpace(nc.AlteradoRF) && !nc.AlteradoRF.Equals(0) ? nc.AlteradoRF : nc.CriadoRF,
                                AlteradoEm = nc.AlteradoEm.HasValue && !nc.AlteradoRF.Equals(0) ? nc.AlteradoEm.Value : nc.CriadoEm,
                            }).First();

                        retorno.AuditoriaBimestreAlterado = $"Nota final do bimestre alterada por {(dadosAuditoriaAlteracaoBimestre.AlteradoPor)}({dadosAuditoriaAlteracaoBimestre.AlteradoRf}) em {dadosAuditoriaAlteracaoBimestre.AlteradoEm.ToString("dd/MM/yyyy")}, às {dadosAuditoriaAlteracaoBimestre.AlteradoEm.ToString("HH:mm")}.";
                    }

                    if (componenteReferencia.Regencia)
                    {
                        // Regencia carrega disciplinas mesmo sem nota de fechamento
                        foreach (var disciplinaRegencia in disciplinasRegencia)
                        {
                            var nota = new FechamentoNotaRetornoDto()
                            {
                                DisciplinaId = disciplinaRegencia.CodigoComponenteCurricular,
                                Disciplina = disciplinaRegencia.Nome,
                            };
                            var notaRegencia = notasConceitoBimestre?.FirstOrDefault(c => c.DisciplinaId == disciplinaRegencia.CodigoComponenteCurricular);
                            if (notaRegencia != null)
                            {
                                nota.NotaConceito = (notaRegencia.ConceitoId.HasValue ? notaRegencia.ConceitoId.Value : notaRegencia.Nota);
                                nota.EhConceito = notaRegencia.ConceitoId.HasValue;
                            }

                            notaConceitoAluno.NotasBimestre.Add(nota);
                        }
                    }
                    else
                    {
                        foreach (var notaConceitoBimestre in notasConceitoBimestre)
                            notaConceitoAluno.NotasBimestre.Add(new FechamentoNotaRetornoDto()
                            {
                                DisciplinaId = notaConceitoBimestre.DisciplinaId,
                                Disciplina = componenteReferencia.Nome,
                                NotaConceito = notaConceitoBimestre.ConceitoId.HasValue ?
                                    notaConceitoBimestre.ConceitoId.Value :
                                    notaConceitoBimestre.Nota,
                                EhConceito = notaConceitoBimestre.ConceitoId.HasValue
                            });
                    }
                }
                else if (componenteReferencia.Regencia && componenteReferencia != null)
                {
                    // Regencia carrega disciplinas mesmo sem nota de fechamento
                    foreach (var disciplinaRegencia in disciplinasRegencia)
                    {
                        notaConceitoAluno.NotasBimestre.Add(new FechamentoNotaRetornoDto()
                        {
                            DisciplinaId = disciplinaRegencia.CodigoComponenteCurricular,
                            Disciplina = disciplinaRegencia.Nome,
                        });
                    }
                }

                // Carrega Frequencia Aluno                        
                var frequenciaAluno = frequenciasDosAlunos.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);
                notaConceitoAluno.PercentualFrequencia = frequenciaAluno != null ?
                                ((int)Math.Round(frequenciaAluno.PercentualFrequencia, 0)).ToString() :
                                "";

                listaAlunosDoBimestre.Add(notaConceitoAluno);
            }

            foreach (var avaliacao in atividadesAvaliativasdoBimestre)
            {
                var avaliacaoDoBimestre = new NotasConceitosAvaliacaoRetornoDto()
                {
                    Id = avaliacao.Id,
                    Data = avaliacao.DataAvaliacao,
                    Descricao = avaliacao.DescricaoAvaliacao,
                    Nome = avaliacao.NomeAvaliacao,
                    EhCJ = avaliacao.EhCj
                };
                if (avaliacao.Categoria.Equals(CategoriaAtividadeAvaliativa.Interdisciplinar))
                {
                    avaliacaoDoBimestre.EhInterdisciplinar = true;
                    var nomesDisciplinas = componentesCurricularesCompletos.Select(d => d.Nome).ToArray();
                    avaliacaoDoBimestre.Disciplinas = nomesDisciplinas;
                }
                bimestreParaAdicionar.Avaliacoes.Add(avaliacaoDoBimestre);

                if (atividadeAvaliativaParaObterTipoNota == null)
                    atividadeAvaliativaParaObterTipoNota = avaliacao;
            }

            bimestreParaAdicionar.Alunos = listaAlunosDoBimestre;
            bimestreParaAdicionar.QtdAvaliacoesBimestrais = atividadesAvaliativasdoBimestre
                .Count(x => x.TipoAvaliacaoId == tipoAvaliacaoBimestral.Id);

            //REFATORAR -> Obtendo turma Full por causa dessa função
            bimestreParaAdicionar.PodeLancarNotaFinal = await VerificaPeriodoFechamentoEmAberto(turmaCompleta, filtro.Bimestre);

            // Valida Avaliações Bimestrais
            ValidaMinimoAvaliacoesBimestrais(componenteReferencia, componentesCurricularesCompletos, tipoAvaliacaoBimestral, bimestreParaAdicionar, atividadesAvaliativaEBimestres, filtro.Bimestre);

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

        private IEnumerable<DisciplinaResposta> MapearParaDto(IEnumerable<ComponenteCurricularEol> disciplinasRegenciaEol)
        {
            foreach (var disciplina in disciplinasRegenciaEol)
            {
                yield return new DisciplinaResposta()
                {
                    CodigoComponenteCurricular = disciplina.Codigo,
                    Compartilhada = disciplina.Compartilhada,
                    CodigoComponenteCurricularPai = disciplina.CodigoComponenteCurricularPai,
                    Nome = disciplina.Descricao,
                    Regencia = disciplina.Regencia,
                    RegistroFrequencia = disciplina.RegistraFrequencia,
                    TerritorioSaber = disciplina.TerritorioSaber,
                    LancaNota = disciplina.LancaNota,
                    BaseNacional = disciplina.BaseNacional,
                    GrupoMatriz = new Integracoes.Respostas.GrupoMatriz { Id = disciplina.GrupoMatriz.Id, Nome = disciplina.GrupoMatriz.Nome }
                };
            }
        }
        private IEnumerable<DisciplinaResposta> MapearParaDto(IEnumerable<DisciplinaDto> disciplinasRegenciaCJ)
        {
            foreach (var disciplina in disciplinasRegenciaCJ)
            {
                yield return new DisciplinaResposta()
                {
                    CodigoComponenteCurricular = disciplina.CodigoComponenteCurricular,
                    Compartilhada = disciplina.Compartilhada,
                    CodigoComponenteCurricularPai = disciplina.CdComponenteCurricularPai,
                    Nome = disciplina.Nome,
                    Regencia = disciplina.Regencia,
                    RegistroFrequencia = disciplina.RegistraFrequencia,
                    TerritorioSaber = disciplina.TerritorioSaber,
                    LancaNota = disciplina.LancaNota,
                    GrupoMatriz = new Integracoes.Respostas.GrupoMatriz { Id = disciplina.GrupoMatrizId, Nome = disciplina.GrupoMatrizNome }
                };
            }
        }
        private void ObterValoresDeAuditoria(DateTime? dataUltimaNotaConceitoInserida, DateTime? dataUltimaNotaConceitoAlterada, string usuarioInseriu, string usuarioAlterou, TipoNota tipoNota, NotasConceitosRetornoDto notasConceitosRetornoDto, string nomeAvaliacaoInclusao, string nomeAvaliacaoAlteracao)
        {
            var tituloNotasOuConceitos = tipoNota == TipoNota.Conceito ? "Conceitos" : "Notas";

            if (dataUltimaNotaConceitoInserida.HasValue)
                notasConceitosRetornoDto.AuditoriaInserido = $"{tituloNotasOuConceitos} da avaliação {nomeAvaliacaoInclusao} inseridos por {usuarioInseriu} em {dataUltimaNotaConceitoInserida.Value.ToString("dd/MM/yyyy")}, às {dataUltimaNotaConceitoInserida.Value.ToString("HH:mm")}.";
            if (dataUltimaNotaConceitoAlterada.HasValue)
                notasConceitosRetornoDto.AuditoriaAlterado = $"{tituloNotasOuConceitos} da avaliação {nomeAvaliacaoAlteracao} alterados por {usuarioAlterou} em {dataUltimaNotaConceitoAlterada.Value.ToString("dd/MM/yyyy")}, às {dataUltimaNotaConceitoAlterada.Value.ToString("HH:mm")}.";
        }
        private void ValidaMinimoAvaliacoesBimestrais(DisciplinaDto componenteCurricular, IEnumerable<DisciplinaDto> disciplinasRegencia, TipoAvaliacao tipoAvaliacaoBimestral, NotasConceitosBimestreRetornoDto bimestreDto, IEnumerable<AtividadeAvaliativa> atividadeAvaliativas, int bimestre)
        {
            if (componenteCurricular.Regencia)
            {
                var disciplinasObservacao = new List<string>();
                foreach (var disciplinaRegencia in disciplinasRegencia)
                {
                    var avaliacoes = atividadeAvaliativas.SelectMany(a => a.Disciplinas.Where(b => b.DisciplinaId == disciplinaRegencia.CodigoComponenteCurricular.ToString()));  // await repositorioAtividadeAvaliativaRegencia.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaCodigo, disciplinaRegencia.CodigoComponenteCurricular.ToString(), bimestre);
                    if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                        disciplinasObservacao.Add(disciplinaRegencia.Nome);
                }
                if (disciplinasObservacao.Count > 0)
                    bimestreDto.Observacoes.Add($"O(s) componente(s) curricular(es) [{string.Join(",", disciplinasObservacao)}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
            else
            {
                var avaliacoes = atividadeAvaliativas.SelectMany(a => a.Disciplinas.Where(b => b.DisciplinaId == componenteCurricular.CodigoComponenteCurricular.ToString()));
                if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                    bimestreDto.Observacoes.Add($"O componente curricular [{componenteCurricular.Nome}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
        }
        private async Task<bool> VerificaPeriodoFechamentoEmAberto(Turma turma, int bimestre)
        {
            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTimeExtension.HorarioBrasilia(), bimestre, true));
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
    }
}
