using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public static class ListExtension
    {
        public static IEnumerable<List<T>> DividirEmListasMenores<T>(this List<T> itens, int tamanhoADividirLista = 30)
        {
            for (int i = 0; i < itens.Count; i += tamanhoADividirLista)
            {
                yield return itens.GetRange(i, Math.Min(tamanhoADividirLista, itens.Count - i));
            }
        }
    }
}
