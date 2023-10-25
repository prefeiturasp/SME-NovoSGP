using SME.SGP.Dominio.Constantes;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SME.SGP.Infra
{
    public static class LongExtension
    {
        public static bool EhMaiorQueZero(this long valor)
        {
            return valor > 0;
        }
        
        public static bool EhMenorQueZero(this long valor)
        {
            return valor < 0;
        }
        
        public static bool EhIgualZero(this long valor)
        {
            return valor == 0;
        }
    }
}
