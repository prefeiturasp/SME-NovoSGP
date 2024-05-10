using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDevolutivaPorTurmaComponenteNoPeriodoQueryHandler : IRequestHandler<ObterDevolutivaPorTurmaComponenteNoPeriodoQuery, IEnumerable<long>>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ObterDevolutivaPorTurmaComponenteNoPeriodoQueryHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<IEnumerable<long>> Handle(ObterDevolutivaPorTurmaComponenteNoPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioDevolutiva.ObterDevolutivasPorTurmaComponenteNoPeriodo(request.TurmaCodigo, request.ComponenteCurricularCodigo, request.PeriodoInicio, request.PeriodoFim);
    }
}
