using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConceito : RepositorioBase<Conceito>, IRepositorioConceito
    {
        public RepositorioConceito(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<Conceito>> ObterPorData(DateTime dataAvaliacao)
        {
            var sql = @"select id, valor, descricao, aprovado, ativo, inicio_vigencia, fim_vigencia,
                    criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
                    from conceito_valores where date(inicio_vigencia) <= @dataAvaliacao
                    and(date(fim_vigencia) >= @dataAvaliacao or ativo = true)";

            var parametros = new { dataAvaliacao = dataAvaliacao.Date };

            return await database.QueryAsync<Conceito>(sql, parametros);
        }
    }
}