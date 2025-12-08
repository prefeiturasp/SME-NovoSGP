using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPrioridadeAtendimentoNAAPAQuery :  IRequest<IEnumerable<PrioridadeAtendimentoNAAPADto>>
    {
        private static ObterPrioridadeAtendimentoNAAPAQuery _instance;
        public static ObterPrioridadeAtendimentoNAAPAQuery Instance => _instance ??= new();
    }
}
