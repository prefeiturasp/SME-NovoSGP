using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class CompensacaoAusenciaDisciplinaRegenciaMap : BaseEntityMap<CompensacaoAusenciaDisciplinaRegencia>
    {
        public CompensacaoAusenciaDisciplinaRegenciaMap()
        {
            ToTable("compensacao_ausencia_disciplina_regencia");
            Map(nameof(CompensacaoAusenciaDisciplinaRegencia.Excluido), "excluido");
            Map(nameof(CompensacaoAusenciaDisciplinaRegencia.CompensacaoAusenciaId), "compensacao_ausencia_id");
            Map(nameof(CompensacaoAusenciaDisciplinaRegencia.DisciplinaId), "disciplina_id");
        }
    }
}