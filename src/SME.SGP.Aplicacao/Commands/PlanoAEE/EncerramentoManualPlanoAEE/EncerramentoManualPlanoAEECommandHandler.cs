using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    internal class EncerramentoManualPlanoAEECommandHandler : IRequestHandler<EncerramentoManualPlanoAEECommand, RetornoEncerramentoPlanoAEEDto>
    {

        private readonly IRepositorioPlanoAEE repositorio;
        private readonly IMediator mediator;

        public EncerramentoManualPlanoAEECommandHandler(
            IRepositorioPlanoAEE repositorio,
            IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RetornoEncerramentoPlanoAEEDto> Handle(EncerramentoManualPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(request.PlanoId));

            planoAEE.EncerramentoManualPlanoAEE();

            var planoId = await repositorio.SalvarAsync(planoAEE);

            return new RetornoEncerramentoPlanoAEEDto(planoId, planoAEE.Situacao);
        }


    }
}
