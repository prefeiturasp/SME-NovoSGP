using MediatR;
using Minio.DataModel;
using SME.SGP.Aplicacao;
using SME.SGP.Dados.Mapeamentos;
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
        private readonly IRepositorioAulaConsulta repositorioAula;
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
                                          IRepositorioAulaConsulta repositorioAula,
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

        public async Task<int> ValidarAulasReposicaoPendente(long fechamentoId, string turmaCodigo, string turmaNome, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre, long turmaId)
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
                    var professoresTitulares = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(aula.TurmaId));

                    Usuario professor = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(aula.ProfessorRf);

                    mensagem.AppendLine($"Professor {professor.CodigoRf} - {professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")}.<br>");
                    mensagemHtml.Append($"<tr><td>{aula.DataAula.ToString("dd/MM/yyyy")}</td><td>{professor.Nome} - {professor.CodigoRf}</td></tr>");
                }
                mensagemHtml.Append("</table>");
                var professorRf = aulasPendentes.First().ProfessorRf;
                await GerarPendencia(fechamentoId, TipoPendencia.AulasReposicaoPendenteAprovacao, mensagem.ToString(), professorRf, mensagemHtml.ToString(), bimestre, turmaId);
            }
            else
                await repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasReposicaoPendenteAprovacao);

            aulasReposicaoPendentes = aulasPendentes.Count();
            return aulasReposicaoPendentes;
        }

        private async Task<ProfessorTitularDisciplinaEol> BuscaProfessorTitularPorTurmaEDisciplina(string turmaCodigo, long disciplinaId)
        {
            var professoresTitularesPorTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(turmaCodigo));
            var professorTitularPorTurmaEDisciplina = professoresTitularesPorTurma.FirstOrDefault(o => o.DisciplinasId.Contains(disciplinaId));

            if (professorTitularPorTurmaEDisciplina == null)
                throw new NegocioException($"Não existe professor titular para esta turma/disciplina {turmaCodigo}/{disciplinaId}");

            return professorTitularPorTurmaEDisciplina;
        }

        private async Task<DisciplinaDto> BuscaInformacoesDaDisciplina(long disciplinaId)
        {
            var componenteCurricular = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId })).ToList()?.FirstOrDefault();

            if (componenteCurricular == null)
                throw new NegocioException("Componente curricular não encontrado.");

            return componenteCurricular; //teste
        }

        public async Task GerarPendenciaAulasFechamento(TipoPendencia tipoPendencia, string msg, string msgHtml, IEnumerable<Aula> aulas, IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDaTurma, long fechamentoId, long disciplinaId, int bimestre, long turmaId, bool aulasCJ = false)
        {

            var mensagem = new StringBuilder(msg);
            var mensagemHtml = new StringBuilder(msgHtml);

            var usuariosPendencias = CarregaListaProfessores(aulas.Select(a => (a.ProfessorRf, a.TurmaId, a.DisciplinaId)).Distinct(), professoresTitularesDaTurma).ToList();
            var idsAula = aulas.Select(a => a.Id).Distinct().ToList();

            if (usuariosPendencias.All(u => u.cp))
            {
                foreach (var aula in aulas.OrderBy(x => x.DataAula))
                {
                    mensagem.AppendLine($"CP @CodigoRfCP - @NomeCP, dia {aula.DataAula.ToString("dd/MM/yyyy")}{(aula.EhReposicao() ? " - Reposição" : String.Empty)}.<br>");
                    mensagemHtml.Append($"<tr><td>{aula.DataAula.ToString("dd/MM/yyyy")}{(aula.EhReposicao() ? " - Reposição" : String.Empty)}</td><td>@NomeCP - @CodigoRfCP</td></tr>");
                }
                mensagemHtml.Append("</table>");
                foreach (var usuarioCp in usuariosPendencias)
                {
                    var mensagemReplace = mensagem.ToString().Replace("@CodigoRfCP", usuarioCp.usuario.CodigoRf).Replace("@NomeCP", usuarioCp.usuario.Nome);
                    var mensagemHtmlReplace = mensagemHtml.ToString().Replace("@CodigoRfCP", usuarioCp.usuario.CodigoRf).Replace("@NomeCP", usuarioCp.usuario.Nome);
                    await GerarPendencia(fechamentoId, tipoPendencia, mensagemReplace, usuarioCp.usuario.CodigoRf, mensagemHtmlReplace, bimestre, turmaId, idsAula, null, true);
                }
            }
            else
            {
                foreach (var aula in aulas.OrderBy(x => x.DataAula))
                {
                    var professor = usuariosPendencias
                        .FirstOrDefault(c => c.usuario.CodigoRf == aula.ProfessorRf && professoresTitularesDaTurma.Any(p => p.ProfessorRf == c.usuario.CodigoRf)).usuario ?? usuariosPendencias.First(up => up.turmaCodigo.Equals(aula.TurmaId) && up.disciplinaId == aula.DisciplinaId).usuario;

                    var professorTitularAtualDaTurma = professoresTitularesDaTurma.FirstOrDefault(p => p.DisciplinasId.Any(d => d == long.Parse(aula.DisciplinaId)));

                    professor = professorTitularAtualDaTurma != null && professor.CodigoRf != professorTitularAtualDaTurma.ProfessorRf
                        ? new Usuario()
                        {
                            CodigoRf = professorTitularAtualDaTurma.ProfessorRf,
                            Nome = professorTitularAtualDaTurma.ProfessorNome
                        } 
                        : professor;

                    mensagem.AppendLine($"Professor {professor.CodigoRf} - {professor.Nome}, dia {aula.DataAula.ToString("dd/MM/yyyy")} {(aula.EhReposicao() ? " - Reposição" : String.Empty)}.<br>");
                    mensagemHtml.Append($"<tr><td>{aula.DataAula.ToString("dd/MM/yyyy")}{(aula.EhReposicao() ? " - Reposição" : String.Empty)}</td><td>{professor.Nome} - {professor.CodigoRf}</td></tr>");
                }
                mensagemHtml.Append("</table>");

                var professorRf = professoresTitularesDaTurma.Where(professor => professor.DisciplinasId.Contains(disciplinaId)).FirstOrDefault()?.ProfessorRf;
                if (string.IsNullOrWhiteSpace(professorRf) || aulasCJ)
                    professorRf = aulas.FirstOrDefault()?.ProfessorRf;

                if (string.IsNullOrWhiteSpace(professorRf))
                    professorRf = usuariosPendencias.First().usuario.CodigoRf;

                await GerarPendencia(fechamentoId, tipoPendencia, mensagem.ToString(), professorRf, mensagemHtml.ToString(), bimestre, turmaId, idsAula, null, true);
            }
        }

        public async Task GerarPendenciaAulasFechamento(TipoPendencia tipoPendencia, string msg, string msgHtml, IEnumerable<AtividadeAvaliativa> atividades, IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDaTurma, long fechamentoId, long disciplinaId, int bimestre, long turmaId, bool aulasCJ = false)
        {
            var mensagem = new StringBuilder(msg);
            var mensagemHtml = new StringBuilder(msgHtml);

            var atividadesTurma = atividades.Select(a => (a.ProfessorRf, a.TurmaId, a.Disciplinas.Any() ? a.Disciplinas.First().DisciplinaId : null)).Distinct().ToList();
            var usuariosPendencias = CarregaListaProfessores(atividadesTurma, professoresTitularesDaTurma).ToList();
            var idsAtividadeAvaliativa = atividades.Select(a => a.Id).Distinct().ToList();

            if (usuariosPendencias.All(u => u.cp))
            {
                foreach (var atividade in atividades.OrderBy(x => x.DataAvaliacao))
                {
                    mensagem.AppendLine($"CP @CodigoRfCP - @NomeCP, dia {atividade.DataAvaliacao.ToString("dd/MM/yyyy")}.<br>");
                    mensagemHtml.Append($"<tr><td>{atividade.DataAvaliacao.ToString("dd/MM/yyyy")}</td><td>@NomeCP - @CodigoRfCP</td></tr>");
                }
                mensagemHtml.Append("</table>");
                foreach (var usuarioCp in usuariosPendencias)
                {
                    var mensagemReplace = mensagem.ToString().Replace("@CodigoRfCP", usuarioCp.usuario.CodigoRf).Replace("@NomeCP", usuarioCp.usuario.Nome);
                    var mensagemHtmlReplace = mensagemHtml.ToString().Replace("@CodigoRfCP", usuarioCp.usuario.CodigoRf).Replace("@NomeCP", usuarioCp.usuario.Nome);
                    await GerarPendencia(fechamentoId, tipoPendencia, mensagemReplace, usuarioCp.usuario.CodigoRf, mensagemHtmlReplace, bimestre, turmaId, null, idsAtividadeAvaliativa, true);
                }
            }
            else
            {
                foreach (var atividade in atividades.OrderBy(x => x.DataAvaliacao))
                {
                    var professor = usuariosPendencias
                        .FirstOrDefault(c => c.usuario.CodigoRf == atividade.ProfessorRf && professoresTitularesDaTurma.Any(p => p.ProfessorRf == c.usuario.CodigoRf)).usuario ??
                                                  usuariosPendencias.First(up => up.turmaCodigo.Equals(atividade.TurmaId) && 
                                                                           (atividade.Disciplinas.Any() ? up.disciplinaId == atividade.Disciplinas.First().DisciplinaId : true)).usuario;


                    mensagem.AppendLine($"Professor {professor.CodigoRf} - {professor.Nome}, dia {atividade.DataAvaliacao.ToString("dd/MM/yyyy")}.<br>");
                    mensagemHtml.Append($"<tr><td>{atividade.DataAvaliacao.ToString("dd/MM/yyyy")}</td><td>{atividade.NomeAvaliacao}</td><td>{professor.Nome} - {professor.CodigoRf}</td></tr>");
                }
                mensagemHtml.Append("</table>");

                var professorRf = professoresTitularesDaTurma.Where(professor => professor.DisciplinasId.Contains(disciplinaId)).FirstOrDefault()?.ProfessorRf;
                if (string.IsNullOrWhiteSpace(professorRf) || aulasCJ)
                    professorRf = atividades.FirstOrDefault()?.ProfessorRf;

                if (string.IsNullOrWhiteSpace(professorRf))
                    professorRf = usuariosPendencias.First().usuario.CodigoRf;

                await GerarPendencia(fechamentoId, tipoPendencia, mensagem.ToString(), professorRf, mensagemHtml.ToString(), bimestre, turmaId, null, idsAtividadeAvaliativa, true);
            }
        }

        public async Task<int> ValidarAulasSemFrequenciaRegistrada(long fechamentoId, string turmaCodigo, string turmaNome, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre, long turmaId)
        {
            var registrosAulas = repositorioAula
                .ObterAulasSemFrequenciaRegistrada(turmaCodigo, disciplinaId.ToString(), inicioPeriodo, fimPeriodo)
                .Where(a => a.PermiteRegistroFrequencia());
                
            var registrosAulasSemFrequencia = (await ObterAulasValidasParaPendencia(registrosAulas, fechamentoId, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento)).ToList();

            if (registrosAulasSemFrequencia != null && registrosAulasSemFrequencia.Any())
            {
                var componenteCurricular = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId })).ToList()?.FirstOrDefault();
                if (componenteCurricular == null)
                    throw new NegocioException("Componente curricular não encontrado.");

                var aulasNormais = registrosAulasSemFrequencia.Where(w => !w.AulaCJ);
                var aulasCJ = registrosAulasSemFrequencia.Where(w => w.AulaCJ);
                
                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turmaNome} a seguir estão sem frequência:<br>");
                var mensagemHtml = new StringBuilder($"<table><tr class=\"nao-exibir\"><td colspan=\"2\">A aulas de {componenteCurricular.Nome} da turma {turmaNome} a seguir estão sem frequência:</td></tr>");
                mensagemHtml.Append("<tr class=\"cabecalho\"><td>Data da aula</td><td>Professor</td></tr>");


                await ExcluirPendencia(fechamentoId, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento);
                if (aulasNormais.Any())
                {
                    var professoresTitularesDaTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(turmaCodigo));
                    await GerarPendenciaAulasFechamento(TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, mensagem.ToString(), mensagemHtml.ToString(), aulasNormais, professoresTitularesDaTurma, fechamentoId, disciplinaId, bimestre, turmaId);
                }
                if (aulasCJ.Any())
                    await GerarPendenciaAulasFechamento(TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, mensagem.ToString(), mensagemHtml.ToString(), aulasCJ, new List<ProfessorTitularDisciplinaEol>(), fechamentoId, disciplinaId, bimestre, turmaId, true);
            }
            else
                await repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasSemFrequenciaNaDataDoFechamento);

            aulasSemFrequencia = registrosAulasSemFrequencia?.Count ?? 0;
            return aulasSemFrequencia;
        }

        public async Task<int> ValidarAulasSemPlanoAulaNaDataDoFechamento(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre, long turmaId)
        {
            var registrosAulas = repositorioAula.ObterAulasSemPlanoAulaNaDataAtual(turma.CodigoTurma,
                                                                            disciplinaId.ToString(),
                                                                            inicioPeriodo,
                                                                            fimPeriodo);

            var registrosAulasSemPlanoAula = await ObterAulasValidasParaPendencia(registrosAulas, fechamentoId, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);

            if (registrosAulasSemPlanoAula != null && registrosAulasSemPlanoAula.Any())
            {
                var componenteCurricular = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId })).ToList()?.FirstOrDefault();

                if (componenteCurricular == null)
                    throw new NegocioException("Componente curricular não encontrado.");

                var mensagem = new StringBuilder($"A aulas de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão sem plano de aula registrado até a data do fechamento:<br>");
                var mensagemHtml = new StringBuilder($"<table><tr class=\"nao-exibir\"><td colspan=\"2\">A aulas de {componenteCurricular.Nome} da turma {turma.Nome} a seguir estão sem plano de aula registrado até a data do fechamento:</td></tr>");
                mensagemHtml.Append("<tr class=\"cabecalho\"><td>Data da aula</td><td>Professor</td></tr>");

                var aulasNormais = registrosAulasSemPlanoAula.Where(w => !w.AulaCJ);
                var aulasCJ = registrosAulasSemPlanoAula.Where(w => w.AulaCJ);

                await ExcluirPendencia(fechamentoId, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);
                if (aulasNormais.Any())
                {
                    var professoresTitularesDaTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(turma.CodigoTurma));
                    await GerarPendenciaAulasFechamento(TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento, mensagem.ToString(), mensagemHtml.ToString(), aulasNormais, professoresTitularesDaTurma, fechamentoId, disciplinaId, bimestre, turmaId);
                }
                if (aulasCJ.Any())
                    await GerarPendenciaAulasFechamento(TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento, mensagem.ToString(), mensagemHtml.ToString(), aulasCJ, new List<ProfessorTitularDisciplinaEol>(), fechamentoId, disciplinaId, bimestre, turmaId, true);
            }
            else
                await repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento);

            aulasSemPlanoAula = registrosAulasSemPlanoAula.Count();
            return aulasSemPlanoAula;
        }

        public async Task<int> ValidarAvaliacoesSemNotasParaNenhumAluno(long fechamentoId, string codigoTurma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre, long turmaId)
        {
            var registrosAvaliacoes = repositorioAtividadeAvaliativa.ObterAtividadesAvaliativasSemNotaParaNenhumAluno(codigoTurma,
                                                                            disciplinaId.ToString(),
                                                                            inicioPeriodo,
                                                                            fimPeriodo,
                                                                            (int)TipoAvaliacaoCodigo.AtividadeClassroom);

            var registrosAvaliacoesSemNotaParaNenhumAluno = await ObterAtividadeesValidasParaPendencia(registrosAvaliacoes, fechamentoId, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);

            if (registrosAvaliacoesSemNotaParaNenhumAluno != null && registrosAvaliacoesSemNotaParaNenhumAluno.Any())
            {
                var mensagem = new StringBuilder($"As avaliações a seguir não tiveram notas lançadas para nenhum aluno<br>");
                var mensagemHtml = new StringBuilder($"<table><tr class=\"nao-exibir\"><td colspan=\"3\">As avaliações a seguir não tiveram notas lançadas para nenhum aluno:</td></tr>");
                mensagemHtml.Append("<tr class=\"cabecalho\"><td>Data da avaliação</td><td>Título</td><td>Professor</td></tr>");

                var aulasNormais = registrosAvaliacoesSemNotaParaNenhumAluno.Where(w => !w.EhCj);
                var aulasCJ = registrosAvaliacoesSemNotaParaNenhumAluno.Where(w => w.EhCj);

                await ExcluirPendencia(fechamentoId, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);
                if (aulasNormais.Any())
                {
                    var professoresTitularesDaTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(codigoTurma));
                    await GerarPendenciaAulasFechamento(TipoPendencia.AvaliacaoSemNotaParaNenhumAluno, mensagem.ToString(), mensagemHtml.ToString(), aulasNormais, professoresTitularesDaTurma, fechamentoId, disciplinaId, bimestre, turmaId);
                }
                if (aulasCJ.Any())
                    await GerarPendenciaAulasFechamento(TipoPendencia.AvaliacaoSemNotaParaNenhumAluno, mensagem.ToString(), mensagemHtml.ToString(), aulasCJ, new List<ProfessorTitularDisciplinaEol>(), fechamentoId, disciplinaId, bimestre, turmaId, true);
            }
            else
                await repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AvaliacaoSemNotaParaNenhumAluno);

            avaliacoesSemnota = registrosAvaliacoesSemNotaParaNenhumAluno.Count();
            return avaliacoesSemnota;
        }

        public async Task<int> ValidarPercentualAlunosAbaixoDaMedia(long fechamentoTurmaDisciplinaId, string justificativa, string criadoRF, int bimestre, long turmaId)
        {
            if (!string.IsNullOrEmpty(justificativa))
            {
                var percentualReprovacao = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));

                var mensagem = $"O fechamento do bimestre possui mais de {percentualReprovacao}% das notas consideradas insuficientes<br>";

                var mensagemHtml = $"<table><tr><td class=\"sem-borda\">O fechamento do bimestre possui mais de {percentualReprovacao}% das notas consideradas insuficientes. Justificativa : {justificativa} </td></tr></table>";

                await GerarPendencia(fechamentoTurmaDisciplinaId, TipoPendencia.ResultadosFinaisAbaixoDaMedia, mensagem, criadoRF, mensagemHtml, bimestre, turmaId);
                alunosAbaixoMedia = 1;
            }
            else
            {
                alunosAbaixoMedia = 0;
                await repositorioPendencia.AtualizarPendencias(fechamentoTurmaDisciplinaId, SituacaoPendencia.Resolvida, TipoPendencia.ResultadosFinaisAbaixoDaMedia);
            }

            return alunosAbaixoMedia;
        }

        private IEnumerable<(string turmaCodigo, Usuario usuario, string disciplinaId, bool cp)> CarregaListaProfessores(IEnumerable<(string rf, string codigoTurma, string disciplnaId)> listaRFs, IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDaTurma)
        {
            foreach (var professorRF in listaRFs)
            {
                var rfProfTitularTurma = string.Empty;

                if (!string.IsNullOrWhiteSpace(professorRF.disciplnaId))
                    rfProfTitularTurma = professoresTitularesDaTurma.Where(professor => professor.DisciplinasId.Contains(long.Parse(professorRF.disciplnaId))).FirstOrDefault()?.ProfessorRf;
                else
                    rfProfTitularTurma = professoresTitularesDaTurma.FirstOrDefault()?.ProfessorRf;
                
                var rfConsiderado = !string.IsNullOrWhiteSpace(professorRF.rf) ? professorRF.rf : rfProfTitularTurma;
                if (!string.IsNullOrWhiteSpace(rfConsiderado))
                {
                    var professor = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(rfConsiderado).Result;
                    if (professor == null)
                        throw new NegocioException($"Professor com RF {rfConsiderado} não encontrado.");

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

        private Task ExcluirPendencia(long fechamentoId,
                                        TipoPendencia tipoPendencia)
        {
            return repositorioPendencia.ExcluirPendenciasFechamento(fechamentoId, tipoPendencia);
        }

        private async Task GerarPendencia(
                                        long fechamentoId, 
                                        TipoPendencia tipoPendencia, 
                                        string mensagem, 
                                        string professorRf, 
                                        string descricaoHtml, 
                                        int bimestre, 
                                        long turmaId, 
                                        IEnumerable<long> idsAula = null,
                                        IEnumerable<long> idsAtividadeAvaliativa = null,
                                        bool IgnorarExclusaoPendencia = false)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                if (!IgnorarExclusaoPendencia)
                    await ExcluirPendencia(fechamentoId, tipoPendencia);

                var tituloPendencia = $"{tipoPendencia.Name()} - {bimestre}º bimestre";
                var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(tipoPendencia, null, turmaId, mensagem, "", tituloPendencia, descricaoHtml));
                var pendenciaFechamentoId = await mediator.Send(new SalvarPendenciaFechamentoCommand(fechamentoId, pendenciaId));                

                await RelacionaPendenciaUsuario(pendenciaId, professorRf);
                await GerarPendenciaFechamentoAula(pendenciaFechamentoId, idsAula);
                await GerarPendenciaFechamentoAtividadeAvaliativa(pendenciaFechamentoId, idsAtividadeAvaliativa);

                unitOfWork.PersistirTransacao();
            }
        }

        private async Task GerarPendenciaFechamentoAula(long pendenciaFechamentoId, IEnumerable<long> idsAula)
        {
            if (pendenciaFechamentoId > 0 && idsAula != null && idsAula.Any())
            {
                foreach(var idAula in idsAula)
                {
                    await mediator.Send(new SalvarPendenciaFechamentoAulaCommand(idAula, pendenciaFechamentoId));
                }
            }
        }

        private async Task GerarPendenciaFechamentoAtividadeAvaliativa(long pendenciaFechamentoId, IEnumerable<long> idsAtividadeAvaliativa)
        {
            if (pendenciaFechamentoId > 0 && idsAtividadeAvaliativa != null && idsAtividadeAvaliativa.Any())
            {
                foreach (var idAtividadeAvaliativa in idsAtividadeAvaliativa)
                {
                    await mediator.Send(new SalvarPendenciaFechamentoAtividadeAvaliativaCommand(idAtividadeAvaliativa, pendenciaFechamentoId));
                }
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

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.VerificaPendenciasFechamentoTurma,
                                                           new VerificaPendenciasFechamentoCommand(pendenciaFechamento.FechamentoId, pendenciaFechamento.Bimestre, pendenciaFechamento.TurmaId),
                                                           Guid.NewGuid(),
                                                           null, 
                                                           false));
            return auditoriaDto;
        }

        public bool VerificarPendenciasEmAbertoPorFechamento(long fechamentoId)
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

        public async Task<int> ValidarAlteracaoExtemporanea(long fechamentoId, string turmaCodigo, string professorRf, int bimestre, long turmaId)
        {
            var registrosNotasAlteradas = await repositorioFechamentoNota.ObterNotasEmAprovacaoPorFechamento(fechamentoId);

            if (registrosNotasAlteradas != null && registrosNotasAlteradas.Any())
            {
                var mensagem = $"Notas de fechamento alteradas fora do ano de vigência da turma {turmaCodigo}. Necessário aprovação do workflow";

                var mensagemHtml = $"<table><tr><td class=\"sem-borda\">Notas de fechamento alteradas fora do ano de vigência da turma {turmaCodigo}. Necessário aprovação do workflow</td></tr></table>";
                await GerarPendencia(fechamentoId, TipoPendencia.AlteracaoNotaFechamento, mensagem, professorRf, mensagemHtml, bimestre, turmaId);
            }
            else
                await repositorioPendencia.AtualizarPendencias(fechamentoId, SituacaoPendencia.Resolvida, TipoPendencia.AlteracaoNotaFechamento);

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

        private async Task<IEnumerable<Aula>> ObterAulasValidasParaPendencia(IEnumerable<Aula> aulas, long fechamentoId, TipoPendencia tipoPendencia)
        {
            var idsPendenciaFechamento = await mediator.Send(new ObterIdPendenciaFechamentoAprovadaResolvidaQuery(fechamentoId, tipoPendencia));

            if (idsPendenciaFechamento.Any())
            {
                var idsAulaFechamento = await mediator.Send(new ObterIdsAulaDaPendenciaDeFechamentoQuery(idsPendenciaFechamento));

                return aulas.ToList().FindAll(a => !idsAulaFechamento.Contains(a.Id));
            }

            return aulas;
        }

        private async Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadeesValidasParaPendencia(IEnumerable<AtividadeAvaliativa> atividadeAvaliativas, long fechamentoId, TipoPendencia tipoPendencia)
        {
            var idsPendenciaFechamento = await mediator.Send(new ObterIdPendenciaFechamentoAprovadaResolvidaQuery(fechamentoId, tipoPendencia));

            if (idsPendenciaFechamento.Any())
            {
                var idsAtividadesAvaliativas = await mediator.Send(new ObterIdsAtividadeAvaliativaDaPendenciaDeFechamentoQuery(idsPendenciaFechamento));

                return atividadeAvaliativas.ToList().FindAll(a => !idsAtividadesAvaliativas.Contains(a.Id));
            }

            return atividadeAvaliativas;
        }
    }
}