using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanejamentoAnualObjetivosAprendizagemMap : BaseMap<PlanejamentoAnualObjetivoAprendizagem>
    {
        public PlanejamentoAnualObjetivosAprendizagemMap()
        {
            ToTable("planejamento_anual_objetivos_aprendizagem");
            Map(c => c.PlanejamentoAnualComponenteId).ToColumn("planejamento_anual_componente_id");
            Map(c => c.ObjetivoAprendizagemId).ToColumn("objetivo_aprendizagem_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}