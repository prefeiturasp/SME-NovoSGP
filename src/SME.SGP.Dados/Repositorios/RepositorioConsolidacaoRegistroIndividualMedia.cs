using Dapper;
using Dommel;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public class RepositorioConsolidacaoRegistroIndividualMedia : IRepositorioConsolidacaoRegistroIndividualMedia
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoRegistroIndividualMedia(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> Inserir(ConsolidacaoRegistroIndividualMedia consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task LimparConsolidacaoMediaRegistrosIndividuaisPorAno(int anoLetivo)
        {
            var query = @" delete from consolidacao_registro_individual_media
                           where turma_id in (
                                               select 
                                                    distinct
                                                    ri.turma_id
                                                from registro_individual ri 
                                                inner join turma tu on ri.turma_id = tu.id 
                                                where not ri.excluido 
	                                                and tu.ano_letivo = @anoLetivo
	                                                and tu.modalidade_codigo in (1,2)
                                                order by ri.turma_id )";

            await database.Conexao.ExecuteScalarAsync(query, new { anoLetivo });
        }

        public async Task<IEnumerable<RegistroIndividualMediaPorAnoDto>> ObterRegistrosItineranciasMediaPorAnoAsync(int anoLetivo, long dreId, Modalidade modalidade)
        {
            var query = new StringBuilder($@"select (sum(cr.quantidade)/count(t.ano)) as quantidade, 
	                                                t.ano,
	                                                t.modalidade_codigo as modalidade
                                               from consolidacao_registro_individual_media cr 
                                              inner join turma t on t.id = cr.turma_id 
                                              inner join ue on ue.id = t.ue_id 
                                              inner join dre on dre.id = ue.dre_id
                                              where t.modalidade_codigo = @modalidade
                                                and t.ano_letivo = @anoLetivo ");
            if (dreId > 0)
                query.AppendLine(" and dre.id = @dreId ");

            query.AppendLine(@"group by t.ano, t.modalidade_codigo
                               order by t.ano, t.modalidade_codigo");

            return await database.Conexao.QueryAsync<RegistroIndividualMediaPorAnoDto>(query.ToString(), new { anoLetivo, dreId, modalidade });
        }

        public async Task<IEnumerable<GraficoBaseQuantidadeDoubleDto>> ObterRegistrosItineranciasMediaPorTurmaAsync(int anoLetivo, long ueId, Modalidade modalidade)
        {
            var query = @" select cr.quantidade as quantidade, 
	                              t.nome as descricao
                             from consolidacao_registro_individual_media cr 
                            inner join turma t on t.id = cr.turma_id 
                            where t.modalidade_codigo = @modalidade
                              and t.ano_letivo = @anoLetivo
                              and t.ue_id = @ueId 
                              order by t.nome";

            return await database.Conexao.QueryAsync<GraficoBaseQuantidadeDoubleDto>(query, new { anoLetivo, ueId, modalidade });
        }
    }
}
