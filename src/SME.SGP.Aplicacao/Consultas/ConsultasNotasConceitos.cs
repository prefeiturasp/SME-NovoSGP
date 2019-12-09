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
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;
        private readonly IServicoDeNotasConceitos servicoDeNotasConceitos;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasNotasConceitos(IServicoEOL servicoEOL, IConsultaAtividadeAvaliativa consultasAtividadeAvaliativa,
            IServicoDeNotasConceitos servicoDeNotasConceitos, IRepositorioNotasConceitos repositorioNotasConceitos,
            IRepositorioFrequencia repositorioFrequencia, IServicoUsuario servicoUsuario)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasAtividadeAvaliativa = consultasAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(consultasAtividadeAvaliativa));
            this.servicoDeNotasConceitos = servicoDeNotasConceitos ?? throw new ArgumentNullException(nameof(servicoDeNotasConceitos));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
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

            for (int i = 0; i < atividadesAvaliativaEBimestres.quantidadeBimestres; i++)
            {
                AtividadeAvaliativa atividadeAvaliativaParaObterTipoNota = null;
                var valorBimestreAtual = i + 1;
                var bimestreParaAdicionar = new NotasConceitosBimestreRetornoDto() { Descricao = $"{valorBimestreAtual}º Bimestre", Numero = valorBimestreAtual };

                if (valorBimestreAtual == atividadesAvaliativaEBimestres.bimestreAtual)
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
                            //TODO: Buscar a Nota se já foi lançada
                            string notaParaMostrar = ObterNotaParaVisualizacao(notas, aluno, atividadeAvaliativa);

                            //TODO: Buscar se houve ausencia
                            var ausente = ausenciasAtividadesAvaliativas.Any(a => a.AlunoCodigo == aluno.CodigoAluno && a.AulaData.Date == atividadeAvaliativa.DataAvaliacao.Date);

                            //TODO: Buscar se pode editar
                            bool podeEditar = PodeEditarNotaOuConceito(usuarioLogado, professorRfTitularTurmaDisciplina, atividadeAvaliativa);

                            var notaAvaliacao = new NotasConceitosNotaAvaliacaoRetornoDto() { AtividadeAvaliativaId = atividadeAvaliativa.Id, NotaConceito = notaParaMostrar, Ausente = ausente, PodeEditar = podeEditar };

                            notasAvaliacoes.Add(notaAvaliacao);
                        }

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
                }

                if (atividadeAvaliativaParaObterTipoNota != null)
                {
                    var notaTipo = servicoDeNotasConceitos.TipoNotaPorAvaliacao(atividadeAvaliativaParaObterTipoNota);
                    if (notaTipo == null)
                        throw new NegocioException("Não foi possível obter o tipo de nota desta avaliação.");

                    retorno.NotaTipo = notaTipo.TipoNota;
                }

                retorno.Bimestres.Add(bimestreParaAdicionar);
            }

            return retorno;
        }

        private static string ObterNotaParaVisualizacao(IEnumerable<NotaConceito> notas, Integracoes.Respostas.AlunoPorTurmaResposta aluno, AtividadeAvaliativa atividadeAvaliativa)
        {
            var notaDoAluno = notas.FirstOrDefault(a => a.AlunoId == aluno.CodigoAluno && a.AtividadeAvaliativaID == atividadeAvaliativa.Id);
            var notaParaMostrar = string.Empty;
            if (notaDoAluno != null)
                notaParaMostrar = notaDoAluno.ObterNota();
            return notaParaMostrar;
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
    }
}