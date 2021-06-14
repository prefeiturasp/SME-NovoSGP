using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio.Interfaces.Repositorios;

namespace SME.SGP.Aplicacao
{
    public class NotificarObservacaoPlanoAEECommandHandler : IRequestHandler<NotificarObservacaoPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNotificacaoPlanoAEEObservacao repositorioNotificacaoPlanoAEEObservacao;
        private readonly IConfiguration configuration;

        public NotificarObservacaoPlanoAEECommandHandler(IMediator mediator, IRepositorioNotificacaoPlanoAEEObservacao repositorioNotificacaoPlanoAEEObservacao, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioNotificacaoPlanoAEEObservacao = repositorioNotificacaoPlanoAEEObservacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacaoPlanoAEEObservacao));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(NotificarObservacaoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var hostAplicacao = configuration["UrlFrontEnd"];

            var titulo = $"Nova observação no Plano AEE - {request.CriadorNome} ({request.CriadorRF}) - {request.UeNome} ({request.DreAbreviacao})";
            var mensagem = $@"O usuário {request.CriadorNome} ({request.CriadorRF}) inseriu uma nova observação no Plano AEE do estudante {request.AlunoNome} ({request.AlunoCodigo}) da {request.UeNome} ({request.DreAbreviacao}).<br/>
                Observação: {request.Observacao} <br/> {GerarBotao(hostAplicacao, request.PlanoAEEId)}";

            var notificacoes = await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo, mensagem, Dominio.NotificacaoCategoria.Alerta, Dominio.NotificacaoTipo.AEE, request.Usuarios));
            foreach(var notificacaoId in notificacoes)
                await repositorioNotificacaoPlanoAEEObservacao.SalvarAsync(new Dominio.NotificacaoPlanoAEEObservacao(notificacaoId, request.PlanoAEEObservacaoId));

            return true;
        }

        private string GerarBotao(string hostAplicacao, long planoAEEId)
            => $@"<a class='btn btn-primary active' href='{hostAplicacao}aee/plano/editar/{planoAEEId}' role='button'>
                <i class='sc-jvjKQy iRIIgg fa fa-eye mr-2 py-1'></i>
                Consultar observações</a>";
    }
}
