using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoDevolutivasTurmaCommand : IRequest<long>
    {
        public RegistraConsolidacaoDevolutivasTurmaCommand(long turmaId, int quantidadeEstimadaDevolutivas, int quantidadeRegistradaDevolutivas)
        {
            TurmaId = turmaId;
            QuantidadeEstimadaDevolutivas = quantidadeEstimadaDevolutivas;
            QuantidadeRegistradaDevolutivas = quantidadeRegistradaDevolutivas;
        }

        public long TurmaId { get; }
        public int QuantidadeEstimadaDevolutivas { get; }
        public int QuantidadeRegistradaDevolutivas { get; }
    }
}
