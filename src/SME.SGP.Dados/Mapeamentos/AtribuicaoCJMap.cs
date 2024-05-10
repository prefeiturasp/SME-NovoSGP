using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtribuicaoCJMap : BaseMap<AtribuicaoCJ>
    {
        public AtribuicaoCJMap()
        {
            ToTable("atribuicao_cj");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.Modalidade).ToColumn("modalidade");
            Map(c => c.ProfessorRf).ToColumn("professor_rf");
            Map(c => c.Substituir).ToColumn("substituir");
            Map(c => c.Turma).Ignore();
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.UeId).ToColumn("ue_id");
        }
    }
}