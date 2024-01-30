using MediatR;
using System.Linq;
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

            var diasLetivos = diasLetivosENaoLetivosDaUe.Count(c => c.EhLetivo);
            var diasNaoLetivos = diasLetivosENaoLetivosDaUe.Count(c => c.EhNaoLetivo);

            return Task.FromResult( diasLetivos - diasNaoLetivos );
        }
    }
}
