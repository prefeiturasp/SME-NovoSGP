namespace SME.SGP.Dominio.Constantes
{
    public static class ArmazenamentoObjetos
    {
        public const string EXPRESSAO_NOME_ARQUIVO = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
        public const string EXPRESSAO_NOME_ARQUIVO_COM_PASTA = @"((\/\w+)*\/)[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
    }
}