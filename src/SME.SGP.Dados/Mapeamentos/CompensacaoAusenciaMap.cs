using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class CompensacaoAusenciaMap : BaseEntityMap<CompensacaoAusencia>
    {
        public CompensacaoAusenciaMap()
        {
            ToTable("compensacao_ausencia");
            Map(nameof(CompensacaoAusencia.AnoLetivo), "ano_letivo");
            Map(nameof(CompensacaoAusencia.Excluido), "excluido");
            Map(nameof(CompensacaoAusencia.Migrado), "migrado");
            Map(nameof(CompensacaoAusencia.Bimestre), "bimestre");
            Map(nameof(CompensacaoAusencia.DisciplinaId), "disciplina_id");
            Map(nameof(CompensacaoAusencia.TurmaId), "turma_id");
            Map(nameof(CompensacaoAusencia.Nome), "nome");
            Map(nameof(CompensacaoAusencia.Descricao), "descricao");
            Map(nameof(CompensacaoAusencia.ProfessorRf), "professor_rf");
        }
    }
}