using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public static class ListExtension
    {
        public static bool PossuiRegistros<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.NaoEhNulo() && enumerable.Any();
        }

        public static bool NaoPossuiRegistros<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.PossuiRegistros();
        }

        public static bool PossuiRegistros<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable.NaoEhNulo() && enumerable.Any(predicate);
        }

        public static bool NaoPossuiRegistros<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return !enumerable.PossuiRegistros(predicate);
        }
    }
}
