using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasIdFrequenciasExistentesPorAnosLetivosQueryHandler : IRequestHandler<ObterTurmasIdFrequenciasExistentesPorAnosLetivosQuery, IEnumerable<string>>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterTurmasIdFrequenciasExistentesPorAnosLetivosQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<IEnumerable<string>> Handle(ObterTurmasIdFrequenciasExistentesPorAnosLetivosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequencia.ObterTurmasCodigosFrequenciasExistentesPorAnoAsync(request.AnosLetivos);
        }
    }
}
