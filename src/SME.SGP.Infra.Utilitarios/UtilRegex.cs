using System;
using System.Text.RegularExpressions;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilRegex
    {
        //Regex pré compilados
        public static readonly Regex RegexTagsBR = new(RegexConstants.EXPRESSAO_TAG_BR, RegexOptions.Compiled);
        public static readonly Regex RegexTagsP = new(RegexConstants.EXPRESSAO_TAG_P, RegexOptions.Compiled);
        public static readonly Regex RegexTagsLI = new(RegexConstants.EXPRESSAO_TAG_LI, RegexOptions.Compiled);
        public static readonly Regex RegexTagsHTMLQualquer = new(RegexConstants.EXPRESSAO_TAG_HTML_QUALQUER, RegexOptions.Compiled);
        public static readonly Regex RegexEspacosEmBranco = new(RegexConstants.EXPRESSAO_ESPACO_BRANCO, RegexOptions.Compiled);
        public static readonly Regex RegexTagsIMG = new(RegexConstants.EXPRESSAO_TAG_IMG, RegexOptions.Compiled);
        public static readonly Regex RegexTagsFRAME = new(RegexConstants.EXPRESSAO_TAG_FRAME, RegexOptions.Compiled);
        public static readonly Regex RegexTagsVIDEO = new(RegexConstants.EXPRESSAO_TAG_VIDEO, RegexOptions.Compiled);
        public static readonly Regex RegexPontoSemEspacoSemFinalLinha = new(RegexConstants.EXPRESSAO_PONTO_SEM_ESPACO_SEM_FINAL_LINHA, RegexOptions.Compiled);
        public static readonly Regex RegexNomesArquivosUUID = new(RegexConstants.EXPRESSAO_NOME_ARQUIVO_UUID, RegexOptions.Compiled);
        public static readonly Regex RegexNomesArquivosUUIDComPasta = new(RegexConstants.EXPRESSAO_NOME_ARQUIVO_UUID_COM_PASTA, RegexOptions.Compiled);
        public static readonly Regex RegexChaveValorJsonAtributoId = new(RegexConstants.EXPRESSAO_CHAVE_VALOR_ID, RegexOptions.Compiled);
        public static readonly Regex RegexAnoTurma = new(RegexConstants.EXPRESSAO_ANO_TURMA, RegexOptions.Compiled);

        public static string RemoverTagsHtml(string texto)
        {
            texto = RegexTagsBR.Replace(texto, " ");
            texto = RegexTagsP.Replace(texto, " ");
            texto = RegexTagsLI.Replace(texto, " ");
            texto = RegexTagsHTMLQualquer.Replace(texto, String.Empty);
            texto = RegexEspacosEmBranco.Replace(texto, " ").Trim();
            return texto.Trim();
        }

        public static string RemoverTagsHtmlMidia(string texto)
        {
            texto = RegexTagsIMG.Replace(texto, " [arquivo indisponível na visualização] ");
            texto = RegexTagsFRAME.Replace(texto, " [arquivo indisponível na visualização] ");
            texto = RegexTagsVIDEO.Replace(texto, " [arquivo indisponível na visualização] ");
            return texto;
        }

        public static string AdicionarEspacos(string texto)
        {
            texto = RegexPontoSemEspacoSemFinalLinha.Replace(texto, ". ");
            return texto;
        }

        public static string ObterJsonSemAtributoId(string texto)
        {
            return UtilRegex.RegexChaveValorJsonAtributoId.Replace(texto, "");
        }
    }
}
