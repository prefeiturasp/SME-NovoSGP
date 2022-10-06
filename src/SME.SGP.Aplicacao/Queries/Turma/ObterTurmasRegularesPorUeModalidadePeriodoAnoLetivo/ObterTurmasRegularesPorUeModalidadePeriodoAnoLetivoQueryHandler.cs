using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQueryHandler : IRequestHandler<ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQuery,IEnumerable<AbrangenciaTurmaRetorno>>
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;

        public ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQueryHandler(IConsultasAbrangencia consultasAbrangencia)
        {
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> Handle(ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await this.consultasAbrangencia.ObterTurmasRegulares(request.CodigoUe,request.Modalidade,request.Periodo,request.ConsideraHistorico,request.AnoLetivo);
        }
    }
}