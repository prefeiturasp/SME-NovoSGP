using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioPeriodicoPAPTurmaMap : BaseEntityMap<RelatorioPeriodicoPAPTurma>
    {
        public RelatorioPeriodicoPAPTurmaMap()
        {
            ToTable("relatorio_periodico_pap_turma");

            Map(nameof(RelatorioPeriodicoPAPTurma.TurmaId), "turma_id");
            Map(nameof(RelatorioPeriodicoPAPTurma.PeriodoRelatorioId), "periodo_relatorio_pap_id");
            Map(nameof(RelatorioPeriodicoPAPTurma.Excluido), "excluido");
        }
    }
}