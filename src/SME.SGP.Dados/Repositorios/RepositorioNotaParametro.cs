using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using SME.SGP.Infra;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados
{
    public class RepositorioNotaParametro : RepositorioBase<NotaParametro>, IRepositorioNotaParametro
    {
        public RepositorioNotaParametro(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<NotaParametro> ObterPorDataAvaliacao(DateTime dataAvaliacao)
        {
            var sql = @"select id, valor_maximo, valor_medio, valor_minimo, incremento, ativo, inicio_vigencia,
                        fim_vigencia, criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
                        from notas_parametros where inicio_vigencia <= @dataAvaliacao
                        and(ativo = true or fim_vigencia >= @dataAvaliacao)";

            var parametros = new { dataAvaliacao };

            return await database.QueryFirstOrDefaultAsync<NotaParametro>(sql, parametros);
        }
        public Task<NotaParametroDto> ObterParametrosArredondamentoNotaPorDataAvaliacao(DateTime dataAvaliacao)
        {
            var sql = @"select id, valor_maximo as Maxima, valor_minimo as Minima, incremento
                        from notas_parametros where inicio_vigencia <= @dataAvaliacao
                        and(fim_vigencia is null or fim_vigencia >= @dataAvaliacao)";

            var parametros = new { dataAvaliacao };

            return database.QueryFirstOrDefaultAsync<NotaParametroDto>(sql, parametros);
        }

    }
}