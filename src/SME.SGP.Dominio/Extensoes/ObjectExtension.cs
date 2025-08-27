using Newtonsoft.Json;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SME.SGP.Dominio
{
    public static class ObjectExtension
    {
        public static bool EhNulo(this object objeto) /* assertNull */
        {
            return objeto is null;
        }

        public static bool NaoEhNulo(this object objeto) /* assertNotNull */
        {
            return !(objeto is null);
        }

        public static void LancarExcecaoNegocioSeEhNulo(this object objeto, string msgErro)
        {
            if (objeto.EhNulo())
                throw new NegocioException(msgErro);
        }

        public static bool NaoEhArquivoXlsx(this string texto)
        {
            return !EhArquivoXlsx(texto);
        }

        public static bool EhArquivoXlsx(this string texto)
        {
            return texto.Equals(MensagemNegocioComuns.CONTENT_TYPE_EXCEL);
        }

        public static string RemoverAcentuacao(this string valor)
        {
            if (valor.ItemSemPreenchimento())
                return valor;

            return new string(valor
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        public static bool ItemSemPreenchimento(this string? str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool SaoDiferentes(this string valor, string valorAComparar)
        {
            return !valor.ToLower().Equals(valorAComparar.ToLower());
        }

        public static bool NaoEhArquivoXlsx(this string texto)
        {
            return !EhArquivoXlsx(texto);
        }

        public static bool EhArquivoXlsx(this string texto)
        {
            return texto.Equals(MensagemNegocioComuns.CONTENT_TYPE_EXCEL);
        }

        public static string RemoverAcentuacao(this string valor)
        {
            if (valor.ItemSemPreenchimento())
                return valor;

            return new string(valor
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        public static bool ItemSemPreenchimento(this string? str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool SaoDiferentes(this string valor, string valorAComparar)
        {
            return !valor.ToLower().Equals(valorAComparar.ToLower());
        }
    }
}
