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
    public class ObterTurmaPorIdQueryHandler : IRequestHandler<ObterTurmaPorIdQuery, Turma>
    {
        private readonly IMediator mediator;

        public ObterTurmaPorIdQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Turma> Handle(ObterTurmaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await this.mediator.Send(new ObterTurmaComUeEDrePorIdQuery(request.TurmaId));
        }
    }
}
