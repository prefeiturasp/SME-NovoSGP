

using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComDevolutivaPorAulaTurmaIdQueryHandler : IRequestHandler<ObterTurmasComDevolutivaPorAulaTurmaIdQuery,IEnumerable<long>>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;
        public ObterTurmasComDevolutivaPorAulaTurmaIdQueryHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<IEnumerable<long>> Handle(ObterTurmasComDevolutivaPorAulaTurmaIdQuery request, CancellationToken cancellationToken)
          => await repositorioDevolutiva.ObterTurmasInfantilComDevolutivasPorTurmaIdAula(request.AulaTurmaId);
    }
}
