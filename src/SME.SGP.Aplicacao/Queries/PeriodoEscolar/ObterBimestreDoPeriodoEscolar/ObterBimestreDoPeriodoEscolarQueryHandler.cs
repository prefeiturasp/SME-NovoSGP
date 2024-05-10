using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreDoPeriodoEscolarQueryHandler : IRequestHandler<ObterBimestreDoPeriodoEscolarQuery, int>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterBimestreDoPeriodoEscolarQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<int> Handle(ObterBimestreDoPeriodoEscolarQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.ObterBimestre(request.PeriodoEscolarId);
    }
}
