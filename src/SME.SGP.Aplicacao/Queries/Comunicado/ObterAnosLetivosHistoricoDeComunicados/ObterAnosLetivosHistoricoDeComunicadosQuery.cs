using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosHistoricoDeComunicadosQuery : IRequest<IEnumerable<int>>
    {
        public ObterAnosLetivosHistoricoDeComunicadosQuery(DateTime? dataInicio, DateTime dataFim)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public DateTime? DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
