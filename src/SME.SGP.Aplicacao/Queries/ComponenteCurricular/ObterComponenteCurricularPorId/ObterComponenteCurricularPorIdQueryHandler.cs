using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteCurricularPorIdQueryHandler : IRequestHandler<ObterComponenteCurricularPorIdQuery, DisciplinaDto>
    {
        private readonly IMediator mediator;

        public ObterComponenteCurricularPorIdQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<DisciplinaDto> Handle(ObterComponenteCurricularPorIdQuery request, CancellationToken cancellationToken)
            => (await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { request.ComponenteCurricularId }))).FirstOrDefault();
    }
}
