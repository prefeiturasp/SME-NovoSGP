using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaFechamentoAulaMap : SimpleMap<PendenciaFechamentoAula>
    {
        public PendenciaFechamentoAulaMap()
        {
            ToTable("pendencia_fechamento_aula");
            Map(nameof(PendenciaFechamentoAula.AulaId), "aula_id");
            Map(nameof(PendenciaFechamentoAula.PendenciaFechamentoId), "pendencia_fechamento_id");
        }
    }
}