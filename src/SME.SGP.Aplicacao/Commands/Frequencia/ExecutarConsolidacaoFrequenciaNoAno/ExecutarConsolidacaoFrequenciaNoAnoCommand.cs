using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoFrequenciaNoAnoCommand : IRequest
    {
        public ExecutarConsolidacaoFrequenciaNoAnoCommand(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; }
    }
}
