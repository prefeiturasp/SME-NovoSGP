using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SecaoRelatorioPeriodicoPAPMAp : BaseMap<SecaoRelatorioPeriodicoPAP>
    {
        public SecaoRelatorioPeriodicoPAPMAp()
        {
            ToTable("secao_relatorio_periodico_pap");

            Map(c => c.QuestionarioId).ToColumn("questionario_id");
            Map(c => c.ConfiguracaoRelatorioId).ToColumn("configuracao_relatorio_pap_id");
            Map(c => c.NomeComponente).ToColumn("nome_componente");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.Etapa).ToColumn("etapa");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
