using Dapper;
using Microsoft.Extensions.Primitives;
using Pipelines.Sockets.Unofficial.Arenas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoDevolutivasConsulta : IRepositorioConsolidacaoDevolutivasConsulta
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoDevolutivasConsulta(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }


        public async Task<IEnumerable<QuantidadeDevolutivasEstimadasEConfirmadasPorTurmaAnoDto>> ObterDevolutivasEstimadasEConfirmadasAsync(int anoLetivo, Modalidade modalidade, long? dreId, long? ueId)
        {
            var possuiFiltroUe = ueId.HasValue;
            var query = new StringBuilder();

            query.AppendLine("with lista as (");
            query.AppendLine("select t.ano,");
            query.AppendLine("       cd.quantidade_estimada_devolutivas,");
            query.AppendLine("       cd.quantidade_registrada_devolutivas,");
            query.AppendLine("         row_number() over(partition by cd.turma_id order by cd.id desc) sequencia");
            query.AppendLine("  from consolidacao_devolutivas cd");
            query.AppendLine("      inner join turma t");
            query.AppendLine("          on cd.turma_id = t.id");
            query.AppendLine("      inner join ue");
            query.AppendLine("          on t.ue_id = ue.id");
            query.AppendLine("where t.ano_letivo = @anoLetivo");
            query.AppendLine("      and t.modalidade_codigo = @modalidade");

            if (dreId.HasValue)
                query.AppendLine("and ue.dre_id = @dreId");

            if (ueId.HasValue)
                query.AppendLine("and ue.id = @ueId");

            query.AppendLine(")");
            query.AppendLine("select ano TurmaAno,");

            if (possuiFiltroUe)
            {
                query.AppendLine("         quantidade_estimada_devolutivas DevolutivasEstimadas,");
                query.AppendLine("         quantidade_registrada_devolutivas DevolutivasRegistradas");
            }
            else
            {
                query.AppendLine("         sum(quantidade_estimada_devolutivas) DevolutivasEstimadas,");
                query.AppendLine("         sum(quantidade_registrada_devolutivas) DevolutivasRegistradas");
            }            

            query.AppendLine("  from lista");
            query.AppendLine("where sequencia = 1");

            if (!possuiFiltroUe)
                query.AppendLine("group by ano");

            var parametros = new
            {
                anoLetivo,
                modalidade,
                dreId,
                ueId
            };

            return await database.QueryAsync<QuantidadeDevolutivasEstimadasEConfirmadasPorTurmaAnoDto>(query.ToString(), parametros);
        }

        public async Task<bool> ExisteConsolidacaoDevolutivaTurmaPorAno(int ano)
        {
            var query = @"select 1 
                          from consolidacao_devolutivas cd
                         inner join turma t on t.id = cd.turma_id
                         where t.ano_letivo = @ano";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { ano });
        }

        public async Task<IEnumerable<GraficoBaseDto>> ObterTotalDevolutivasPorDre(int anoLetivo, string ano)
        {
            var filtroAno = !string.IsNullOrEmpty(ano) ? "and t.ano = @ano" : "";
            var query = $@"select
                            dre.abreviacao AS descricao,
                            sum(cd.quantidade_registrada_devolutivas) AS Quantidade
                        from consolidacao_devolutivas cd
                        inner join turma t on cd.turma_id = t.id
                        inner join ue u on t.ue_id = u.id
                        inner join dre on dre.id = u.dre_id
                        WHERE t.ano_letivo = @anoLetivo
                            {filtroAno}
                        GROUP BY dre.abreviacao, dre.dre_id 
                        order by dre.dre_id ";

            return await database.Conexao.QueryAsync<GraficoBaseDto>(query, new { anoLetivo, ano });
        }

        public Task<ConsolidacaoDevolutivas> ObterConsolidacaoDevolutivasPorTurmaId(long turmaId)
        {
            var query = @"select id, turma_id, quantidade_estimada_devolutivas, quantidade_registrada_devolutivas 
                          from consolidacao_devolutivas 
                          where turma_id = @turmaId";

            return database.Conexao.QueryFirstOrDefaultAsync<ConsolidacaoDevolutivas>(query, new { turmaId });
        }
    }
}