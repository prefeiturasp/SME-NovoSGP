﻿using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra.Contexto
{
    public abstract class ContextoBase : IContextoAplicacao
    {
        protected ContextoBase()
        {
            Variaveis = new Dictionary<string, object>();
        }

        public string NomeUsuario => ObterVarivel<string>("NomeUsuario") ?? "Sistema";
        public string UsuarioLogado => ObterVarivel<string>("UsuarioLogado") ?? "Sistema";
        public Guid UsuarioPerfil => ObterPerfil();
            public IDictionary<string, object> Variaveis { get; set; }

        public abstract void AdicionarVariaveis(IDictionary<string, object> variaveis);
        public abstract IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto);

        public T ObterVarivel<T>(string nome)
        {
            object valor = null;

            if (Variaveis.TryGetValue(nome, out valor))
                return (T)valor;

            return default(T);
        }

        public Guid ObterPerfil()
        {
            string perfil = ObterVarivel<string>("perfil");
            
            if(perfil == string.Empty)
                return Guid.Empty;

            return new Guid(perfil);
        }
    }
}