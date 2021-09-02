using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ImportarNotaAtividadeGsaCommand : IRequest
    {
        public ImportarNotaAtividadeGsaCommand(NotaAtividadeGsaDto notaAtividadeGsa)
        {
            NotaAtividadeGsaDto = notaAtividadeGsa;
        }

        public NotaAtividadeGsaDto NotaAtividadeGsaDto { get; }
    }
}
