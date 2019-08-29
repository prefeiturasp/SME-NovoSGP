using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Auxiliares
{
    public static class ConversaoNumeroAuxiliar
    {
        public static int ObtemNumero(string descricao)
        {
            var listaNumeros = ObtemLista();

            if (listaNumeros.TryGetValue(descricao, out int anoSerie))
                return anoSerie;
            else throw new NegocioException($"Ano Série {descricao} não localizado!");
        }

        private static Dictionary<string, int> ObtemLista()
        {
            var listaNumeros = new Dictionary<string, int>();
            listaNumeros.Add("first", 1);
            listaNumeros.Add("second", 2);
            listaNumeros.Add("third", 3);
            listaNumeros.Add("fourth", 4);
            listaNumeros.Add("fifth", 5);
            listaNumeros.Add("sixth", 6);
            listaNumeros.Add("seventh", 7);
            listaNumeros.Add("eighth", 8);
            listaNumeros.Add("nineth", 9);

            return listaNumeros;
        }
    }
}
