using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ImportacaoLogErroMap : BaseMap<ImportacaoLogErro>
    {
        public ImportacaoLogErroMap()
        {
            ToTable("importacao_log_erro");
            Map(nameof(ImportacaoLogErro.ImportacaoLogId), "importacao_log_id");
            Map(nameof(ImportacaoLogErro.LinhaArquivo), "linha_arquivo");
            Map(nameof(ImportacaoLogErro.MotivoFalha), "motivo_falha");
        }
    }
}