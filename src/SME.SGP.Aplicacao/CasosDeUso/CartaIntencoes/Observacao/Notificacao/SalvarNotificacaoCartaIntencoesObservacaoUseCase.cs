using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotificacaoCartaIntencoesObservacaoUseCase : ISalvarNotificacaoCartaIntencoesObservacaoUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNotificacaoCartaIntencoesObservacao repositorioNotificacaoCartaIntencoesObservacao;
        private readonly IServicoNotificacao servicoNotificacao;

        public SalvarNotificacaoCartaIntencoesObservacaoUseCase(IMediator mediator, IServicoNotificacao servicoNotificacao,
            IRepositorioNotificacaoCartaIntencoesObservacao repositorioNotificacaoCartaIntencoesObservacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioNotificacaoCartaIntencoesObservacao = repositorioNotificacaoCartaIntencoesObservacao ?? throw new ArgumentNullException(nameof(repositorioNotificacaoCartaIntencoesObservacao));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<SalvarNotificacaoCartaIntencoesObservacaoDto>();

            var turma = dadosMensagem.Turma;
            var usuarioLogado = dadosMensagem.Usuario;
            var cartaObservacaoId = dadosMensagem.CartaIntencoesObservacaoId;

            var titulares = await ObterRfsTitulares(turma.CodigoTurma);
            if (titulares.NaoEhNulo())
            {
                var mensagem = ConstruirMensagem(dadosMensagem, turma);

                var listaRfs = ObterListaRfs(titulares);
                await NotificarTitulares(listaRfs, usuarioLogado.CodigoRf, turma, mensagem, cartaObservacaoId);

                return true;
            }

            return false;
        }

        private async Task<IEnumerable<string>> ObterRfsTitulares(string codigoTurma)
        {
            var professores = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(codigoTurma));
            return professores?.Select(x => x.ProfessorRf);
        }

        private static string ConstruirMensagem(SalvarNotificacaoCartaIntencoesObservacaoDto dados, Turma turma)
        {
            var mensagem = new StringBuilder(
                $"O usuário {dados.Usuario.Nome} ({dados.Usuario.CodigoRf}) inseriu uma nova observação na Carta de intenções da turma " +
                $"<strong>{turma.Nome}</strong> da <strong>{turma.Ue.TipoEscola}-{turma.Ue.Nome}</strong> ({turma.Ue.Dre.Abreviacao}).");

            if (dados.Observacao.Trim().Length > 200)
                mensagem.AppendLine("<br/><br/>Observação: Acesse Carta de intenções para consultar o conteúdo da observação.");
            else
                mensagem.AppendLine($"<br/><br/>Observação: {dados.Observacao.TrimEnd('.').Trim()}.");

            return mensagem.ToString();
        }

        private static IEnumerable<string> ObterListaRfs(IEnumerable<string> titulares)
        {
            if (titulares.Count() == 1)
                return titulares.First().Split(',').Select(rf => rf.Trim());

            return titulares.Select(rf => rf.Trim());
        }

        private async Task NotificarTitulares(IEnumerable<string> rfs, string rfUsuarioLogado, Turma turma, string mensagem, long cartaObservacaoId)
        {
            foreach (var rf in rfs)
            {
                if (rf == rfUsuarioLogado || string.IsNullOrWhiteSpace(rf))
                    continue;

                var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(rf));
                if (usuario.NaoEhNulo())
                {
                    var notificacao = new Notificacao()
                    {
                        Ano = DateTime.Now.Year,
                        Categoria = NotificacaoCategoria.Aviso,
                        Tipo = NotificacaoTipo.Planejamento,
                        Titulo = $"Nova observação na Carta de intenções da turma {turma.Nome}",
                        Mensagem = mensagem,
                        UsuarioId = usuario.Id,
                        TurmaId = "",
                        UeId = "",
                        DreId = "",
                    };

                    await servicoNotificacao.Salvar(notificacao);

                    var notificacaoObservacao = new NotificacaoCartaIntencoesObservacao()
                    {
                        NotificacaoId = notificacao.Id,
                        CartaIntencoesObservacaoId = cartaObservacaoId
                    };

                    await repositorioNotificacaoCartaIntencoesObservacao.Salvar(notificacaoObservacao);
                }
            }
        }

    }
}
