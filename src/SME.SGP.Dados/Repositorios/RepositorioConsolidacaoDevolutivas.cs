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

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoDevolutivas : IRepositorioConsolidacaoDevolutivas
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoDevolutivas(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<QuantidadeDevolutivasEstimadasEConfirmadasPorTurmaAnoDto>> ObterDevolutivasEstimadasEConfirmadasAsync(int anoLetivo, Modalidade modalidade, long? dreId, long? ueId)
        {
            var possuiFiltroUe = ueId.HasValue;
            var query = new StringBuilder();

            query.AppendLine("with lista as (");
            query.AppendLine("select t.ano,");
            query.AppendLine("       cd.quantidade_estimada_devolutivas,");
            query.AppendLine("       cd.quantidade_registrada_devolutivas,");
            query.AppendLine("	     row_number() over(partition by cd.turma_id order by cd.id desc) sequencia");
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
                query.AppendLine("	     quantidade_estimada_devolutivas DevolutivasEstimadas,");
                query.AppendLine("	     quantidade_registrada_devolutivas DevolutivasRegistradas");
            }
            else
            {
                query.AppendLine("	     sum(quantidade_estimada_devolutivas) DevolutivasEstimadas,");
                query.AppendLine("	     sum(quantidade_registrada_devolutivas) DevolutivasRegistradas");
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

        private string DefinirSelectQueryDevolutivasEstimadasEConfirmadas(bool possuiFiltroDeUe)
            => possuiFiltroDeUe
                ? @"select
                        t.turma_id,
                        t.nome as TurmaAno,
                        cd.quantidade_estimada_devolutivas as DevolutivasEstimadas,
	                    cd.quantidade_registrada_devolutivas as DevolutivasRegistradas,"
                : @"select
                        t.ano as TurmaAno,
                        sum(cd.quantidade_estimada_devolutivas) as DevolutivasEstimadas,
	                    sum(cd.quantidade_registrada_devolutivas) as DevolutivasRegistradas,";

        public async Task<long> Inserir(ConsolidacaoDevolutivas consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task LimparConsolidacaoDevolutivasPorAno(int anoLetivo)
        {
            var query = @" delete from consolidacao_devolutivas
                           where turma_id in (
                                               select 
	                                                distinct
                                                    t.id
                                                from diario_bordo db 
                                                    inner join aula a on a.id = db.aula_id
                                                    inner join turma t on t.turma_id = a.turma_id 
                                                    inner join ue ue on ue.id = t.ue_id 
                                                where not db.excluido 
                                                    and t.ano_letivo = @anoLetivo
                                                    and t.modalidade_codigo in (1,2)
                                                    and a.data_aula < current_date )  ";

            await database.Conexao.ExecuteScalarAsync(query, new { anoLetivo });

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
    }
}