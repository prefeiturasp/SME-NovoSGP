using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdPeriodoRelatorioPAPQueryHandler : IRequestHandler<ObterIdPeriodoRelatorioPAPQuery, long>
    {
        private readonly IRepositorioPeriodoRelatorioPAP repositorio;

        public ObterIdPeriodoRelatorioPAPQueryHandler(IRepositorioPeriodoRelatorioPAP repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<long> Handle(ObterIdPeriodoRelatorioPAPQuery request, CancellationToken cancellationToken)
        {
            return this.repositorio.ObterIdPeriodoRelatorioPAP(request.AnoLetivo, request.Periodo, request.TipoPeriodo);
        }
    }
}
