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
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia;

        public ServicoNotificacaoFrequencia(IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia,
                                            IRepositorioParametrosSistema repositorioParametrosSistema,
                                            IRepositorioAula repositorioAula,
                                            IRepositorioFrequencia repositorioFrequencia,
                                            IServicoNotificacao servicoNotificacao)
        {
            this.repositorioNotificacaoFrequencia = repositorioNotificacaoFrequencia ?? throw new ArgumentNullException(nameof(repositorioNotificacaoFrequencia));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }

        public void ExecutaNotificacaoFrequencia()
        {
            var turmasSemRegistro = repositorioFrequencia.ObterTurmasSemRegistroDeFrequencia();

            NotificarAusenciaFrequencia(turmasSemRegistro, TipoNotificacaoFrequencia.Professor);
            NotificarAusenciaFrequencia(turmasSemRegistro, TipoNotificacaoFrequencia.GestorUe);
            NotificarAusenciaFrequencia(turmasSemRegistro, TipoNotificacaoFrequencia.SupervisorUe);
        }

        private void NotificarAusenciaFrequencia(IEnumerable<RegistroFrequenciaFaltanteDto> turmasSemRegistro, TipoNotificacaoFrequencia tipo)
        {
            // Busca parametro do sistema de quantidade de aulas sem frequencia para notificação
            var qtdAulasNotificacao = QuantidadeAulasParaNotificacao(tipo);

            // Filtra turmas que atendem a regra de numero de aulas sem registro de frequencia
            var turmasNotificacao = turmasSemRegistro.Where(c => c.QuantidadeAulasSemFrequencia >= qtdAulasNotificacao);

            foreach(var turmaSemRegistro in turmasNotificacao)
            {
                // Busca Professor/Gestor/Supervisor da Turma ou Ue
                var usuarioId = tipo == TipoNotificacaoFrequencia.Professor ? BuscaProfessorAula(turmaSemRegistro.CodigoTurma, turmaSemRegistro.DisciplinaId) 
                                : tipo == TipoNotificacaoFrequencia.GestorUe ? BuscaGestorUe(turmaSemRegistro.CodigoUe)
                                : BuscaSupervisorUe(turmaSemRegistro.CodigoUe);


                if (!repositorioNotificacaoFrequencia.UsuarioNotificado(usuarioId, tipo))
                {
                    NotificaUsuario(usuarioId, turmaSemRegistro, tipo);
                }
            }
        }

        private long BuscaSupervisorUe(string codigoUe)
        {
            // TODO Buscar supervisor da Ue
            return 0;
        }

        private long BuscaGestorUe(string codigoUe)
        {
            // TODO Buscar gestor da Ue
            return 0;
        }

        private long BuscaProfessorAula(string codigoTurma, string disciplinaId)
        {
            // TODO Buscar professor por turma e disciplina
            return 0;
        }

        private int QuantidadeAulasParaNotificacao(TipoNotificacaoFrequencia tipo)
            => int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(
                                        tipo == TipoNotificacaoFrequencia.Professor ? TipoParametroSistema.QuantidadeAulasNotificarProfessor
                                            : tipo == TipoNotificacaoFrequencia.GestorUe ? TipoParametroSistema.QuantidadeAulasNotificarGestorUE
                                            : TipoParametroSistema.QuantidadeAulasNotificarSupervisorUE,
                                        DateTime.Now.Year));

        private void NotificaUsuario(long usuarioId, RegistroFrequenciaFaltanteDto turmaSemRegistro, TipoNotificacaoFrequencia tipo)
        {
            var tituloMensagem = "Notificação de Frequência não regitrada";
            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.Append($"A turma a seguir esta a <b>{turmaSemRegistro.QuantidadeAulasSemFrequencia} aulas</b> sem registro de frequência");

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
