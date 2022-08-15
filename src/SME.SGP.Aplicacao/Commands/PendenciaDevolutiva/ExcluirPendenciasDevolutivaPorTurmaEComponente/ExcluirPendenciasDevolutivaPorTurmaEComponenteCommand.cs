using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasDevolutivaPorTurmaEComponenteCommand : IRequest<bool>
    {
        public ExcluirPendenciasDevolutivaPorTurmaEComponenteCommand(long turmaId, long componenteId)
        {
            TurmaId = turmaId;
            ComponenteId = componenteId;
        }

        public long TurmaId { get; }
        public long ComponenteId { get; }
    }
}
