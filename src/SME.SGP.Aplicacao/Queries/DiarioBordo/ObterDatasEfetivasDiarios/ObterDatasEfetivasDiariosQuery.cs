using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasEfetivasDiariosQuery : IRequest<IEnumerable<Tuple<long, DateTime>>>
    {
        public DateTime PeriodoInicio { get; }
        public DateTime PeriodoFim { get; }

        public ObterDatasEfetivasDiariosQuery(DateTime periodoInicio, DateTime periodoFim)
        {
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }
    }
}
