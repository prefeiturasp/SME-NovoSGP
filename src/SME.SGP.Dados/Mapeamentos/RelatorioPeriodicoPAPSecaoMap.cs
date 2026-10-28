using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioPeriodicoPAPSecaoMap : BaseMap<RelatorioPeriodicoPAPSecao>
    {
        public RelatorioPeriodicoPAPSecaoMap()
        {
            ToTable("relatorio_periodico_pap_secao");

            Map(nameof(RelatorioPeriodicoPAPSecao.RelatorioPeriodicoAlunoId), "relatorio_periodico_pap_aluno_id");
            Map(nameof(RelatorioPeriodicoPAPSecao.SecaoRelatorioPeriodicoId), "secao_relatorio_periodico_pap_id");
            Map(nameof(RelatorioPeriodicoPAPSecao.Concluido), "concluido");
            Map(nameof(RelatorioPeriodicoPAPSecao.Excluido), "excluido");
        }
    }
}