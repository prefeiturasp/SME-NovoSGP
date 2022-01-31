using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ObterCodigosTurmasAbrangenciaPorUeModalidadeAnoQueryHandler : IRequestHandler<ObterCodigosTurmasAbrangenciaPorUeModalidadeAnoQuery, IEnumerable<long>>
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;
        public ObterCodigosTurmasAbrangenciaPorUeModalidadeAnoQueryHandler(IConsultasAbrangencia consultasAbrangencia)
        {
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
        }

        public async Task<IEnumerable<long>> Handle(ObterCodigosTurmasAbrangenciaPorUeModalidadeAnoQuery request, CancellationToken cancellationToken)
        {
            return await consultasAbrangencia.ObterCodigoTurmasAbrangencia(request.UeCodigo,request.Modalidade,request.Periodo,request.ConsideraHistorico,request.AnoLetivo,request.Tipos,request.DesconsideraNovosAnosInfantil);
        }
    }
}
