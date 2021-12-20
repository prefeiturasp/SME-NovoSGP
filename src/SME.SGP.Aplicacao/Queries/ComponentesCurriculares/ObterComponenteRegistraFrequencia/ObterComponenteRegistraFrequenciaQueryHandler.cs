using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteRegistraFrequenciaQueryHandler : IRequestHandler<ObterComponenteRegistraFrequenciaQuery, bool>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterComponenteRegistraFrequenciaQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<bool> Handle(ObterComponenteRegistraFrequenciaQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.RegistraFrequencia(request.ComponenteCurricularId);
    }
}
