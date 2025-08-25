using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ImportacaoLogErroMap : BaseMap<ImportacaoLogErro>
    {
        public ImportacaoLogErroMap()
        {
            ToTable("importacao_log_erro");
            Map(c => c.ImportacaoLogId).ToColumn("importacao_log_id");
            Map(c => c.LinhaArquivo).ToColumn("linha_arquivo");
            Map(c => c.MotivoFalha).ToColumn("motivo_falha");
        }
    }
}
