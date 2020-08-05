using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordo: RepositorioBase<DiarioBordo>, IRepositorioDiarioBordo
    {
        public RepositorioDiarioBordo(ISgpContext conexao) : base(conexao) { }



        public async Task<DiarioBordo> ObterPorAulaId(long aulaId)
        {
            var sql = @"select id, aula_id, devolutiva_id, planejamento, reflexoes_replanejamento,
                    criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
                    from diario_bordo where aula_id = @aulaId";

            var parametros = new { aulaId = aulaId };

            return await database.QueryFirstOrDefaultAsync<DiarioBordo>(sql, parametros);
        }
    }
}
