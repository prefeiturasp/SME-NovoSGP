using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Retorna a mesma lista removendo os itens duplicados de acordo com o filtro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista">Enumerable para remover duplicados</param>
        /// <param name="filtro">Function com o campo que deve comparar</param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> lista, Func<T, object> filtro)
        {
            return lista.GroupBy(filtro).Select(x => x.First());
        }
    }
}
