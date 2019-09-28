using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPrioridadePerfil : RepositorioBase<PrioridadePerfil>, IRepositorioPrioridadePerfil
    {
        public RepositorioPrioridadePerfil(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<PrioridadePerfil> ObterPerfisPorIds(IEnumerable<Guid> idsPerfis)
        {
            var query = new StringBuilder();
            query.AppendLine("select * from prioridade_perfil where codigo_perfil = Any(@Ids) order by ordem");

            return database.Conexao.Query<PrioridadePerfil>(query.ToString(), new { Ids = idsPerfis });
        }
    }
}