using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseRecomendacaoMap : BaseMap<ConselhoClasseRecomendacao>
    {
        public ConselhoClasseRecomendacaoMap()
        {
            ToTable("conselho_classe_recomendacao");
        }
    }
}