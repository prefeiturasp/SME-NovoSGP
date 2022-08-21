using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoCommandHandler : AsyncRequestHandler<ExcluirNotificacaoCommand>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacaoconsulta;

        public ExcluirNotificacaoCommandHandler(IMediator mediator,
                                                IRepositorioNotificacao repositorioNotificacao,
                                                IRepositorioNotificacaoConsulta repositorioNotificacaoconsulta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioNotificacaoconsulta = repositorioNotificacaoconsulta ?? throw new ArgumentNullException(nameof(repositorioNotificacaoconsulta));
        }

        protected override async Task Handle(ExcluirNotificacaoCommand request, CancellationToken cancellationToken)
        {
            var usuarioRf = await repositorioNotificacaoconsulta.ObterUsuarioNotificacaoPorId(request.Notificacao.Id);

            await repositorioNotificacao.RemoverAsync(request.Notificacao);
            await mediator.Send(new NotificarExclusaoNotificacaoCommand(request.Notificacao.Codigo, usuarioRf));
        }
    }
}
