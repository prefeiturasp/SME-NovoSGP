using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterExistePeriodoPorUeDataBimestreQueryHandler : IRequestHandler<ObterExistePeriodoPorUeDataBimestreQuery, bool>
    {
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;

        public ObterExistePeriodoPorUeDataBimestreQueryHandler(IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }
        public async Task<bool> Handle(ObterExistePeriodoPorUeDataBimestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoFechamento.ExistePeriodoPorUeDataBimestre(request.UeId, request.DataParaVerificar, request.Bimestre);            
        }
    }
}
