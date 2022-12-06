﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task AtualizarUltimoLogin(long id, DateTime ultimoLogin)
        {
            var comando = "update usuario set ultimo_login = @ultimoLogin where id = @id";
            await database.Conexao.ExecuteScalarAsync(comando, new { id, ultimoLogin });
        }

        public async Task<IEnumerable<Usuario>> ObterPorIdsAsync(long[] ids)
        {
            var query = "select * from usuario where id = any(@ids)";

            return await database.Conexao.QueryAsync<Usuario>(query, new { ids });
        }
    }
}