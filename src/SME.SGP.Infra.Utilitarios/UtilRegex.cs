using System;
using System.Text.RegularExpressions;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilRegex
    {
        public static string RemoverTagsHtml(string texto)
        {
            texto = Regex.Replace(texto, @"<br[^>]*>", " ");
            texto = Regex.Replace(texto, @"<p[^>]*>", " ");
            texto = Regex.Replace(texto, @"<li[^>]*>", " ");
            texto = Regex.Replace(texto, @"<[^>]*>", String.Empty);
            texto = Regex.Replace(texto, @"&nbsp;", " ").Trim();
            return texto.Trim();
        }

        public static string RemoverTagsHtmlMidia(string texto)
        {
            texto = Regex.Replace(texto, @"<img[^>]*>", " [arquivo indisponível na visualização] ");
            texto = Regex.Replace(texto, @"<iframe[^>]*>", " [arquivo indisponível na visualização] ");
            texto = Regex.Replace(texto, @"<video.+</video>", " [arquivo indisponível na visualização] ");
            return texto;
        }
    }
}
