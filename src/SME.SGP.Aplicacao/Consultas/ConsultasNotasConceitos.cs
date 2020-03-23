using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasNotasConceitos : IConsultasNotasConceitos
    {
        private readonly IConsultaAtividadeAvaliativa consultasAtividadeAvaliativa;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasFechamento consultasFechamento;
        private readonly IConsultasFechamentoTurmaDisciplina consultasFechamentoTurmaDisciplina;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IRepositorioConceito repositorioConceito;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAluno;
        private readonly IRepositorioNotaParametro repositorioNotaParametro;
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoDeNotasConceitos servicoDeNotasConceitos;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasNotasConceitos(IServicoEOL servicoEOL, IConsultaAtividadeAvaliativa consultasAtividadeAvaliativa,
            IConsultasFechamentoTurmaDisciplina consultasFechamentoTurmaDisciplina, IConsultasDisciplina consultasDisciplina,
            IConsultasFechamento consultasFechamento,
            IServicoDeNotasConceitos servicoDeNotasConceitos, IRepositorioNotasConceitos repositorioNotasConceitos,
            IRepositorioFrequencia repositorioFrequencia, IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAluno,
            IServicoUsuario servicoUsuario, IServicoAluno servicoAluno, IRepositorioTipoCalendario repositorioTipoCalendario,
            IRepositorioNotaParametro repositorioNotaParametro, IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina, IRepositorioConceito repositorioConceito,
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioParametrosSistema repositorioParametrosSistema,
            IRepositorioTipoAvaliacao repositorioTipoAvaliacao, IRepositorioTurma repositorioTurma, IRepositorioUe repositorioUe,
            IRepositorioDre repositorioDre, IRepositorioEvento repositorioEvento, IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasAtividadeAvaliativa = consultasAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(consultasAtividadeAvaliativa));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasFechamentoTurmaDisciplina = consultasFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(consultasFechamentoTurmaDisciplina));
            this.consultasFechamento = consultasFechamento ?? throw new ArgumentNullException(nameof(consultasFechamento));
            this.servicoDeNotasConceitos = servicoDeNotasConceitos ?? throw new ArgumentNullException(nameof(servicoDeNotasConceitos));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioFrequenciaAluno = repositorioFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAluno));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoAluno = servicoAluno ?? throw new ArgumentNullException(nameof(servicoAluno));
            this.repositorioNotaParametro = repositorioNotaParametro ?? throw new ArgumentNullException(nameof(repositorioNotaParametro));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioTipoAvaliacao = repositorioTipoAvaliacao ?? throw new ArgumentNullException(nameof(repositorioTipoAvaliacao));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaRegencia));
        }

        public async Task<NotasConceitosRetornoDto> ListarNotasConceitos(ListaNotasConceitosConsultaDto filtro)
        {
            var modalidadeTipoCalendario = ObterModalidadeCalendario(filtro.Modalidade);

            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(filtro.AnoLetivo, modalidadeTipoCalendario);
            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            var bimestre = filtro.Bimestre;
            if (!bimestre.HasValue || bimestre == 0)
                bimestre = ObterBimestreAtual(periodosEscolares);

            var periodoAtual = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestre);
            if (periodoAtual == null)
                throw new NegocioException("Não foi encontrado período escolar para o bimestre solicitado.");

            List<AtividadeAvaliativa> atividadesAvaliativaEBimestres = new List<AtividadeAvaliativa>();
            // Carrega disciplinas filhas da disciplina passada como parametro
            var disciplinasProfessor = await consultasDisciplina.ObterDisciplinasPorProfessorETurma(filtro.TurmaCodigo, true);
            var disciplinasFilha = disciplinasProfessor.Where(d => d.CdComponenteCurricularPai == int.Parse(filtro.DisciplinaCodigo));

            if (disciplinasFilha.Any())
            {
                foreach (var disciplinaFilha in disciplinasFilha)
                    atividadesAvaliativaEBimestres.AddRange(await consultasAtividadeAvaliativa.ObterAvaliacoesNoBimestre(filtro.TurmaCodigo, disciplinaFilha.CodigoComponenteCurricular.ToString(), periodoAtual.PeriodoInicio, periodoAtual.PeriodoFim));
            }
            else
                // Disciplina não tem disciplinas filhas então carrega avaliações da propria
                atividadesAvaliativaEBimestres.AddRange(await consultasAtividadeAvaliativa.ObterAvaliacoesNoBimestre(filtro.TurmaCodigo, filtro.DisciplinaCodigo, periodoAtual.PeriodoInicio, periodoAtual.PeriodoFim));

            if (atividadesAvaliativaEBimestres is null || !atividadesAvaliativaEBimestres.Any())
                return ObterRetornoGenericoBimestreAtualVazio(periodosEscolares, bimestre.Value);

            var alunos = await servicoEOL.ObterAlunosPorTurma(filtro.TurmaCodigo);
            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");

            var retorno = new NotasConceitosRetornoDto();
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            var tipoAvaliacaoBimestral = await repositorioTipoAvaliacao.ObterTipoAvaliacaoBimestral();

            retorno.BimestreAtual = bimestre.Value;
            retorno.MediaAprovacaoBimestre = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));
            retorno.MinimoAvaliacoesBimestrais = tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre;
            retorno.PercentualAlunosInsuficientes = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.PercentualAlunosInsuficientes));

            DateTime? dataUltimaNotaConceitoInserida = null;
            DateTime? dataUltimaNotaConceitoAlterada = null;
            var usuarioRfUltimaNotaConceitoInserida = string.Empty;
            var usuarioRfUltimaNotaConceitoAlterada = string.Empty;
            var nomeAvaliacaoAuditoriaInclusao = string.Empty;
            var nomeAvaliacaoAuditoriaAlteracao = string.Empty;

            foreach (var periodoEscolar in periodosEscolares)
            {
                AtividadeAvaliativa atividadeAvaliativaParaObterTipoNota = null;
                var valorBimestreAtual = periodoEscolar.Bimestre;
                var bimestreParaAdicionar = new NotasConceitosBimestreRetornoDto()
                {
                    Descricao = $"{valorBimestreAtual}º Bimestre",
                    Numero = valorBimestreAtual,
                    PeriodoInicio = periodoEscolar.PeriodoInicio,
                    PeriodoFim = periodoEscolar.PeriodoFim
                };

                if (valorBimestreAtual == periodoAtual.Bimestre)
                {
                    var listaAlunosDoBimestre = new List<NotasConceitosAlunoRetornoDto>();

                    var atividadesAvaliativasdoBimestre = atividadesAvaliativaEBimestres.Where(a => a.DataAvaliacao.Date >= periodoAtual.PeriodoInicio.Date
                        && periodoAtual.PeriodoFim.Date >= a.DataAvaliacao.Date)
                        .OrderBy(a => a.DataAvaliacao)
                        .ToList();
                    var alunosIds = alunos.Select(a => a.CodigoAluno).Distinct();
                    var notas = repositorioNotasConceitos.ObterNotasPorAlunosAtividadesAvaliativas(atividadesAvaliativasdoBimestre.Select(a => a.Id).Distinct(), alunosIds, filtro.DisciplinaCodigo);
                    var ausenciasAtividadesAvaliativas = await repositorioFrequencia.ObterAusencias(filtro.TurmaCodigo, filtro.DisciplinaCodigo, atividadesAvaliativasdoBimestre.Select(a => a.DataAvaliacao).Distinct().ToArray(), alunosIds.ToArray());

                    var consultaEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(filtro.DisciplinaCodigo) });
                    if (consultaEOL == null || !consultaEOL.Any())
                        throw new NegocioException("Disciplina informada não encontrada no EOL");
                    var disciplinaEOL = consultaEOL.First();

                    IEnumerable<DisciplinaResposta> disciplinasRegencia = null;

                    if (disciplinaEOL.Regencia)
                        disciplinasRegencia = await servicoEOL.ObterDisciplinasParaPlanejamento(long.Parse(filtro.TurmaCodigo), servicoUsuario.ObterLoginAtual(), servicoUsuario.ObterPerfilAtual());

                    var professorRfTitularTurmaDisciplina = string.Empty;

                    professorRfTitularTurmaDisciplina = await ObterRfProfessorTitularDisciplina(filtro.TurmaCodigo, filtro.DisciplinaCodigo, atividadesAvaliativasdoBimestre);
                    var fechamentoTurma = await consultasFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(filtro.TurmaCodigo, long.Parse(filtro.DisciplinaCodigo), valorBimestreAtual);

                    foreach (var aluno in alunos.Where(a => a.NumeroAlunoChamada > 0 || a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Ativo)).OrderBy(a => a.NumeroAlunoChamada).ThenBy(a => a.NomeValido()))
                    {
                        var notaConceitoAluno = new NotasConceitosAlunoRetornoDto() { Id = aluno.CodigoAluno, Nome = aluno.NomeValido(), NumeroChamada = aluno.NumeroAlunoChamada };
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
                                if (notaDoAluno.AlteradoEm.HasValue)
                                {
                                    if (!dataUltimaNotaConceitoAlterada.HasValue || notaDoAluno.AlteradoEm.Value > dataUltimaNotaConceitoAlterada.Value)
                                    {
                                        usuarioRfUltimaNotaConceitoAlterada = $"{notaDoAluno.AlteradoPor}({notaDoAluno.AlteradoRF})";
                                        dataUltimaNotaConceitoAlterada = notaDoAluno.AlteradoEm;
                                        nomeAvaliacaoAuditoriaAlteracao = atividadeAvaliativa.NomeAvaliacao;
                                    }
                                }
                            }

                            var ausente = ausenciasAtividadesAvaliativas.Any(a => a.AlunoCodigo == aluno.CodigoAluno && a.AulaData.Date == atividadeAvaliativa.DataAvaliacao.Date);

                            bool podeEditar = PodeEditarNotaOuConceito(usuarioLogado, professorRfTitularTurmaDisciplina, atividadeAvaliativa, aluno);

                            var notaAvaliacao = new NotasConceitosNotaAvaliacaoRetornoDto() { AtividadeAvaliativaId = atividadeAvaliativa.Id, NotaConceito = notaParaVisualizar, Ausente = ausente, PodeEditar = podeEditar };
                            notasAvaliacoes.Add(notaAvaliacao);
                        }

                        notaConceitoAluno.Marcador = servicoAluno.ObterMarcadorAluno(aluno, new PeriodoEscolarDto()
                        {
                            Bimestre = valorBimestreAtual,
                            PeriodoInicio = periodoAtual.PeriodoInicio,
                            PeriodoFim = periodoAtual.PeriodoFim
                        });

                        notaConceitoAluno.PodeEditar = notaConceitoAluno.Marcador == null || notaConceitoAluno.Marcador.Tipo == TipoMarcadorFrequencia.Novo;
                        notaConceitoAluno.NotasAvaliacoes = notasAvaliacoes;

                        // Carrega Notas do Bimestre
                        if (fechamentoTurma != null)
                        {
                            bimestreParaAdicionar.FechamentoTurmaId = fechamentoTurma.Id;
                            retorno.AuditoriaBimestreInserido = $"Nota final do bimestre inserida por {fechamentoTurma.CriadoPor} em {fechamentoTurma.CriadoEm.ToString("dd/MM/yyyy")}, às {fechamentoTurma.CriadoEm.ToString("hh:mm:ss")}.";
                            if (fechamentoTurma.AlteradoEm.HasValue)
                                retorno.AuditoriaBimestreAlterado = $"Nota final do bimestre alterada por {fechamentoTurma.AlteradoPor} em {fechamentoTurma.AlteradoEm.Value.ToString("dd/MM/yyyy")}, às {fechamentoTurma.AlteradoEm.Value.ToString("hh:mm:ss")}.";

                            var notasConceitoBimestre = await consultasFechamentoTurmaDisciplina.ObterNotasBimestre(aluno.CodigoAluno, fechamentoTurma.Id);
                            if (disciplinaEOL.Regencia)
                            {
                                // Regencia carrega disciplinas mesmo sem nota de fechamento
                                foreach (var disciplinaRegencia in disciplinasRegencia)
                                {
                                    var nota = new NotaConceitoBimestreRetornoDto()
                                    {
                                        DisciplinaId = disciplinaRegencia.CodigoComponenteCurricular,
                                        Disciplina = disciplinaRegencia.Nome,
                                    };
                                    var notaRegencia = notasConceitoBimestre?.FirstOrDefault(c => c.DisciplinaId == disciplinaRegencia.CodigoComponenteCurricular);
                                    if (notaRegencia != null)
                                        nota.NotaConceito = (notaRegencia.Nota > 0 ? notaRegencia.Nota : notaRegencia.ConceitoId).ToString();

                                    notaConceitoAluno.NotasBimestre.Add(nota);
                                }
                            }
                            else
                                foreach (var notaConceitoBimestre in notasConceitoBimestre)
                                    notaConceitoAluno.NotasBimestre.Add(new NotaConceitoBimestreRetornoDto()
                                    {
                                        DisciplinaId = notaConceitoBimestre.DisciplinaId,
                                        Disciplina = disciplinaEOL.Nome,
                                        NotaConceito = notaConceitoBimestre.Nota > 0 ?
                                            notaConceitoBimestre.Nota.ToString() :
                                            notaConceitoBimestre.ConceitoId.ToString()
                                    });
                        }
                        else
                        if (disciplinaEOL.Regencia)
                        {
                            // Regencia carrega disciplinas mesmo sem nota de fechamento
                            foreach (var disciplinaRegencia in disciplinasRegencia)
                            {
                                notaConceitoAluno.NotasBimestre.Add(new NotaConceitoBimestreRetornoDto()
                                {
                                    DisciplinaId = disciplinaRegencia.CodigoComponenteCurricular,
                                    Disciplina = disciplinaRegencia.Nome,
                                });
                            }
                        }

                        // Carrega Frequencia Aluno
                        var frequenciaAluno = repositorioFrequenciaAluno.ObterPorAlunoData(aluno.CodigoAluno, periodoAtual.PeriodoFim, TipoFrequenciaAluno.PorDisciplina, filtro.DisciplinaCodigo);
                        notaConceitoAluno.PercentualFrequencia = frequenciaAluno != null ?
                                        (int)Math.Round(frequenciaAluno.PercentualFrequencia, 0) :
                                        100;

                        listaAlunosDoBimestre.Add(notaConceitoAluno);
                    }

                    foreach (var avaliacao in atividadesAvaliativasdoBimestre)
                    {
                        var avaliacaoDoBimestre = new NotasConceitosAvaliacaoRetornoDto()
                        {
                            Id = avaliacao.Id,
                            Data = avaliacao.DataAvaliacao,
                            Descricao = avaliacao.DescricaoAvaliacao,
                            Nome = avaliacao.NomeAvaliacao
                        };
                        if (avaliacao.Categoria.Equals(CategoriaAtividadeAvaliativa.Interdisciplinar))
                        {
                            avaliacaoDoBimestre.EhInterdisciplinar = true;
                            var atividadeDisciplinas = await repositorioAtividadeAvaliativaDisciplina.ListarPorIdAtividade(avaliacao.Id);
                            var idsDisciplinas = atividadeDisciplinas.Select(a => long.Parse(a.DisciplinaId)).ToArray();
                            var disciplinas = servicoEOL.ObterDisciplinasPorIds(idsDisciplinas);
                            var nomesDisciplinas = disciplinas.Select(d => d.Nome).ToArray();
                            avaliacaoDoBimestre.Disciplinas = nomesDisciplinas;
                        }
                        bimestreParaAdicionar.Avaliacoes.Add(avaliacaoDoBimestre);

                        if (atividadeAvaliativaParaObterTipoNota == null)
                            atividadeAvaliativaParaObterTipoNota = avaliacao;
                    }
                    bimestreParaAdicionar.Alunos = listaAlunosDoBimestre;
                    bimestreParaAdicionar.QtdAvaliacoesBimestrais = atividadesAvaliativasdoBimestre.Where(x => x.TipoAvaliacaoId == tipoAvaliacaoBimestral.Id).Count();
                    bimestreParaAdicionar.PodeLancarNotaFinal = await VerificaPeriodoFechamentoEmAberto(filtro.TurmaCodigo, periodoAtual.Bimestre);

                    // Valida Avaliações Bimestrais
                    await ValidaMinimoAvaliacoesBimestrais(disciplinaEOL, disciplinasRegencia, tipoCalendario.Id, filtro.TurmaCodigo, valorBimestreAtual, tipoAvaliacaoBimestral, bimestreParaAdicionar);

                    if (atividadeAvaliativaParaObterTipoNota != null)
                    {
                        var notaTipo = await servicoDeNotasConceitos.TipoNotaPorAvaliacao(atividadeAvaliativaParaObterTipoNota, filtro.TurmaHistorico);
                        if (notaTipo == null)
                            throw new NegocioException("Não foi possível obter o tipo de nota desta avaliação.");

                        retorno.NotaTipo = notaTipo.TipoNota;
                        ObterValoresDeAuditoria(dataUltimaNotaConceitoInserida, dataUltimaNotaConceitoAlterada, usuarioRfUltimaNotaConceitoInserida, usuarioRfUltimaNotaConceitoAlterada, notaTipo.TipoNota, retorno, nomeAvaliacaoAuditoriaInclusao, nomeAvaliacaoAuditoriaAlteracao);
                    }
                }

                retorno.Bimestres.Add(bimestreParaAdicionar);
            }

            return retorno;
        }

        public IEnumerable<ConceitoDto> ObterConceitos(DateTime data)
            => MapearParaDto(repositorioConceito.ObterPorData(data));

        public async Task<TipoNota> ObterNotaTipo(long turmaId, int anoLetivo, bool consideraHistorico)
        {
            var notaTipo = await servicoDeNotasConceitos.TipoNotaPorAvaliacao(new AtividadeAvaliativa()
            {
                TurmaId = turmaId.ToString(),
                DataAvaliacao = new DateTime(anoLetivo, 3, 1)
            }, consideraHistorico);

            return notaTipo.TipoNota;
        }

        public double ObterValorArredondado(long atividadeAvaliativaId, double nota)
        {
            var atividadeAvaliativa = repositorioAtividadeAvaliativa.ObterPorId(atividadeAvaliativaId);
            if (atividadeAvaliativa == null)
                throw new NegocioException("Não foi possível localizar a atividade avaliativa.");

            return ObterValorArredondado(atividadeAvaliativa.DataAvaliacao, nota);
        }

        public double ObterValorArredondado(DateTime data, double nota)
        {
            var notaParametro = repositorioNotaParametro.ObterPorDataAvaliacao(data);
            if (notaParametro == null)
                throw new NegocioException("Não foi possível localizar o parâmetro da nota.");

            return notaParametro.Arredondar(nota);
        }

        private static ModalidadeTipoCalendario ObterModalidadeCalendario(Modalidade modalidade)
        {
            return modalidade == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio;
        }

        private static NotaConceito ObterNotaParaVisualizacao(IEnumerable<NotaConceito> notas, AlunoPorTurmaResposta aluno, AtividadeAvaliativa atividadeAvaliativa)
        {
            var notaDoAluno = notas.FirstOrDefault(a => a.AlunoId == aluno.CodigoAluno && a.AtividadeAvaliativaID == atividadeAvaliativa.Id);

            return notaDoAluno;
        }

        private static bool PodeEditarNotaOuConceito(Usuario usuarioLogado, string professorTitularDaTurmaDisciplinaRf,
            AtividadeAvaliativa atividadeAvaliativa, AlunoPorTurmaResposta aluno)
        {
            if (atividadeAvaliativa.DataAvaliacao >= aluno.DataSituacao &&
                (aluno.CodigoSituacaoMatricula != SituacaoMatriculaAluno.Ativo &&
                aluno.CodigoSituacaoMatricula != SituacaoMatriculaAluno.PendenteRematricula &&
                aluno.CodigoSituacaoMatricula != SituacaoMatriculaAluno.Rematriculado &&
                aluno.CodigoSituacaoMatricula != SituacaoMatriculaAluno.SemContinuidade))
                return false;

            if (atividadeAvaliativa.DataAvaliacao.Date > DateTime.Today)
                return false;

            if (usuarioLogado.PerfilAtual == Perfis.PERFIL_CJ)
            {
                if (atividadeAvaliativa.CriadoRF != usuarioLogado.CodigoRf)
                    return false;
            }
            else
            {
                if (usuarioLogado.CodigoRf != professorTitularDaTurmaDisciplinaRf)
                    return false;
            }

            return true;
        }

        private IEnumerable<ConceitoDto> MapearParaDto(IEnumerable<Conceito> conceitos)
        {
            foreach (var conceito in conceitos)
            {
                yield return new ConceitoDto()
                {
                    Id = conceito.Id,
                    Valor = conceito.Valor,
                    Descricao = conceito.Descricao,
                    Aprovado = conceito.Aprovado
                };
            }
        }

        private int ObterBimestreAtual(IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dataPesquisa = DateTime.Now;

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);

            if (periodoEscolar == null)
                return 1;
            else return periodoEscolar.Bimestre;
        }

        private NotasConceitosBimestreRetornoDto ObterBimestreGenerico(int bimestreAtual)
        {
            return new NotasConceitosBimestreRetornoDto()
            {
                Descricao = $"{bimestreAtual}º Bimestre",
                Numero = bimestreAtual
            };
        }

        private IEnumerable<NotasConceitosBimestreRetornoDto> ObterListaBimestreGenerico(int quantidadeBimestres)
        {
            for (int i = 0; i < quantidadeBimestres; i++)
            {
                yield return ObterBimestreGenerico(i + 1);
            }
        }

        private NotasConceitosRetornoDto ObterRetornoGenericoBimestreAtualVazio(IEnumerable<PeriodoEscolar> periodosEscolares, int bimestre)
        {
            return new NotasConceitosRetornoDto
            {
                BimestreAtual = bimestre,
                Bimestres = ObterListaBimestreGenerico(periodosEscolares.Count()).ToList(),
            };
        }

        private async Task<string> ObterRfProfessorTitularDisciplina(string turmaCodigo, string disciplinaCodigo, List<AtividadeAvaliativa> atividadesAvaliativasdoBimestre)
        {
            if (atividadesAvaliativasdoBimestre.Any())
            {
                var professoresTitularesDaTurma = await servicoEOL.ObterProfessoresTitularesDisciplinas(turmaCodigo);
                var professorTitularDaDisciplina = professoresTitularesDaTurma.FirstOrDefault(a => a.DisciplinaId == int.Parse(disciplinaCodigo) && a.ProfessorRf != string.Empty);
                return professorTitularDaDisciplina == null ? string.Empty : professorTitularDaDisciplina.ProfessorRf;
            }

            return string.Empty;
        }

        private void ObterValoresDeAuditoria(DateTime? dataUltimaNotaConceitoInserida, DateTime? dataUltimaNotaConceitoAlterada, string usuarioInseriu, string usuarioAlterou, TipoNota tipoNota, NotasConceitosRetornoDto notasConceitosRetornoDto, string nomeAvaliacaoInclusao, string nomeAvaliacaoAlteracao)
        {
            var tituloNotasOuConceitos = tipoNota == TipoNota.Conceito ? "Conceitos" : "Notas";

            if (dataUltimaNotaConceitoInserida.HasValue)
                notasConceitosRetornoDto.AuditoriaInserido = $"{tituloNotasOuConceitos} da avaliação {nomeAvaliacaoInclusao} inseridos por Nome {usuarioInseriu} em {dataUltimaNotaConceitoInserida.Value.Day}/{dataUltimaNotaConceitoInserida.Value.Month}/{dataUltimaNotaConceitoInserida.Value.Year}, às {dataUltimaNotaConceitoInserida.Value.TimeOfDay.Hours}:{dataUltimaNotaConceitoInserida.Value.TimeOfDay.Minutes}.";
            if (dataUltimaNotaConceitoAlterada.HasValue)
                notasConceitosRetornoDto.AuditoriaAlterado = $"{tituloNotasOuConceitos} da avaliação {nomeAvaliacaoAlteracao} alterados por Nome {usuarioAlterou} em {dataUltimaNotaConceitoAlterada.Value.Day}/{dataUltimaNotaConceitoAlterada.Value.Month}/{dataUltimaNotaConceitoAlterada.Value.Year}, às {dataUltimaNotaConceitoAlterada.Value.TimeOfDay.Hours}:{dataUltimaNotaConceitoAlterada.Value.TimeOfDay.Minutes}.";
        }

        private async Task ValidaMinimoAvaliacoesBimestrais(DisciplinaDto disciplinaEOL, IEnumerable<DisciplinaResposta> disciplinasRegencia, long tipoCalendarioId, string turmaCodigo, int bimestre, TipoAvaliacao tipoAvaliacaoBimestral, NotasConceitosBimestreRetornoDto bimestreDto)
        {
            if (disciplinaEOL.Regencia)
            {
                var disciplinasObservacao = new List<string>();
                foreach (var disciplinaRegencia in disciplinasRegencia)
                {
                    var avaliacoes = await repositorioAtividadeAvaliativaRegencia.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaCodigo, disciplinaRegencia.CodigoComponenteCurricular.ToString(), bimestre);
                    if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                        disciplinasObservacao.Add(disciplinaRegencia.Nome);
                }
                if (disciplinasObservacao.Count > 0)
                    bimestreDto.Observacoes.Add($"A(s) disciplina(s) [{string.Join(",", disciplinasObservacao)}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
            else
            {
                var avaliacoes = await repositorioAtividadeAvaliativaDisciplina.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaCodigo, disciplinaEOL.CodigoComponenteCurricular.ToString(), bimestre);
                if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                    bimestreDto.Observacoes.Add($"A disciplina [{disciplinaEOL.Nome}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
        }

        private async Task<bool> VerificaPeriodoFechamentoEmAberto(string turmaCodigo, int bimestre)
            => await consultasFechamento.TurmaEmPeriodoDeFechamento(turmaCodigo, DateTime.Now, bimestre);
    }
}