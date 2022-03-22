using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class GradeDisciplinaMap : BaseMap<GradeDisciplina>
    {
        public GradeDisciplinaMap()
        {
            ToTable("grade_disciplina");
            Map(c => c.GradeId).ToColumn("grade_id");
            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.QuantidadeAulas).ToColumn("quantidade_aulas");
        }
    }
}