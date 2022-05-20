using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AtribuicaoEsporadicaMap : BaseMap<AtribuicaoEsporadica>
    {
        public AtribuicaoEsporadicaMap()
        {
            ToTable("atribuicao_esporadica");
            Map(c => c.DataFim).ToColumn("data_fim");
            Map(c => c.DataInicio).ToColumn("data_inicio");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.ProfessorRf).ToColumn("professor_rf");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
        }
    }
}