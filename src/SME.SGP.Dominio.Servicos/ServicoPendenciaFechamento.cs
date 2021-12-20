using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SME.SGP.Dominio.Perfis;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoPendenciaFechamento : IServicoPendenciaFechamento
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoAbrangencia servicoAbrangencia;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
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
                                          IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular,
                                          IRepositorioFechamentoNotaConsulta repositorioFechamentoNota,
                                          IServicoUsuario servicoUsuario,
                                          IServicoAbrangencia servicoAbrangencia,
                                          IMediator mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoAbrangencia = servicoAbrangencia ?? throw new ArgumentNullException(nameof(servicoAbrangencia));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<int> ValidarAulasReposicaoPendente(long fechamentoId, string turmaCodigo, string turmaNome, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var aulasPendentes = repositorioAula.ObterAulasReposicaoPendentes(turmaCodigo, disciplinaId.ToString(), inicioPeriodo, fimPeriodo);
            if (aulasPendentes != null && aulasPendentes.Any())
            {
                var componenteCurricular = await BuscaInformacoesDaDisciplina(disciplinaId);

                var mensagem = new StringBuilder($"A aulas de reposição de {componenteCurricular.Nome} da turma {turmaNome} a seguir estão pendentes de aprovação:<br>");

                var mensagemHtml = new StringBuilder($"<table><tr class=\"nao-exibir\"><td colspan=\"2\">A aulas de reposição de {componenteCurricular.Nome} da turma {turmaNome} a seguir estão pendentes de aprovação:</td></tr>");

                mensagemHtml.Append("<tr class=\"cabecalho\"><td>Data da aula</td><td>Professor</td></tr>");

                var professorTitularPorTurmaEDisciplina = await BuscaProfessorTitularPorTurmaEDisciplina(turmaCodigo, disciplinaId);

                foreach (var aula in aulasPendentes.OrderBy(a => a.DataAula))
                {
                    var professoresTitulares = await mediator.Send(new ObterProfessoresTitularesDaTurmaQuery(aula.TurmaId));

                    Usuario professor = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(aula.ProfessorRf);

                    mensagem.AppendLine($"Professor {professor.CodigoRf} - {professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                    mensagemHtml.Append($"<tr><td>{aula.DataAula.ToString("dd/MM/yyyy")}</td><td>{professor.Nome} - {professor.CodigoRf}</td></tr>");
                }
                mensagemHtml.Append("</table>");
                
                await GerarPendencia(fechamentoId, TipoPendencia.AulasReposicaoPendenteAprovacao, mensagem.ToString(), professorTitularPorTurmaEDisciplina.ProfessorRf, mensagemHtml.ToString());
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasReposicaoPendenteAprovacao);

            aulasReposicaoPendentes = aulasPendentes.Count();
            return aulasReposicaoPendentes;
        }

        private async Task<ProfessorTitularDisciplinaEol> BuscaProfessorTitularPorTurmaEDisciplina(string turmaCodigo, long disciplinaId)
        {
            var professoresTitularesPorTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(turmaCodigo));
            var professorTitularPorTurmaEDisciplina = professoresTitularesPorTurma.FirstOrDefault(o => o.DisciplinaId == disciplinaId);

            if (professorTitularPorTurmaEDisciplina == null)
                throw new NegocioException($"Não existe professor titular para esta turma/disciplina {turmaCodigo}/{disciplinaId}");

            return professorTitularPorTurmaEDisciplina;
        }

        private async Task<DisciplinaDto> BuscaInformacoesDaDisciplina(long disciplinaId)
        {
            var componenteCurricular = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId })).ToList()?.FirstOrDefault();

            if (componenteCurricular == null)
                throw new NegocioException("Componente curricular não encontrado.");

            return componenteCurricular;
        }

        public async Task<int> ValidarAulasSemFrequenciaRegistrada(long fechamentoId, string turmaCodigo, string turmaNome, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var registrosAulasSemFrequencia = repositorioAula
                .ObterAulasSemFrequenciaRegistrada(turmaCodigo, disciplinaId.ToString(), inicioPeriodo, fimPeriodo)
                .Where(a => a.PermiteRegistroFrequencia())
                .ToList();

            if (registrosAulasSemFrequencia != null && registrosAulasSemFrequencia.Any())
            {
                var componenteCurricular = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId })).ToList()?.FirstOrDefault();
                if (componenteCurricular == null)
                    throw new NegocioException("Componente curricular não encontrado.");

                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turmaNome} a seguir estão sem frequência:<br>");

                var mensagemHtml = new StringBuilder($"<table><tr class=\"nao-exibir\"><td colspan=\"2\">A aulas de {componenteCurricular.Nome} da turma {turmaNome} a seguir estão sem frequência:</td></tr>");

                mensagemHtml.Append("<tr class=\"cabecalho\"><td>Data da aula</td><td>Professor</td></tr>");

                var usuariosPendencias = CarregaListaProfessores(registrosAulasSemFrequencia.Select(a => (a.ProfessorRf, a.TurmaId, a.DisciplinaId)).Distinct()).ToList();

                if (usuariosPendencias.All(u => u.cp))
                {
                    foreach (var usuarioCp in usuariosPendencias)
                    {
                        foreach (var aula in registrosAulasSemFrequencia.OrderBy(x => x.DataAula))
                        {
                            mensagem.AppendLine($"CP {usuarioCp.usuario.CodigoRf} - {usuarioCp.usuario.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                            mensagemHtml.Append($"<tr><td>{aula.DataAula.ToString("dd/MM/yyyy")}</td><td>{usuarioCp.usuario.Nome} - {usuarioCp.usuario.CodigoRf}</td></tr>");
                        }
                        mensagemHtml.Append("</table>");

                        await GerarPendencia(fechamentoId, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, mensagem.ToString(), usuarioCp.usuario.CodigoRf, mensagemHtml.ToString());
                    }
                }
                else
                {
                    foreach (var aula in registrosAulasSemFrequencia.OrderBy(x => x.DataAula))
                    {
                        var professor = usuariosPendencias
                            .FirstOrDefault(c => c.usuario.CodigoRf == aula.ProfessorRf).usuario ?? usuariosPendencias.First(up => up.turmaCodigo.Equals(aula.TurmaId) && up.disciplinaId == aula.DisciplinaId).usuario;

                        mensagem.AppendLine($"Professor {professor.CodigoRf} - {professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                        mensagemHtml.Append($"<tr><td>{aula.DataAula.ToString("dd/MM/yyyy")}</td><td>{professor.Nome} - {professor.CodigoRf}</td></tr>");
                    }
                    mensagemHtml.Append("</table>");

                    var professorRf = registrosAulasSemFrequencia.FirstOrDefault()?.ProfessorRf;

                    if (string.IsNullOrWhiteSpace(professorRf))
                        professorRf = usuariosPendencias.First().usuario.CodigoRf;

                    await GerarPendencia(fechamentoId, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, mensagem.ToString(), professorRf, mensagemHtml.ToString());
                }                
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento);

            aulasSemFrequencia = registrosAulasSemFrequencia?.Count ?? 0;
            return aulasSemFrequencia;
        }

        public async Task<int> ValidarAulasSemPlanoAulaNaDataDoFechamento(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var registrosAulasSemPlanoAula = repositorioAula.ObterAulasSemPlanoAulaNaDataAtual(turma.CodigoTurma,
                                                                            disciplinaId.ToString(),
                                                                            inicioPeriodo,
                                                                            fimPeriodo);

            if (registrosAulasSemPlanoAula != null && registrosAulasSemPlanoAula.Any())
            {
                var componenteCurricular = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId })).ToList()?.FirstOrDefault();

                if (componenteCurricular == null)
                    throw new NegocioException("Componente curricular não encontrado.");

                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão sem plano de aula registrado até a data do fechamento:<br>");

                var mensagemHtml = new StringBuilder($"<table><tr class=\"nao-exibir\"><td colspan=\"2\">A aulas de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão sem plano de aula registrado até a data do fechamento:</td></tr>");

                mensagemHtml.Append("<tr class=\"cabecalho\"><td>Data da aula</td><td>Professor</td></tr>");

                var usuariosPendencias = CarregaListaProfessores(registrosAulasSemPlanoAula.Select(a => (a.ProfessorRf, a.TurmaId, a.DisciplinaId)).Distinct()).ToList();

                if (usuariosPendencias.All(u => u.cp))
                {
                    foreach (var usuarioCp in usuariosPendencias)
                    {
                        foreach (var aula in registrosAulasSemPlanoAula.OrderBy(x => x.DataAula))
                        {
                            mensagem.AppendLine($"CP {usuarioCp.usuario.CodigoRf} - {usuarioCp.usuario.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                            mensagemHtml.Append($"<tr><td>{aula.DataAula.ToString("dd/MM/yyyy")}</td><td>{usuarioCp.usuario.Nome} - {usuarioCp.usuario.CodigoRf}</td></tr>");
                        }
                        mensagemHtml.Append("</table>");

                        await GerarPendencia(fechamentoId, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, mensagem.ToString(), usuarioCp.usuario.CodigoRf, mensagemHtml.ToString());
                    }
                }
                else
                {
                    foreach (var aula in registrosAulasSemPlanoAula.OrderBy(a => a.DataAula))
                    {
                        var professor = usuariosPendencias
                            .FirstOrDefault(c => c.usuario.CodigoRf == aula.ProfessorRf).usuario ?? usuariosPendencias.First(up => up.turmaCodigo.Equals(aula.TurmaId) && up.disciplinaId == aula.DisciplinaId).usuario;

                        mensagem.AppendLine($"Professor {aula.ProfessorRf} - {professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                        mensagemHtml.Append($"<tr><td>{aula.DataAula.ToString("dd/MM/yyyy")}</td><td>{professor.Nome} - {aula.ProfessorRf}</td></tr>");
                    }
                    mensagemHtml.Append("</table>");

                    var professorRf = registrosAulasSemPlanoAula.FirstOrDefault()?.ProfessorRf;

                    if (string.IsNullOrWhiteSpace(professorRf))
                        professorRf = usuariosPendencias.First().usuario.CodigoRf;

                    await GerarPendencia(fechamentoId, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento, mensagem.ToString(), professorRf, mensagemHtml.ToString());
                }                
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);

            aulasSemPlanoAula = registrosAulasSemPlanoAula.Count();
            return aulasSemPlanoAula;
        }

        public async Task<int> ValidarAvaliacoesSemNotasParaNenhumAluno(long fechamentoId, string codigoTurma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var registrosAvaliacoesSemNotaParaNenhumAluno = repositorioAtividadeAvaliativa.ObterAtividadesAvaliativasSemNotaParaNenhumAluno(codigoTurma,
                                                                            disciplinaId.ToString(),
                                                                            inicioPeriodo,
                                                                            fimPeriodo,
                                                                            (int)TipoAvaliacaoCodigo.AtividadeClassroom);

            if (registrosAvaliacoesSemNotaParaNenhumAluno != null && registrosAvaliacoesSemNotaParaNenhumAluno.Any())
            {
                var mensagem = new StringBuilder($"As avaliações a seguir não tiveram notas lançadas para nenhum aluno<br>");

                var mensagemHtml = new StringBuilder($"<table><tr class=\"nao-exibir\"><td colspan=\"3\">As avaliações a seguir não tiveram notas lançadas para nenhum aluno:</td></tr>");

                mensagemHtml.Append("<tr class=\"cabecalho\"><td>Data da avaliação</td><td>Título</td><td>Professor</td></tr>");

                var atividadesTurma = registrosAvaliacoesSemNotaParaNenhumAluno.Select(a => (a.ProfessorRf, a.TurmaId, a.Disciplinas.Any() ? a.Disciplinas.First().DisciplinaId : null)).Distinct().ToList();
                var usuariosPendencias = CarregaListaProfessores(atividadesTurma).ToList();

                if (usuariosPendencias.All(u => u.cp))
                {
                    foreach (var usuarioCp in usuariosPendencias)
                    {
                        foreach (var aula in registrosAvaliacoesSemNotaParaNenhumAluno.OrderBy(x => x.DataAvaliacao))
                        {
                            mensagem.AppendLine($"CP {usuarioCp.usuario.CodigoRf} - {usuarioCp.usuario.Nome}, dia {aula.DataAvaliacao.ToString("dd/MM/yyyy")}.<br>");
                            mensagemHtml.Append($"<tr><td>{aula.DataAvaliacao.ToString("dd/MM/yyyy")}</td><td>{usuarioCp.usuario.Nome} - {usuarioCp.usuario.CodigoRf}</td></tr>");
                        }
                        mensagemHtml.Append("</table>");

                        await GerarPendencia(fechamentoId, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, mensagem.ToString(), usuarioCp.usuario.CodigoRf, mensagemHtml.ToString());
                    }
                }
                else
                {
                    foreach (var avaliacao in registrosAvaliacoesSemNotaParaNenhumAluno.OrderBy(x => x.DataAvaliacao))
                    {
                        var professor = usuariosPendencias
                            .FirstOrDefault(c => c.usuario.CodigoRf == avaliacao.ProfessorRf).usuario ?? usuariosPendencias.First(up => up.turmaCodigo.Equals(avaliacao.TurmaId) && up.disciplinaId == avaliacao.Disciplinas.First().DisciplinaId).usuario;

                        mensagem.AppendLine($"Professor {avaliacao.ProfessorRf} - {professor.Nome} - {avaliacao.NomeAvaliacao}.<br>");
                        mensagemHtml.Append($"<tr><td>{avaliacao.DataAvaliacao.ToString("dd/MM/yyyy")}</td><td>{avaliacao.NomeAvaliacao}</td><td>{professor.Nome} - {avaliacao.ProfessorRf}</td></tr>");
                    }
                    mensagemHtml.Append("</table>");

                    var professorRf = registrosAvaliacoesSemNotaParaNenhumAluno.FirstOrDefault()?.ProfessorRf;

                    if (string.IsNullOrWhiteSpace(professorRf))
                        professorRf = usuariosPendencias.First().usuario.CodigoRf;

                    await GerarPendencia(fechamentoId, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno, mensagem.ToString(), professorRf, mensagemHtml.ToString());
                }                
            }
            else
                repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);

            avaliacoesSemnota = registrosAvaliacoesSemNotaParaNenhumAluno.Count();
            return avaliacoesSemnota;
        }

        public async Task<int> ValidarPercentualAlunosAbaixoDaMedia(long fechamentoTurmaDisciplinaId, string justificativa, string criadoRF)
        {
            if (!string.IsNullOrEmpty(justificativa))
            {
                var percentualReprovacao = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));

                var mensagem = $"O fechamento do bimestre possui mais de {percentualReprovacao}% das notas consideradas insuficientes<br>";

                var mensagemHtml = $"<table><tr><td class=\"sem-borda\">O fechamento do bimestre possui mais de {percentualReprovacao}% das notas consideradas insuficientes. Justificativa : {justificativa} </td></tr></table>";

                await GerarPendencia(fechamentoTurmaDisciplinaId, TipoPendencia.ResultadosFinaisAbaixoDaMedia, mensagem, criadoRF, mensagemHtml);
                alunosAbaixoMedia = 1;
            }
            else
            {
                alunosAbaixoMedia = 0;
                repositorioPendencia.AtualizarPendencias(fechamentoTurmaDisciplinaId, SituacaoPendencia.Resolvida, TipoPendencia.ResultadosFinaisAbaixoDaMedia);
            }

            return alunosAbaixoMedia;
        }

        private IEnumerable<(string turmaCodigo, Usuario usuario, string disciplinaId, bool cp)> CarregaListaProfessores(IEnumerable<(string rf, string codigoTurma, string disciplnaId)> listaRFs)
        {
            foreach (var professorRF in listaRFs)
            {
                var professoresTurma = new List<string>();

                if (!string.IsNullOrWhiteSpace(professorRF.disciplnaId))
                {
                    var professoresTurmaDisciplina = mediator.Send(new ProfessoresTurmaDisciplinaQuery(professorRF.codigoTurma, professorRF.disciplnaId, DateTime.Today.Date)).Result;
                    professoresTurma.AddRange(professoresTurmaDisciplina.Select(pt => pt.CodigoRf));
                }
                else
                {
                    var titulares = mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(professorRF.codigoTurma)).Result;
                    professoresTurma.AddRange(titulares.Select(t => t.ProfessorRf));
                }
                
                var rfConsiderado = !string.IsNullOrWhiteSpace(professorRF.rf) ? professorRF.rf : professoresTurma.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(rfConsiderado))
                {
                    var professor = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(rfConsiderado).Result;

                    if (professor == null)
                        throw new NegocioException($"Professor com RF {professorRF} não encontrado.");

                    yield return (professorRF.codigoTurma, professor, professorRF.disciplnaId, false);
                }

                var turma = mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(professorRF.codigoTurma)).Result;
                var loginsCPs = servicoAbrangencia.ObterLoginsAbrangenciaUePorPerfil(turma.UeId, PERFIL_CP, false).Result;

                foreach (var loginCp in loginsCPs)
                {
                    var cp = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(loginCp).Result;
                    yield return (professorRF.codigoTurma, cp, professorRF.disciplnaId, true);
                }
            }
        }

        private async Task GerarPendencia(long fechamentoId, TipoPendencia tipoPendencia, string mensagem, string professorRf, string descricaoHtml)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                repositorioPendencia.ExcluirPendenciasFechamento(fechamentoId, tipoPendencia);

                var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(tipoPendencia, mensagem, "", tipoPendencia.Name(), descricaoHtml));

                await mediator.Send(new SalvarPendenciaFechamentoCommand(fechamentoId, pendenciaId));

                await RelacionaPendenciaUsuario(pendenciaId, professorRf);

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

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.VerificaPendenciasFechamentoTurma,
                                                           new VerificaPendenciasFechamentoCommand(pendenciaFechamento.FechamentoId),
                                                           Guid.NewGuid(),
                                                           null, 
                                                           false));
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

        public async Task<int> ValidarAlteracaoExtemporanea(long fechamentoId, string turmaCodigo, string professorRf)
        {
            var registrosNotasAlteradas = await repositorioFechamentoNota.ObterNotasEmAprovacaoPorFechamento(fechamentoId);

            if (registrosNotasAlteradas != null && registrosNotasAlteradas.Any())
            {
                var mensagem = $"Notas de fechamento alteradas fora do ano de vigência da turma {turmaCodigo}. Necessário aprovação do workflow";

                var mensagemHtml = $"<table><tr><td class=\"sem-borda\">Notas de fechamento alteradas fora do ano de vigência da turma {turmaCodigo}. Necessário aprovação do workflow</td></tr></table>";
                await GerarPendencia(fechamentoId, TipoPendencia.AlteracaoNotaFechamento, mensagem, professorRf, mensagemHtml);
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