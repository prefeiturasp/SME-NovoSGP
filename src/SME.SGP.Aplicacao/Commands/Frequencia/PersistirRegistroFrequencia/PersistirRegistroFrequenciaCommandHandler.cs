using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PersistirRegistroFrequenciaCommandHandler : IRequestHandler<PersistirRegistroFrequenciaCommand, long>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IMediator mediator;

        public PersistirRegistroFrequenciaCommandHandler(IRepositorioFrequencia repositorioFrequencia, IMediator mediator)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(PersistirRegistroFrequenciaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioFrequencia.SalvarAsync(request.RegistroFrequencia);
        }
    }
}
