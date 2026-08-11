using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaAulaMap : SimpleMap<PendenciaAula>
    {
        public PendenciaAulaMap()
        {
            ToTable("pendencia_aula");
            Map(nameof(PendenciaAula.AulaId), "aula_id");
            Map(nameof(PendenciaAula.PendenciaId), "pendencia_id");
            Map(nameof(PendenciaAula.Motivo), "motivo");
        }
    }
}