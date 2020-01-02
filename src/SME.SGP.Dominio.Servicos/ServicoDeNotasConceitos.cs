using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public class ServicoDeNotasConceitos : IServicoDeNotasConceitos
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioCiclo repositorioCiclo;
        private readonly IRepositorioConceito repositorioConceito;
        private readonly IRepositorioNotaParametro repositorioNotaParametro;
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;
        private readonly IRepositorioNotaTipoValor repositorioNotaTipoValor;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IServicoNotificacao servicoNotificacao;

        public ServicoDeNotasConceitos(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IServicoEOL servicoEOL, IConsultasAbrangencia consultasAbrangencia,
            IRepositorioNotaTipoValor repositorioNotaTipoValor, IRepositorioCiclo repositorioCiclo,
            IRepositorioConceito repositorioConceito, IRepositorioNotaParametro repositorioNotaParametro,
            IRepositorioNotasConceitos repositorioNotasConceitos, IUnitOfWork unitOfWork,
            IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina,
            IServicoNotificacao servicoNotificacao, IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IRepositorioAula repositorioAula, IRepositorioTurma repositorioTurma, IServicoUsuario servicoUsuario)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.repositorioCiclo = repositorioCiclo ?? throw new ArgumentNullException(nameof(repositorioCiclo));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.repositorioNotaParametro = repositorioNotaParametro ?? throw new ArgumentNullException(nameof(repositorioNotaParametro));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task Salvar(IEnumerable<NotaConceito> notasConceitos, string professorRf, string turmaId, string disciplinaId)
        {
            var idsAtividadesAvaliativas = notasConceitos.Select(x => x.AtividadeAvaliativaID);

            var atividadesAvaliativas = repositorioAtividadeAvaliativa.ListarPorIds(idsAtividadesAvaliativas);

            var alunos = await servicoEOL.ObterAlunosPorTurma(turmaId);

            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado nenhum aluno para a turma informada");

            ValidarAvaliacoes(idsAtividadesAvaliativas, atividadesAvaliativas, professorRf);

            var entidadesSalvar = new List<NotaConceito>();

            var notasPorAvaliacoes = notasConceitos.GroupBy(x => x.AtividadeAvaliativaID);

            foreach (var notasPorAvaliacao in notasPorAvaliacoes)
            {
                var avaliacao = atividadesAvaliativas.FirstOrDefault(x => x.Id == notasPorAvaliacao.Key);

                entidadesSalvar.AddRange(ValidarEObter(notasPorAvaliacao.ToList(), avaliacao, alunos, professorRf, disciplinaId));
            }

            SalvarNoBanco(entidadesSalvar);
        }

        public NotaTipoValor TipoNotaPorAvaliacao(AtividadeAvaliativa atividadeAvaliativa)
        {
            var notaTipo = ObterNotaTipo(atividadeAvaliativa.TurmaId, atividadeAvaliativa.DataAvaliacao).Result;

            if (notaTipo == null)
                throw new NegocioException("Não foi encontrado tipo de nota para a avaliação informada");

            return notaTipo;
        }

        private static void ValidarSeAtividadesAvaliativasExistem(IEnumerable<long> avaliacoesAlteradasIds, IEnumerable<AtividadeAvaliativa> avaliacoes)
        {
            avaliacoesAlteradasIds.ToList().ForEach(avalicaoAlteradaId =>
            {
                var atividadeavaliativa = avaliacoes.FirstOrDefault(avaliacao => avaliacao.Id == avalicaoAlteradaId);

                if (atividadeavaliativa == null)
                    throw new NegocioException($"Não foi encontrada atividade avaliativa com o codigo {avalicaoAlteradaId}");
            });
        }

        private async Task<NotaTipoValor> ObterNotaTipo(string turmaId, DateTime dataAvaliacao)
        {
            var turma = await consultasAbrangencia.ObterAbrangenciaTurma(turmaId);

            if (turma == null)
                throw new NegocioException("Não foi encontrada a turma informada");

            var ciclo = repositorioCiclo.ObterCicloPorAnoModalidade(turma.Ano, turma.Modalidade);

            if (ciclo == null)
                throw new NegocioException("Não foi encontrado o ciclo da turma informada");

            return repositorioNotaTipoValor.ObterPorCicloIdDataAvalicacao(ciclo.Id, dataAvaliacao);
        }

        private void SalvarNoBanco(List<NotaConceito> EntidadesSalvar)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var entidade in EntidadesSalvar)
                {
                    repositorioNotasConceitos.Salvar(entidade);
                }

                unitOfWork.PersistirTransacao();
            }
        }

        private void ValidarAvaliacoes(IEnumerable<long> avaliacoesAlteradasIds, IEnumerable<AtividadeAvaliativa> atividadesAvaliativas, string professorRf)
        {
            if (atividadesAvaliativas == null || !atividadesAvaliativas.Any())
                throw new NegocioException("Não foi encontrada nenhuma da(s) avaliação(es) informada(s)");

            ValidarSeAtividadesAvaliativasExistem(avaliacoesAlteradasIds, atividadesAvaliativas);

            atividadesAvaliativas.ToList().ForEach(atividadeAvaliativa => ValidarDataAvaliacaoECriador(atividadeAvaliativa, professorRf));
        }

        private void ValidarDataAvaliacaoECriador(AtividadeAvaliativa atividadeAvaliativa, string professorRf)
        {
            if (atividadeAvaliativa.DataAvaliacao.Date > DateTime.Today)
                throw new NegocioException("Não é possivel atribuir notas/conceitos para avaliação(es) com data(s) futura(s)");

            if (!atividadeAvaliativa.ProfessorRf.Equals(professorRf))
                throw new NegocioException("Somente o professor que criou a avaliação, pode atribuir e/ou editar notas/conceitos");
        }

        private IEnumerable<NotaConceito> ValidarEObter(IEnumerable<NotaConceito> notasConceitos, AtividadeAvaliativa atividadeAvaliativa, IEnumerable<AlunoPorTurmaResposta> alunos, string professorRf, string disciplinaId)
        {
            var notasMultidisciplina = new List<NotaConceito>();
            var alunosNotasExtemporaneas = new StringBuilder();
            notasConceitos.ToList().ForEach(notaConceito =>
            {
                var tipoNota = TipoNotaPorAvaliacao(atividadeAvaliativa);

                if (notaConceito.Id > 0)
                {
                    notaConceito.Validar(professorRf);
                }

                var aluno = alunos.FirstOrDefault(a => a.CodigoAluno.Equals(notaConceito.AlunoId));

                if (aluno == null)
                    throw new NegocioException($"Não foi encontrado aluno com o codigo {notaConceito.AlunoId}");

                if (tipoNota.TipoNota == TipoNota.Nota)
                {
                    var notaParametro = repositorioNotaParametro.ObterPorDataAvaliacao(atividadeAvaliativa.DataAvaliacao);
                    if (notaParametro == null)
                        throw new NegocioException("Não foi possível localizar o parâmetro de nota.");
                    notaConceito.ValidarNota(notaParametro, aluno.NomeAluno);
                }
                else
                {
                    var conceitos = repositorioConceito.ObterPorDataAvaliacao(atividadeAvaliativa.DataAvaliacao);
                    if (conceitos == null)
                        throw new NegocioException("Não foi possível localizar o parâmetro de conceito.");
                    notaConceito.ValidarConceitos(conceitos, aluno.NomeAluno);
                }

                notaConceito.TipoNota = (TipoNota)tipoNota.Id;
                notaConceito.DisciplinaId = disciplinaId;
                if (atividadeAvaliativa.Categoria.Equals(CategoriaAtividadeAvaliativa.Interdisciplinar) && notaConceito.Id.Equals(0))
                {
                    var atividadeDisciplinas = repositorioAtividadeAvaliativaDisciplina.ListarPorIdAtividade(atividadeAvaliativa.Id).Result;
                    foreach(var atividade in atividadeDisciplinas)
                    {
                        if (!atividade.DisciplinaId.Equals(disciplinaId))
                        {
                            notasMultidisciplina.Add(new NotaConceito
                            {
                                AlunoId = notaConceito.AlunoId,
                                AtividadeAvaliativaID = notaConceito.AtividadeAvaliativaID,
                                DisciplinaId = atividade.DisciplinaId,
                                Nota = notaConceito.Nota,
                                Conceito = notaConceito.Conceito,
                                TipoNota = notaConceito.TipoNota
                            });
                        }
                    }
                }

                if(notaConceito.Id > 0)
                {
                    var dataFinal = atividadeAvaliativa.DataAvaliacao.AddHours(23).AddMinutes(59).AddSeconds(59);
                    var aula = repositorioAula.ObterAulaIntervaloTurmaDisciplina(atividadeAvaliativa.DataAvaliacao, dataFinal, atividadeAvaliativa.TurmaId, disciplinaId).Result;

                    IEnumerable<PeriodoEscolar> periodosEscolares = repositorioPeriodoEscolar.ObterPorTipoCalendario(aula.TipoCalendarioId);
                    var dataPesquisa = DateTime.Now;

                    var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);
                    var periodoEscolarInformado = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= atividadeAvaliativa.DataAvaliacao.Date && x.PeriodoFim.Date >= atividadeAvaliativa.DataAvaliacao.Date);
                    var ehBimestreAtual = periodoEscolar.Bimestre.Equals(periodoEscolarInformado.Bimestre);

                    if (!ehBimestreAtual)
                    {
                        alunosNotasExtemporaneas.AppendLine($"<li>{aluno.CodigoAluno} - {aluno.NomeAluno}</li>");
                    }
                }
            });

            if (alunosNotasExtemporaneas.ToString().Length > 0)
            {
                var turma = repositorioTurma.ObterTurmaComUeEDrePorId(atividadeAvaliativa.TurmaId);
                var usuario = servicoUsuario.ObterUsuarioLogado().Result;
                var dataAtual = DateTime.Now;
                string mensagem = $"<p>Os resultados da atividade avaliativa 'Redação sobre as férias' da turma {turma.Nome} da {turma.Ue.Nome} (DRE {turma.Ue.Dre.Nome}) no bimestre 2 de {turma.AnoLetivo} foram alterados " +
                    $"pelo Professor ${usuario.Nome} ({usuario.CodigoRf}) em {dataAtual.ToString("dd/MM/yyyy")} às {dataAtual.ToString("HH:mm")} para os seguintes alunos:</p><br/>{alunosNotasExtemporaneas.ToString()}";
                servicoNotificacao.Salvar(new Notificacao()
                {
                    Ano = atividadeAvaliativa.CriadoEm.Year,
                    Categoria = NotificacaoCategoria.Alerta,
                    DreId = atividadeAvaliativa.DreId,
                    Mensagem = mensagem,
                    UsuarioId = usuario.Id,
                    Tipo = NotificacaoTipo.Notas,
                    Titulo = $"Alteração em Atividade Avaliativa - Turma {turma.Nome}",
                    TurmaId = atividadeAvaliativa.TurmaId,
                    UeId = atividadeAvaliativa.UeId,
                });
            }

            var result = notasConceitos.ToList();
            result.AddRange(notasMultidisciplina);
            return result;
        }

    }
}