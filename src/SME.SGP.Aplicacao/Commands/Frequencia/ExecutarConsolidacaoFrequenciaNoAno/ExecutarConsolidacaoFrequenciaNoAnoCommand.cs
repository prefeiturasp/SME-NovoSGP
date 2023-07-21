using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoFrequenciaNoAnoCommand : IRequest
    {
        public ExecutarConsolidacaoFrequenciaNoAnoCommand(DateTime data)
        {
            Data = data;
        }

        public DateTime Data { get; }
    }
}
