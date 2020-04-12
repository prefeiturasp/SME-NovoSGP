using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComunicacaoGrupo : IRepositorioComunicadoGrupo
    {
        protected readonly ISgpContext database;

        public RepositorioComunicacaoGrupo(ISgpContext database)
        {
            this.database = database;
        }

        public async Task ExcluirPorIdComunicado(long id)
        {
            var query = "DELETE FROM comunidado_grupo WHERE comunicado_id = @id";
            await database.Conexao.ExecuteAsync(query, new { id });
        }

        public virtual async Task<long> SalvarAsync(ComunicadoGrupo comunicadoGrupo)
        {
            if (comunicadoGrupo.Id > 0)
                await database.Conexao.UpdateAsync(comunicadoGrupo);
            else
                comunicadoGrupo.Id = (long)(await database.Conexao.InsertAsync(comunicadoGrupo));

            return comunicadoGrupo.Id;
        }
    }
}