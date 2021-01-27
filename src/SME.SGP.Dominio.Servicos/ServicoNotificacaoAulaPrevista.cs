using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoNotificacaoAulaPrevista : IServicoNotificacaoAulaPrevista
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioNotificacaoAulaPrevista repositorioNotificacaoAulaPrevista;
        private readonly IRepositorioAulaPrevista repositorioAulaPrevista;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;

        public ServicoNotificacaoAulaPrevista(IRepositorioParametrosSistema repositorioParametrosSistema,
                                            IRepositorioNotificacaoAulaPrevista repositorioNotificacaoAulaPrevista,
                                            IRepositorioAulaPrevista repositorioAulaPrevista,
                                            IServicoNotificacao servicoNotificacao,
                                            IServicoUsuario servicoUsuario,
                                            IConfiguration configuration,
                                            IMediator mediator)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioNotificacaoAulaPrevista = repositorioNotificacaoAulaPrevista ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAulaPrevista));
            this.repositorioAulaPrevista = repositorioAulaPrevista ?? throw new ArgumentNullException(nameof(repositorioAulaPrevista));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task ExecutaNotificacaoAulaPrevista()
        {            
            var qtdDiasBimestreNotificacao = await QuantidadeDiasFimBimestreParaNotificacao();

            // Busca registro de aula sem frequencia e sem notificação do tipo
            var turmasAulasPrevistasDivergentes = repositorioNotificacaoAulaPrevista.ObterTurmasAulasPrevistasDivergentes(qtdDiasBimestreNotificacao);

            foreach (var turma in turmasAulasPrevistasDivergentes)
            {
                // Busca Professor/Gestor/Supervisor da Turma ou Ue
                var usuarios = BuscaProfessorAula(turma);

                if (usuarios != null)
                    foreach (var usuario in usuarios)
                    {
                        if (!repositorioNotificacaoAulaPrevista.UsuarioNotificado(usuario.Id, turma.Bimestre, turma.CodigoTurma, turma.DisciplinaId))
                            NotificaRegistroDivergencia(usuario, turma);
                    }


            }
        }
        
        private async Task<int> QuantidadeDiasFimBimestreParaNotificacao()
            => int.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeDiasNotificarProfessor, DateTime.Today.Year)));

        private IEnumerable<Usuario> BuscaProfessorAula(RegistroAulaPrevistaDivergenteDto turma)
        {
            // Buscar professor da ultima aula
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(turma.ProfessorRf);

            return usuario != null ? new List<Usuario>()
            {
                usuario
            } : null;
        }

        private void NotificaRegistroDivergencia(Usuario usuario, RegistroAulaPrevistaDivergenteDto registroAulaPrevistaDivergente)
        {
            var tituloMensagem = $"Diferença entre aulas previstas e dadas - Turma {registroAulaPrevistaDivergente.NomeTurma}";
            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.Append($"O total de aulas previstas não corresponde ao total de aulas dadas no {registroAulaPrevistaDivergente.Bimestre}º bimestre ");
            mensagemUsuario.Append($"para a turma {registroAulaPrevistaDivergente.NomeTurma} da escola {registroAulaPrevistaDivergente.NomeUe} ({registroAulaPrevistaDivergente.NomeDre})");

            var hostAplicacao = configuration["UrlFrontEnd"];
            mensagemUsuario.Append($"<a href='{hostAplicacao}diario-classe/aula-dada-aula-prevista'>Clique aqui para visualizar os detalhes.</a>");

            var notificacao = new Notificacao()
            {
                Ano = DateTime.Now.Year,
                Categoria = NotificacaoCategoria.Alerta,
                Tipo = NotificacaoTipo.Calendario,
                Titulo = tituloMensagem,
                Mensagem = mensagemUsuario.ToString(),
                UsuarioId = usuario.Id,
                TurmaId = registroAulaPrevistaDivergente.CodigoTurma,
                UeId = registroAulaPrevistaDivergente.CodigoUe,
                DreId = registroAulaPrevistaDivergente.CodigoDre,
            };
            servicoNotificacao.Salvar(notificacao);

            repositorioNotificacaoAulaPrevista.Salvar(new NotificacaoAulaPrevista()
            {
                Bimestre = registroAulaPrevistaDivergente.Bimestre,
                NotificacaoCodigo = notificacao.Codigo,
                TurmaId = registroAulaPrevistaDivergente.CodigoTurma,
                DisciplinaId = registroAulaPrevistaDivergente.DisciplinaId,
            });
        }
    }
}
