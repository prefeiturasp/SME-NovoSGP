using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseRecomendacaoMap : BaseMap<ConselhoClasseRecomendacao>
    {
        public ConselhoClasseRecomendacaoMap()
        {
            ToTable("conselho_classe_recomendacao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Recomendacao).ToColumn("recomendacao");
            Map(c => c.Tipo).ToColumn("tipo");
        }
    }
}