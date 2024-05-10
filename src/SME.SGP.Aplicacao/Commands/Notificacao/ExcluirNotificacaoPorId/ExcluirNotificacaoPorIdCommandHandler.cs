using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoPorIdCommandHandler : IRequestHandler<ExcluirNotificacaoPorIdCommand, bool>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacaoConsulta;
        private readonly IMediator mediator;

        public ExcluirNotificacaoPorIdCommandHandler(IRepositorioNotificacao repositorioNotificacao,
                                                     IRepositorioNotificacaoConsulta repositorioNotificacaoConsulta,
                                                     IMediator mediator)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioNotificacaoConsulta = repositorioNotificacaoConsulta ?? throw new ArgumentNullException(nameof(repositorioNotificacaoConsulta));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirNotificacaoPorIdCommand request, CancellationToken cancellationToken)
        {
            var notificacoes = await repositorioNotificacaoConsulta.ObterUsuariosNotificacoesPorIds(new[] { request.Id });
            if (notificacoes.EhNulo())
                throw new NegocioException("Não localizado usuário da notificação");

            var notificacao = notificacoes.First();

            repositorioNotificacao.Remover(request.Id);
            await mediator.Send(new NotificarExclusaoNotificacaoCommand(notificacao.Codigo, notificacao.Status, notificacao.UsuarioRf));

            return true;
        }
    }
}
