using MediatR;

namespace SME.SGP.Aplicacao
{
    public class PendenciaDiasNaoLetivosCommand : IRequest<bool>
    {
        public PendenciaDiasNaoLetivosCommand(long turmaId, string componenteCurricularId, long tipoCalendarioId, int bimestre)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            TipoCalendarioId = tipoCalendarioId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public string ComponenteCurricularId { get; set; }
        public long TipoCalendarioId { get; set; }
        public int Bimestre { get; set; }
        
    }
}
