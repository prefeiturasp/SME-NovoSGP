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

        public Task<WfAprovacaoItinerancia> ObterPorWorkflowId(long workflowId)
        {
            throw new NotImplementedException();
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
