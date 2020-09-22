using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.MotivosAusencia.ObterMotivosAusencia
{
    public class ObterMotivosAusenciaQueryHandler : IRequestHandler<ObterMotivosAusenciaQuery, IEnumerable<MotivoAusencia>>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioMotivoAusencia repositorioMotivoAusencia;

        public ObterMotivosAusenciaQueryHandler(IRepositorioCache repositorioCache, IRepositorioMotivoAusencia repositorioMotivoAusencia)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioMotivoAusencia = repositorioMotivoAusencia ?? throw new ArgumentNullException(nameof(repositorioMotivoAusencia));
        }
        public async Task<IEnumerable<MotivoAusencia>> Handle(ObterMotivosAusenciaQuery request, CancellationToken cancellationToken)
        {
            var motivoAusencia = await repositorioCache.ObterAsync("MotivoAusencia", async () => await repositorioMotivoAusencia.ListarAsync());
            if (motivoAusencia == null)
            {
                throw new NegocioException("Não foi possível recuperar a lista de motivo ausência.");
            }

            return motivoAusencia;
        }
    }
}
