using Dapper;
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
        public async Task<IEnumerable<RegistroItineranciaMediaPorAnoDto>> ObterRegistrosItineranciasMediaPorAnoAsync(int anoLetivo, long dreId, Modalidade modalidade)
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

            return await database.Conexao.QueryAsync<RegistroItineranciaMediaPorAnoDto>(query.ToString(), new { anoLetivo, dreId, modalidade });
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
