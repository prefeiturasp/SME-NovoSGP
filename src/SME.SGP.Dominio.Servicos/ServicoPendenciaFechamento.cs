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
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        private int avaliacoesSemnota;
        private int aulasReposicaoPendentes;
        private int aulasSemPlanoAula;
        private int aulasSemFrequencia;
        private int alunosAbaixoMedia;
        private int notasExtemporaneasAlteradas;

        public ServicoPendenciaFechamento(IUnitOfWork unitOfWork,
                                          IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
                                          IRepositorioPendencia repositorioPendencia,
                                          IRepositorioPendenciaFechamento repositorioPendenciaFechamento,
                                          IRepositorioAula repositorioAula,
                                          IRepositorioParametrosSistema repositorioParametrosSistema,
                                          IRepositorioFechamentoNota repositorioFechamentoNota,
                                          IServicoEOL servicoEOL,
                                          IServicoUsuario servicoUsuario)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
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
                foreach (var aula in aulasPendentes.OrderBy(a => a.DataAula))
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

            aulasReposicaoPendentes = aulasPendentes.Count();
            return aulasReposicaoPendentes;
        }

        public int ValidarAulasSemFrequenciaRegistrada(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var registrosAulasSemFrequencia = repositorioAula.ObterAulasSemFrequenciaRegistrada(turma.CodigoTurma, disciplinaId.ToString(), inicioPeriodo, fimPeriodo);
            if (registrosAulasSemFrequencia != null && registrosAulasSemFrequencia.Any())
            {
                var componenteCurricular = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaId })?.FirstOrDefault();
                if (componenteCurricular == null)
                {
                    throw new NegocioException("Componente curricular não encontrado.");
                }
                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão sem frequência:<br>");

                // Carrega lista de professores
                var usuariosProfessores = CarregaListaProfessores(registrosAulasSemFrequencia.Select(a => a.ProfessorRf).Distinct());

                foreach (var aula in registrosAulasSemFrequencia.OrderBy(x => x.DataAula))
                {
                    var professor = usuariosProfessores.FirstOrDefault(c => c.CodigoRf == aula.ProfessorRf);
                    mensagem.AppendLine($"Professor { aula.ProfessorRf} - { professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                }

                GerarPendencia(fechamentoId, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, mensagem.ToString());
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento);

            aulasSemFrequencia = registrosAulasSemFrequencia.Count();
            return aulasSemFrequencia;
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
            var registrosAulasSemPlanoAula = repositorioAula.ObterAulasSemPlanoAulaNaDataAtual(turma.CodigoTurma,
                                                                            disciplinaId.ToString(),
                                                                            inicioPeriodo,
                                                                            fimPeriodo);

            if (registrosAulasSemPlanoAula != null && registrosAulasSemPlanoAula.Any())
            {
                var componenteCurricular = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaId })?.FirstOrDefault();
                if (componenteCurricular == null)
                {
                    throw new NegocioException("Componente curricular não encontrado.");
                }
                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão sem plano de aula registrado até a data do fechamento:<br>");

                var usuariosProfessores = CarregaListaProfessores(registrosAulasSemPlanoAula.Select(a => a.ProfessorRf).Distinct());
                foreach (var aula in registrosAulasSemPlanoAula.OrderBy(a => a.DataAula))
                {
                    var professor = usuariosProfessores.FirstOrDefault(c => c.CodigoRf == aula.ProfessorRf);
                    mensagem.AppendLine($"Professor { aula.ProfessorRf} - { professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                }

                GerarPendencia(fechamentoId, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento, mensagem.ToString());
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);

            aulasSemPlanoAula = registrosAulasSemPlanoAula.Count();
            return aulasSemPlanoAula;
        }

        public int ValidarAvaliacoesSemNotasParaNenhumAluno(long fechamentoId, string codigoTurma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var registrosAvaliacoesSemNotaParaNenhumAluno = repositorioAtividadeAvaliativa.ObterAtividadesAvaliativasSemNotaParaNenhumAluno(codigoTurma,
                                                                            disciplinaId.ToString(),
                                                                            inicioPeriodo,
                                                                            fimPeriodo);

            if (registrosAvaliacoesSemNotaParaNenhumAluno != null && registrosAvaliacoesSemNotaParaNenhumAluno.Any())
            {
                var mensagem = new StringBuilder($"As avaliações a seguir não tiveram notas lançadas para nenhum aluno<br>");
                var usuariosProfessores = CarregaListaProfessores(registrosAvaliacoesSemNotaParaNenhumAluno.Select(a => a.ProfessorRf).Distinct());
                foreach (var avaliacao in registrosAvaliacoesSemNotaParaNenhumAluno.OrderBy(x => x.DataAvaliacao))
                {
                    var professor = usuariosProfessores.FirstOrDefault(c => c.CodigoRf == avaliacao.ProfessorRf);
                    mensagem.AppendLine($"Professor { avaliacao.ProfessorRf} - { professor.Nome} - {avaliacao.NomeAvaliacao}.<br>");
                }

                GerarPendencia(fechamentoId, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno, mensagem.ToString());
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);

            avaliacoesSemnota = registrosAvaliacoesSemNotaParaNenhumAluno.Count();
            return avaliacoesSemnota;
        }

        public int ValidarPercentualAlunosAbaixoDaMedia(FechamentoTurmaDisciplina fechamentoTurma)
        {
            if (!string.IsNullOrEmpty(fechamentoTurma.Justificativa))
            {
                var percentualReprovacao = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.PercentualAlunosInsuficientes));
                var mensagem = new StringBuilder($"O fechamento do bimestre possui mais de {percentualReprovacao}% das notas consideradas insuficientes<br>");

                GerarPendencia(fechamentoTurma.Id, TipoPendencia.ResultadosFinaisAbaixoDaMedia, mensagem.ToString());
                alunosAbaixoMedia = 1;
            }
            else
            {
                alunosAbaixoMedia = 0;
                repositorioPendencia.AtualizarPendencias(fechamentoTurma.Id, SituacaoPendencia.Resolvida, TipoPendencia.ResultadosFinaisAbaixoDaMedia);
            }

            return alunosAbaixoMedia;
        }

        private void GerarPendencia(long fechamentoId, TipoPendencia tipoPendencia, string mensagem)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                repositorioPendencia.ExcluirPendenciasFechamento(fechamentoId, tipoPendencia);

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
            var auditoriaDto = await AtualizarPendencia(pendenciaId, SituacaoPendencia.Aprovada);

            var pendenciaFechamento = await repositorioPendenciaFechamento.ObterPorPendenciaId(pendenciaId);
            if (pendenciaFechamento == null)
                throw new NegocioException("Pendência de fechamento não localizada com o identificador consultado");

            Cliente.Executar<IServicoFechamentoTurmaDisciplina>(c => c.VerificaPendenciasFechamento(pendenciaFechamento.FechamentoId));
            return auditoriaDto;
        }

        public bool VerificaPendenciasFechamento(long fechamentoId)
            => repositorioPendenciaFechamento.VerificaPendenciasAbertoPorFechamento(fechamentoId);

        public async Task<AuditoriaPersistenciaDto> AtualizarPendencia(long pendenciaId, SituacaoPendencia situacaoPendencia)
        {
            var pendencia = repositorioPendencia.ObterPorId(pendenciaId);
            if (pendencia == null)
                throw new NegocioException("Pendência de fechamento não localizada com o identificador consultado");

            pendencia.Situacao = situacaoPendencia;
            await repositorioPendencia.SalvarAsync(pendencia);
            return (AuditoriaPersistenciaDto)pendencia;
        }

        public int ObterQuantidadePendenciasGeradas()
            => (avaliacoesSemnota > 0 ? 1 : 0)
            + (aulasReposicaoPendentes > 0 ? 1 : 0)
            + (aulasSemPlanoAula > 0 ? 1 : 0)
            + (aulasSemFrequencia > 0 ? 1 : 0)
            + (alunosAbaixoMedia > 0 ? 1 : 0)
            + (notasExtemporaneasAlteradas > 0 ? 1 : 0);

        public int ValidarAlteracaoExtemporanea(long fechamentoId, string turmaCodigo, long disciplinaId)
        {
            var registrosNotasAlteradas = repositorioFechamentoNota.ObterNotasEmAprovacaoPorFechamento(fechamentoId).Result;

            if (registrosNotasAlteradas != null && registrosNotasAlteradas.Any())
            {
                var mensagem = new StringBuilder($"Notas de fechamento alteradas fora do ano de vigência da turma {turmaCodigo}. Necessário aprovação do workflow");
                GerarPendencia(fechamentoId, TipoPendencia.AlteracaoNotaFechamento, mensagem.ToString());
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AlteracaoNotaFechamento);

            notasExtemporaneasAlteradas = registrosNotasAlteradas.Count();
            return notasExtemporaneasAlteradas;
        }
    }
}