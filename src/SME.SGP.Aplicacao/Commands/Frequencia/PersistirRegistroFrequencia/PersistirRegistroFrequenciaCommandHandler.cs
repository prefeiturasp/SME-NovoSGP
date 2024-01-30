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
        
        public PersistirRegistroFrequenciaCommandHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<long> Handle(PersistirRegistroFrequenciaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioFrequencia.SalvarAsync(request.RegistroFrequencia);
        }
    }
}
