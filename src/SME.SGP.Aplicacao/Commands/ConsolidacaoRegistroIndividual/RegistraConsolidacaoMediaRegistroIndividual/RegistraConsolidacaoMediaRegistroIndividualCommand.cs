using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoMediaRegistroIndividualCommand : IRequest<long>
    {
        public RegistraConsolidacaoMediaRegistroIndividualCommand(long turmaId, int quantidade)
        {
            TurmaId = turmaId;
            Quantidade = quantidade;
        }

        public long TurmaId { get; }
        public int Quantidade { get; }
    }
}
