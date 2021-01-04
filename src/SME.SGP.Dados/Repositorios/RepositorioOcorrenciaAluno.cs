using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrenciaAluno : IRepositorioOcorrenciaAluno
    {
        private readonly ISgpContext database;

        public RepositorioOcorrenciaAluno(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(OcorrenciaAluno entidade)
        {
            if (entidade.Id > 0)
            {
                await database.Conexao.UpdateAsync(entidade);
            }
            else
            {
                entidade.Id = (long)(await database.Conexao.InsertAsync(entidade));
            }

            return entidade.Id;
        }
    }
}
