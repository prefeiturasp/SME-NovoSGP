using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioPeriodicoPAPSecaoMap : BaseMap<RelatorioPeriodicoPAPSecao>
    {
        public RelatorioPeriodicoPAPSecaoMap()
        {
            ToTable("relatorio_periodico_pap_secao");

            Map(c => c.RelatorioPeriodicoAlunoId).ToColumn("relatorio_periodico_pap_aluno_id");
            Map(c => c.SecaoRelatorioPeriodicoId).ToColumn("secao_relatorio_periodico_pap_id");
            Map(c => c.Concluido).ToColumn("concluido");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
