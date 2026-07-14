using MediatR;

namespace SME.SGP.Aplicacao
{
    public class VerificaExclusaoPendenciasAusenciaFechamentoCommand : IRequest<bool>
    {
        
        public VerificaExclusaoPendenciasAusenciaFechamentoCommand(long disciplinaId, long? periodoEscolarId, long turmaId)
        {
            DisciplinaId = disciplinaId;
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
        }
        public long DisciplinaId { get; set; }
        public long TurmaId { get; set; }
        public long? PeriodoEscolarId { get; set; }
    }
}
