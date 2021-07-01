using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComunicadoModalidade : IRepositorioComunicadoModalidade
    {
        protected readonly ISgpContext database;

        public RepositorioComunicadoModalidade(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public virtual async Task<long> SalvarAsync(ComunicadoModalidade comunicadoModalidade)
        {
            if (comunicadoModalidade.Id > 0)
                await database.Conexao.UpdateAsync(comunicadoModalidade);
            else
                comunicadoModalidade.Id = (long)(await database.Conexao.InsertAsync(comunicadoModalidade));

            return comunicadoModalidade.Id;
        }
        public async Task ExcluirPorIdComunicado(long id)
        {
            var query = "update comunicado_modalidade set excluido=true WHERE comunicado_id = @id";
            await database.Conexao.ExecuteAsync(query, new { id });
        }
    }
}
