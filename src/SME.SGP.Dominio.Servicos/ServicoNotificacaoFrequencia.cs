using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoNotificacaoFrequencia : IServicoNotificacaoFrequencia
    {
        private const string breadcrumb = "SGP API - Notificação de Frequência";
        private readonly IConfiguration configuration;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IRepositorioUe repositorioUe;
        private readonly string sentryDSN;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoNotificacaoFrequencia(IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia,
                                            IRepositorioParametrosSistema repositorioParametrosSistema,
                                            IRepositorioFrequencia repositorioFrequencia,
                                            IRepositorioAula repositorioAula,
                                            IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                            IServicoNotificacao servicoNotificacao,
                                            IServicoUsuario servicoUsuario,
                                            IServicoEOL servicoEOL,
                                            IConfiguration configuration,
                                            IRepositorioDre repositorioDre,
                                            IRepositorioUe repositorioUe)
        {
            this.repositorioNotificacaoFrequencia = repositorioNotificacaoFrequencia ?? throw new ArgumentNullException(nameof(repositorioNotificacaoFrequencia));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            sentryDSN = configuration.GetValue<string>("Sentry:DSN");
        }

        public void ExecutaNotificacaoFrequencia()
        {
            using (SentrySdk.Init(sentryDSN))
            {
                try
                {
                    try
                    {
                        var dres = repositorioDre.ObterTodas();
                        if (dres != null)
                        {
                            foreach (var dre in dres)
                            {
                                try
                                {
                                    var ues = repositorioUe.ObterPorDre(dre.Id);
                                    if (ues != null)
                                    {
                                        foreach (var ue in ues)
                                        {
                                            try
                                            {
                                                NotificarAusenciaFrequencia(TipoNotificacaoFrequencia.Professor, ue.CodigoUe);
                                            }
                                            catch (Exception ex)
                                            {
                                                SentrySdk.CaptureException(ex);
                                                SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"{breadcrumb} - {TipoNotificacaoFrequencia.Professor} - {ue.CodigoUe}")));
                                            }
                                            try
                                            {
                                                NotificarAusenciaFrequencia(TipoNotificacaoFrequencia.GestorUe, ue.CodigoUe);
                                            }
                                            catch (Exception ex)
                                            {
                                                SentrySdk.CaptureException(ex);
                                                SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"{breadcrumb} - {TipoNotificacaoFrequencia.GestorUe} - {ue.CodigoUe}")));
                                            }
                                            try
                                            {
                                                NotificarAusenciaFrequencia(TipoNotificacaoFrequencia.SupervisorUe, ue.CodigoUe);
                                            }
                                            catch (Exception ex)
                                            {
                                                SentrySdk.CaptureException(ex);
                                                SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"{breadcrumb} - {TipoNotificacaoFrequencia.SupervisorUe} - {ue.CodigoUe}")));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"{breadcrumb} - Nenhuma UE encontrada no banco de dados")));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SentrySdk.CaptureException(ex);
                                    SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"{breadcrumb} - Nenhuma UE encontrada no banco de dados")));
                                }
                            }
                        }
                        else
                        {
                            SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"{breadcrumb} - Nenhuma DRE encontrada no banco de dados")));
                        }
                    }
                    catch (Exception ex)
                    {
                        SentrySdk.CaptureEvent(new SentryEvent(ex));
                        SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"{breadcrumb} - Nenhuma DRE encontrada no banco de dados")));
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureEvent(new SentryEvent(ex));
                    SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"{breadcrumb} - {ex.Message}")));
                }
            }
        }

        public void VerificaRegraAlteracaoFrequencia(long registroFrequenciaId, DateTime criadoEm, DateTime alteradoEm, long usuarioAlteracaoId)
        {
            // Parametro do sistema de dias para notificacao
            var qtdDiasParametro = int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(
                                                    TipoParametroSistema.QuantidadeDiasNotificarAlteracaoChamadaEfetivada,
                                                    DateTime.Now.Year));

            var qtdDiasAlteracao = (alteradoEm.Date - criadoEm.Date).TotalDays;

            // Verifica se ultrapassou o limite de dias para alteração
            if (qtdDiasAlteracao >= qtdDiasParametro)
            {
                var usuariosNotificacao = new List<Usuario>();

                // Dados da Aula
                var registroFrequencia = repositorioFrequencia.ObterAulaDaFrequencia(registroFrequenciaId);
                MeusDadosDto professor = servicoEOL.ObterMeusDados(registroFrequencia.ProfessorRf).Result;

                // Gestores
                var usuarios = BuscaGestoresUe(registroFrequencia.CodigoUe);
                if (usuarios != null)
                    usuariosNotificacao.AddRange(usuarios);

                // Supervisores
                usuarios = BuscaSupervisoresUe(registroFrequencia.CodigoUe);
                if (usuarios != null)
                    usuariosNotificacao.AddRange(usuarios);

                foreach (var usuario in usuariosNotificacao)
                {
                    NotificaAlteracaoFrequencia(usuario, registroFrequencia, professor.Nome);
                }
            }
        }

        private IEnumerable<Usuario> BuscaGestoresUe(string codigoUe)
        {
            // Buscar gestor da Ue
            List<UsuarioEolRetornoDto> funcionariosRetornoEol = new List<UsuarioEolRetornoDto>();

            var cps = servicoEOL.ObterFuncionariosPorCargoUe(codigoUe, (int)Cargo.CP);
            if (cps != null && cps.Any())
                funcionariosRetornoEol.AddRange(cps);
            var diretores = servicoEOL.ObterFuncionariosPorCargoUe(codigoUe, (int)Cargo.Diretor);
            if (diretores != null && diretores.Any())
                funcionariosRetornoEol.AddRange(diretores);

            var usuarios = new List<Usuario>();
            foreach (var usuarioEol in funcionariosRetornoEol)
                usuarios.Add(servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuarioEol.CodigoRf));

            return usuarios;
        }

        private IEnumerable<Usuario> BuscaProfessorAula(RegistroFrequenciaFaltanteDto turma)
        {
            // Buscar professor da ultima aula
            var professorRf = turma.Aulas
                    .OrderBy(o => o.DataAula)
                    .Last().ProfessorId;
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(professorRf.ToString());

            return usuario != null ? new List<Usuario>()
            {
                usuario
            } : null;
        }

        private IEnumerable<Usuario> BuscaSupervisoresUe(string codigoUe)
        {
            // Buscar supervisor da Ue
            var supervisoresEscola = repositorioSupervisorEscolaDre.ObtemSupervisoresPorUe(codigoUe);
            if (supervisoresEscola == null || supervisoresEscola.Count() == 0)
                return null;

            var usuarios = new List<Usuario>();
            foreach (var supervisorEscola in supervisoresEscola)
                usuarios.Add(servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(supervisorEscola.SupervisorId));

            return usuarios;
        }

        private IEnumerable<Usuario> BuscaUsuarioNotificacao(RegistroFrequenciaFaltanteDto turma, TipoNotificacaoFrequencia tipo)
        {
            IEnumerable<Usuario> usuarios = Enumerable.Empty<Usuario>();
            switch (tipo)
            {
                case TipoNotificacaoFrequencia.Professor:
                    usuarios = BuscaProfessorAula(turma);
                    break;

                case TipoNotificacaoFrequencia.GestorUe:
                    usuarios = BuscaGestoresUe(turma.CodigoUe);
                    break;

                case TipoNotificacaoFrequencia.SupervisorUe:
                    usuarios = BuscaSupervisoresUe(turma.CodigoUe);
                    break;

                default:
                    usuarios = BuscaProfessorAula(turma);
                    break;
            }
            return usuarios;
        }

        private void NotificaAlteracaoFrequencia(Usuario usuario, RegistroFrequenciaAulaDto registroFrequencia, string usuarioAlteracao)
        {
            // TODO carregar nomes da turma, escola, disciplina e professor para notificacao
            var disciplina = ObterNomeDisciplina(registroFrequencia.CodigoDisciplina);

            var tituloMensagem = $"Título: Alteração extemporânea de frequência  da turma {registroFrequencia.NomeTurma} na disciplina {disciplina}.";

            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.Append($"O Professor {usuarioAlteracao} realizou alterações no registro de frequência do dia {registroFrequencia.DataAula} da turma {registroFrequencia.NomeTurma} ({registroFrequencia.NomeUe}) na disciplina {disciplina}.");

            var hostAplicacao = configuration["UrlFrontEnd"];
            mensagemUsuario.Append($"<a href='{hostAplicacao}/diario-classe/frequencia-plano-aula'>Clique aqui para acessar esse registro.</a>");

            var notificacao = new Notificacao()
            {
                Ano = DateTime.Now.Year,
                Categoria = NotificacaoCategoria.Alerta,
                Tipo = NotificacaoTipo.Frequencia,
                Titulo = tituloMensagem,
                Mensagem = mensagemUsuario.ToString(),
                UsuarioId = usuario.Id,
                TurmaId = registroFrequencia.CodigoTurma,
                UeId = registroFrequencia.CodigoUe,
                DreId = registroFrequencia.CodigoDre,
            };
            servicoNotificacao.Salvar(notificacao);
        }

        private void NotificarAusenciaFrequencia(TipoNotificacaoFrequencia tipo, string ueId)
        {
            // Busca registro de aula sem frequencia e sem notificação do tipo
            IEnumerable<RegistroFrequenciaFaltanteDto> turmasSemRegistro = null;
            try
            {
                Console.WriteLine($"Buscando turmas: {tipo} - {ueId}");

                turmasSemRegistro = repositorioNotificacaoFrequencia.ObterTurmasSemRegistroDeFrequencia(tipo, ueId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (turmasSemRegistro != null)
            {
                Console.WriteLine(turmasSemRegistro.Count());
                Console.WriteLine($"Buscando quantidade de aulas: {tipo} ");
                // Busca parametro do sistema de quantidade de aulas sem frequencia para notificação
                var qtdAulasNotificacao = QuantidadeAulasParaNotificacao(tipo);
                foreach (var turma in turmasSemRegistro)
                {
                    Console.WriteLine($"Buscando aulas: {turma.CodigoTurma} - {turma.DisciplinaId} ");
                    // Carrega todas as aulas sem registro de frequencia da turma e disciplina para notificação
                    turma.Aulas = repositorioFrequencia.ObterAulasSemRegistroFrequencia(turma.CodigoTurma, turma.DisciplinaId);
                    if (turma.Aulas != null && turma.Aulas.Count() >= qtdAulasNotificacao)
                    {
                        Console.WriteLine($"Busca Professor/Gestor/Supervisor: {tipo} ");
                        // Busca Professor/Gestor/Supervisor da Turma ou Ue
                        var usuarios = BuscaUsuarioNotificacao(turma, tipo);

                        if (usuarios != null)
                            foreach (var usuario in usuarios)
                            {
                                NotificaRegistroFrequencia(usuario, turma, tipo);
                            }
                    }
                }
            }
        }

        private void NotificaRegistroFrequencia(Usuario usuario, RegistroFrequenciaFaltanteDto turmaSemRegistro, TipoNotificacaoFrequencia tipo)
        {
            Console.WriteLine($"Busca disciplina Eol: {turmaSemRegistro.DisciplinaId} ");
            var disciplina = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(turmaSemRegistro.DisciplinaId) }).FirstOrDefault();

            var tituloMensagem = $"Frequência da turma {turmaSemRegistro.NomeTurma} - {turmaSemRegistro.DisciplinaId} ({turmaSemRegistro.NomeUe})";
            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.Append($"A turma a seguir esta a <b>{turmaSemRegistro.Aulas.Count()} aulas</b> sem registro de frequência da turma");
            mensagemUsuario.Append("<br />");
            mensagemUsuario.Append($"<br />Escola: <b>{turmaSemRegistro.NomeUe}</b>");
            mensagemUsuario.Append($"<br />Turma: <b>{turmaSemRegistro.NomeTurma}</b>");
            mensagemUsuario.Append($"<br />Disciplina: <b>{disciplina.Nome}</b>");
            mensagemUsuario.Append($"<br />Aulas:");

            mensagemUsuario.Append("<ul>");
            foreach (var aula in turmaSemRegistro.Aulas)
            {
                mensagemUsuario.Append($"<li>Data: {aula.DataAula}</li>");
            }
            mensagemUsuario.Append("</ul>");

            var hostAplicacao = configuration["UrlFrontEnd"];
            var parametros = $"turma={turmaSemRegistro.CodigoTurma}&DataAula={turmaSemRegistro.Aulas.FirstOrDefault().DataAula.ToShortDateString()}&disciplina={turmaSemRegistro.DisciplinaId}";
            mensagemUsuario.Append($"<a href='{hostAplicacao}diario-classe/frequencia-plano-aula?{parametros}'>Clique aqui para regularizar.</a>");

            var notificacao = new Notificacao()
            {
                Ano = DateTime.Now.Year,
                Categoria = NotificacaoCategoria.Alerta,
                Tipo = NotificacaoTipo.Frequencia,
                Titulo = tituloMensagem,
                Mensagem = mensagemUsuario.ToString(),
                UsuarioId = usuario.Id,
                TurmaId = turmaSemRegistro.CodigoTurma,
                UeId = turmaSemRegistro.CodigoUe,
                DreId = turmaSemRegistro.CodigoDre,
            };
            Console.WriteLine($"Salva notificação");
            try
            {
                servicoNotificacao.Salvar(notificacao);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro notificação: {ex.Message} - __________________________________________________________________________");
                SentrySdk.CaptureEvent(new SentryEvent(ex));
                SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"servicoNotificacao.Salvar")));
            }
            try
            {
                foreach (var aula in turmaSemRegistro.Aulas)
                {
                    Console.WriteLine($"Salva notificação frequencia");
                    repositorioNotificacaoFrequencia.Salvar(new NotificacaoFrequencia()
                    {
                        Tipo = tipo,
                        NotificacaoCodigo = notificacao.Codigo,
                        AulaId = aula.Id,
                        DisciplinaCodigo = turmaSemRegistro.DisciplinaId
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro notificação frequencia: {ex.Message} - __________________________________________________________________________");
                SentrySdk.CaptureEvent(new SentryEvent(ex));
                SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"repositorioNotificacaoFrequencia.Salvar")));
            }
        }

        private string ObterNomeDisciplina(string codigoDisciplina)
        {
            long[] disciplinaId = { long.Parse(codigoDisciplina) };
            var disciplina = servicoEOL.ObterDisciplinasPorIds(disciplinaId);

            if (!disciplina.Any())
                throw new NegocioException("Disciplina não encontrada no EOL.");

            return disciplina.FirstOrDefault().Nome;
        }

        private int QuantidadeAulasParaNotificacao(TipoNotificacaoFrequencia tipo)
                            => int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(
                                        tipo == TipoNotificacaoFrequencia.Professor ? TipoParametroSistema.QuantidadeAulasNotificarProfessor
                                            : tipo == TipoNotificacaoFrequencia.GestorUe ? TipoParametroSistema.QuantidadeAulasNotificarGestorUE
                                            : TipoParametroSistema.QuantidadeAulasNotificarSupervisorUE,
                                        DateTime.Now.Year));
    }
}