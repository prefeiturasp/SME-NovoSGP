using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsDiariosBordoPorDevolutivaQueryHandler : IRequestHandler<ObterIdsDiariosBordoPorDevolutivaQuery, IEnumerable<long>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterIdsDiariosBordoPorDevolutivaQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsDiariosBordoPorDevolutivaQuery request, CancellationToken cancellationToken)
            => await repositorioDiarioBordo.ObterIdsPorDevolutiva(request.DevolutivaId);
    }
}
