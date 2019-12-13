using SME.SGP.Aplicacao.Integracoes;
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
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioNotaParametro repositorioNotaParametro;
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoDeNotasConceitos servicoDeNotasConceitos;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasNotasConceitos(IServicoEOL servicoEOL, IConsultaAtividadeAvaliativa consultasAtividadeAvaliativa,
            IServicoDeNotasConceitos servicoDeNotasConceitos, IRepositorioNotasConceitos repositorioNotasConceitos,
            IRepositorioFrequencia repositorioFrequencia, IServicoUsuario servicoUsuario, IServicoAluno servicoAluno, IRepositorioNotaParametro repositorioNotaParametro, IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasAtividadeAvaliativa = consultasAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(consultasAtividadeAvaliativa));
            this.servicoDeNotasConceitos = servicoDeNotasConceitos ?? throw new ArgumentNullException(nameof(servicoDeNotasConceitos));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoAluno = servicoAluno ?? throw new ArgumentNullException(nameof(servicoAluno));
            this.repositorioNotaParametro = repositorioNotaParametro ?? throw new ArgumentNullException(nameof(repositorioNotaParametro));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }

        public async Task<NotasConceitosRetornoDto> ListarNotasConceitos(string turmaCodigo, int? bimestre, int anoLetivo, string disciplinaCodigo, Modalidade modalidade)
        {
            ModalidadeTipoCalendario modalidadeTipoCalendario = (modalidade == Modalidade.Fundamental || modalidade == Modalidade.Medio) ? ModalidadeTipoCalendario.FundamentalMedio : ModalidadeTipoCalendario.EJA;

            var atividadesAvaliativaEBimestres = await consultasAtividadeAvaliativa.ObterAvaliacoesEBimestres(turmaCodigo, disciplinaCodigo, anoLetivo, bimestre, modalidadeTipoCalendario);

            if (atividadesAvaliativaEBimestres.Item1 == null || !atividadesAvaliativaEBimestres.Item1.Any())
                throw new NegocioException("Não foi possível localizar atividades avaliativas");

            var alunos = await servicoEOL.ObterAlunosPorTurma(turmaCodigo);

            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");

            var retorno = new NotasConceitosRetornoDto();
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            retorno.BimestreAtual = atividadesAvaliativaEBimestres.periodoAtual.Bimestre;

            DateTime? dataUltimaNotaConceitoInserida = null;
            DateTime? dataUltimaNotaConceitoAlterada = null;
            var usuarioRfUltimaNotaConceitoInserida = string.Empty;
            var usuarioRfUltimaNotaConceitoAlterada = string.Empty;
            var nomeAvaliacaoAuditoriaInclusao = string.Empty;
            var nomeAvaliacaoAuditoriaAlteracao = string.Empty;

            for (int i = 0; i < atividadesAvaliativaEBimestres.quantidadeBimestres; i++)
            {
                AtividadeAvaliativa atividadeAvaliativaParaObterTipoNota = null;
                var valorBimestreAtual = i + 1;
                var bimestreParaAdicionar = new NotasConceitosBimestreRetornoDto() { Descricao = $"{valorBimestreAtual}º Bimestre", Numero = valorBimestreAtual };

                if (valorBimestreAtual == atividadesAvaliativaEBimestres.periodoAtual.Bimestre)
                {
                    var listaAlunosDoBimestre = new List<NotasConceitosAlunoRetornoDto>();

                    var atividadesAvaliativasdoBimestre = atividadesAvaliativaEBimestres.Item1.Where(a => a.DataAvaliacao.Date >= atividadesAvaliativaEBimestres.periodoAtual.PeriodoInicio.Date
                        && atividadesAvaliativaEBimestres.periodoAtual.PeriodoFim.Date >= a.DataAvaliacao.Date)
                        .OrderBy(a => a.DataAvaliacao)
                        .ToList();
                    var alunosIds = alunos.Select(a => a.CodigoAluno).Distinct();
                    var notas = repositorioNotasConceitos.ObterNotasPorAlunosAtividadesAvaliativas(atividadesAvaliativasdoBimestre.Select(a => a.Id).Distinct(), alunosIds);
                    var ausenciasAtividadesAvaliativas = await repositorioFrequencia.ObterAusencias(turmaCodigo, disciplinaCodigo, atividadesAvaliativasdoBimestre.Select(a => a.DataAvaliacao).Distinct().ToArray(), alunosIds.ToArray());

                    var professorRfTitularTurmaDisciplina = string.Empty;

                    professorRfTitularTurmaDisciplina = await ObterRfProfessorTitularDisciplina(turmaCodigo, disciplinaCodigo, atividadesAvaliativasdoBimestre);

                    foreach (var aluno in alunos.OrderBy(a => a.NumeroAlunoChamada).ThenBy(a => a.NomeValido()))
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

                            bool podeEditar = PodeEditarNotaOuConceito(usuarioLogado, professorRfTitularTurmaDisciplina, atividadeAvaliativa);

                            var notaAvaliacao = new NotasConceitosNotaAvaliacaoRetornoDto() { AtividadeAvaliativaId = atividadeAvaliativa.Id, NotaConceito = notaParaVisualizar, Ausente = ausente, PodeEditar = podeEditar };
                            notasAvaliacoes.Add(notaAvaliacao);
                        }

                        notaConceitoAluno.Marcador = servicoAluno.ObterMarcadorAluno(aluno, new PeriodoEscolarDto()
                        {
                            Bimestre = valorBimestreAtual,
                            PeriodoInicio = atividadesAvaliativaEBimestres.periodoAtual.PeriodoInicio,
                            PeriodoFim = atividadesAvaliativaEBimestres.periodoAtual.PeriodoFim
                        });

                        notaConceitoAluno.NotasAvaliacoes = notasAvaliacoes;
                        listaAlunosDoBimestre.Add(notaConceitoAluno);
                    }

                    foreach (var avaliacao in atividadesAvaliativasdoBimestre)
                    {
                        var avaliacaoDoBimestre = new NotasConceitosAvaliacaoRetornoDto() { Id = avaliacao.Id, Data = avaliacao.DataAvaliacao, Descricao = avaliacao.DescricaoAvaliacao, Nome = avaliacao.NomeAvaliacao };
                        bimestreParaAdicionar.Avaliacoes.Add(avaliacaoDoBimestre);

                        if (atividadeAvaliativaParaObterTipoNota == null)
                            atividadeAvaliativaParaObterTipoNota = avaliacao;
                    }
                    bimestreParaAdicionar.Alunos = listaAlunosDoBimestre;

                    if (atividadeAvaliativaParaObterTipoNota != null)
                    {
                        var notaTipo = servicoDeNotasConceitos.TipoNotaPorAvaliacao(atividadeAvaliativaParaObterTipoNota);
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

        public TipoNota ObterNotaTipo(long turmaId, int anoLetivo)
        {
            var notaTipo = servicoDeNotasConceitos.TipoNotaPorAvaliacao(new AtividadeAvaliativa()
            {
                TurmaId = turmaId.ToString(),
                DataAvaliacao = new DateTime(anoLetivo, 3, 1)
            });

            return notaTipo.TipoNota;
        }

        public double ObterValorArredondado(long atividadeAvaliativaId, double nota)
        {
            var atividadeAvaliativa = repositorioAtividadeAvaliativa.ObterPorId(atividadeAvaliativaId);
            if (atividadeAvaliativa == null)
                throw new NegocioException("Não foi possível localizar a atividade avaliativa.");

            var notaParametro = repositorioNotaParametro.ObterPorDataAvaliacao(atividadeAvaliativa.DataAvaliacao);
            if (notaParametro == null)
                throw new NegocioException("Não foi possível localizar o parâmetro da nota.");

            return notaParametro.Arredondar(nota);
        }

        private static NotaConceito ObterNotaParaVisualizacao(IEnumerable<NotaConceito> notas, AlunoPorTurmaResposta aluno, AtividadeAvaliativa atividadeAvaliativa)
        {
            var notaDoAluno = notas.FirstOrDefault(a => a.AlunoId == aluno.CodigoAluno && a.AtividadeAvaliativaID == atividadeAvaliativa.Id);

            return notaDoAluno;
        }

        private static bool PodeEditarNotaOuConceito(Usuario usuarioLogado, string professorTitularDaTurmaDisciplinaRf, AtividadeAvaliativa atividadeAvaliativa)
        {
            var podeEditar = true;
            if (atividadeAvaliativa.DataAvaliacao.Date > DateTime.Today)
                podeEditar = false;

            if (usuarioLogado.PerfilAtual == Perfis.PERFIL_CJ)
            {
                if (atividadeAvaliativa.CriadoRF != usuarioLogado.CodigoRf)
                    podeEditar = false;
            }
            else
            {
                if (usuarioLogado.CodigoRf != professorTitularDaTurmaDisciplinaRf)
                    podeEditar = false;
            }

            return podeEditar;
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
            //retornoMock.AuditoriaAlterado = "Notas (ou conceitos) da avaliação XYZ alterados por por Nome Usuário(9999999) em 10/01/2019, às 15:00.";
            //retornoMock.AuditoriaInserido = "Notas (ou conceitos) da avaliação XYZ inseridos por por Nome Usuário(9999999) em 10/01/2019, às 15:00.";
            var tituloNotasOuConceitos = tipoNota == TipoNota.Conceito ? "Conceitos" : "Notas";

            if (dataUltimaNotaConceitoInserida.HasValue)
                notasConceitosRetornoDto.AuditoriaInserido = $"{tituloNotasOuConceitos} da avaliação {nomeAvaliacaoInclusao} inseridos por Nome {usuarioInseriu} em {dataUltimaNotaConceitoInserida.Value.Day}/{dataUltimaNotaConceitoInserida.Value.Month}/{dataUltimaNotaConceitoInserida.Value.Year}, às {dataUltimaNotaConceitoInserida.Value.TimeOfDay.Hours}:{dataUltimaNotaConceitoInserida.Value.TimeOfDay.Minutes}.";
            if (dataUltimaNotaConceitoAlterada.HasValue)
                notasConceitosRetornoDto.AuditoriaAlterado = $"{tituloNotasOuConceitos} da avaliação {nomeAvaliacaoAlteracao} alterados por Nome {usuarioAlterou} em {dataUltimaNotaConceitoAlterada.Value.Day}/{dataUltimaNotaConceitoAlterada.Value.Month}/{dataUltimaNotaConceitoAlterada.Value.Year}, às {dataUltimaNotaConceitoAlterada.Value.TimeOfDay.Hours}:{dataUltimaNotaConceitoAlterada.Value.TimeOfDay.Minutes}.";
        }
    }
}