using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class GradeDisciplinaMap : BaseMap<GradeDisciplina>
    {
        public GradeDisciplinaMap()
        {
            ToTable("grade_disciplina");
            Map(a => a.GradeId).ToColumn("grade_id");
            Map(a => a.Ano).ToColumn("ano");
            Map(a => a.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(a => a.QuantidadeAulas).ToColumn("quantidade_aulas");
        }
    }
}