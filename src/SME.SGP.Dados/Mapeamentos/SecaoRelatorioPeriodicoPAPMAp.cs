using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SecaoRelatorioPeriodicoPAPMAp : BaseMap<SecaoRelatorioPeriodicoPAP>
    {
        public SecaoRelatorioPeriodicoPAPMAp()
        {
            ToTable("secao_relatorio_periodico_pap");

            Map(nameof(SecaoRelatorioPeriodicoPAP.QuestionarioId), "questionario_id");
            Map(nameof(SecaoRelatorioPeriodicoPAP.NomeComponente), "nome_componente");
            Map(nameof(SecaoRelatorioPeriodicoPAP.Nome), "nome");
            Map(nameof(SecaoRelatorioPeriodicoPAP.Ordem), "ordem");
            Map(nameof(SecaoRelatorioPeriodicoPAP.Etapa), "etapa");
            Map(nameof(SecaoRelatorioPeriodicoPAP.Excluido), "excluido");
        }
    }
}