using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Contexto
{
    public abstract class ContextoBase : IContextoAplicacao
    {
        public ContextoBase()
        {
            Variaveis = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Variaveis { get; set; }
        public string UsuarioLogado => ObterVarivel<string>("UsuarioLogado");
        public string NomeUsuario => ObterVarivel<string>("NomeUsuario");

        public abstract IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto);

        public T ObterVarivel<T>(string nome)
        {
            object valor = null;

            if (Variaveis.TryGetValue(nome, out valor))
                return (T)valor;

            return default(T);
        }
    }
}
