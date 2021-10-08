using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaObjetivoMap : BaseMap<RecuperacaoParalelaObjetivo>
    {
        public RecuperacaoParalelaObjetivoMap()
        {
            ToTable("recuperacao_paralela_objetivo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.DtFim).ToColumn("dt_fim");
            Map(c => c.DtInicio).ToColumn("dt_inicio");
            Map(c => c.EhEspecifico).ToColumn("eh_especifico");
            Map(c => c.EixoId).ToColumn("eixo_id");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Ordem).ToColumn("ordem");
        }
    }
}