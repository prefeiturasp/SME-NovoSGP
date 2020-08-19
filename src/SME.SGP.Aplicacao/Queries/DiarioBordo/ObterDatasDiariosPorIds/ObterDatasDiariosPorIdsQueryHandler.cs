using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasDiariosPorIdsQueryHandler : IRequestHandler<ObterDatasDiariosPorIdsQuery, IEnumerable<DateTime>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDatasDiariosPorIdsQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<IEnumerable<DateTime>> Handle(ObterDatasDiariosPorIdsQuery request, CancellationToken cancellationToken)
        {
            var datas = await repositorioDiarioBordo.ObterDatasPorIds(request.DiariosBordoIds);

            return datas;
        }
    }
}
