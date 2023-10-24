//namespace SME.SGP.Dominio
namespace SME.SGP.Infra
{
    public static class RegexConstants
    {
        //ARQUIVOS
        public const string EXPRESSAO_NOME_ARQUIVO_UUID = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
        public const string EXPRESSAO_NOME_ARQUIVO_UUID_COM_PASTA = @"((\/\w+)*\/)[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+";
        //TAGS HTML
        public const string EXPRESSAO_TAG_IMG = @"<img[^>]*>";
        public const string EXPRESSAO_TAG_BR = @"<br[^>]*>";
        public const string EXPRESSAO_TAG_P = @"<p[^>]*>";
        public const string EXPRESSAO_TAG_LI = @"<li[^>]*>";
        public const string EXPRESSAO_TAG_HTML_QUALQUER = @"<[^>]*>";
        public const string EXPRESSAO_TAG_FRAME = @"<iframe[^>]*>";
        public const string EXPRESSAO_TAG_VIDEO = @"<video.+</video>";
        //JSON
        public const string EXPRESSAO_CHAVE_VALOR_ID = @"""id"":""(.*?)""";
        //GENERICO
        public const string EXPRESSAO_HORA = @"^([01][0-9]|2[0-3]):([0-5][0-9])$";
        public const string EXPRESSAO_PONTO_SEM_ESPACO_SEM_FINAL_LINHA = @"\.(?! |$)";
        public const string EXPRESSAO_ESPACO_BRANCO = @"&nbsp;";
        //SGP
        public const string EXPRESSAO_ANO_TURMA = @"^[1-9]{1}";
    }
}