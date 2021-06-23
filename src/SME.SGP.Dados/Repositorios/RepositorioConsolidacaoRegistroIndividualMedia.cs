using Dapper;
using Dommel;
using SME.SGP.Infra;
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
    }
}
