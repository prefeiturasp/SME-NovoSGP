using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class CompensacaoAusenciaMap : BaseMap<CompensacaoAusencia>
    {
        public CompensacaoAusenciaMap()
        {
            ToTable("compensacao_ausencia");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Descricao).ToColumn("descricao");
        }
    }
}
