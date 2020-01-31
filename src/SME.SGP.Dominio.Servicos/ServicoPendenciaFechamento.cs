using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Text;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoPendenciaFechamento : IServicoPendenciaFechamento
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoPendenciaFechamento(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
                                          IRepositorioPendencia repositorioPendencia,
                                          IRepositorioAula repositorioAula,
                                          IServicoEOL servicoEOL,
                                          IServicoUsuario servicoUsuario)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.servicoEOL = servicoEOL;
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public bool ValidarAulasReposicaoPendente(long fechamentoId, Turma turma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var aulasPendentes = repositorioAula.ObterAulasReposicaoPendentes(turma.CodigoTurma, disciplinaId, inicioPeriodo, fimPeriodo);
            if (aulasPendentes != null && aulasPendentes.Any())
            {
                var componenteCurricular = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(disciplinaId) })?.FirstOrDefault();
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
                repositorioPendencia.RemoverPendenciasPorTipo(fechamentoId, TipoPendencia.AulasReposicaoPendenteAprovacao);
                var pendencia = new Pendencia("Aula de reposição pendente de aprovação",
                                              mensagem.ToString(),
                                              fechamentoId,
                                              TipoPendencia.AulasReposicaoPendenteAprovacao);

                repositorioPendencia.Salvar(pendencia);
                return true;
            }
            else
            {
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasReposicaoPendenteAprovacao);
                return false;
            }
        }

        public bool ValidarAulasSemFrequenciaRegistrada(long fechamentoId, Turma turma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var aulasSemFrequencia = repositorioAula.ObterAulasSemFrequenciaRegistrada(turma.CodigoTurma, disciplinaId, inicioPeriodo, fimPeriodo);
            if (aulasSemFrequencia != null && aulasSemFrequencia.Any())
            {
                var componenteCurricular = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(disciplinaId) })?.FirstOrDefault();
                if (componenteCurricular == null)
                {
                    throw new NegocioException("Componente curricular não encontrado.");
                }
                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão sem frequência:<br>");
                foreach (var aula in aulasSemFrequencia)
                {
                    var professor = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(aula.ProfessorRf);
                    if (professor == null)
                    {
                        throw new NegocioException($"Professor com RF {aula.ProfessorRf} não encontrado.");
                    }
                    mensagem.AppendLine($"Professor { aula.ProfessorRf} - { professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                }
                repositorioPendencia.RemoverPendenciasPorTipo(fechamentoId, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento);
                var pendencia = new Pendencia("Aulas sem frequência registrada",
                                              mensagem.ToString(),
                                              fechamentoId,
                                              TipoPendencia.AulasSemFrequenciaNaDataDoFechamento);

                repositorioPendencia.Salvar(pendencia);
                return true;
            }
            else
            {
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasReposicaoPendenteAprovacao);
                return false;
            }
        }

        public bool ValidarAulasSemPlanoAulaNaDataDoFechamento(long fechamentoId, Turma turma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var aulasSemPlanoAula = repositorioAula.ObterAulasSemPlanoAulaNaDataAtual(turma.CodigoTurma,
                                                                            disciplinaId,
                                                                            inicioPeriodo,
                                                                            fimPeriodo);

            if (aulasSemPlanoAula != null && aulasSemPlanoAula.Any())
            {
                var componenteCurricular = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(disciplinaId) })?.FirstOrDefault();
                if (componenteCurricular == null)
                {
                    throw new NegocioException("Componente curricular não encontrado.");
                }
                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão sem plano de aula registrado até a data do fechamento:<br>");
                foreach (var aula in aulasSemPlanoAula)
                {
                    var professor = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(aula.ProfessorRf);
                    if (professor == null)
                    {
                        throw new NegocioException($"Professor com RF {aula.ProfessorRf} não encontrado.");
                    }
                    mensagem.AppendLine($"Professor { aula.ProfessorRf} - { professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                }
                repositorioPendencia.RemoverPendenciasPorTipo(fechamentoId, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);
                var pendencia = new Pendencia("Aula sem plano de aula registrado",
                                              mensagem.ToString(),
                                              fechamentoId,
                                              TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);

                repositorioPendencia.Salvar(pendencia);
                return true;
            }
            else
            {
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);
                return false;
            }
        }

        public bool ValidarAvaliacoesSemNotasParaNenhumAluno(long fechamentoId, string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var avaliacoesSemNotaParaNenhumAluno = repositorioAtividadeAvaliativa.ObterAtividadesAvaliativasSemNotaParaNenhumAluno(codigoTurma,
                                                                            disciplinaId,
                                                                            inicioPeriodo,
                                                                            fimPeriodo);

            if (avaliacoesSemNotaParaNenhumAluno != null && avaliacoesSemNotaParaNenhumAluno.Any())
            {
                var mensagem = new StringBuilder($"As avaliações a seguir não tiveram notas lançadas para nenhum aluno<br>");
                foreach (var avaliacao in avaliacoesSemNotaParaNenhumAluno)
                {
                    var professor = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(avaliacao.ProfessorRf);
                    if (professor == null)
                    {
                        throw new NegocioException($"Professor com RF {avaliacao.ProfessorRf} não encontrado.");
                    }
                    mensagem.AppendLine($"Professor { avaliacao.ProfessorRf} - { professor.Nome} - {avaliacao.NomeAvaliacao}.<br>");
                }

                repositorioPendencia.RemoverPendenciasPorTipo(fechamentoId, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);
                var pendencia = new Pendencia("Avaliação sem notas/conceitos lançados",
                                              mensagem.ToString(),
                                              fechamentoId,
                                              TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);

                repositorioPendencia.Salvar(pendencia);
                return true;
            }
            else
            {
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);
                return false;
            }
        }
    }
}