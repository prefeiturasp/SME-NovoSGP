using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsPendenciaDiarioBordoPorAnoLetivoQueryHandler : IRequestHandler<ObterIdsPendenciaDiarioBordoPorAnoLetivoQuery, IEnumerable<long>>
    {
        private readonly IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo;

        public ObterIdsPendenciaDiarioBordoPorAnoLetivoQueryHandler(IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo)
        {
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordo));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsPendenciaDiarioBordoPorAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorioPendenciaDiarioBordo.ObterIdsPendencias(request.AnoLetivo, request.CodigoUe);
        }
    }
}
