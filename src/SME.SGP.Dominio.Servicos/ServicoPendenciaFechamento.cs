using SME.Background.Core;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoPendenciaFechamento : IServicoPendenciaFechamento
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoPendenciaFechamento(IUnitOfWork unitOfWork,
                                          IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
                                          IRepositorioPendencia repositorioPendencia,
                                          IRepositorioPendenciaFechamento repositorioPendenciaFechamento,
                                          IRepositorioAula repositorioAula,
                                          IRepositorioParametrosSistema repositorioParametrosSistema,
                                          IServicoEOL servicoEOL,
                                          IServicoUsuario servicoUsuario)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.servicoEOL = servicoEOL;
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public int ValidarAulasReposicaoPendente(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var aulasPendentes = repositorioAula.ObterAulasReposicaoPendentes(turma.CodigoTurma, disciplinaId.ToString(), inicioPeriodo, fimPeriodo);
            if (aulasPendentes != null && aulasPendentes.Any())
            {
                var componenteCurricular = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaId })?.FirstOrDefault();
                if (componenteCurricular == null)
                {
                    throw new NegocioException("Componente curricular não encontrado.");
                }
                var mensagem = new StringBuilder($"A aulas de reposição de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão pendentes de aprovação:<br>");
                foreach (var aula in aulasPendentes)
                {
                    var professor = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(aula.ProfessorRf);
                    if (professor == null)
                    {
                        throw new NegocioException($"Professor com RF {aula.ProfessorRf} não encontrado.");
                    }
                    mensagem.AppendLine($"Professor { aula.ProfessorRf} - { professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                }

                GerarPendencia(fechamentoId, TipoPendencia.AulasReposicaoPendenteAprovacao, mensagem.ToString());
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasReposicaoPendenteAprovacao);
            return aulasPendentes.Count();
        }

        public int ValidarAulasSemFrequenciaRegistrada(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var aulasSemFrequencia = repositorioAula.ObterAulasSemFrequenciaRegistrada(turma.CodigoTurma, disciplinaId.ToString(), inicioPeriodo, fimPeriodo);
            if (aulasSemFrequencia != null && aulasSemFrequencia.Any())
            {
                var componenteCurricular = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaId })?.FirstOrDefault();
                if (componenteCurricular == null)
                {
                    throw new NegocioException("Componente curricular não encontrado.");
                }
                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão sem frequência:<br>");

                // Carrega lista de professores
                var usuariosProfessores = CarregaListaProfessores(aulasSemFrequencia.Select(a => a.ProfessorRf).Distinct());

                foreach (var aula in aulasSemFrequencia)
                {
                    var professor = usuariosProfessores.FirstOrDefault(c => c.CodigoRf == aula.ProfessorRf);
                    mensagem.AppendLine($"Professor { aula.ProfessorRf} - { professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                }

                GerarPendencia(fechamentoId, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, mensagem.ToString());
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasReposicaoPendenteAprovacao);
            return aulasSemFrequencia.Count();
        }

        private IEnumerable<Usuario> CarregaListaProfessores(IEnumerable<string> listaRFs)
        {
            foreach (var professorRF in listaRFs)
            {
                var professor = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(professorRF);
                if (professor == null)
                    throw new NegocioException($"Professor com RF {professorRF} não encontrado.");

                yield return professor;
            }
        }

        public int ValidarAulasSemPlanoAulaNaDataDoFechamento(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var aulasSemPlanoAula = repositorioAula.ObterAulasSemPlanoAulaNaDataAtual(turma.CodigoTurma,
                                                                            disciplinaId.ToString(),
                                                                            inicioPeriodo,
                                                                            fimPeriodo);

            if (aulasSemPlanoAula != null && aulasSemPlanoAula.Any())
            {
                var componenteCurricular = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaId })?.FirstOrDefault();
                if (componenteCurricular == null)
                {
                    throw new NegocioException("Componente curricular não encontrado.");
                }
                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão sem plano de aula registrado até a data do fechamento:<br>");

                var usuariosProfessores = CarregaListaProfessores(aulasSemPlanoAula.Select(a => a.ProfessorRf).Distinct());
                foreach (var aula in aulasSemPlanoAula)
                {
                    var professor = usuariosProfessores.FirstOrDefault(c => c.CodigoRf == aula.ProfessorRf);
                    mensagem.AppendLine($"Professor { aula.ProfessorRf} - { professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                }

                GerarPendencia(fechamentoId, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento, mensagem.ToString());
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);
            return aulasSemPlanoAula.Count();
        }

        public int ValidarAvaliacoesSemNotasParaNenhumAluno(long fechamentoId, string codigoTurma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var avaliacoesSemNotaParaNenhumAluno = repositorioAtividadeAvaliativa.ObterAtividadesAvaliativasSemNotaParaNenhumAluno(codigoTurma,
                                                                            disciplinaId.ToString(),
                                                                            inicioPeriodo,
                                                                            fimPeriodo);

            if (avaliacoesSemNotaParaNenhumAluno != null && avaliacoesSemNotaParaNenhumAluno.Any())
            {
                var mensagem = new StringBuilder($"As avaliações a seguir não tiveram notas lançadas para nenhum aluno<br>");
                var usuariosProfessores = CarregaListaProfessores(avaliacoesSemNotaParaNenhumAluno.Select(a => a.ProfessorRf).Distinct());
                foreach (var avaliacao in avaliacoesSemNotaParaNenhumAluno)
                {
                    var professor = usuariosProfessores.FirstOrDefault(c => c.CodigoRf == avaliacao.ProfessorRf);
                    mensagem.AppendLine($"Professor { avaliacao.ProfessorRf} - { professor.Nome} - {avaliacao.NomeAvaliacao}.<br>");
                }

                GerarPendencia(fechamentoId, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno, mensagem.ToString());
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);
            return avaliacoesSemNotaParaNenhumAluno.Count();
        }

        public int ValidarPercentualAlunosAbaixoDaMedia(FechamentoTurmaDisciplina fechamentoTurma)
        {
            if (!string.IsNullOrEmpty(fechamentoTurma.Justificativa))
            {
                var percentualReprovacao = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.PercentualAlunosInsuficientes));
                var mensagem = new StringBuilder($"O fechamento do bimestre possui mais de {percentualReprovacao}% das notas consideradas insuficientes<br>");

                GerarPendencia(fechamentoTurma.Id, TipoPendencia.ResultadosFinaisAbaixoDaMedia, mensagem.ToString());
                return 1;
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoTurma.Id, SituacaoPendencia.Resolvida, TipoPendencia.ResultadosFinaisAbaixoDaMedia);

            return 0;
        }

        private void GerarPendencia(long fechamentoId, TipoPendencia tipoPendencia, string mensagem)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                var pendencia = new Pendencia(tipoPendencia.Name(),
                                        mensagem,
                                        tipoPendencia);
                repositorioPendencia.Salvar(pendencia);

                var pendenciaFechamento = new PendenciaFechamento(fechamentoId, pendencia.Id);
                repositorioPendenciaFechamento.Salvar(pendenciaFechamento);

                unitOfWork.PersistirTransacao();
            }
        }

        public async Task<AuditoriaPersistenciaDto> Aprovar(long pendenciaId)
        {
            var auditoriaDto = await repositorioPendencia.AtualizarPendencia(pendenciaId, SituacaoPendencia.Aprovada);

            var pendenciaFechamento = await repositorioPendenciaFechamento.ObterPorPendenciaId(pendenciaId);
            if (pendenciaFechamento == null)
                throw new NegocioException("Pendência de fechamento não localizada com o identificador consultado");

            Cliente.Executar<IServicoFechamentoTurmaDisciplina>(c => c.VerificaPendenciasFechamento(pendenciaFechamento.FechamentoId));
            return auditoriaDto;
        }

        public bool VerificaPendenciasFechamento(long fechamentoId)
            => repositorioPendenciaFechamento.VerificaPendenciasAbertoPorFechamento(fechamentoId);
    }
}