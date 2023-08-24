using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEEVigenteQuery : IRequest<IEnumerable<EncaminhamentoAEEVigenteDto>>
    {
        public ObterEncaminhamentoAEEVigenteQuery(long? anoLetivo = null)
        {
            AnoLetivo = anoLetivo;
        }

        public long? AnoLetivo { get; set; }
    }
}
