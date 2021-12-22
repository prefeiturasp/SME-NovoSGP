using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPossuiFrequenciaQueryHandler : IRequestHandler<ObterAulaPossuiFrequenciaQuery, bool>
    {
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;

        public ObterAulaPossuiFrequenciaQueryHandler(IRepositorioFrequenciaConsulta repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }
        public async Task<bool> Handle(ObterAulaPossuiFrequenciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequencia.FrequenciaAulaRegistrada(request.AulaId);
        }
    }
}
