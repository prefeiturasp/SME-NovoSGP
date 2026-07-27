using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoTurmaMap : BaseEntityMap<FechamentoTurma>
    {
        public FechamentoTurmaMap()
        {
            ToTable("fechamento_turma");
            Map(nameof(FechamentoTurma.PeriodoEscolarId), "periodo_escolar_id");
            Map(nameof(FechamentoTurma.TurmaId), "turma_id");
            Map(nameof(FechamentoTurma.Migrado), "migrado");
            Map(nameof(FechamentoTurma.Excluido), "excluido");
        }
    }
}