using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEPorTurmaIdQueryHandler : IRequestHandler<ObterUEPorTurmaIdQuery, Ue>
    {
        private readonly IMediator mediator;

        public ObterUEPorTurmaIdQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Ue> Handle(ObterUEPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            var turma = await this.mediator.Send(new ObterTurmaComUeEDrePorIdQuery(request.TurmaId));

            return turma?.Ue;
        }
    }
}
