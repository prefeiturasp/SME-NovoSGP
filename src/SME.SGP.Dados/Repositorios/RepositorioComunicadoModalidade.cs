using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
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
        public async Task<bool> ExcluirPorIdComunicado(long id)
        {
            var query = "update comunicado_modalidade set excluido=true WHERE comunicado_id = @id";
            return await database.Conexao.ExecuteAsync(query, new { id }) != 0;
        }
        public async Task<IEnumerable<int>> ObterModalidadesPorComunicadoId(long id)
        {
            var sql = @"select modalidade 
                          from comunicado_modalidade cm
                         where cm.comunicado_id = @id
                           and not excluido";
            var parametros = new { id };
            return await database.QueryAsync<int>(sql, parametros);
        }
    }
}
