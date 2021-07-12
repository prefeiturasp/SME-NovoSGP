using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AtribuicaoEsporadicaMap : BaseMap<AtribuicaoEsporadica>
    {
        public AtribuicaoEsporadicaMap()
        {
            ToTable("atribuicao_esporadica");
            Map(a => a.DreId).ToColumn("dre_id");
            Map(a => a.UeId).ToColumn("ue_id");
            Map(a => a.ProfessorRf).ToColumn("professor_rf");
            Map(a => a.DataFim).ToColumn("data_fim");
            Map(a => a.DataInicio).ToColumn("data_inicio");
            Map(a => a.AnoLetivo).ToColumn("ano_letivo");
        }
    }
}