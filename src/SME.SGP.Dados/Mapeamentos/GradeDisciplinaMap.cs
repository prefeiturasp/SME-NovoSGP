using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class GradeDisciplinaMap : BaseEntityMap<GradeDisciplina>
    {
        public GradeDisciplinaMap()
        {
            ToTable("grade_disciplina");
            Map(nameof(GradeDisciplina.GradeId), "grade_id");
            Map(nameof(GradeDisciplina.Ano), "ano");
            Map(nameof(GradeDisciplina.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(GradeDisciplina.QuantidadeAulas), "quantidade_aulas");
        }
    }
}