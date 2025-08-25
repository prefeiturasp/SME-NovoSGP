using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ImportacaoLogMap : BaseMap<ImportacaoLog>
    {
        public ImportacaoLogMap()
        {
            ToTable("importacao_log");
            Map(c => c.NomeArquivo).ToColumn("nome_arquivo");
            Map(c => c.TipoArquivoImportacao).ToColumn("tipo_arquivo_importacao");
            Map(c => c.DataInicioProcessamento).ToColumn("data_inicio_processamento");
            Map(c => c.DataFimProcessamento).ToColumn("data_fim_processamento");
            Map(c => c.TotalRegistros).ToColumn("total_registros");
            Map(c => c.RegistrosProcessados).ToColumn("registros_processados");
            Map(c => c.RegistrosComFalha).ToColumn("registros_com_falha");
            Map(c => c.StatusImportacao).ToColumn("status_importacao");
        }
    }
}
