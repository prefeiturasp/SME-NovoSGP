using System;
using System.Linq;

namespace SME.SGP.Dominio
{
    public static class EnumExtension
    {
        public static bool EhUmDosValores(this Enum valorEnum, params Enum[] valores)
        {
            return valores.Contains(valorEnum);
        }
    }
}
