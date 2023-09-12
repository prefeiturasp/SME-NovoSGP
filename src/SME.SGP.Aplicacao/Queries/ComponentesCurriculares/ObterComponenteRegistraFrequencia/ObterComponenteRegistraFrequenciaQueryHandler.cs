using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteRegistraFrequenciaQueryHandler : IRequestHandler<ObterComponenteRegistraFrequenciaQuery, bool>
    {
        private readonly IMediator mediator;

        public ObterComponenteRegistraFrequenciaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ObterComponenteRegistraFrequenciaQuery request, CancellationToken cancellationToken)
        {
            var componenteCurricular = await mediator.Send(new ObterComponenteCurricularPorIdQuery(request.ComponenteCurricularId));
            return !(componenteCurricular is null) && componenteCurricular.RegistraFrequencia;
        }
    }
}
