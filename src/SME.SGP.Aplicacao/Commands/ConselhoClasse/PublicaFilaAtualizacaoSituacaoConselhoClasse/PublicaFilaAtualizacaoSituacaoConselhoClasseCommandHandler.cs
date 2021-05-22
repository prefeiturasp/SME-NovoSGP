using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaAtualizacaoSituacaoConselhoClasseCommandHandler : IRequestHandler<PublicaFilaAtualizacaoSituacaoConselhoClasseCommand, bool>
    {
        private readonly IMediator mediator;

        public PublicaFilaAtualizacaoSituacaoConselhoClasseCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PublicaFilaAtualizacaoSituacaoConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            return await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaAtualizacaoSituacaoConselhoClasse,
                                                       new AtualizaSituacaoConselhoClasseCommand(request.ConselhoClasseId),
                                                       Guid.NewGuid(),
                                                       request.Usuario));
        }
    }
}
