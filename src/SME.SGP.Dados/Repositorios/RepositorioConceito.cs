using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConceito : RepositorioBase<Conceito>, IRepositorioConceito
    {
        public RepositorioConceito(ISgpContext database) : base(database)
        {
        }

        public IEnumerable<Conceito> ObterPorDataAvaliacao(DateTime dataAvaliacao)
        {
            var sql = @"select id, valor, descricao, aprovado, ativo, inicio_vigencia, fim_vigencia,
                    criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
                    from conceito_valores where inicio_vigencia <= @dataAvaliacao
                    and(fim_vigencia >= @dataAvaliacao or ativo = true)";

            var parametros = new { dataAvaliacao };

            return database.Query<Conceito>(sql, parametros);
        }
    }
}