using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoaMap : BaseMap<Objetivo>
    {
        public ObjetivoaMap()
        {
            ToTable("objetivo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.DtInicio).ToColumn("dt_inicio");
            Map(c => c.DtFim).ToColumn("dt_fim");
            Map(c => c.EixoId).ToColumn("eixo_id");
        }
    }
}