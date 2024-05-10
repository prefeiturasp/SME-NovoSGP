using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class GradeFiltroMap : BaseMap<GradeFiltro>
    {
        public GradeFiltroMap()
        {
            ToTable("grade_filtro");
            Map(c => c.GradeId).ToColumn("grade_id");
            Map(c => c.TipoEscola).ToColumn("tipo_escola");
            Map(c => c.Modalidade).ToColumn("modalidade");
            Map(c => c.DuracaoTurno).ToColumn("duracao_turno");
        }
    }
}