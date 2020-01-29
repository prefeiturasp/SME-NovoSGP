using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaMap : BaseMap<Pendencia>
    {
        public PendenciaMap()
        {
            ToTable("pendencia");
            Map(c => c.FechamentoId).ToColumn("fechamento_id");
            Map(c => c.Fechamento).Ignore();
        }
    }
}