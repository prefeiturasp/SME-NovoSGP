using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordosPorAulaQueryHandler : IRequestHandler<ObterDiariosDeBordosPorAulaQuery, IEnumerable<DiarioBordo>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDiariosDeBordosPorAulaQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public Task<IEnumerable<DiarioBordo>> Handle(ObterDiariosDeBordosPorAulaQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioDiarioBordo.ObterPorAulaId(request.AulaId);
        }
    }
}
