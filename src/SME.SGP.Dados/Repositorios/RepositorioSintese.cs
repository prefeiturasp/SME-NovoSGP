using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
   public class RepositorioSintese : RepositorioBase<Sintese>, IRepositorioSintese
    {
        public RepositorioSintese(ISgpContext database) : base(database)
        {
        }

        public IEnumerable<Sintese> ObterPorData(DateTime dataAvaliacao)
        {
            var sql = @"select id, valor, descricao, aprovado, ativo, inicio_vigencia, fim_vigencia,
                    criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
                    from sintese_valores where date(inicio_vigencia) <= @dataAvaliacao
                    and(date(fim_vigencia) >= @dataAvaliacao or ativo = true)";

            var parametros = new { dataAvaliacao = dataAvaliacao.Date };

            return database.Query<Sintese>(sql, parametros);
        }
    }
}
