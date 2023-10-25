using SME.SGP.Dominio.Constantes;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SME.SGP.Infra
{
    public static class IntExtension
    {
        public static bool EhMaiorQueZero(this int valor)
        {
            return valor > 0;
        }
        
        public static bool EhMenorQueZero(this int valor)
        {
            return valor < 0;
        }
        
        public static bool EhIgualZero(this int valor)
        {
            return valor == 0;
        }
    }
}
