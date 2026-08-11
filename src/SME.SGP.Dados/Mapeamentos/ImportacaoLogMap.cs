using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ImportacaoLogMap : BaseMap<ImportacaoLog>
    {
        public ImportacaoLogMap()
        {
            ToTable("importacao_log");
            Map(nameof(ImportacaoLog.NomeArquivo), "nome_arquivo");
            Map(nameof(ImportacaoLog.TipoArquivoImportacao), "tipo_arquivo_importacao");
            Map(nameof(ImportacaoLog.DataInicioProcessamento), "data_inicio_processamento");
            Map(nameof(ImportacaoLog.DataFimProcessamento), "data_fim_processamento");
            Map(nameof(ImportacaoLog.TotalRegistros), "total_registros");
            Map(nameof(ImportacaoLog.RegistrosProcessados), "registros_processados");
            Map(nameof(ImportacaoLog.RegistrosComFalha), "registros_com_falha");
            Map(nameof(ImportacaoLog.StatusImportacao), "status_importacao");
        }
    }
}