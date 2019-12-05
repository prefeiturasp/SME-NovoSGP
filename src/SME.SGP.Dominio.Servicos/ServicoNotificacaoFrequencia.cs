using Microsoft.Extensions.Configuration;
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
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoEOL servicoEOL;
        private readonly IConfiguration configuration;

        public ServicoNotificacaoFrequencia(IRepositorioNotificacaoFrequencia repositorioNotificacaoFrequencia,
                                            IRepositorioParametrosSistema repositorioParametrosSistema,
                                            IRepositorioFrequencia repositorioFrequencia,
                                            IRepositorioAula repositorioAula,
                                            IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                            IServicoNotificacao servicoNotificacao,
                                            IServicoUsuario servicoUsuario,
                                            IServicoEOL servicoEOL,
                                            IConfiguration configuration)
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
                            NotificaRegistroFrequencia(usuario, turma, tipo);
                        }
                }
            }
        }

        private IEnumerable<Usuario> BuscaUsuarioNotificacao(RegistroFrequenciaFaltanteDto turma, TipoNotificacaoFrequencia tipo)
        {
            return tipo == TipoNotificacaoFrequencia.Professor ? BuscaProfessorAula(turma)
                        : tipo == TipoNotificacaoFrequencia.GestorUe ? BuscaGestoresUe(turma.CodigoUe)
                        : BuscaSupervisoresUe(turma.CodigoUe);
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

        private IEnumerable<Usuario> BuscaGestoresUe(string codigoUe)
        {
            // Buscar gestor da Ue
            List<UsuarioEolRetornoDto> funcionariosRetornoEol = new List<UsuarioEolRetornoDto>();

            funcionariosRetornoEol.AddRange(servicoEOL.ObterFuncionariosPorCargoUe(codigoUe, (int)Cargo.CP));
            funcionariosRetornoEol.AddRange(servicoEOL.ObterFuncionariosPorCargoUe(codigoUe, (int)Cargo.Diretor));

            if (funcionariosRetornoEol == null)
                return null;

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

        private int QuantidadeAulasParaNotificacao(TipoNotificacaoFrequencia tipo)
            => int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(
                                        tipo == TipoNotificacaoFrequencia.Professor ? TipoParametroSistema.QuantidadeAulasNotificarProfessor
                                            : tipo == TipoNotificacaoFrequencia.GestorUe ? TipoParametroSistema.QuantidadeAulasNotificarGestorUE
                                            : TipoParametroSistema.QuantidadeAulasNotificarSupervisorUE,
                                        DateTime.Now.Year));

        private void NotificaRegistroFrequencia(Usuario usuario, RegistroFrequenciaFaltanteDto turmaSemRegistro, TipoNotificacaoFrequencia tipo)
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

                // Dados da Ue
                var registroFrequencia = repositorioFrequencia.ObterPorId(registroFrequenciaId);
                var aula = repositorioAula.ObterPorId(registroFrequencia.AulaId);
                registroFrequencia.Aula = aula;

                usuariosNotificacao.AddRange(BuscaGestoresUe(aula.UeId));
                usuariosNotificacao.AddRange(BuscaSupervisoresUe(aula.UeId));

                foreach(var usuario in usuariosNotificacao)
                {
                    NotificaAlteracaoFrequencia(usuario, registroFrequencia, alteradoEm, usuarioAlteracaoId);
                }
            }
        }

        private void NotificaAlteracaoFrequencia(Usuario usuario, RegistroFrequencia registroFrequencia, DateTime alteradoEm, long usuarioAlteracaoId)
        {
            // TODO carregar nomes da turma, escola, disciplina e professor para notificacao
            var turmaNome = "";
            var escolaNome = "";
            var disciplina = "";

            var tituloMensagem = $"Título: Alteração extemporânea de frequência  da turma {turmaNome} na disciplina {disciplina}.";
            
            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.Append($"O Professor {usuarioAlteracaoId} realizou alterações no registro de frequência do dia {alteradoEm} da turma {turmaNome} ({escolaNome}) na disciplina {disciplina}.");

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
                //TurmaId = turmaSemRegistro.CodigoTurma,
                //UeId = turmaSemRegistro.CodigoUe,
                //DreId = turmaSemRegistro.CodigoDre,
            };
            servicoNotificacao.Salvar(notificacao);
        }
        }
    }
