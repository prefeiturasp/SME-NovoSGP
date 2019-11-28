using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtribuicaoCJMap : BaseMap<AtribuicaoCJ>
    {
        public AtribuicaoCJMap()
        {
            ToTable("atribuicao_cj");

            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.ProfessorRf).ToColumn("professor_rf");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}