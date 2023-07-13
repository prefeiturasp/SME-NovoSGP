using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PeriodoRelatorioPAPQueryHandler : IRequestHandler<PeriodoRelatorioPAPQuery, PeriodoRelatorioPAP>
    {
        private readonly IRepositorioPeriodoRelatorioPAP repositorio;
        public PeriodoRelatorioPAPQueryHandler(IRepositorioPeriodoRelatorioPAP repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<PeriodoRelatorioPAP> Handle(PeriodoRelatorioPAPQuery request, CancellationToken cancellationToken)
        {
            return this.repositorio.ObterComPeriodosEscolares(request.PeriodoIdPAP);
        }
    }
}
