using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoDevolutivas : IRepositorioConsolidacaoDevolutivas
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoDevolutivas(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }
        
        public async Task Salvar(ConsolidacaoDevolutivas consolidacaoDevolutivas)
        {
            if (consolidacaoDevolutivas.Id > 0)
                await database.Conexao.UpdateAsync(consolidacaoDevolutivas);
            else
                await database.Conexao.InsertAsync(consolidacaoDevolutivas);
        }
    }
}