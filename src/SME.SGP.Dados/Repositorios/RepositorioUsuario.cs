﻿using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Text;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(ISgpContext conexao) : base(conexao)
        {
        }

        public Usuario ObterPorCodigoRfLogin(string codigoRf, string login)
        {
            var query = new StringBuilder();
            query.AppendLine("select * from usuario");
            query.AppendLine("where 1=1");

            if (!string.IsNullOrEmpty(login))
                query.AppendLine("and login = @login");

          else if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("and rf_codigo = @codigoRf");

            return database.Conexao.Query<Usuario>(query.ToString(), new { codigoRf, login })
                .FirstOrDefault();
        }

        public Usuario ObterPorTokenRecuperacaoSenha(Guid token)
        {
            var query = new StringBuilder();
            query.Append("select * from usuario ");
            query.Append("where token_recuperacao_senha = @token ");

            return database.Conexao.Query<Usuario>(query.ToString(), new { token })
                .FirstOrDefault();
        }
    }
}