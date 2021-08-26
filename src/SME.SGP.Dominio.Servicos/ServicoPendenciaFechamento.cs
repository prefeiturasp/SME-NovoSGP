using MediatR;
using SME.Background.Core;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IMediator mediator;

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
                                          IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                          IRepositorioFechamentoNota repositorioFechamentoNota,
                                          IServicoUsuario servicoUsuario,
                                          IMediator mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<int> ValidarAulasReposicaoPendente(long fechamentoId, string turmaCodigo, string turmaNome, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre)
        {
            var aulasPendentes = repositorioAula.ObterAulasReposicaoPendentes(turmaCodigo, disciplinaId.ToString(), inicioPeriodo, fimPeriodo);
            if (aulasPendentes != null && aulasPendentes.Any())
            {
                var componenteCurricular = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId })).ToList()?.FirstOrDefault();
                if (componenteCurricular == null)
                {
                    throw new NegocioException("Componente curricular não encontrado.");
                }
                var mensagem = new StringBuilder($"A aulas de reposição de {componenteCurricular.Nome} da turma {turmaNome} a seguir estão pendentes de aprovação:<br>");
                foreach (var aula in aulasPendentes.OrderBy(a => a.DataAula))
                {
                    var professor = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(aula.ProfessorRf);
                    if (professor == null)
                    {
                        throw new NegocioException($"Professor com RF {aula.ProfessorRf} não encontrado.");
                    }
                    mensagem.AppendLine($"Professor { aula.ProfessorRf} - { professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                }

                var professorRf = aulasPendentes.First().ProfessorRf;
                await GerarPendencia(fechamentoId, TipoPendencia.AulasReposicaoPendenteAprovacao, mensagem.ToString(), professorRf, bimestre);
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasReposicaoPendenteAprovacao);

            aulasReposicaoPendentes = aulasPendentes.Count();
            return aulasReposicaoPendentes;
        }

        public async Task<int> ValidarAulasSemFrequenciaRegistrada(long fechamentoId, string turmaCodigo, string turmaNome, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre)
        {
            var registrosAulasSemFrequencia = repositorioAula.ObterAulasSemFrequenciaRegistrada(turmaCodigo, disciplinaId.ToString(), inicioPeriodo, fimPeriodo)
                .Where(a => a.PermiteRegistroFrequencia());
            if (registrosAulasSemFrequencia != null && registrosAulasSemFrequencia.Any())
            {
                var componenteCurricular = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId })).ToList()?.FirstOrDefault();
                if (componenteCurricular == null)
                {
                    throw new NegocioException("Componente curricular não encontrado.");
                }

                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turmaNome} a seguir estão sem frequência:<br>");

                // Carrega lista de professores
                var usuariosProfessores = CarregaListaProfessores(registrosAulasSemFrequencia.Select(a => a.ProfessorRf).Distinct());

                foreach (var aula in registrosAulasSemFrequencia.OrderBy(x => x.DataAula))
                {
                    var professor = usuariosProfessores.FirstOrDefault(c => c.CodigoRf == aula.ProfessorRf);
                    mensagem.AppendLine($"Professor { aula.ProfessorRf} - { professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                }

                var professorRf = registrosAulasSemFrequencia.First().ProfessorRf;
                await GerarPendencia(fechamentoId, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, mensagem.ToString(), professorRf, bimestre);
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

        public async Task<int> ValidarAulasSemPlanoAulaNaDataDoFechamento(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre)
        {
            var registrosAulasSemPlanoAula = repositorioAula.ObterAulasSemPlanoAulaNaDataAtual(turma.CodigoTurma,
                                                                            disciplinaId.ToString(),
                                                                            inicioPeriodo,
                                                                            fimPeriodo);

            if (registrosAulasSemPlanoAula != null && registrosAulasSemPlanoAula.Any())
            {
                var componenteCurricular = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId })).ToList()?.FirstOrDefault();
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

                var professorRf = registrosAulasSemPlanoAula.First().ProfessorRf;
                await GerarPendencia(fechamentoId, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento, mensagem.ToString(), professorRf, bimestre);
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);

            aulasSemPlanoAula = registrosAulasSemPlanoAula.Count();
            return aulasSemPlanoAula;
        }

        public async Task<int> ValidarAvaliacoesSemNotasParaNenhumAluno(long fechamentoId, string codigoTurma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre)
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

                
                var professorRf = registrosAvaliacoesSemNotaParaNenhumAluno.First().ProfessorRf;
                await GerarPendencia(fechamentoId, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno , mensagem.ToString(), professorRf, bimestre);
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);

            avaliacoesSemnota = registrosAvaliacoesSemNotaParaNenhumAluno.Count();
            return avaliacoesSemnota;
        }

        public async Task<int> ValidarPercentualAlunosAbaixoDaMedia(long fechamentoTurmaDisciplinaId, string justificativa, string criadoRF, int bimestre)
        {
            if (!string.IsNullOrEmpty(justificativa))
            {
                var percentualReprovacao = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));
                var mensagem = new StringBuilder($"O fechamento do bimestre possui mais de {percentualReprovacao}% das notas consideradas insuficientes<br>");

                await GerarPendencia(fechamentoTurmaDisciplinaId, TipoPendencia.ResultadosFinaisAbaixoDaMedia, mensagem.ToString(), criadoRF, bimestre);
                alunosAbaixoMedia = 1;
            }
            else
            {
                alunosAbaixoMedia = 0;
                repositorioPendencia.AtualizarPendencias(fechamentoTurmaDisciplinaId, SituacaoPendencia.Resolvida, TipoPendencia.ResultadosFinaisAbaixoDaMedia);
            }

            return alunosAbaixoMedia;
        }

        private async Task GerarPendencia(long fechamentoId, TipoPendencia tipoPendencia, string mensagem, string professorRf, int bimestre)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                repositorioPendencia.ExcluirPendenciasFechamento(fechamentoId, tipoPendencia);
                
                var tituloPendencia = $"{tipoPendencia.Name()}-{bimestre}º bimestre";
                var pendencia = new Pendencia(tipoPendencia, tituloPendencia, mensagem);
                repositorioPendencia.Salvar(pendencia);

                var pendenciaFechamento = new PendenciaFechamento(fechamentoId, pendencia.Id);
                repositorioPendenciaFechamento.Salvar(pendenciaFechamento);

                await RelacionaPendenciaUsuario(pendencia.Id, professorRf);

                unitOfWork.PersistirTransacao();
            }
        }

        private async Task RelacionaPendenciaUsuario(long pendenciaId, string professorRf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorRf));
            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
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
            => Convert.ToInt32(AvaliacoesSemNota())
            + Convert.ToInt32(AulasReposicaoPendentes())
            + Convert.ToInt32(AulasSemPlanoAula())
            + Convert.ToInt32(AulasSemFrequencia())
            + Convert.ToInt32(AlunosAbaixoMedia())
            + Convert.ToInt32(NotasExtemporaneasAlteradas());

        public bool AvaliacoesSemNota()
            => avaliacoesSemnota > 0;

        public bool AulasReposicaoPendentes()
            => aulasReposicaoPendentes > 0;

        public bool AulasSemPlanoAula()
            => aulasSemPlanoAula > 0;

        public bool AulasSemFrequencia()
            => aulasSemFrequencia > 0;

        public bool AlunosAbaixoMedia()
            => alunosAbaixoMedia > 0;

        public bool NotasExtemporaneasAlteradas()
            => notasExtemporaneasAlteradas > 0;

        public async Task<int> ValidarAlteracaoExtemporanea(long fechamentoId, string turmaCodigo, string professorRf, int bimestre)
        {
            var registrosNotasAlteradas = await repositorioFechamentoNota.ObterNotasEmAprovacaoPorFechamento(fechamentoId);

            if (registrosNotasAlteradas != null && registrosNotasAlteradas.Any())
            {
                var mensagem = new StringBuilder($"Notas de fechamento alteradas fora do ano de vigência da turma {turmaCodigo}. Necessário aprovação do workflow");
                await GerarPendencia(fechamentoId, TipoPendencia.AlteracaoNotaFechamento, mensagem.ToString(), professorRf, bimestre);
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AlteracaoNotaFechamento);

            notasExtemporaneasAlteradas = registrosNotasAlteradas.Count();
            return notasExtemporaneasAlteradas;
        }

        public IEnumerable<string> ObterDescricaoPendenciasGeradas()
        {
            var listaPendencias = new List<string>();

            if (AvaliacoesSemNota())
                listaPendencias.Add("Avaliação sem notas lançadas");

            if (AulasReposicaoPendentes())
                listaPendencias.Add("Aulas de reposição pendentes de aprovação");

            if (AulasSemPlanoAula())
                listaPendencias.Add("Aulas sem plano de aula registrado");

            if (AulasSemFrequencia())
                listaPendencias.Add("Aulas sem frequência registrada");

            if (AlunosAbaixoMedia())
                listaPendencias.Add("Fechamento com percentual de aprovação insuficiente");

            if (NotasExtemporaneasAlteradas())
                listaPendencias.Add("Notas de fechamento alteradas fora do ano de vigência da turma");

            return listaPendencias;
        }
    }
}