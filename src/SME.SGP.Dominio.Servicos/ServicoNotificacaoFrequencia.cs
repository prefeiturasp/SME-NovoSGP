using Microsoft.Extensions.Configuration;
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
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IConfiguration configuration;

        public ServicoNotificacaoFrequencia(IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia,
                                            IRepositorioParametrosSistema repositorioParametrosSistema,
                                            IRepositorioFrequencia repositorioFrequencia,
                                            IRepositorioAula repositorioAula,
                                            IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                            IServicoNotificacao servicoNotificacao,
                                            IServicoUsuario servicoUsuario,
                                            IConfiguration configuration)
        {
            this.repositorioNotificacaoFrequencia = repositorioNotificacaoFrequencia ?? throw new ArgumentNullException(nameof(repositorioNotificacaoFrequencia));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ExecutaNotificacaoFrequencia()
        {
            NotificarAusenciaFrequencia(TipoNotificacaoFrequencia.Professor);
            NotificarAusenciaFrequencia(TipoNotificacaoFrequencia.GestorUe);
            NotificarAusenciaFrequencia(TipoNotificacaoFrequencia.SupervisorUe);
        }

        private void NotificarAusenciaFrequencia(TipoNotificacaoFrequencia tipo)
        {
            // Busca registro de aula sem frequencia e sem notificação do tipo
            var turmasSemRegistro = repositorioNotificacaoFrequencia.ObterTurmasSemRegistroDeFrequencia(tipo);

            // Busca parametro do sistema de quantidade de aulas sem frequencia para notificação
            var qtdAulasNotificacao = QuantidadeAulasParaNotificacao(tipo);

            foreach (var turma in turmasSemRegistro)
            {
                // Carrega todas as aulas sem registro de frequencia da turma e disciplina para notificação
                turma.Aulas = repositorioFrequencia.ObterAulasSemRegistroFrequencia(turma.CodigoTurma, turma.DisciplinaId);

                if (turma.Aulas.Count() >= qtdAulasNotificacao)
                {
                    // Busca Professor/Gestor/Supervisor da Turma ou Ue
                    var usuarios = BuscaUsuarioNotificacao(turma, tipo);

                    if (usuarios != null)
                        foreach(var usuario in usuarios)
                        {
                            NotificaUsuario(usuario, turma, tipo);
                        }
                }
            }
        }

        private IEnumerable<Usuario> BuscaUsuarioNotificacao(RegistroFrequenciaFaltanteDto turma, TipoNotificacaoFrequencia tipo)
        {
            return tipo == TipoNotificacaoFrequencia.Professor ? BuscaProfessorAula(turma)
                        : tipo == TipoNotificacaoFrequencia.GestorUe ? BuscaGestorUe(turma.CodigoUe)
                        : BuscaSupervisorUe(turma.CodigoUe);
        }

        private IEnumerable<Usuario> BuscaSupervisorUe(string codigoUe)
        {
            // Buscar supervisor da Ue
            var supervisoresEscola = repositorioSupervisorEscolaDre.ObtemSupervisoresPorUe(codigoUe);
            if (supervisoresEscola == null || supervisoresEscola.Count() == 0)
                return null;

            var usuarios = new List<Usuario>();
            foreach (var supervisorEscola in supervisoresEscola)
            {
                usuarios.Add(servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(supervisorEscola.SupervisorId));
            }

            return usuarios;
        }

        private IEnumerable<Usuario> BuscaGestorUe(string codigoUe)
        {
            // TODO Buscar gestor da Ue

            return null;
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

        private int QuantidadeAulasParaNotificacao(TipoNotificacaoFrequencia tipo)
            => int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(
                                        tipo == TipoNotificacaoFrequencia.Professor ? TipoParametroSistema.QuantidadeAulasNotificarProfessor
                                            : tipo == TipoNotificacaoFrequencia.GestorUe ? TipoParametroSistema.QuantidadeAulasNotificarGestorUE
                                            : TipoParametroSistema.QuantidadeAulasNotificarSupervisorUE,
                                        DateTime.Now.Year));

        private void NotificaUsuario(Usuario usuario, RegistroFrequenciaFaltanteDto turmaSemRegistro, TipoNotificacaoFrequencia tipo)
        {
            var tituloMensagem = $"Frequência da turma {turmaSemRegistro.NomeTurma} - {turmaSemRegistro.DisciplinaId} ({turmaSemRegistro.NomeUe})";
            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.Append($"A turma a seguir esta a <b>{turmaSemRegistro.Aulas.Count()} aulas</b> sem registro de frequência da turma");
            mensagemUsuario.Append("<br />");
            mensagemUsuario.Append($"<br />Escola: <b>{turmaSemRegistro.NomeUe}</b>");
            mensagemUsuario.Append($"<br />Turma: <b>{turmaSemRegistro.NomeTurma}</b>");
            mensagemUsuario.Append($"<br />Disciplina: <b>{turmaSemRegistro.DisciplinaId}</b>");
            mensagemUsuario.Append($"<br />Aulas:");

            mensagemUsuario.Append("<ul>");
            foreach (var aula in turmaSemRegistro.Aulas)
            {
                mensagemUsuario.Append($"<li>Data: {aula.DataAula}</li>");
            }
            mensagemUsuario.Append("</ul>");

            var hostAplicacao = configuration["UrlFrontEnd"];
            mensagemUsuario.Append($"<a href='{hostAplicacao}/diario-classe/frequencia-plano-aula'>Clique aqui para regularizar.</a>");

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
    }
}
