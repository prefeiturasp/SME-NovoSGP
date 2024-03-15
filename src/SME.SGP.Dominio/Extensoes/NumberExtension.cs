using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public static class NumberExtension
    {
        public static long[] ToArray<T>(this long source)
        => new long[] { source };
    }
}
