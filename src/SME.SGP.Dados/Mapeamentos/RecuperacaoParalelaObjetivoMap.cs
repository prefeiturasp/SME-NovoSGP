using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaObjetivoMap : BaseMap<RecuperacaoParalelaObjetivo>
    {
        public RecuperacaoParalelaObjetivoMap()
        {
            ToTable("recuperacao_paralela_objetivo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.DtInicio).ToColumn("dt_inicio");
            Map(c => c.DtFim).ToColumn("dt_fim");
            Map(c => c.EixoId).ToColumn("eixo_id");
        }
    }
}