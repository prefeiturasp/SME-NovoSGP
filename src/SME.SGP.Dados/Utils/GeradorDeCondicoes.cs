using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class GeradorDeCondicoes
    {
        private List<Condicao> Condicoes;
        private StringBuilder QueryCondicao;

        public GeradorDeCondicoes(string condicao)
        {
            Condicoes = new List<Condicao>();
            QueryCondicao = new StringBuilder();
            QueryCondicao.Append(condicao);
        }

        public void AdicioneCondicao(bool adicionar, string condicao)
        {
            Condicoes.Add(new Condicao(adicionar, condicao));
        }

        public string ObterCondicao()
        {
            foreach (var condicao in Condicoes)
            {
                if (condicao.Adicionar)
                    QueryCondicao.Append(condicao.Query);
            }

            return QueryCondicao.ToString();
        }

        private class Condicao
        {
            public Condicao(bool adicionar, string query)
            {
                Adicionar = adicionar;
                Query = query;
            }
            public string Query { get; set; }
            public bool Adicionar { get; set; }
        }
    }
}
