using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoDevolutivasTurmaCommand : IRequest
    {
        public RegistraConsolidacaoDevolutivasTurmaCommand(ConsolidacaoDevolutivas consolidacaoDevolutivas)
        {
            ConsolidacaoDevolutivas = consolidacaoDevolutivas;
        }

        public ConsolidacaoDevolutivas ConsolidacaoDevolutivas { get; }
    }
}
