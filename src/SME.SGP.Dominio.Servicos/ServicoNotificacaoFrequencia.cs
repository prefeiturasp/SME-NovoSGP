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
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IConfiguration configuration;

        public ServicoNotificacaoFrequencia(IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia,
                                            IRepositorioParametrosSistema repositorioParametrosSistema,
                                            IRepositorioFrequencia repositorioFrequencia,
                                            IRepositorioAula repositorioAula,
                                            IServicoNotificacao servicoNotificacao,
                                            IConfiguration configuration)
        {
            this.repositorioNotificacaoFrequencia = repositorioNotificacaoFrequencia ?? throw new ArgumentNullException(nameof(repositorioNotificacaoFrequencia));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
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
                turma.Aulas = repositorioFrequencia.ObterAulasSemRegistroFrequencia(turma.CodigoTurma, turma.DisciplinaId);

                if (turma.Aulas.Count() >= qtdAulasNotificacao)
                {
                    // Busca Professor/Gestor/Supervisor da Turma ou Ue
                    var usuarios = BuscaUsuarioNotificacao(turma, tipo);

                    foreach(var usuarioId in usuarios)
                    {
                        NotificaUsuario(usuarioId, turma, tipo);
                    }
                }
            }

        }

        private IEnumerable<long> BuscaUsuarioNotificacao(RegistroFrequenciaFaltanteDto turma, TipoNotificacaoFrequencia tipo)
        {
            return tipo == TipoNotificacaoFrequencia.Professor ? BuscaProfessorAula(turma.CodigoTurma, turma.DisciplinaId)
                        : tipo == TipoNotificacaoFrequencia.GestorUe ? BuscaGestorUe(turma.CodigoUe)
                        : BuscaSupervisorUe(turma.CodigoUe);
        }

        private IEnumerable<long> BuscaSupervisorUe(string codigoUe)
        {
            // TODO Buscar supervisor da Ue
            return null;
        }

        private IEnumerable<long> BuscaGestorUe(string codigoUe)
        {
            // TODO Buscar gestor da Ue
            return null;
        }

        private IEnumerable<long> BuscaProfessorAula(string codigoTurma, string disciplinaId)
        {
            // TODO Buscar professor por turma e disciplina
            return null;
        }

        private int QuantidadeAulasParaNotificacao(TipoNotificacaoFrequencia tipo)
            => int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(
                                        tipo == TipoNotificacaoFrequencia.Professor ? TipoParametroSistema.QuantidadeAulasNotificarProfessor
                                            : tipo == TipoNotificacaoFrequencia.GestorUe ? TipoParametroSistema.QuantidadeAulasNotificarGestorUE
                                            : TipoParametroSistema.QuantidadeAulasNotificarSupervisorUE,
                                        DateTime.Now.Year));

        private void NotificaUsuario(long usuarioId, RegistroFrequenciaFaltanteDto turmaSemRegistro, TipoNotificacaoFrequencia tipo)
        {
            var tituloMensagem = $"Frequência da turma {turmaSemRegistro.NomeTurma} - {turmaSemRegistro.DisciplinaId} ({turmaSemRegistro.NomeUe})";
            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.Append($"A turma a seguir esta a <b>{turmaSemRegistro.Aulas.Count} aulas</b> sem registro de frequência da turma");
            mensagemUsuario.Append("<br />");
            mensagemUsuario.Append($"<br />Escola: <b>{turmaSemRegistro.NomeUe}</b>");
            mensagemUsuario.Append($"<br />Turma: <b>{turmaSemRegistro.NomeTurma}</b>");
            mensagemUsuario.Append($"<br />Disciplina: <b>{turmaSemRegistro.DisciplinaId}</b>");
            mensagemUsuario.Append($"<br />Aulas:");

            foreach(var aula in turmaSemRegistro.Aulas)
            {
                mensagemUsuario.Append($"<br />   Datas da aula: {aula.DataAula}");
            }
            var hostAplicacao = configuration["UrlFrontEnd"];
            mensagemUsuario.Append($"<a href='{hostAplicacao}/diario-classe/frequencia-plano-aula'>Clique aqui para regularizar.</a>");

            var notificacao = new Notificacao()
            {
                Ano = DateTime.Now.Year,
                Categoria = NotificacaoCategoria.Alerta,
                Tipo = NotificacaoTipo.Frequencia,
                Titulo = tituloMensagem,
                Mensagem = mensagemUsuario.ToString(),
                UsuarioId = usuarioId,
                TurmaId = turmaSemRegistro.CodigoTurma,
                UeId = turmaSemRegistro.CodigoUe,
                DreId = turmaSemRegistro.CodigoDre,
            };
            servicoNotificacao.Salvar(notificacao);

            repositorioNotificacaoFrequencia.Salvar(new NotificacaoFrequencia()
            {
                NotificacaoCodigo = notificacao.Codigo,
                Tipo = tipo
            });
        }
    }
}
