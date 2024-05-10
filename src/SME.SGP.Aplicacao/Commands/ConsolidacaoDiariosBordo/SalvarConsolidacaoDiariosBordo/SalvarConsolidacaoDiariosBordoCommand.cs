using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidacaoDiariosBordoCommand : IRequest
    {
        public SalvarConsolidacaoDiariosBordoCommand(ConsolidacaoDiariosBordo consolidacaoDiariosBordo)
        {
            ConsolidacaoDiariosBordo = consolidacaoDiariosBordo;
        }

        public ConsolidacaoDiariosBordo ConsolidacaoDiariosBordo { get; }
    }
}
