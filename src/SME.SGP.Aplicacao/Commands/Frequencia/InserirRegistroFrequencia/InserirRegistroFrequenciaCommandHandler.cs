using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroFrequenciaCommandHandler : IRequestHandler<InserirRegistroFrequenciaCommand, long>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IMediator mediator;

        public InserirRegistroFrequenciaCommandHandler(IRepositorioFrequencia repositorioFrequencia, IMediator mediator)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(InserirRegistroFrequenciaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioFrequencia.SalvarAsync(request.RegistroFrequencia);
        }
    }
}
