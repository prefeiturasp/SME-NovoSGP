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
        private readonly IConfiguration configuration;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoNotificacaoFrequencia(IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia,
                                            IRepositorioParametrosSistema repositorioParametrosSistema,
                                            IRepositorioFrequencia repositorioFrequencia,
                                            IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                            IServicoNotificacao servicoNotificacao,
                                            IServicoUsuario servicoUsuario,
                                            IServicoEOL servicoEOL,
                                            IConfiguration configuration)
        {
            this.repositorioNotificacaoFrequencia = repositorioNotificacaoFrequencia ?? throw new ArgumentNullException(nameof(repositorioNotificacaoFrequencia));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ExecutaNotificacaoFrequencia()
        {
            var cargosNotificados = new List<(string, Cargo?)>();

            Console.WriteLine($"Notificando usuários de aulas sem frequência.");

            NotificarAusenciaFrequencia(TipoNotificacaoFrequencia.Professor, ref cargosNotificados);
            NotificarAusenciaFrequencia(TipoNotificacaoFrequencia.SupervisorUe, ref cargosNotificados);
            NotificarAusenciaFrequencia(TipoNotificacaoFrequencia.GestorUe, ref cargosNotificados);

            Console.WriteLine($"Rotina finalizada.");
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
                var usuariosNotificacao = new List<(Cargo?, Usuario)>();

                // Dados da Aula
                var registroFrequencia = repositorioFrequencia.ObterAulaDaFrequencia(registroFrequenciaId);
                MeusDadosDto professor = servicoEOL.ObterMeusDados(registroFrequencia.ProfessorRf).Result;

                // Gestores
                var usuarios = BuscaGestoresUe(registroFrequencia.CodigoUe);
                if (usuarios != null)
                    usuariosNotificacao.AddRange(usuarios);

                // Supervisores
                usuarios = BuscaSupervisoresUe(registroFrequencia.CodigoUe, usuariosNotificacao.Select(u => u.Item1));
                if (usuarios != null)
                    usuariosNotificacao.AddRange(usuarios);

                foreach (var usuario in usuariosNotificacao)
                {
                    NotificaAlteracaoFrequencia(usuario.Item2, registroFrequencia, professor.Nome);
                }
            }
        }

        private IEnumerable<(Cargo? Cargo, Usuario Usuario)> BuscaGestoresUe(string codigoUe)
        {
            // Buscar gestor da Ue
            var funcionariosRetornoEol = servicoNotificacao.ObterFuncionariosPorNivel(codigoUe, Cargo.CP);

            if (funcionariosRetornoEol == null)
                return null;

            var cargoNotificacao = funcionariosRetornoEol.GroupBy(f => f.Cargo).Select(f => f.Key).First();
            Cargo? proximoNivel = null;

            if (cargoNotificacao == Cargo.CP || cargoNotificacao == Cargo.AD)
                proximoNivel = Cargo.Diretor;
            else
                proximoNivel = servicoNotificacao.ObterProximoNivel(cargoNotificacao, false);

            if (proximoNivel != null)
                funcionariosRetornoEol = funcionariosRetornoEol.Concat(servicoNotificacao.ObterFuncionariosPorNivel(codigoUe, proximoNivel));
            else
                return null;

            var usuarios = new List<(Cargo?, Usuario)>();
            foreach (var usuarioEol in funcionariosRetornoEol)
                usuarios.Add((usuarioEol.Cargo, servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuarioEol.Id)));

            return usuarios;
        }

        private IEnumerable<(Cargo?, Usuario)> BuscaProfessorAula(RegistroFrequenciaFaltanteDto turma)
        {
            // Buscar professor da ultima aula
            var professorRf = turma.Aulas
                    .OrderBy(o => o.DataAula)
                    .Last().ProfessorId;
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(professorRf.ToString());

            return usuario != null ? new List<(Cargo?, Usuario)>()
            {
                (null, usuario)
            } : null;
        }

        private IEnumerable<(Cargo?, Usuario)> BuscaSupervisoresUe(string codigoUe, IEnumerable<Cargo?> cargosNotificados)
        {
            var funcionariosRetorno = servicoNotificacao.ObterFuncionariosPorNivel(codigoUe, Cargo.Supervisor);

            if (funcionariosRetorno == null || cargosNotificados.Any(c => funcionariosRetorno.Any(f => f.Cargo == c)))
                return null;

            var usuarios = new List<(Cargo?, Usuario)>();
            foreach (var funcionario in funcionariosRetorno)
                usuarios.Add((funcionario.Cargo, servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(funcionario.Id)));

            return usuarios;
        }

        private IEnumerable<(Cargo?, Usuario)> BuscaUsuarioNotificacao(RegistroFrequenciaFaltanteDto turma, TipoNotificacaoFrequencia tipo)
        {
            IEnumerable<(Cargo?, Usuario)> usuarios = Enumerable.Empty<(Cargo?, Usuario)>();
            switch (tipo)
            {
                case TipoNotificacaoFrequencia.Professor:
                    usuarios = BuscaProfessorAula(turma);
                    break;

                case TipoNotificacaoFrequencia.GestorUe:
                    usuarios = BuscaGestoresUe(turma.CodigoUe);
                    break;

                case TipoNotificacaoFrequencia.SupervisorUe:
                    usuarios = BuscaSupervisoresUe(turma.CodigoUe, usuarios.Select(u => u.Item1));
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

        private void NotificarAusenciaFrequencia(TipoNotificacaoFrequencia tipo, ref List<(string, Cargo?)> cargosNotificados)
        {
            // Busca registro de aula sem frequencia e sem notificação do tipo
            IEnumerable<RegistroFrequenciaFaltanteDto> turmasSemRegistro = null;
            turmasSemRegistro = repositorioNotificacaoFrequencia.ObterTurmasSemRegistroDeFrequencia(tipo);

            if (turmasSemRegistro != null)
            {
                // Busca parametro do sistema de quantidade de aulas sem frequencia para notificação
                var qtdAulasNotificacao = QuantidadeAulasParaNotificacao(tipo);
                foreach (var turma in turmasSemRegistro)
                {
                    // Carrega todas as aulas sem registro de frequencia da turma e disciplina para notificação
                    turma.Aulas = repositorioFrequencia.ObterAulasSemRegistroFrequencia(turma.CodigoTurma, turma.DisciplinaId, tipo);
                    if (turma.Aulas != null && turma.Aulas.Count() >= qtdAulasNotificacao)
                    {
                        // Busca Professor/Gestor/Supervisor da Turma ou Ue
                        var usuarios = BuscaUsuarioNotificacao(turma, tipo);

                        if (usuarios != null)
                        {
                            var cargosLinq = cargosNotificados;
                            var cargosNaoNotificados = usuarios.Select(u => u.Item1)
                                                        .GroupBy(u => u)
                                                        .Where(w => !cargosLinq.Any(l => l.Item1 == turma.CodigoTurma && l.Item2 == w.Key))
                                                        .Select(s => new { turma.CodigoTurma, s.Key });

                            foreach (var usuario in usuarios.Where(u => cargosNaoNotificados.Select(c => c.Key).Contains(u.Item1)))
                            {
                                NotificaRegistroFrequencia(usuario.Item2, turma, tipo);
                            }

                            cargosNotificados.AddRange(cargosNaoNotificados.Select(n => (n.CodigoTurma, n.Key)));
                        }
                    }
                    else
                        Console.WriteLine($"Notificação não necessária pois quantidade de aulas sem frequência: {turma.Aulas?.Count() ?? 0 } está dentro do limite: {qtdAulasNotificacao}.");
                }
            }
        }

        private void NotificaRegistroFrequencia(Usuario usuario, RegistroFrequenciaFaltanteDto turmaSemRegistro, TipoNotificacaoFrequencia tipo)
        {
            var disciplinas = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(turmaSemRegistro.DisciplinaId) });
            if (disciplinas != null && disciplinas.Any())
            {
                var disciplina = disciplinas.FirstOrDefault();

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
                servicoNotificacao.Salvar(notificacao);
                foreach (var aula in turmaSemRegistro.Aulas)
                {
                    repositorioNotificacaoFrequencia.Salvar(new NotificacaoFrequencia()
                    {
                        Tipo = tipo,
                        NotificacaoCodigo = notificacao.Codigo,
                        AulaId = aula.Id,
                        DisciplinaCodigo = turmaSemRegistro.DisciplinaId
                    });
                }
            }
            else
            {
                Console.WriteLine("Não foi possível obter a disciplina pois o EOL não respondeu");
                SentrySdk.CaptureEvent(new SentryEvent(new NegocioException("Não foi possível obter a disciplina pois o EOL não respondeu")));
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