using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarPlanoAEERespostaPeriodoEscolarCommandHandler : IRequestHandler<AtualizarPlanoAEERespostaPeriodoEscolarCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPlanoAEEResposta repositorioPlanoAEEResposta;

        public AtualizarPlanoAEERespostaPeriodoEscolarCommandHandler(IMediator mediator, IRepositorioPlanoAEEResposta repositorioPlanoAEEResposta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAEEResposta = repositorioPlanoAEEResposta ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEResposta));
        }

        public async Task<bool> Handle(AtualizarPlanoAEERespostaPeriodoEscolarCommand request, CancellationToken cancellationToken)
        {
            await repositorioPlanoAEEResposta.Atualizar(request.RespostaPeriodoEscolar, request.RespostaId);
            return true;
        }
    }
}
