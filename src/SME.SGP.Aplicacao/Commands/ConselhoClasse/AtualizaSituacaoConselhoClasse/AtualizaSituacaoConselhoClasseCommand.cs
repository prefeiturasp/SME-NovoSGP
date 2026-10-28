using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AtualizaSituacaoConselhoClasseCommand : IRequest<bool>
    {
        public AtualizaSituacaoConselhoClasseCommand(long conselhoClasseId, string codigoTurma = null)
        {
            ConselhoClasseId = conselhoClasseId;
            CodigoTurma = codigoTurma;
        }

        public long ConselhoClasseId { get; set; }
        public string CodigoTurma { get; set; }
    }
}
