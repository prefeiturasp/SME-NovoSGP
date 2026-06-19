using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosFechamentoEscolasPorDataFinalQuery : IRequest<IEnumerable<PeriodoFechamentoBimestre>>
    {
        public ObterPeriodosFechamentoEscolasPorDataFinalQuery(DateTime dataFinal)
        {
            DataFinal = dataFinal;
        }

        public DateTime DataFinal { get; set; }
    }
}
