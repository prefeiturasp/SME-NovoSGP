using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasLetivosDaUeQueryHandler : IRequestHandler<ObterDiasLetivosDaUeQuery, int>
    {
        public Task<int> Handle(ObterDiasLetivosDaUeQuery request, CancellationToken cancellationToken)
        {
            var diasLetivosENaoLetivosDaUe = request.DiasLetivosENaoLetivos.Where(x =>
                                // Eventos SME
                                (!x.UesIds.Any() && !x.DreIds.Any())
                                // Eventos Dre
                                || (x.DreIds.Contains(request.DreCodigo) && !x.UesIds.Any())
                                // Eventos Ue
                                || (x.DreIds.Contains(request.DreCodigo) && x.UesIds.Contains(request.UeCodigo)));

            var diasLetivos = diasLetivosENaoLetivosDaUe.Where(c => c.EhLetivo).Count();
            var diasNaoLetivos = diasLetivosENaoLetivosDaUe.Where(c => c.EhNaoLetivo).Count();

            return Task.FromResult( diasLetivos - diasNaoLetivos );
        }
    }
}
