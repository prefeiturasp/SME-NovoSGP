using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaPlanoAEE : EntidadeBase
    {
        public PendenciaPlanoAEE() { }
        public PendenciaPlanoAEE(long pendenciaId, long planoAEEId) 
        {
            PendenciaId = pendenciaId;
            PlanoAEEId = planoAEEId;
        }
        [Computed]
        public PlanoAEE PlanoAEE { get; set; }
        public long PlanoAEEId { get; set; }
        [Computed]
        public Pendencia Pendencia { get; set; }
        public long PendenciaId { get; set; }
    }
}
