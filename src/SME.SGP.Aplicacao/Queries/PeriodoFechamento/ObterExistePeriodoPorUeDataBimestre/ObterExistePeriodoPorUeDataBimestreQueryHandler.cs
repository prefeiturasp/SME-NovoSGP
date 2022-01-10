using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterExistePeriodoPorUeDataBimestreQueryHandler : IRequestHandler<ObterExistePeriodoPorUeDataBimestreQuery, PeriodoFechamento>
    {
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;

        public ObterExistePeriodoPorUeDataBimestreQueryHandler(IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }
        public async Task<PeriodoFechamento> Handle(ObterExistePeriodoPorUeDataBimestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoFechamento.ObterPeriodoPorUeDataBimestreAsync(request.UeId, request.DataParaVerificar, request.Bimestre);            
        }
    }
}
