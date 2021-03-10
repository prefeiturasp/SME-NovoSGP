﻿using System;
using System.Text.RegularExpressions;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilRegex
    {
        public static string RemoverTagsHtml(string texto)
        {
            texto = Regex.Replace(texto, @"<br[^>]*>", " ");
            texto = Regex.Replace(texto, @"<[^>]*>", String.Empty);
            texto = Regex.Replace(texto, @"&nbsp;", " ").Trim();
            return texto.Trim();
        }
    }
}
