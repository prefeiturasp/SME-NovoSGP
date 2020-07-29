using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
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
            var query = "update comunidado_grupo set excluido=true WHERE comunicado_id = @id";
            await database.Conexao.ExecuteAsync(query, new { id });
        }

        public async Task<IEnumerable<ComunicadoGrupo>> ObterPorComunicado(long comunicadoId)
        {
            var query = "select * from comunidado_grupo where comunicado_id = @comunicadoId";

            return await database.QueryAsync<ComunicadoGrupo>(query, new { comunicadoId });
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