using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class GradeFiltroMap : BaseEntityMap<GradeFiltro>
    {
        public GradeFiltroMap()
        {
            ToTable("grade_filtro");
            Map(nameof(GradeFiltro.GradeId), "grade_id");
            Map(nameof(GradeFiltro.TipoEscola), "tipo_escola");
            Map(nameof(GradeFiltro.Modalidade), "modalidade");
            Map(nameof(GradeFiltro.DuracaoTurno), "duracao_turno");
        }
    }
}