using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ImportarAtividadeGsaCommand : IRequest
    {
        public ImportarAtividadeGsaCommand(AtividadeGsaDto atividadeGsa)
        {
            AtividadeGsa = atividadeGsa;
        }

        public AtividadeGsaDto AtividadeGsa { get; }
    }
}
