using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarNotificacaoDiarioBordoCommandHandler : IRequestHandler<AlterarNotificacaoDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao;
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public AlterarNotificacaoDiarioBordoCommandHandler(IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao, 
                                                           IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioDiarioBordoObservacaoNotificacao = repositorioDiarioBordoObservacaoNotificacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacaoNotificacao));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<AuditoriaDto> Handle(AlterarNotificacaoDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var notificacaoObs = await repositorioDiarioBordoObservacaoNotificacao.ObterPorObservacaoUsuarioId(request.ObservacaoId, request.UsuarioId);
            var notificacao = await repositorioNotificacao.ObterPorIdAsync(notificacaoObs.IdNotificacao);

            notificacao.Mensagem = "O usuário foi removido da observação direcionada.";

            await repositorioNotificacao.SalvarAsync(notificacao);

            return (AuditoriaDto)notificacao;
        }
    }
}
