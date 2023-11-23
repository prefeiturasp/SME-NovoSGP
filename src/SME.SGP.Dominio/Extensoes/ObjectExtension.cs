using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public static class ObjectExtension
    {
        public static bool EhNulo(this object objeto)
        {
            return objeto is null;
        }

        public static bool NaoEhNulo(this object objeto)
        {
            return !(objeto is null);
        }
    }
}
