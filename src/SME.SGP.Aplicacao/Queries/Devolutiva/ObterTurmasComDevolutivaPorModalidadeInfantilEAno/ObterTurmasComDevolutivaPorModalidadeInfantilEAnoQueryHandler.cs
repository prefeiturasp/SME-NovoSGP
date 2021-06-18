using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQueryHandler : IRequestHandler<ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery, IEnumerable<DevolutivaTurmaDTO>>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQueryHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<IEnumerable<DevolutivaTurmaDTO>> Handle(ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery request, CancellationToken cancellationToken)
            => await repositorioDevolutiva.ObterTurmasInfantilComDevolutivasPorAno(request.AnoLetivo);
    }
}
