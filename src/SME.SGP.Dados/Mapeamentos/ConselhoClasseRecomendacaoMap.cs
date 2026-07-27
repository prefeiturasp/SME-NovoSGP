using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseRecomendacaoMap : BaseEntityMap<ConselhoClasseRecomendacao>
    {
        public ConselhoClasseRecomendacaoMap()
        {
            ToTable("conselho_classe_recomendacao");
            Map(nameof(ConselhoClasseRecomendacao.Excluido), "excluido");
            Map(nameof(ConselhoClasseRecomendacao.Recomendacao), "recomendacao");
            Map(nameof(ConselhoClasseRecomendacao.Tipo), "tipo");
        }
    }
}