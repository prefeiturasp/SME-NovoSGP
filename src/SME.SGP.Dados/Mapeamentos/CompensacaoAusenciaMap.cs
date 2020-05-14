using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class CompensacaoAusenciaMap : BaseMap<CompensacaoAusencia>
    {
        public CompensacaoAusenciaMap()
        {
            ToTable("compensacao_ausencia");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
        }
    }
}
