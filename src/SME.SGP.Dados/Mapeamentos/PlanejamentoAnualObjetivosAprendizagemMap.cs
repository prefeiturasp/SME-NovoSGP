using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanejamentoAnualObjetivosAprendizagemMap : BaseMap<PlanejamentoAnualObjetivoAprendizagem>
    {
        public PlanejamentoAnualObjetivosAprendizagemMap()
        {
            ToTable("planejamento_anual_objetivos_aprendizagem");
            Map(nameof(PlanejamentoAnualObjetivoAprendizagem.PlanejamentoAnualComponenteId), "planejamento_anual_componente_id");
            Map(nameof(PlanejamentoAnualObjetivoAprendizagem.ObjetivoAprendizagemId), "objetivo_aprendizagem_id");
            Map(nameof(PlanejamentoAnualObjetivoAprendizagem.Excluido), "excluido");
        }
    }
}