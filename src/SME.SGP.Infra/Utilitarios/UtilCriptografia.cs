﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilCriptografia
    {
        public static string ConverterBase64(string entrada)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(entrada);

            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public static string DesconverterBase64(string entrada)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(entrada);

            return Encoding.ASCII.GetString(encodedDataAsBytes);
        }
    }
}
