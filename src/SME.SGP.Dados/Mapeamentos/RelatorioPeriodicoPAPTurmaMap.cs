using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioPeriodicoPAPTurmaMap : BaseMap<RelatorioPeriodicoPAPTurma>
    {
        public RelatorioPeriodicoPAPTurmaMap()
        {
            ToTable("relatorio_periodico_pap_turma");

            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.PeriodoRelatorioId).ToColumn("periodo_relatorio_pap_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
