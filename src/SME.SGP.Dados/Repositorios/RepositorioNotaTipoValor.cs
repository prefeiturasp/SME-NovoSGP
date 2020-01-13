using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotaTipoValor : RepositorioBase<NotaTipoValor>, IRepositorioNotaTipoValor
    {
        public RepositorioNotaTipoValor(ISgpContext database) : base(database)
        {
        }

        public NotaTipoValor ObterPorCicloIdDataAvalicacao(long cicloId, DateTime dataAvalicao)
        {
            var sql = @"select ntv.* from notas_tipo_valor ntv
                        inner join notas_conceitos_ciclos_parametos nccp
                        on nccp.tipo_nota = ntv.id
                        where nccp.ciclo = @cicloId and @dataAvalicao >= nccp.inicio_vigencia
                        and (nccp.ativo = true or @dataAvalicao <= nccp.fim_vigencia)
                        order by nccp.id asc";

            var parametros = new { cicloId, dataAvalicao };

            return database.QueryFirstOrDefault<NotaTipoValor>(sql, parametros);
        }
    }
}