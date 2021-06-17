using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAreasConhecimentoComponenteCurricularQuery : IRequest<IEnumerable<AreaDoConhecimentoDto>>
    {
        public ObterAreasConhecimentoComponenteCurricularQuery(long[] codigosComponenteCurricular)
        {
            CodigosComponenteCurricular = codigosComponenteCurricular;
        }

        public long[] CodigosComponenteCurricular { get; set; }
    }
}
