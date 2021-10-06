using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoDiariosBordo : IRepositorioConsolidacaoDiariosBordo
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoDiariosBordo(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public async Task ExcluirPorAno(int anoLetivo)
        {
            var query = @"delete from consolidacao_diarios_bordo where ano_letivo = @anoLetivo";

            await database.Conexao.ExecuteScalarAsync(query, new { anoLetivo });
        }

        public async Task<IEnumerable<ConsolidacaoDiariosBordo>> GerarConsolidacaoPorUe(long ueId, int anoLetivo)
        {
            var query = @"select t.id as TurmaId
                            , t.ano_letivo as AnoLetivo
                            , count(a.id) filter (where db.id is not null) as QuantidadePreenchidos
                            , count(a.id) filter (where db.id is null) as QuantidadePendentes
                          from aula a
                          left join diario_bordo db on db.aula_id = a.id and not db.excluido 
                         inner join turma t on t.turma_id = a.turma_id 
                         where not a.excluido 
                           and a.data_aula < NOW()
                           and t.ue_id = @ueId
                           and t.ano_letivo = @anoLetivo
                        group by t.id, t.ano_letivo";

            return await database.Conexao.QueryAsync<ConsolidacaoDiariosBordo>(query, new { ueId, anoLetivo });
        }

        public async Task Salvar(ConsolidacaoDiariosBordo entidade)
        {
            if (entidade.Id > 0)
                await database.Conexao.UpdateAsync(entidade);
            else
                await database.Conexao.InsertAsync(entidade);
        }

        public async Task<IEnumerable<GraficoTotalDiariosEDevolutivasPorDreDTO>> ObterQuantidadeTotalDeDiariosPendentesPorDre(int anoLetivo, string ano)
        {
            var filtroAno = !string.IsNullOrEmpty(ano) ? "and t.ano = @ano" : "";

            var query = $@"select * from (
                    select dre.abreviacao as Dre
	                    , RIGHT(dre.abreviacao,2) as Grupo
	                    , dre.dre_id as DreId
	                    , sum(c.quantidade_pendentes) as Quantidade
	                    , 'Quantidade de Diarios de Bordos Pendentes' as Descricao
                      from consolidacao_diarios_bordo c
                     inner join turma t on t.turma_id = a.turma_id 
                     inner join ue on ue.id = t.ue_id 
                     inner join dre on dre.id = ue.dre_id 
                     where t.ano_letivo = @anoLetivo
                       {filtroAno}
                    group by dre.abreviacao, dre.dre_id
   
                    union all

                    select dre.abreviacao as Dre
	                    , RIGHT(dre.abreviacao,2) as Grupo
	                    , dre.dre_id as DreId
	                    , sum(c.quantidade_preenchidos) as Quantidade
	                    , 'Quantidade de Diarios de Bordos Preenchidos' as Descricao
                      from consolidacao_diarios_bordo c
                     inner join turma t on t.turma_id = a.turma_id 
                     inner join ue on ue.id = t.ue_id 
                     inner join dre on dre.id = ue.dre_id 
                     where t.ano_letivo = @anoLetivo
                       {filtroAno}
                    group by dre.abreviacao, dre.dre_id) t
                    order by DreId";

            return await database.Conexao.QueryAsync<GraficoTotalDiariosEDevolutivasPorDreDTO>(query, new { anoLetivo });
        }
    }
}
