using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentosTurmaDisciplinasDuplicadosQuery : IRequest<IEnumerable<long>>
    {
        public ObterFechamentosTurmaDisciplinasDuplicadosQuery(DateTime? dataInicio)
        {
            DataInicio = !dataInicio.HasValue || dataInicio.Value == DateTime.MinValue ? new DateTime(DateTime.Today.Year, 1, 1) : dataInicio.Value.Date;
        }
        public DateTime DataInicio { get; private set; }
    }
}
