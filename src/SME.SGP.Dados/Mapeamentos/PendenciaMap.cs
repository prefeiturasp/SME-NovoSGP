using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaMap : BaseMap<Pendencia>
    {
        public PendenciaMap()
        {
            ToTable("pendencia");
            Map(c => c.DescricaoHtml).ToColumn("descracao_html");
        }
    }
}