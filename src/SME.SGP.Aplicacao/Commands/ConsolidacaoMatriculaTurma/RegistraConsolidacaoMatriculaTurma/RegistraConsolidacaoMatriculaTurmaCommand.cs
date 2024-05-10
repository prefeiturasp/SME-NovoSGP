using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoMatriculaTurmaCommand : IRequest<long>
    {
        public RegistraConsolidacaoMatriculaTurmaCommand(long turmaId, int quantidade)
        {
            TurmaId = turmaId;
            Quantidade = quantidade;
        }

        public long TurmaId { get; }
        public int Quantidade { get; }
    }
}
