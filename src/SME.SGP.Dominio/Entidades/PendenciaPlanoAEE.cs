using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PendenciaPlanoAEE : EntidadeBase
    {
        public PendenciaPlanoAEE() { }
        public PendenciaPlanoAEE(long pendenciaId, long planoAEEId) 
        {
            PendenciaId = pendenciaId;
            PlanoAEEId = planoAEEId;
        }

        public PlanoAEE PlanoAEE { get; set; }
        public long PlanoAEEId { get; set; }

        public Pendencia Pendencia { get; set; }
        public long PendenciaId { get; set; }
    }
}
