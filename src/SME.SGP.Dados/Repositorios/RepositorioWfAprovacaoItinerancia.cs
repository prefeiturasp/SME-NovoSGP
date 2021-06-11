using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWfAprovacaoItinerancia : IRepositorioWfAprovacaoItinerancia
    {
        protected readonly ISgpContext database;

        public RepositorioWfAprovacaoItinerancia(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<WfAprovacaoItinerancia> ObterPorItineranciaId(long itineranciaId)
        {
            var query = @"select 
	                        wf_aprovacao_id as WfAprovacaoId,
	                        itinerancia_id as ItineranciaId,
	                        status_aprovacao as StatusAprovacao
                        from wf_aprovacao_itinerancia
                        where itinerancia_id = @itineranciaId order by id desc";

            return await database.Conexao.QueryFirstOrDefaultAsync<WfAprovacaoItinerancia>(query, new { itineranciaId });
        }

        public async Task<WfAprovacaoItinerancia> ObterPorWorkflowId(long workflowId)
        {
            var query = @"select 
	                        wf_aprovacao_id as WfAprovacaoId,
	                        itinerancia_id as ItineranciaId,
	                        status_aprovacao as StatusAprovacao
                        from wf_aprovacao_itinerancia
                        where wf_aprovacao_id = @workflowId order by id desc";

            return await database.Conexao.QueryFirstOrDefaultAsync<WfAprovacaoItinerancia>(query, new { workflowId });
        }

        public async Task SalvarAsync(WfAprovacaoItinerancia entidade)
        {
            if (entidade.Id > 0)
                await database.Conexao.UpdateAsync(entidade);
            else
                await database.Conexao.InsertAsync(entidade);
        }
    }
}
