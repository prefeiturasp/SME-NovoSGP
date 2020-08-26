using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;

namespace SME.SGP.Integracao
{
    public class ContextoTesteIntegrado : ContextoBase
    {

        public ContextoTesteIntegrado(string tokenAtual)
        {
            InicializarVariaveis(tokenAtual);
        }

        private void InicializarVariaveis(string tokenAtual)
        {
            Variaveis.Add("RF", "7777710");
            Variaveis.Add("Claims", "");
            Variaveis.Add("login", "7777710");
            Variaveis.Add("NumeroPagina", "0");
            Variaveis.Add("NumeroRegistros", "0");

            Variaveis.Add("UsuarioLogado", "Sistema");
            Variaveis.Add("NomeUsuario", "Sistema");


            Variaveis.Add("TemAuthorizationHeader", true);
            Variaveis.Add("TokenAtual", tokenAtual);

        }

        public override IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto)
        {
            throw new Exception("Este tipo de conexto não permite atribuição");
        }

        public override void AdicionarVariaveis(IDictionary<string, object> variaveis)
        {
            Variaveis = variaveis;
        }
    }
}
