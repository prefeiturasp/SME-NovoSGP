using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasDiarioBordoQueryHandler : IRequestHandler<ObterPendenciasDiarioBordoQuery, IEnumerable<Aula>>
    {
        private readonly IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo;

        public ObterPendenciasDiarioBordoQueryHandler(IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo)
        {
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordo));
        }

        public async Task<IEnumerable<Aula>> Handle(ObterPendenciasDiarioBordoQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaDiarioBordo.ListarPendenciasDiario(request.DreId, request.AnoLetivo);
    }
}
