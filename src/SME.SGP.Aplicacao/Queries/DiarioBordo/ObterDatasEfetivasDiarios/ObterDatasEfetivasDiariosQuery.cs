using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasEfetivasDiariosQuery : IRequest<IEnumerable<Tuple<long, DateTime>>>
    {
        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public DateTime PeriodoInicio { get; }
        public DateTime PeriodoFim { get; }

        public ObterDatasEfetivasDiariosQuery(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }
    }
}
